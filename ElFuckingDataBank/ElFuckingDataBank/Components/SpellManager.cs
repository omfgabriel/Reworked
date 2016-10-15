namespace ElFuckingDataBank.Components
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using ElFuckingDataBank.Enumerations;
    using ElFuckingDataBank.Utils;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The spell manager.
    /// </summary>
    internal class SpellManager
    {

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SpellManager" /> class.
        /// </summary>
        internal SpellManager()
        {
            try
            {
                foreach (var spellData in ObjectManager.Player.Spellbook.Spells)
                {
                    Logging.AddEntry(LoggingEntryTrype.Debug, "Spell name: {0}", spellData.SData.Name);
                    Logging.AddEntry(LoggingEntryTrype.Debug, "Spell width: {0}", spellData.SData.LineWidth);
                    Logging.AddEntry(LoggingEntryTrype.Debug, "Spell speed: {0}", spellData.SData.MissileSpeed);
                    Logging.AddEntry(LoggingEntryTrype.Debug, "Spell range: {0}", spellData.SData.CastRange);
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryTrype.Error, "@SpellManager.cs: Can not initialize the spells - {0}", e);
                throw;
            }

            Drawing.OnDraw += OnDraw;
            Game.OnUpdate += this.Game_OnUpdate;
            GameObject.OnCreate += ObjSpellMissileOnOnCreate;
        }

        #endregion

        #region Methods


        private static void OnDraw(EventArgs args)
        {
            //Render.Circle.DrawCircle(Game.CursorPos, 150f, Color.DeepSkyBlue);
        }

        private static void ObjSpellMissileOnOnCreate(GameObject sender, EventArgs args)
        {
            var missile = sender as MissileClient;

            if (missile == null || !missile.IsValid)
            {
                return;
            }

            var unit = missile.SpellCaster as Obj_AI_Hero;

            if (unit != null && unit.IsMe)
            {
                Game.PrintChat("Projectile Created: {0} - Distance: {1} - Radius: {2} - Speed: {3} ", missile.SData.Name, missile.SData.CastRange, missile.SData.LineWidth, missile.SData.MissileSpeed);
                Logging.AddEntry(LoggingEntryTrype.Debug, "Projectile Created: {0} - Distance: {1} - Radius: {2} - Speed: {3} ", missile.SData.Name, missile.SData.CastRange, missile.SData.LineWidth, missile.SData.MissileSpeed);
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
        }

        #endregion
    }
}
