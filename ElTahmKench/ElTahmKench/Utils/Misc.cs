namespace ElTahmKench.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ElTahmKench.Enumerations;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The misc.
    /// </summary>
    internal static class Misc
    {
        #region Methods

        /// <summary>
        ///     The last devoured target type
        /// </summary>
        internal static DevourType LastDevouredType;

        /// <summary>
        /// 
        /// </summary>
        internal static string DevouredBuffName = "tahmkenchwhasdevouredtarget";

        /// <summary>
        /// 
        /// </summary>
        internal static string DevouredCastBuffName = "tahmkenchwdevoured";

        /// <summary>
        ///     Player has the devoured buff.
        /// </summary>
        internal static bool HasDevouredBuff => ObjectManager.Player.HasBuff(DevouredBuffName);

        /// <summary>
        ///     Gets the buff indexes handled.
        /// </summary>
        /// <value>
        ///     The buff indexes handled.
        /// </value>
        internal static Dictionary<int, List<int>> BuffIndexesHandled { get; } = new Dictionary<int, List<int>>();

        /// <summary>
        ///     The buffs types to devourer an enemy.
        /// </summary>
        public static BuffType[] DevourerBuffTypes = new[]
        {
            BuffType.Charm, BuffType.Flee,
            BuffType.Polymorph, BuffType.Silence, BuffType.Snare, BuffType.Stun,
            BuffType.Taunt
        };

        /// <summary>
        ///     Gets the passive stacks.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>
        ///     <see cref="GetPassiveStacks" />
        /// </returns>
        internal static int GetPassiveStacks(Obj_AI_Hero target) => target.GetBuffCount("tahmkenchpdebuffcounter");

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
                Logging.AddEntry(LoggingEntryType.Error, "@Misc.cs: Can not return target - {0}", e);
                throw;
            }
        }

        #endregion
    }
}