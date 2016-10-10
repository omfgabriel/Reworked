namespace ElVarusRevamped.Components.Spells
{
    using System;
    using System.Linq;

    using ElVarusRevamped.Enumerations;
    using ElVarusRevamped.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    /// <summary>
    ///     The spell Q.
    /// </summary>
    internal class SpellE : ISpell
    {

        #region Properties

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal override TargetSelector.DamageType DamageType => TargetSelector.DamageType.Physical;

        /// <summary>
        ///     Gets the delay.
        /// </summary>
        internal override float Delay => 1000f;

        /// <summary>
        ///     Gets the range.
        /// </summary>
        internal override float Range => 950f;

        /// <summary>
        ///     Gets or sets the skillshot type.
        /// </summary>
        internal override SkillshotType SkillshotType => SkillshotType.SkillshotCircle;

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        internal override float Speed => 1500f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.E;

        /// <summary>
        ///     Gets the width.
        /// </summary>
        internal override float Width => 235f;

        #endregion

        #region Methods

        /// <summary>
        ///     The on combo callback.
        /// </summary>
        internal override void OnCombo()
        {
            try
            {
                if (this.SpellObject == null)
                {
                    return;
                }

            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellE.cs: Can not run OnCombo - {0}", e);
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