using Modding;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Kronk.Counters
{
    public static class CompletionCount
    {

        public static void Hook()
        {
            Kronk.instance.Log("Hooking Completion Count...");
            On.PlayerData.SetBool += CountCompletion;
        }

        private static void CountCompletion(On.PlayerData.orig_SetBool orig, PlayerData self, string boolName, bool value)
        {
            orig(self, boolName, value);
            if (Kronk.globalSettings.countingMode == CountingMode.Completion)
            {
                GameManager.instance.CountGameCompletion();
                Display.UpdateText();
            }
        }
    }
}
