namespace ElVarusRevamped.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ElVarusRevamped.Components.Spells;
    using ElVarusRevamped.Enumerations;
    using ElVarusRevamped.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    /// <summary>
    ///     The spell manager.
    /// </summary>
    internal class SpellManager
    {
        #region Fields

        /// <summary>
        ///     The spells.
        /// </summary>
        private readonly List<ISpell> spells = new List<ISpell>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpellManager" /> class.
        /// </summary>
        internal SpellManager()
        {
            try
            {
                this.LoadSpells(new List<ISpell>() { new SpellQ(), new SpellE(), new SpellR() });
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellManager.cs: Can not initialize the spells - {0}", e);
                throw;
            }

            Game.OnUpdate += this.Game_OnUpdate;
            AntiGapcloser.OnEnemyGapcloser += this.OnEnemyGapcloser;
        }


        /// <summary>
        ///     
        /// </summary>
        /// <param name="gapcloser"></param>
        private void OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            try
            {
                var qSpell = new SpellQ();
                if (qSpell.SpellObject.IsCharging)
                {
                    return;
                }

                if (MyMenu.RootMenu.Item("gapclosereuse").IsActive())
                {
                    var eSpell = new SpellE();
                    if (!gapcloser.Sender.IsValidTarget(eSpell.Range))
                    {
                        return;
                    }

                    if (eSpell.SpellSlot.IsReady())
                    {
                        eSpell.SpellObject.Cast(gapcloser.End);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellManager.cs: AntiGapcloser - {0}", e);
                throw;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The is the spell active method.
        /// </summary>
        /// <param name="spellSlot">
        ///     The spell slot.
        /// </param>
        /// <param name="orbwalkingMode">
        ///     The orbwalking mode.
        /// </param>
        /// <returns>
        ///     <see cref="bool" />
        /// </returns>
        private static bool IsSpellActive(SpellSlot spellSlot, Orbwalking.OrbwalkingMode orbwalkingMode)
        {
            if (Program.Orbwalker.ActiveMode != orbwalkingMode || !spellSlot.IsReady())
            {
                return false;
            }

            try
            {
                var orbwalkerModeLower = Program.Orbwalker.ActiveMode.ToString().ToLower();
                var spellSlotNameLower = spellSlot.ToString().ToLower();

                // Fixing this later
                if ((orbwalkerModeLower.Equals("lasthit")
                    && (spellSlotNameLower.Equals("w")
                        || spellSlotNameLower.Equals("r") || spellSlotNameLower.Equals("e"))) || (orbwalkerModeLower.Equals("laneclear") && (spellSlotNameLower.Equals("r")))
                        || (orbwalkerModeLower.Equals("mixed") && (spellSlotNameLower.Equals("r"))))
                {
                    return false;
                }

                return MyMenu.RootMenu.Item(orbwalkerModeLower + spellSlotNameLower + "use").GetValue<bool>()
                       && MyMenu.RootMenu.Item(orbwalkerModeLower + spellSlotNameLower + "mana")
                              .GetValue<Slider>()
                              .Value <= ObjectManager.Player.ManaPercent;
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellManager.cs: Can not get spell active state for slot {0} - {1}", spellSlot.ToString(), e);
                throw;
            }
        }

        /// <summary>
        ///     The game on update callback.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        private void Game_OnUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead || MenuGUI.IsChatOpen || MenuGUI.IsShopOpen) return;

            this.spells.Where(spell => IsSpellActive(spell.SpellSlot, Orbwalking.OrbwalkingMode.Combo))
                .ToList()
                .ForEach(spell => spell.OnCombo());

            this.spells.Where(spell => IsSpellActive(spell.SpellSlot, Orbwalking.OrbwalkingMode.LaneClear))
                .ToList()
                .ForEach(spell => spell.OnLaneClear());

            this.spells.Where(spell => IsSpellActive(spell.SpellSlot, Orbwalking.OrbwalkingMode.LaneClear))
               .ToList()
               .ForEach(spell => spell.OnJungleClear());

            this.spells.Where(spell => IsSpellActive(spell.SpellSlot, Orbwalking.OrbwalkingMode.LastHit))
                .ToList()
                .ForEach(spell => spell.OnLastHit());

            this.spells.Where(spell => IsSpellActive(spell.SpellSlot, Orbwalking.OrbwalkingMode.Mixed))
                .ToList()
                .ForEach(spell => spell.OnMixed());

            this.spells.ToList().ForEach(spell => spell.OnUpdate());
        }

        /// <summary>
        ///     The killsteal method.
        ///     Disabled for now, need to fix stuff first.
        /// </summary>
        private void KillstealSpells()
        {
            var spellQ = new SpellQ();
            var spellE = new SpellE();

            if (MyMenu.RootMenu.Item("killstealquse").IsActive())
            {
                var killableEnemy =
                    HeroManager.Enemies.FirstOrDefault(e => spellQ.SpellObject.IsKillable(e) && e.IsValidTarget(spellQ.SpellObject.ChargedMaxRange)
                                && ObjectManager.Player.ManaPercent
                                > MyMenu.RootMenu.Item("killstealqmana").GetValue<Slider>().Value);

                if (killableEnemy != null)
                {
                    if (!spellQ.SpellObject.IsCharging)
                    {
                        spellQ.SpellObject.StartCharging();
                    }

                    if (spellQ.SpellObject.IsCharging)
                    {
                        spellQ.SpellObject.Cast(killableEnemy);
                    }
                }
            }

            if (MyMenu.RootMenu.Item("killstealeuse").IsActive())
            {
                if (spellQ.SpellObject.IsCharging)
                {
                    return;
                }

                var killableEnemy =
                    HeroManager.Enemies.FirstOrDefault(e => spellE.SpellObject.IsKillable(e) && e.IsValidTarget(spellE.Range + spellE.Width)
                            && ObjectManager.Player.ManaPercent
                            > MyMenu.RootMenu.Item("killstealemana").GetValue<Slider>().Value);

                if (killableEnemy != null)
                {
                    spellE.SpellObject.Cast(killableEnemy.Position);
                }
            }
        }

        /// <summary>
        ///     The load spells method.
        /// </summary>
        /// <param name="spellList">
        ///     The spells.
        /// </param>
        private void LoadSpells(IEnumerable<ISpell> spellList)
        {
            foreach (var spell in spellList)
            {
                MyMenu.GenerateSpellMenu(spell.SpellSlot);
                this.spells.Add(spell);
            }
        }

        #endregion
    }
}
