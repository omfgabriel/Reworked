namespace ElFuckingDataBank
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    using ElFuckingDataBank.Components;
    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The program.
    /// </summary>
    public static class Program
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The EntryPoint of the solution.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += eventArgs =>
            {
                Bootstrap();
            };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The bootstrapping method for the components.
        /// </summary>
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement", Justification = "They would not be used.")]
        private static void Bootstrap()
        {
            try
            {
                new SpellManager();
            }
            catch (Exception e)
            {
                Console.WriteLine("@Program.cs: Can not bootstrap components - {0}", e);
                throw;
            }
        }

        #endregion
    }
}