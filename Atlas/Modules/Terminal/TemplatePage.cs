using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atlas.Modules.Terminal
{
    public class TemplatePage : Page
    {
        public TemplatePage(GameObject go) : base(go)
        {
            GameObject page = GameObject.Instantiate(Atlas.Assets.LoadAsset<GameObject>("Weapon Template.prefab"));
            page.transform.SetParent(go.transform, false);
            page.SetActive(false);
            Objects.Add(page);
        }
    }
}
