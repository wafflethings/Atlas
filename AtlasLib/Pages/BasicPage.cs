using UnityEngine;

namespace AtlasLib.Pages;

public class BasicPage : Page
{
    private readonly GameObject _prefab;
    
    public BasicPage(GameObject prefab)
    {
        _prefab = prefab;
    }
    
    public override void CreatePage(Transform parent)
    {
        base.CreatePage(parent);
        Objects.Add(Object.Instantiate(_prefab, parent));
    }
}
