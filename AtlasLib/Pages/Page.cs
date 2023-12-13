using System.Collections.Generic;
using UnityEngine;

namespace AtlasLib.Pages
{
    public abstract class Page
    {
        public List<GameObject> Objects = new();

        public virtual void CreatePage(Transform parent)
        {
            Objects.Clear();
        }

        public virtual void EnablePage()
        {
            foreach (GameObject pageObject in Objects)
            {
                pageObject.SetActive(true);
            }
        }
        
        public virtual void DisablePage()
        {
            foreach (GameObject pageObject in Objects)
            {
                pageObject.SetActive(false);
            }
        }
    }
}
