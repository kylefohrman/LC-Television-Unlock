using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC_Television_Unlock
{
    internal class TelevisionConfigManager
    {
        public static TelevisionConfigManager Instance { get; private set; }

        public static void Init(ConfigFile config)
        {
            Instance = new TelevisionConfigManager(config);
        }

        public static ConfigEntry<bool> Television { get; private set; }

        private TelevisionConfigManager(ConfigFile config)
        {
            Television = config.Bind("Decorations", "Television", true, "Unlock the television on new save.");
        }
    }
}
