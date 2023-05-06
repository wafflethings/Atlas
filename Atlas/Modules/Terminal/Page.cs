using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atlas.Modules.Terminal
{
    public class Page
    {
        public GameObject gameObject;

        internal bool IsDefaultPage = false;
        public List<GameObject> Objects = new List<GameObject>();
        public Dictionary<GameObject, bool> State = new Dictionary<GameObject, bool>();

        public Page(GameObject go)
        {
            gameObject = go;
        }
    }
}
