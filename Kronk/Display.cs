using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Kronk
{
    internal static class Display
    {
        private static GameObject canvas;
        private static GameObject canvasText;

        public static void Create()
        {
            if (canvas != null) return;
            // Create base canvas
            canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920, 1080));
            Object.DontDestroyOnLoad(canvas);

            Vector2 position;
            switch (Kronk.globalSettings.counterPosition)
            {
                default:
                case CounterPosition.TopRight:
                    position = new Vector2(0.87f, 0.95f);
                    break;

                case CounterPosition.BottomLeft:
                    position = new Vector2(0.13f, 0.05f);
                    break;
                case CounterPosition.BottomRight:
                    position = new Vector2(0.87f, 0.05f);
                    break;
                case CounterPosition.AboveHUD:
                    position = new Vector2(0.13f, 0.95f);
                    break;
                case CounterPosition.BelowHUD:
                    position = new Vector2(0.13f, 0.75f);
                    break;
            }    


            canvasText = CanvasUtil.CreateTextPanel(canvas, $"0 hit", 24, TextAnchor.MiddleCenter,
                new CanvasUtil.RectData(new Vector2(200, 100), Vector2.zero,
                position, position));

            Show();
        }

        public static void Destroy()
        {
            if (canvas != null) Object.Destroy(canvas);
            canvas = null;
        }

        public static void UpdateText()
        {
            if (canvas == null) Create();
            if (canvasText == null) return;

            switch (Kronk.globalSettings.countingMode)
            {
                case CountingMode.Levers:
                    string leverOrLevers = "Lever" + (Kronk.localSettings.LeversHit == 1 ? "" : "s");

                    canvasText.GetComponent<UnityEngine.UI.Text>().text = $"{Kronk.localSettings.LeversHit} {leverOrLevers}";

                    if (Kronk.localSettings.LeversHit >= Counters.LeverCount.NUMOBJECTS)
                    {
                        canvasText.GetComponent<UnityEngine.UI.Text>().color = Color.yellow;
                    }
                    break;

                case CountingMode.Rocks:
                    string rockOrRocks = "Rock" + (Kronk.localSettings.RocksBroken == 1 ? "" : "s");

                    canvasText.GetComponent<UnityEngine.UI.Text>().text = $"{Kronk.localSettings.RocksBroken} {rockOrRocks}";

                    if (Kronk.localSettings.RocksBroken >= Counters.RockCount.NUMOBJECTS)
                    {
                        canvasText.GetComponent<UnityEngine.UI.Text>().color = Color.yellow;
                    }
                    break;
            }

        }


        public static void Show()
        {
            if (canvas == null) return;
            canvas.SetActive(true);
        }

        public static void Hide()
        {
            if (canvas == null) return;
            canvas.SetActive(false);
        }

        internal static void Hook()
        {
            UnHook();

            ModHooks.AfterSavegameLoadHook += OnLoad;
            On.QuitToMenu.Start += OnQuitToMenu;
            On.InvAnimateUpAndDown.AnimateUp += OnInventoryOpen;
            On.InvAnimateUpAndDown.AnimateDown += OnInventoryClose;
            On.UIManager.GoToPauseMenu += OnPause;
            On.UIManager.UIClosePauseMenu += OnUnpause;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ShowLeverCount;
        }

        internal static void UnHook()
        {
            ModHooks.AfterSavegameLoadHook -= OnLoad;
            On.QuitToMenu.Start -= OnQuitToMenu;
            On.InvAnimateUpAndDown.AnimateUp -= OnInventoryOpen;
            On.InvAnimateUpAndDown.AnimateDown -= OnInventoryClose;
            On.UIManager.GoToPauseMenu -= OnPause;
            On.UIManager.UIClosePauseMenu -= OnUnpause;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= ShowLeverCount;
        }

        private static void ShowLeverCount(Scene arg0, Scene arg1)
        {
            if (arg0.name == "Tutorial_01" && arg1.name == "Town") UpdateText();
        }

        private static void OnLoad(SaveGameData data)
        {
            UpdateText();
        }

        private static IEnumerator OnQuitToMenu(On.QuitToMenu.orig_Start orig, QuitToMenu self)
        {
            Destroy();
            return orig(self);
        }

        private static void OnInventoryOpen(On.InvAnimateUpAndDown.orig_AnimateUp orig, InvAnimateUpAndDown self)
        {
            orig(self);
            Hide();
        }

        private static void OnInventoryClose(On.InvAnimateUpAndDown.orig_AnimateDown orig, InvAnimateUpAndDown self)
        {
            orig(self);
            Show();
        }

        private static IEnumerator OnPause(On.UIManager.orig_GoToPauseMenu orig, UIManager self)
        {
            Destroy();
            return orig(self);
        }

        private static void OnUnpause(On.UIManager.orig_UIClosePauseMenu orig, UIManager self)
        {
            orig(self);
            UpdateText();
        }
    }
}
