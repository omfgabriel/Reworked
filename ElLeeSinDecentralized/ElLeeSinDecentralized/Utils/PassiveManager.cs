namespace ElLeeSinDecentralized.Utils
{
    using System;
    using LeagueSharp;

    internal class PassiveManager
    {
        /// <summary>
        ///     The Flurry stacks.
        /// </summary>
        internal static int FlurryStacks;

        /// <summary>
        ///     Called on buff add.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void Obj_AI_Base_OnBuffAdd(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.Buff.DisplayName.Equals(Misc.BlindMonkFlurry, StringComparison.InvariantCultureIgnoreCase))
            {
                FlurryStacks = 2;
            }
        }

        /// <summary>
        ///     Called on buff remove.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void Obj_AI_Base_OnBuffRemove(Obj_AI_Base sender, Obj_AI_BaseBuffRemoveEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.Buff.DisplayName.Equals(Misc.BlindMonkFlurry, StringComparison.InvariantCultureIgnoreCase))
            {
                FlurryStacks = 0;
            }
        }

        /// <summary>
        ///     Called on buff update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void Obj_AI_Base_OnBuffUpdateCount(Obj_AI_Base sender, Obj_AI_BaseBuffUpdateCountEventArgs args)
        {
            if (!sender.IsMe || !args.Buff.DisplayName.Equals(Misc.BlindMonkFlurry, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            FlurryStacks = args.Buff.Count;
        }
    }
}
