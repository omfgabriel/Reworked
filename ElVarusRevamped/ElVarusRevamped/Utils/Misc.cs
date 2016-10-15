namespace ElVarusRevamped.Utils
{
    using System;
    using System.Linq;

    using ElVarusRevamped.Components.Spells;
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
        ///     Spell Q.
        /// </summary>
        public static SpellQ SpellQ;

        /// <summary>
        ///     Spell E.
        /// </summary>
        public static SpellE SpellE;

        /// <summary>
        ///     Gets the target W stacks
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static int GetWStacks(Obj_AI_Base target)
        {
            try
            {
                return target.GetBuffCount("varuswdebuff");
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@Misc.cs: Can not return target - {0}", e);
                throw;
            }
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