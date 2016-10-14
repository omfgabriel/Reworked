namespace ElTahmKench.Components.Spells
{
    using System;
    using System.Linq;

    using ElTahmKench.Enumerations;
    using ElTahmKench.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    /// <summary>
    ///     The spell E.
    /// </summary>
    internal class SpellE : ISpell
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal override TargetSelector.DamageType DamageType => TargetSelector.DamageType.Magical;

        /// <summary>
        ///     Gets the delay.
        /// </summary>
        internal override float Delay => 0f;

        /// <summary>
        ///     Gets the range.
        /// </summary>
        internal override float Range => 0f;

        /// <summary>
        ///     Gets or sets the skillshot type.
        /// </summary>
        internal override SkillshotType SkillshotType => SkillshotType.SkillshotLine;

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        internal override float Speed => 0f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.E;

        /// <summary>
        ///     Gets the width.
        /// </summary>
        internal override float Width => 0f;

        #endregion

        #region Methods

        /// <summary>
        ///     The on combo callback.
        /// </summary>
        internal override void OnCombo()
        {
            try
            {
                var target = Misc.GetTarget(this.Range, this.DamageType);
                if (target != null)
                {
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@SpellE.cs: Can not run OnCombo - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     The on mixed callback.
        /// </summary>
        internal override void OnMixed()
        {
            this.OnCombo();
        }

        #endregion
    }
}