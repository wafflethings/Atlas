using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Atlas.Modules.Guns
{
    public class Weapon
    {
        protected static PlayerInput InputSource;
        public static Weapon Instance;
        public int OrderInSlot = -1;

        public virtual GameObject Create(Transform parent)
        {
            InputSource = InputManager.Instance.InputSource;
            return new GameObject();
        }

        public virtual int Slot()
        {
            return 0;
        }

        public virtual string Pref()
        {
            return "cool0";
        }

        public int Enabled()
        {
            return PrefsManager.Instance.GetInt("weapon." + Pref());
        }
    }
}
