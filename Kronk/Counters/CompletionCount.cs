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
            On.HeroController.FixedUpdate += CountPercentage;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += Show_Compltio_Without_DreamNail;
            

        }

        private static void Show_Compltio_Without_DreamNail(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Cinematic_Ending_A")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Cinematic_Ending_B");
            }
        }

        private static void CountPercentage(On.HeroController.orig_FixedUpdate orig, HeroController self)
        {
            orig(self);
            GameManager.instance.CountGameCompletion();
            float completion = GameManager.instance.GetPlayerDataFloat("completionPercentage");
            if (completion == Kronk.localSettings.completionCount) return;
            Kronk.localSettings.completionCount = completion;
            Display.UpdateText();


        }
    }
}
