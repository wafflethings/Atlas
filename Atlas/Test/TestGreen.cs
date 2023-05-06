using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atlas.Modules.Guns
{
    public class TestGreen : Gun
    {
        public static GameObject Asset;

        public override GameObject Create(Transform parent)
        {
            base.Create(parent);
            return GameObject.Instantiate(Asset, parent);
        }

        public override int Slot()
        {
            return 5;
        }

        public override string Pref()
        {
            return "cool1"; 
        }
    }
}
