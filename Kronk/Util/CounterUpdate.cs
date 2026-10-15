using System.Collections;
using UnityEngine;

namespace Kronk.Util
{
    public class CounterUpdate
    {
        private readonly int MAXOBJECTS;

        public CounterUpdate(int mAXOBJECTS)
        {
            MAXOBJECTS = mAXOBJECTS;
        }

        internal void CounterUpdated(int currentNum)
        {
            Display.UpdateText();

            PlayerData.instance.ghostCoins = currentNum;

            if (currentNum == MAXOBJECTS)
            {
                SendMessageToLivesplit();
            }
        }

        // Setting the Hunter's Mark playerdata for 0.1s so that Livesplit has a chance to autosplit on the last lever
        private static void SendMessageToLivesplit()
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
    }
}
