using UnityEngine;

namespace AtlasLib.Pages;

public class BasicPage : Page
{
    private readonly GameObject _prefab;
    private readonly string _holderPath;
    
    public BasicPage(GameObject prefab, string holderPath = "")
    {
        _prefab = prefab;
        _holderPath = holderPath;
    }
    
    public override void CreatePage(Transform parent)
    {
        base.CreatePage(parent);
        GameObject instance = Object.Instantiate(_prefab, parent);
        Objects.Add(instance);
        PageButtonHolder = (_holderPath == string.Empty ? instance.transform : instance.transform.Find(_holderPath));
    }
}
