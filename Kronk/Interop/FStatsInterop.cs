using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kronk.Interop
{
    internal static class FStatsInterop
    {
        public static void HookFStats()
        {
            FStats.API.OnBuildExtensionStats += RecordKronkStats;
        }

        private static void RecordKronkStats(Action<string> addEntry)
        {
            if (Kronk.localSettings.LeversHit > 0)
            {
                addEntry($"{Kronk.localSettings.LeversHit} levers hit");
            }
            if (Kronk.localSettings.TotemCount > 0)
            {
                addEntry($"{Kronk.localSettings.TotemCount} totems hit");
            }
            if (Kronk.localSettings.RocksBroken > 0)
            {
                addEntry($"{Kronk.localSettings.RocksBroken} rocks broken");
            }
        }
    }
}
