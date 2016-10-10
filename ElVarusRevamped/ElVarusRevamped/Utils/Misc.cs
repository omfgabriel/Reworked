namespace ElVarusRevamped.Utils
{
    using System;
    using System.Linq;

    using ElVarusRevamped.Enumerations;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The misc.
    /// </summary>
    internal static class Misc
    {
        #region Methods

        /// <summary>
        ///     Gets the target W stacks
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static int GetWStacks(Obj_AI_Base target)
        {
            return target.GetBuffCount("varuswdebuff");
        }

        /// <summary>
        ///     Gets a target from the common target selector.
        /// </summary>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <param name="damageType">
        ///     The damage type.
        /// </param>
        /// <returns>
        ///     <see cref="Obj_AI_Hero" />
        /// </returns>
        internal static Obj_AI_Hero GetTarget(float range, TargetSelector.DamageType damageType)
        {
            try
            {
                return TargetSelector.GetTarget(range, damageType);
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@Misc.cs: Can not return target - {0}", e);
                throw;
            }
        }

        #endregion
    }
}