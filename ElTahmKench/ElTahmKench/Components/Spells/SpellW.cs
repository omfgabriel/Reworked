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
        internal override float Range => 250f;

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
        internal override SpellSlot SpellSlot => SpellSlot.W;

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
                if (this.SpellObject == null)
                {
                    return;
                }

                // todo: Adjust Range 250 to devourer and x spit range.
                var target = Misc.GetTarget(this.Range, this.DamageType);
                if (target != null)
                {
                    // todo : If the range is a little bit bigger then the attack range, option to force orbwalker to walk to target.
                    if (Misc.GetPassiveStacks(target) == 3 && Orbwalking.InAutoAttackRange(target))
                    {
                        this.SpellObject.CastOnUnit(target);
                    }

                    // todo : Move this code to the right place.
                    // todo : Check for Elise spiderlings etc, do not eat them while chasing enemy.
                    // Get the minions in range when the target is out of autoattackrange + a little bit.
                    var minion =
                        MinionManager.GetMinions(this.Range, team: MinionTeam.NotAlly)
                            .OrderBy(obj => obj.Distance(ObjectManager.Player.ServerPosition))
                            .FirstOrDefault();
                    // check if there are any minions.
                    if (minion != null)
                    {
                        // Cast W on the minion
                        // todo: OnbuffAdd set to Misc.LastDevouredType = DevourType.Minion.
                        this.SpellObject.CastOnUnit(minion);
                        // Check if there is indeed a minion eaten.
                        if (Misc.HasDevouredBuff) // Maybe this should be a little different after OnBuffAdd/OnBuffRemove
                        {
                            // todo : Prediction options.
                            // Spit the minion to the target location.
                            this.SpellObject.Cast(target);
                        }
                    }
                    else
                    {
                        Logging.AddEntry(LoggingEntryType.Debug, "@SpellW.cs: There is no minion in range");
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
        }

        /// <summary>
        ///     The on lane clear callback.
        /// </summary>
        internal override void OnLaneClear()
        {
        }

        /// <summary>
        ///     The on jungle clear callback.
        /// </summary>
        internal override void OnJungleClear()
        {
        }

        #endregion
    }
}