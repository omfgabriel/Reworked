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
    ///     The spell W.
    /// </summary>
    internal class SpellW : ISpell
    {
        #region Properties

        public float minionWRange = 700f;

        /// <summary>
        ///     Gets a value indicating whether the combo mode is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if combo mode is active; otherwise, <c>false</c>.
        /// </value>
        public bool ComboModeActive => Orbwalking.Orbwalker.Instances.Any(x => x.ActiveMode == Orbwalking.OrbwalkingMode.Combo);

        /// <summary>
        ///     Gets or sets the damage type.
        /// </summary>
        internal override TargetSelector.DamageType DamageType => TargetSelector.DamageType.Magical;

        /// <summary>
        ///     Gets the delay.
        /// </summary>
        internal override float Delay => 0.5f;

        /// <summary>
        ///     Gets the range.
        /// </summary>
        internal override float Range => Misc.HasDevouredBuff && Misc.LastDevouredType == DevourType.Minion ? 700f : 250f;

        /// <summary>
        ///     Gets or sets the skillshot type.
        /// </summary>
        internal override SkillshotType SkillshotType => SkillshotType.SkillshotLine;

        /// <summary>
        ///     Gets the speed.
        /// </summary>
        internal override float Speed => 950f;

        /// <summary>
        ///     Gets the spell slot.
        /// </summary>
        internal override SpellSlot SpellSlot => SpellSlot.W;

        /// <summary>
        ///     Gets the width.
        /// </summary>
        internal override float Width => 75f;

        /// <summary>
        ///     Spell has collision.
        /// </summary>
        internal override bool Collision => true;

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
                    if (Misc.GetPassiveStacks(target) == 3 && ObjectManager.Player.Distance(target) <= this.Range)
                    {
                        if (target.IsValidTarget(this.Range))
                        {
                            this.SpellObject.CastOnUnit(target);
                        }
                    }
                    else
                    {
                        // test
                        if (MyMenu.RootMenu.Item("combominionuse").IsActive())
                        {
                            if (!target.IsValidTarget(this.Range + 400) || Misc.HasDevouredBuff) // 650
                            {
                                return;
                            }

                            // Get the minions in range
                            var minion = MinionManager.GetMinions(this.Range, team: MinionTeam.NotAlly).Where(n => !n.CharData.Name.ToLower().Contains("spiderling")).OrderBy(obj => obj.Distance(ObjectManager.Player.ServerPosition)).FirstOrDefault();
                            // check if there are any minions.
                            if (minion != null)
                            {
                                // Cast W on the minion.
                                this.SpellObject.CastOnUnit(minion);
                                // Check if player has the devoured buff and that the last devoured type is a minion.
                                if (Misc.HasDevouredBuff && Misc.LastDevouredType == DevourType.Minion)
                                {
                                    var prediction = this.SpellObject.GetPrediction(target);
                                    if (prediction.Hitchance >= HitChance.High)
                                    {
                                        // Spit the minion to the target location.
                                        this.SpellObject.Cast(prediction.CastPosition);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@SpellW.cs: Can not run OnCombo - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     The on update callback.
        /// </summary>
        internal override void OnUpdate()
        {
            if (!MyMenu.RootMenu.Item("allycc").IsActive() && !this.ComboModeActive)
            {
                return;
            }

            foreach (var ally in HeroManager.Allies.Where(a => a.IsValidTarget(this.Range + 100, false) && !a.IsMe))
            {
                foreach (var buff in
                    ally.Buffs.Where(
                        x =>
                            Misc.DevourerBuffTypes.Contains(x.Type) && x.Caster.Type == GameObjectType.obj_AI_Hero && x.Caster.IsEnemy))
                {
                    if (!MyMenu.RootMenu.Item($"buffscc{buff.Type}").IsActive() || !MyMenu.RootMenu.Item($"won{ally.ChampionName}").IsActive() || Misc.BuffIndexesHandled[ally.NetworkId].Contains(buff.Index))
                    {
                        continue;
                    }

                    Misc.BuffIndexesHandled[ally.NetworkId].Add(buff.Index);
                    this.SpellObject.CastOnUnit(ally);
                    Misc.BuffIndexesHandled[ally.NetworkId].Remove(buff.Index);
                }
            }
        }

        /// <summary>
        ///     The on mixed callback.
        /// </summary>
        internal override void OnMixed()
        {
            var target = Misc.GetTarget(this.minionWRange, this.DamageType);
            if (target != null)
            {
                var minion = MinionManager.GetMinions(this.Range, team: MinionTeam.NotAlly).OrderBy(obj => obj.Distance(ObjectManager.Player.ServerPosition)).FirstOrDefault();
                // check if there are any minions.
                if (minion != null)
                {
                    // Cast W on the minion.
                    this.SpellObject.CastOnUnit(minion);
                    // Check if player has the devoured buff and that the last devoured type is a minion.
                    if (Misc.HasDevouredBuff && Misc.LastDevouredType == DevourType.Minion)
                    {
                        var prediction = this.SpellObject.GetPrediction(target);
                        Logging.AddEntry(LoggingEntryType.Debug, "Hitchance {0}", prediction.Hitchance);
                        if (prediction.Hitchance >= HitChance.High)
                        {
                            // Spit the minion to the target location.
                            this.SpellObject.Cast(prediction.CastPosition);
                        }
                    }
                }
            }
        }

        #endregion
    }
}