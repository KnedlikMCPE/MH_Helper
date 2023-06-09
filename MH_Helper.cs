﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Astronautica;
using BepInEx;
using HarmonyLib;

namespace MH_Helper
{
    [BepInPlugin("com.knedlik.mh.helper", "MH Helper", "1.1.0.0")]
    public class MH_Helper : BaseUnityPlugin
    {
        public static MH_Helper Instance;
        public static bool FirstTime;
        public Data data;
        public List<Action> onLoadDataActions = new List<Action>();
        public Dictionary<string, Localisation.Entry> localeEntries;

        private void Awake()
        {
            Instance = this;
            FirstTime = true;
            // Plugin startup logic
            var harmony = new Harmony("com.knedlik.mh.helper");
            harmony.PatchAll();
            
            var entriesData = Localisation.instance.GetType()
                .GetField("entriesDefault", BindingFlags.Instance | BindingFlags.NonPublic);
            localeEntries = (Dictionary<string, Localisation.Entry>) entriesData.GetValue(Localisation.instance);
        }
    }
    

    [HarmonyPatch(typeof(Data))]
    [HarmonyPatch(nameof(Data.instance))]
    [HarmonyPatch(MethodType.Getter)]
    public class PatchGameDataLoader
    {

        static void Postfix(ref Data __result)
        {
            if (MH_Helper.FirstTime)
            {
                MH_Helper.FirstTime = false;
                MH_Helper.Instance.data = __result;
                foreach (var action in MH_Helper.Instance.onLoadDataActions)
                {
                    action.Invoke();
                }
            }
        }
    }
}