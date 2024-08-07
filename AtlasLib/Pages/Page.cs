using System.Collections.Generic;
using UnityEngine;

namespace AtlasLib.Pages;

public abstract class Page
{
    protected readonly List<GameObject> Objects = new();
    private readonly Dictionary<GameObject, bool> _state = new();

    public virtual void CreatePage(Transform parent)
    {
        Objects.Clear();
        _state.Clear();
    }

    public virtual void EnablePage()
    {
        foreach (GameObject pageObject in Objects)
        {
            if (!_state.ContainsKey(pageObject))
            {
                _state.Add(pageObject, pageObject.activeSelf);
            }
                
            pageObject.SetActive(_state[pageObject]);
        }
    }
        
    public virtual void DisablePage()
    {
        foreach (GameObject pageObject in Objects)
        {
            _state[pageObject] = pageObject.activeSelf;
            pageObject.SetActive(false);
        }
    }
}
