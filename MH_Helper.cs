using System;
using System.Collections;
using Astronautica;
using Astronautica.View;
using BepInEx;
using HarmonyLib;
using HarmonyLib.Tools;

namespace MH_Helper
{
    [BepInPlugin("com.knedlik.mh.helper", "MH Helper", "1.0.0.0")]
    public class MH_Helper : BaseUnityPlugin
    {
        public static MH_Helper Instance;
        public static bool FirstTime = true;
        public Data data;

        private void Awake()
        {
            Instance = this;
            // Plugin startup logic
            var harmony = new Harmony("com.knedlik.mh.helper");
            FileLog.Reset();
            HarmonyFileLog.Enabled = true;

            var gameDataOriginal = typeof(GameDataLoader).GetMethod(nameof(GameDataLoader.LoadGameData));
            var gameDataPostfix = typeof(PatchGameDataLoader).GetMethod(nameof(PatchGameDataLoader.Postfix));
            harmony.Patch(gameDataOriginal, null, new HarmonyMethod(gameDataPostfix));
        }

        public static void AddMissions()
        {
            
        }
    }
    
    class SimpleEnumerator : IEnumerable
    {
        public IEnumerator enumerator;
        public Action prefixAction, postfixAction;
        public Action<object> preItemAction, postItemAction;
        public Func<object, object> itemAction;
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator GetEnumerator()
        {
            prefixAction();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                preItemAction(item);
                yield return itemAction(item);
                postItemAction(item);
            }
            postfixAction();
        }
    }
    
    public class PatchGameDataLoader
    {

        public static void Postfix(GameDataLoader __instance, ref IEnumerator __result)
        {
            Action prefixAction = () =>
            {

            };
            Action postfixAction = () =>
            {
                MH_Helper helper = MH_Helper.Instance;
                Data data = Data.instance;
                
                if (MH_Helper.FirstTime)
                {
                    helper.data = data;
                }

                Data.instance.agencyTraits = helper.data.agencyTraits;
                Data.instance.astronautTraits = helper.data.astronautTraits;
                Data.instance.baseLayoutAssets = helper.data.baseLayoutAssets;
                Data.instance.blueprints = helper.data.blueprints;
                Data.instance.constructionTraits = helper.data.constructionTraits;
                Data.instance.contractors = helper.data.contractors;
                Data.instance.creditData = helper.data.creditData;
                Data.instance.customResources = helper.data.customResources;
                Data.instance.eventImages = helper.data.eventImages;
                Data.instance.eventPacks = helper.data.eventPacks;
                Data.instance.historicalEvents = helper.data.historicalEvents;
                Data.instance.installations = helper.data.installations;
                Data.instance.json = helper.data.json;
                Data.instance.launchEvents = helper.data.launchEvents;
                Data.instance.milestoneChallenges = helper.data.milestoneChallenges;
                Data.instance.milestones = helper.data.milestones;
                Data.instance.missionEvents = helper.data.missionEvents;
                Data.instance.missionRules = helper.data.missionRules;
                Data.instance.missionTemplates = helper.data.missionTemplates;
                Data.instance.missionTrainings = helper.data.missionTrainings;
                Data.instance.modules = helper.data.modules;
                Data.instance.payloads = helper.data.payloads;
                Data.instance.researchRework = helper.data.researchRework;
                Data.instance.rules = helper.data.rules;
                Data.instance.scenarios = helper.data.scenarios;
                Data.instance.techTrees = helper.data.techTrees;
                Data.instance.vehicleParts = helper.data.vehicleParts;
                Data.instance.vehicleUpgrades = helper.data.vehicleUpgrades;
                Data.instance.version = helper.data.version;

                MH_Helper.FirstTime = false;
            };
            Action<object> preItemAction = (item) =>
            {

            };
            Action<object> postItemAction = (item) =>
            {

            };
            Func<object, object> itemAction = (item) =>
            {
                return item;
            };

            var enumerator = new SimpleEnumerator()
            {
                enumerator = __result,
                prefixAction = prefixAction,
                postfixAction = postfixAction,
                preItemAction = preItemAction,
                postItemAction = postItemAction,
                itemAction = itemAction
            };
            __result = enumerator.GetEnumerator();
        }
    }
}