using Atlas.Modules.Guns;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Atlas.Modules.Terminal
{
    public class TerminalPageRegistry : Module
    {
        internal static GameObject LeftBtn;
        internal static GameObject RightBtn;

        private static int Page = 0;
        public static List<Page> Pages { get; private set; }
        public static List<Type> PageTypes { get; private set; }

        private static string[] Page1Content =
        {
                "RevolverButton",
                "ShotgunButton",
                "NailgunButton",
                "RailcannonButton",
                "RocketLauncherButton",
                "ArmButton",

                "RevolverWindow",
                "ShotgunWindow",
                "NailgunWindow",
                "RailcannonWindow",
                "RocketLauncherWindow",
                "ArmWindow"
        };

        public override void Patch(Harmony harmony)
        {
            harmony.PatchAll(typeof(TerminalPatches));
        }

        public static void RegisterPage(Type type, int at = -1)
        {
            if (PageTypes == null)
            {
                PageTypes = new List<Type>();
            }

            if (at == -1)
            {
                PageTypes.Add(type);
            }
            else
            {
                PageTypes.Insert(at, type);
            }
        }

        public static void UnregisterPage(int index)
        {
            Pages.RemoveAt(index);
        }

        public static void RefreshPages()
        {
            foreach (Page page in Pages)
            {
                if (page.IsDefaultPage && Pages.IndexOf(page) != Page)
                {
                    page.State.Clear();

                    foreach (string str in Page1Content)
                    {
                        GameObject obj = page.gameObject.ChildByName(str);
                        page.State.Add(obj, obj.activeSelf);
                    }
                }

                foreach (GameObject go in page.Objects)
                {
                    if (Pages.IndexOf(page) == Page)
                    {
                        if (page.State.ContainsKey(go))
                        {
                            go.SetActive(page.State[go]);
                        }
                        else
                        {
                            go.SetActive(true);
                        }
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
            }
        }

        public class TerminalPatches
        {
            [HarmonyPatch(typeof(ShopZone), nameof(ShopZone.Start))]
            [HarmonyPostfix]
            public static void ShopStart(ShopZone __instance)
            {
                if (__instance.gameObject.ChildByName("Canvas").ChildByName("Weapons") != null)
                {
                    // If this is true, it's a yellow shop (e.g: not a testament)

                    GameObject OgBtn = __instance.gameObject.ChildByName("Canvas").ChildByName("Weapons").ChildByName("BackButton (1)");

                    LeftBtn = GameObject.Instantiate(OgBtn, OgBtn.transform.parent);
                    RectTransform LeftRect = LeftBtn.GetComponent<RectTransform>();
                    LeftRect.sizeDelta = new Vector2(LeftRect.sizeDelta.x / 2, LeftRect.sizeDelta.y);
                    LeftBtn.transform.localPosition = new Vector3(-220, -145, -45);

                    RightBtn = GameObject.Instantiate(LeftBtn, OgBtn.transform.parent);
                    RightBtn.transform.localPosition = new Vector3(-140, -145, -45);

                    LeftBtn.GetComponent<ShopButton>().toActivate = new GameObject[0];
                    LeftBtn.GetComponent<ShopButton>().toDeactivate = new GameObject[0];
                    LeftBtn.ChildByName("Text").GetComponent<Text>().text = "<<";

                    RightBtn.GetComponent<ShopButton>().toActivate = new GameObject[0];
                    RightBtn.GetComponent<ShopButton>().toDeactivate = new GameObject[0];
                    RightBtn.ChildByName("Text").GetComponent<Text>().text = ">>";

                    LeftBtn.GetComponent<RectTransform>().SetAsFirstSibling();
                    RightBtn.GetComponent<RectTransform>().SetAsFirstSibling();

                    Page = 0;
                    Pages = new List<Page>();
                    Page Default = new Page(__instance.gameObject.ChildByName("Canvas").ChildByName("Weapons"));
                    Pages.Add(Default);
                    Default.IsDefaultPage = true;

                    Default.State.Clear();

                    foreach (string str in Page1Content)
                    {
                        GameObject obj = Default.gameObject.ChildByName(str);
                        Default.Objects.Add(obj);
                        Default.State.Add(obj, obj.activeSelf);
                    }

                    if (PageTypes != null)
                    {
                        object[] args = new object[1] { __instance.gameObject.ChildByName("Canvas").ChildByName("Weapons") };
                        foreach (Type t in PageTypes)
                        {
                            Pages.Add(Activator.CreateInstance(t, args) as Page);
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(ShopZone), nameof(ShopZone.TurnOn))]
            [HarmonyPostfix]
            public static void RefreshOnEntrance()
            {
                RefreshPages();
            }

            [HarmonyPatch(typeof(ShopButton), nameof(ShopButton.Awake))]
            [HarmonyPostfix]
            public static void AddListener(ShopButton __instance)
            {
                if (__instance.gameObject == RightBtn || __instance.gameObject == LeftBtn)
                {
                    __instance.GetComponent<Button>().onClick.AddListener(

                    () =>
                    {
                        if (LeftBtn == __instance.gameObject)
                        {
                            if (Page > 0)
                            {
                                Page -= 1;

                                RefreshPages();
                            }
                        }
                        else
                        {
                            if (Page < Pages.Count - 1)
                            {
                                Page += 1;

                                RefreshPages();
                            }
                        }
                    });
                }
            }
        }
    }
}
