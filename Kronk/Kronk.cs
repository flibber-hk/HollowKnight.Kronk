using System;
using System.Collections.Generic;
using Modding;


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
                    Name = "Show Counter:",
                    Description = string.Empty,
                    Values = new string[]{ "True", "False" },
                    Saver = opt => globalSettings.displayCounter = opt == 0,
                    Loader = () => globalSettings.displayCounter ? 0 : 1,
                },
                new IMenuMod.MenuEntry
                {
                    Name = "Counting:",
                    Description = string.Empty,
                    Values = Enum.GetNames(typeof(CountingMode)),
                    Saver = opt => globalSettings.countingMode = (CountingMode)opt,
                    Loader = () => (int)globalSettings.countingMode,
                },

                new IMenuMod.MenuEntry
                {
                    Name = "Counter Position:",
                    Description = string.Empty,
                    Values = Enum.GetNames(typeof(CounterPosition)),
                    Saver = opt => globalSettings.counterPosition = (CounterPosition)opt,
                    Loader = () => (int)globalSettings.counterPosition,
                }
            };
        }
        public bool ToggleButtonInsideMenu => false;
        #endregion




        public override void Initialize()
        {
            Log("Initializing...");
            instance = this;

            Counters.LeverCount.Hook();
            Counters.RockCount.Hook();
            Counters.TotemCount.Hook();
            Counters.CompletionCount.Hook();

            Display.Hook();

            if (ModHooks.GetMod("FStatsMod") is not null)
            {
                Interop.FStatsInterop.HookFStats();
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();
    }
}
