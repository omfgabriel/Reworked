namespace ElTahmKench.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ElTahmKench.Components.Spells;
    using ElTahmKench.Enumerations;
    using ElTahmKench.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using LeagueSharp.Data;
    using LeagueSharp.Data.DataTypes;
    
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

        /// <summary>
        ///     Gets or sets the leaguesharp.data spells.
        /// </summary>
        /// <value>
        ///     The spells.
        /// </value>
        private List<SpellDatabaseEntry> Spells { get; } = new List<SpellDatabaseEntry>();

        /// <summary>
        ///     Gets or sets the champion spells.
        /// </summary>
        private Dictionary<string, List<SpellSlot>> ChampionSpells { get; } = new Dictionary<string, List<SpellSlot>>();


        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpellManager" /> class.
        /// </summary>
        internal SpellManager()
        {
            try
            {
                this.LoadSpells(new List<ISpell>() { new SpellQ(), new SpellW(), new SpellE() });

                var dangerousSpells = 
                    Data.Get<SpellDatabase>()
                    .Spells.Where(
                        x => 
                        x.DangerValue >= 5 
                        && HeroManager.Enemies.Any(
                            y => x.ChampionName.Equals(y.ChampionName, StringComparison.InvariantCultureIgnoreCase)))
                    .Select(x => x.SpellName)
                    .ToList();

                foreach (var source in dangerousSpells)
                {
                    var spellName = source;
                    this.Spells.Add(Data.Get<SpellDatabase>().Spells.First(x => x.SpellName.Equals(source)));
                    if (!dangerousSpells.Contains(spellName))
                    {
                        continue;
                    }
                }

                foreach (var spell in this.Spells)
                {
                    if (!this.ChampionSpells.ContainsKey(spell.ChampionName))
                    {
                        this.ChampionSpells[spell.ChampionName] = new List<SpellSlot>();
                    }

                    this.ChampionSpells[spell.ChampionName].Add(spell.Slot);
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@SpellManager.cs: Can not initialize the spells - {0}", e);
                throw;
            }

            Game.OnUpdate += this.Game_OnUpdate;
            Obj_AI_Base.OnBuffAdd += this.OnBuffAdd;
            Obj_AI_Base.OnBuffRemove += this.OnBuffRemove;
            Obj_AI_Base.OnProcessSpellCast += this.ObjAiBaseOnProcessSpellCast;
        }

        /// <summary>
        ///     Fired when the game processes a spell cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        private void ObjAiBaseOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy)
            {
                return;
            }

            if (args.Target.IsEnemy || !args.Target.IsValid<Obj_AI_Hero>())
            {
                return;
            }

            // todo : Generate menu items to toggle spells [optional].
            if (!this.Spells.Any(x => x.SpellName.Equals(args.SData.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            var target = (Obj_AI_Hero)args.Target;
            var spellW = new SpellW();

            if (spellW.SpellSlot.IsReady() && ObjectManager.Player.Distance(target) < spellW.Range)
            {
                spellW.SpellObject.CastOnUnit(target);
            }
        }

        /// <summary>
        ///    The OnBuffRemove event.
        /// </summary>
        /// <param name="sender">
        ///     The sender
        /// </param>
        /// <param name="args">
        ///     The event data
        /// </param>
        private void OnBuffRemove(Obj_AI_Base sender, Obj_AI_BaseBuffRemoveEventArgs args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The OnBuffAdd event.
        /// </summary>
        /// <param name="sender">
        ///     The sender
        /// </param>
        /// <param name="args">
        ///     The event data
        /// </param>
        private void OnBuffAdd(Obj_AI_Base sender, Obj_AI_BaseBuffAddEventArgs args)
        {
            throw new NotImplementedException();
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

                if ((orbwalkerModeLower.Equals("lasthit")
                    && (spellSlotNameLower.Equals("e") || spellSlotNameLower.Equals("w")
                        || spellSlotNameLower.Equals("r"))) || (orbwalkerModeLower.Equals("laneclear") && (spellSlotNameLower.Equals("e"))))
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
                Logging.AddEntry(LoggingEntryType.Error, "@SpellManager.cs: Can not get spell active state for slot {0} - {1}", spellSlot.ToString(), e);
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
