using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HutongGames.PlayMaker;
using Modding;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;


namespace Kronk
{
    public class Kronk : Mod, ILocalSettings<KronkSettings>, IGlobalSettings<GlobalSettings>, IMenuMod
    {
        internal static Kronk instance;

        public static KronkSettings localSettings { get; set; } = new KronkSettings();
        public void OnLoadLocal(KronkSettings s) => localSettings = s;
        public KronkSettings OnSaveLocal() => localSettings;

        public static GlobalSettings globalSettings { get; set; } = new GlobalSettings();
        public void OnLoadGlobal(GlobalSettings s) => globalSettings = s;
        public GlobalSettings OnSaveGlobal() => globalSettings;

        #region Menu
        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return new List<IMenuMod.MenuEntry>()
            {
                new IMenuMod.MenuEntry
                {
                    Name = "Counting:",
                    Description = string.Empty,
                    Values = Enum.GetNames(typeof(CountingMode)),
                    Saver = opt => globalSettings.countingMode = (CountingMode)opt,
                    Loader = () => (int)globalSettings.countingMode,
                }
            };
        }
        public bool ToggleButtonInsideMenu => false;
        #endregion





        // TODO: make this a separate global setting, ideally toggleable in-game
        public enum CountingMode
        {
            Levers = 0,
            Rocks
        }


        public override void Initialize()
        {
            instance = this;

            LeverCount.Hook();
            RockCount.Hook();

            CanvasUtil.CreateFonts();

            Display.Hook();
        }




        // TODO: Move this out of here
        // Setting the Hunter's Mark playerdata for 0.1s so that Livesplit has a chance to autosplit on the last lever
        internal static void SendMessageToLivesplit()
        {
            IEnumerator toggleMark()
            {
                bool temp = PlayerData.instance.killedHunterMark;
                PlayerData.instance.SetBool(nameof(PlayerData.killedHunterMark), true);
                yield return new WaitForSeconds(0.1f);
                PlayerData.instance.SetBool(nameof(PlayerData.killedHunterMark), temp);
            }
            GameManager.instance.StartCoroutine(toggleMark());
        }

        public override string GetVersion()
        {
            return "0.3.1(Rocks)";
        }

    }
}
