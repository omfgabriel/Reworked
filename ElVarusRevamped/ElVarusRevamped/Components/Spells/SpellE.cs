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

        public SpellE()
        {
            this.SpellObject.SetSkillshot(0.25f, 250f, 1500f, false, SkillshotType.SkillshotCircle);
        }

        #region Properties

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal override TargetSelector.DamageType DamageType => TargetSelector.DamageType.Physical;

        /// <summary>
        ///     Gets Aoe
        /// </summary>
        internal override bool Aoe => true;

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

                var spellQ = new SpellQ();
                if (spellQ.SpellObject.IsCharging)
                {
                    return;
                }

                var target = Misc.GetTarget(this.SpellObject.Range + this.SpellObject.Width, this.DamageType);
                var targets = HeroManager.Enemies.FirstOrDefault(x => Misc.GetWStacks(x) > 0);
                if (targets != null)
                {
                    target = targets;
                    Program.Orbwalker.ForceTarget(target);
                    Logging.AddEntry(LoggingEntryTrype.Info, "@SpellE.cs: Target with W passive - {0}", target.ChampionName);
                }

                if (target != null)
                {
                    var prediction = this.SpellObject.GetPrediction(target);
                    if (prediction.Hitchance >= HitChance.VeryHigh)
                    {
                        if(prediction.CastPosition.Equals(Vector3.Zero))
                        {
                            return;
                        }

                        this.SpellObject.Cast(prediction.UnitPosition); 
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellE.cs: Can not run OnCombo - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     The on last hit callback.
        /// </summary>
        internal override void OnLastHit()
        {
            Console.WriteLine("Last");
        }

        /// <summary>
        ///     The on lane clear callback.
        /// </summary>
        internal override void OnLaneClear()
        {
            Console.WriteLine("Last");
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