﻿using System.Linq;
using BepInEx;
using HarmonyLib;
using mqKeezy_DisableTraitConflicts.Properties;

namespace mqKeezy_DisableTraitConflicts
{
    [BepInPlugin(ModInfo.BepInExPluginId, ModInfo.Title, ModInfo.Version)]
    public class MqkSorDisableTraitConflicts : BaseUnityPlugin
    {
        private static bool disabledTraitConflicts;

        private void Awake()
        {
            new Harmony(ModInfo.BepInExHarmonyPatchesId).PatchAll();
        }

        private class Patches
        {
            private class UnlocksPatch
            {
                [HarmonyPatch(typeof(Unlocks), methodName: "LoadInitialUnlocks")]
                private class LoadInitialUnlocks
                {
                    [HarmonyPostfix]
                    private static void Postfix()
                    {
                        if (disabledTraitConflicts)
                        {
                            return;
                        }

                        foreach (Unlock unlock in
                            GameController.gameController.sessionDataBig.unlocks.Where(predicate: unlock =>
                                unlock.unlockType ==
                                "Trait"))
                        {
                            unlock.cancellations.Clear();
                        }

                        disabledTraitConflicts = true;
                    }
                }
            }
        }
    }
}