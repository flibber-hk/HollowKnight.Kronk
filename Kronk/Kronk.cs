using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HutongGames.PlayMaker;
using Modding;
using Kronk.Randomizer;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Kronk
{
    public class Kronk : Mod
    {


        internal static Kronk instance;

        public KronkSettings Settings = new KronkSettings();
        public override ModSettings SaveSettings 
        {
            get => Settings;
            set => Settings = (KronkSettings)value;
        }


        // TODO: make this a separate global setting, ideally toggleable in-game
        public enum CountingMode
        {
            Levers = 0,
            Rocks
        }
        
        public CountingMode countingMode;


        public override void Initialize()
        {
            instance = this;

            LeverCount.Hook();
            RockCount.Hook();

            CanvasUtil.CreateFonts();

            Display.Hook();

            countingMode = CountingMode.Rocks;
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
