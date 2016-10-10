namespace ElVarusRevamped.Components.Spells
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using ElVarusRevamped.Enumerations;
    using ElVarusRevamped.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    /// <summary>
    ///     The spell Q.
    /// </summary>
    internal class SpellQ : ISpell
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        internal SpellQ()
        {
            this.SpellObject.SetCharged("VarusQ", "VarusQ", 250, 1700, 1.5f);
        }

        #region Properties

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal override TargetSelector.DamageType DamageType => TargetSelector.DamageType.Physical;

        /// <summary>
        ///     Gets the delay.
        /// </summary>
        internal override float Delay => 250f;

        /// <summary>
        ///     Gets the range.
        /// </summary>
        internal override float Range => 925f; // Max range 1700

        /// <summary>
        ///     Gets or sets the skillshot type.
        /// </summary>
        internal override SkillshotType SkillshotType => SkillshotType.SkillshotLine;

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        internal override float Speed => 1800f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.Q;

        /// <summary>
        ///     Gets the width.
        /// </summary>
        internal override float Width => 70f;

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

                var target = Misc.GetTarget(this.SpellObject.ChargedMaxRange + this.Width * 1.1f, this.DamageType);
                if (target != null)
                {
                    Logging.AddEntry(LoggingEntryTrype.Info, "@SpellQ.cs: Target - {0}", target.ChampionName);

                    if (this.SpellObject.IsCharging || this.SpellObject.IsKillable(target) || target.Distance(ObjectManager.Player) > Orbwalking.GetRealAutoAttackRange(target) * 1.2f || MyMenu.RootMenu.Item("comboqalways").IsActive() || Misc.GetWStacks(target) >= MyMenu.RootMenu.Item("combow.count").GetValue<Slider>().Value)
                    {
                        if (!this.SpellObject.IsCharging)
                        {
                            this.SpellObject.StartCharging();
                        }
                            
                        if (this.SpellObject.IsCharging || this.MaxRangeQ())
                        {
                            var prediction = this.SpellObject.GetPrediction(target);
                            if (prediction.Hitchance >= HitChance.VeryHigh)
                            {
                                this.SpellObject.Cast(prediction.CastPosition);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellQ.cs: Can not run OnCombo - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     Gets the Q
        /// </summary>
        /// <returns></returns>
        internal bool MaxRangeQ()
        {
            return this.SpellObject.ChargedMaxRange - this.SpellObject.Range < 200;
        }

        /// <summary>
        ///     The on mixed callback.
        /// </summary>
        internal override void OnMixed()
        {
            this.OnCombo();
        }

        /// <summary>
        ///     The on last hit callback.
        /// </summary>
        internal override void OnLastHit()
        {
            var minion =
                MinionManager.GetMinions(this.SpellObject.ChargedMaxRange)
                    .Where(obj => this.SpellObject.IsKillable(obj))
                    .MinOrDefault(obj => obj.Health);

            if (minion != null)
            {
                if (!this.SpellObject.IsCharging)
                {
                    this.SpellObject.StartCharging();
                }

                if (this.SpellObject.IsCharging)
                {
                    if (MyMenu.RootMenu.Item("lasthit.mode").GetValue<StringList>().SelectedIndex == 0)
                    {
                        this.SpellObject.Cast(minion);
                    }
                    else
                    {
                        if (Vector3.Distance(minion.ServerPosition, ObjectManager.Player.ServerPosition)
                            > Orbwalking.GetRealAutoAttackRange(ObjectManager.Player)
                            && ObjectManager.Player.Distance(minion) <= this.Range)
                        {
                            this.SpellObject.Cast(minion);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     The on lane clear callback.
        /// </summary>
        internal override void OnLaneClear()
        {
            var minions = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, this.SpellObject.Range + this.SpellObject.Width);
            var minion = this.SpellObject.GetCircularFarmLocation(minions, this.SpellObject.Width);
            var minionsHit = minion.MinionsHit;

            if (minions != null && minionsHit >= MyMenu.RootMenu.Item("lasthit.count").GetValue<Slider>().Value)
            {
                if (!this.SpellObject.IsCharging)
                {
                    this.SpellObject.StartCharging();
                }

                if (this.SpellObject.IsCharging)
                {
                    this.SpellObject.Cast(minion.Position);
                }
            }
        }

        /// <summary>
        ///     The on jungle clear callback.
        /// </summary>
        internal override void OnJungleClear()
        {
            var minion =
                MinionManager.GetMinions(ObjectManager.Player.ServerPosition, this.Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth)
                .MinOrDefault(obj => obj.MaxHealth);

            if (minion != null)
            {
                this.SpellObject.Cast(minion);
            }
        }

        #endregion
    }
}