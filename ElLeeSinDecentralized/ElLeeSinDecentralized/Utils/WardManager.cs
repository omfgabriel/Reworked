namespace ElLeeSinDecentralized.Utils
{
    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    internal class WardManager
    {
        /// <summary>
        ///     The wardjump handler.
        /// </summary>
        /// <param name="mousePosition">
        ///     The cursor position.
        /// </param>
        /// <param name="jumpToAllies">
        ///     Wardjump to allies.
        /// </param>
        /// <param name="jumpToMinions">
        ///     Wardjump to minions.
        /// </param>
        /// <param name="maxrangeJump">
        ///     Wardjump maxrange.
        /// </param>
        internal static void WardjumpHandler(Vector3 mousePosition, bool jumpToAllies = true, bool jumpToMinions = true, bool maxrangeJump = false)
        {
            var myPosition = ObjectManager.Player.Position.To2D();
            var jumpPosition = (mousePosition.To2D() - ObjectManager.Player.Position.To2D());
        }

        /// <summary>
        ///    Ward jump caster.
        /// </summary>
        internal static void WardJumpCaster()
        {
        }
    }
}
