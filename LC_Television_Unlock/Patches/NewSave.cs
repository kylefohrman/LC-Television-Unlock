using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC_Television_Unlock
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class NewSaveStartPatch
    {
        [HarmonyPatch("firstDayAnimation")]
        [HarmonyPostfix]
        internal static void LoadTelevisionFromConfig()
        {
            if (StartOfRound.Instance.gameStats.daysSpent == 0 && !StartOfRound.Instance.isChallengeFile)
            {
                LCTelevisionUnlockBase.logger.LogInfo("New save detected, loading TV from config.");
            }
            else
            {
                LCTelevisionUnlockBase.logger.LogInfo("Not a new save, not loading TV from config.");
                return;
            }

            // check if the player is a host, if not return
            if (!GameNetworkManager.Instance.isHostingGame) return;

            List<UnlockableItem> unlockablesList = StartOfRound.Instance.unlockablesList.unlockables;

            foreach(UnlockableItem unlockable in unlockablesList)
            {
                var unlockableName = unlockable.unlockableName;
                var unlockableID = unlockablesList.IndexOf(unlockable);

                var televisionResult = CheckConfig(unlockableID, unlockableName);

                if (televisionResult) break;
            }
        }

        private static bool CheckConfig(int unlockableID, string unlockableName)
        {
            if (unlockableName.Equals("Television"))
            {
                if (TelevisionConfigManager.Television.Value)
                {
                    LCTelevisionUnlockBase.logger.LogInfo($"Found Television with unlockableName {unlockableName} and ID {unlockableID}. Unlocking.");
                    UnlockShipItem(StartOfRound.Instance, unlockableID, unlockableName);
                }
                return true;
            }
            return false;
        }

        private static void UnlockShipItem(StartOfRound instance, int unlockableID, string name)
        {
            try
            {
                LCTelevisionUnlockBase.logger.LogInfo($"Attempting to unlock {name}");
                var unlockShipMethod = instance.GetType().GetMethod("UnlockShipObject",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                unlockShipMethod.Invoke(instance, new object[] { unlockableID });
                LCTelevisionUnlockBase.logger.LogInfo($"Spawning {name}");
            }
            catch (NullReferenceException ex)
            {
                LCTelevisionUnlockBase.logger.LogError($"Could not invoke UnlockShipObject method: {ex}");
            }
        }
    }
}
