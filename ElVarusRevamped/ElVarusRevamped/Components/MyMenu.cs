namespace ElVarusRevamped.Components
{
    using System;
    using System.Drawing;

    using ElVarusRevamped.Enumerations;
    using ElVarusRevamped.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Color = SharpDX.Color;

    /// <summary>
    ///     The my menu class.
    /// </summary>
    internal class MyMenu
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MyMenu" /> class.
        /// </summary>
        internal MyMenu()
        {
            RootMenu = new Menu("ElVarusRevamped", "elvarusrevamped", true).SetFontStyle(FontStyle.Bold, Color.BlueViolet);

            RootMenu.AddSubMenu(GetTargetSelectorNode());
            RootMenu.AddSubMenu(GetOrbwalkerNode());

            RootMenu.AddToMainMenu();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the root menu.
        /// </summary>
        internal static Menu RootMenu { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     The generate spell menu method.
        /// </summary>
        /// <param name="spellSlot">
        ///     The spell slot.
        /// </param>
        internal static void GenerateSpellMenu(SpellSlot spellSlot)
        {
            try
            {
                var spellSlotName = spellSlot.ToString();
                var spellSlotNameLower = spellSlotName.ToLower();

                var node = new Menu(spellSlot + " Settings", "spellmenu" + spellSlotNameLower);

                var nodeCombo = new Menu("Combo", spellSlotNameLower + "combomenu");
                {
                    nodeCombo.AddItem(new MenuItem("combo" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(true));
                    nodeCombo.AddItem(new MenuItem("combo" + spellSlotNameLower + "mana", "Min. Mana").SetValue(new Slider(5)));

                    if (spellSlotNameLower.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                    {
                        nodeCombo.AddItem(new MenuItem("comboqalways", "Always use " + spellSlotName).SetValue(true));
                        nodeCombo.AddItem(new MenuItem("combow.count", "Minimum W stacks").SetValue(new Slider(3, 1, 3)));
                    }

                    if (spellSlotNameLower.Equals("r", StringComparison.InvariantCultureIgnoreCase))
                    {
                        nodeCombo.AddItem(new MenuItem("combo.separator-2", String.Empty));
                        nodeCombo.AddItem(new MenuItem("combousersolo", "Use " + spellSlotName + " solo").SetValue(true));
                        nodeCombo.AddItem(new MenuItem("combor.count.solo", "Enemy min health solo").SetValue(new Slider(35)));
                        nodeCombo.AddItem(new MenuItem("combor.count.enemies", "Max enemies in range").SetValue(new Slider(2, 1, 5)));
                        nodeCombo.AddItem(new MenuItem("combo.separator", String.Empty));
                        nodeCombo.AddItem(new MenuItem("combousermultiple", "Use " + spellSlotName + " AoE").SetValue(true));
                        nodeCombo.AddItem(new MenuItem("combor.count", "Minimum enemies hit with R").SetValue(new Slider(3, 1, 5)));
                        nodeCombo.AddItem(new MenuItem("combor.r.radius", "R spread radius").SetValue(new Slider(500, 120, 600)));
                    }
                }

                node.AddSubMenu(nodeCombo);

                if (!spellSlotNameLower.Equals("r", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nodeMixed = new Menu("Mixed", spellSlotNameLower + "mixedmenu");
                    {
                        nodeMixed.AddItem(new MenuItem("mixed" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(true));
                        nodeMixed.AddItem(new MenuItem("mixed" + spellSlotNameLower + "mana", "Min. Mana").SetValue(new Slider(50)));

                        nodeMixed.AddItem(new MenuItem("mixed" + spellSlotNameLower + "usealways", "Always use " + spellSlotName).SetValue(false));
                        nodeMixed.AddItem(new MenuItem("mixed" + spellSlotNameLower + "usealways.count", "Minimum W stacks").SetValue(new Slider(3, 1, 3)));
                    }

                    node.AddSubMenu(nodeMixed);
                }

                if (spellSlotNameLower.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nodeLastHit = new Menu("LastHit", spellSlotNameLower + "lasthitmenu");
                    {
                        nodeLastHit.AddItem(
                            new MenuItem("lasthit" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(false));
                        nodeLastHit.AddItem(
                            new MenuItem("lasthit" + spellSlotNameLower + "mana", "Min. Mana").SetValue(new Slider(5)));

                        if (spellSlotNameLower.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                        {
                            nodeLastHit.AddItem(
                                new MenuItem("lasthit.count.clear", "Minions killable Q").SetValue(new Slider(3, 1, 6)));
                            nodeLastHit.AddItem(
                                new MenuItem("lasthit.mode", "Q mode").SetValue(
                                    new StringList(new[] { "Always", "Killable count" }, 0)));
                        }
                    }
                    node.AddSubMenu(nodeLastHit);
                }

                if (!spellSlotNameLower.Equals("r", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nodeLaneClear = new Menu("Clear", spellSlotNameLower + "laneclearmenu");
                    {
                        nodeLaneClear.AddItem(new MenuItem("laneclear" + spellSlotNameLower + "use", "Use " + spellSlotName).SetValue(true));
                        nodeLaneClear.AddItem(new MenuItem("laneclear" + spellSlotNameLower + "mana", "Min. Mana").SetValue(new Slider(5)));

                        if (spellSlotNameLower.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                        {
                            nodeLaneClear.AddItem(new MenuItem("lasthit.count", "Minions hit Q").SetValue(new Slider(3, 1, 6)));
                            nodeLaneClear.AddItem(new MenuItem("jungleclearuse", "Use Q in jungle").SetValue(true));
                            nodeLaneClear.AddItem(new MenuItem("lasthit.count.jungle", "Minions hit Q jungle").SetValue(new Slider(3, 1, 6)));
                        }

                        if (spellSlotNameLower.Equals("e", StringComparison.InvariantCultureIgnoreCase))
                        {
                            nodeLaneClear.AddItem(new MenuItem("lasthit.count.e", "Minions hit E").SetValue(new Slider(3, 1, 6)));
                            nodeLaneClear.AddItem(new MenuItem("jungleclearusee", "Use E in jungle").SetValue(true));
                        }
                    }

                    node.AddSubMenu(nodeLaneClear);
                }

                if (spellSlotNameLower.Equals("e", StringComparison.InvariantCultureIgnoreCase))
                {
                    var nodeMisc = new Menu("Miscellaneous", spellSlotNameLower + "miscmenu");
                    {
                        if (spellSlotNameLower.Equals("e", StringComparison.InvariantCultureIgnoreCase))
                        {
                            nodeMisc.AddItem(new MenuItem("gapclosereuse", "Use " + spellSlotName + " for gapclosers").SetValue(true));
                        }
                    }

                    node.AddSubMenu(nodeMisc);
                }

                RootMenu.AddSubMenu(node);
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@MyMenu.cs: Can not generate menu for spell - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     The get orbwalker node.
        /// </summary>
        /// <returns>
        ///     <see cref="Menu" /> class for the orbwalker.
        /// </returns>
        private static Menu GetOrbwalkerNode()
        {
            var node = new Menu("Orbwalker", "elvarusrevampedorbwalker");
            Program.Orbwalker = new Orbwalking.Orbwalker(node);

            return node;
        }

        /// <summary>
        ///     The get target selector node.
        /// </summary>
        /// <returns>
        ///     <see cref="Menu" /> class for the target selector.
        /// </returns>
        private static Menu GetTargetSelectorNode()
        {
            var node = new Menu("Target Selector", "elvarusrevampedtargetselector");
            TargetSelector.AddToMenu(node);

            return node;
        }

        #endregion
    }
}