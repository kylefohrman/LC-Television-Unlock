using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LC_Television_Unlock
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class LCTelevisionUnlockBase : BaseUnityPlugin
    {
        private const string modGUID = "Robopirate.LCTelevisionUnlock";
        private const string modName = "LCTelevisionUnlock";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static LCTelevisionUnlockBase Instance;

        internal static ManualLogSource logger;

        public static ConfigEntry<bool> Television { get; private set; }

        void Awake()
        {
            logger = Logger;

            logger.LogInfo($"[{modName}] has awoken :)");

            TelevisionConfigManager.Init(Config);

            harmony.PatchAll(typeof(LCTelevisionUnlockBase));
            harmony.PatchAll(typeof(NewSaveStartPatch));

            logger.LogInfo($"Plugin {modGUID} is loaded!");
        }
    }
}
