using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace AtlasLib.Saving;

// full credit - i wrote and designed this for ultracrypt with @Protract-123 :3 but i think its useful for all of my mods
public class SaveFile<T> : SaveFile where T : new()
{
    public T Data;
    private string DefaultPath => Path.Combine("wafflethings", "AtlasLib");

    internal SaveFile(string fileName) : base(fileName)
    {
        Data = new T();
        _folder = DefaultPath;
    }

    /// <summary>
    /// Creates the save file.
    /// </summary>
    /// <param name="fileName">The name of the file, e.g. settings.json</param>
    /// <param name="folder">The subfolder this is in of AppData/Roaming, e.g. Path.Combine("wafflethings", "AtlasLib")</param>
    public SaveFile(string fileName, string folder) : base(fileName)
    {
        Data = new T();
        _folder = folder;
    }

    protected virtual string Serialize(T value) => JsonConvert.SerializeObject(value);
    protected virtual T Deserialize(string value) => JsonConvert.DeserializeObject<T>(value);

    protected override void LoadData()
    {
        if (!File.Exists(FilePath))
        {
            return;
        }

        try
        {
            Data = Deserialize(File.ReadAllText(FilePath));
        }
        catch (JsonSerializationException)
        {
            Data = default(T);
        }
    }

    protected override void SaveData()
    {
        File.WriteAllText(FilePath, Serialize(Data));
    }
}

public abstract class SaveFile
{
    private static readonly List<SaveFile> s_saveFiles = new();
    protected string _folder;
    private readonly string _fileName;
    
    private static string AppData => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private string FileFolder => Path.Combine(AppData, _folder);
    protected string FilePath => Path.Combine(FileFolder, _fileName);
    
    protected SaveFile(string fileName)
    {
        _fileName = fileName;
    }

    protected abstract void LoadData();
    protected abstract void SaveData();

    public static void SaveAll()
    {
        foreach (SaveFile file in s_saveFiles)
        {
            if (!Directory.Exists(file.FileFolder))
            {
                Directory.CreateDirectory(file.FileFolder);
            }
            
            file.SaveData();
        }
    }

    public static SaveFile<T> RegisterFile<T>(SaveFile<T> file) where T : new()
    {
        file.LoadData();
        s_saveFiles.Add(file);
        return file;
    }
}
