using System;
using HutongGames.PlayMaker;

namespace Kronk.Util
{
    public static class PlayMakerExtensions
    {
        public static FsmState GetState(this PlayMakerFSM self, string name)
        {
            return self.Fsm.GetState(name);
        }

        public static void AddFirstAction(this FsmState self, FsmStateAction action)
        {
            FsmStateAction[] actions = new FsmStateAction[self.Actions.Length + 1];
            Array.Copy(self.Actions, 0, actions, 1, self.Actions.Length);
            actions[0] = action;

            self.Actions = actions;
            action.Init(self);
        }
    }
}
