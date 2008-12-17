using System;
using XnaGamesInfrastructure.ObjectModel.Screens;
namespace XnaGamesInfrastructure.ServiceInterfaces
{
    /// <summary>
    /// Implements a base interface for a screen manager
    /// </summary>
    public interface IScreensMananger
    {
        /// <summary>
        /// Getter for the current active screen
        /// </summary>
        GameScreen  ActiveScreen { get; }

        /// <summary>
        /// Sets the current active screen to the specified screen
        /// </summary>
        /// <param name="i_NewScreen">New active screen</param>
        void    SetCurrentScreen(GameScreen i_NewScreen);

        /// <summary>
        /// Removes specified screen from managed screens
        /// </summary>
        /// <param name="i_Screen">Screen to be removed</param>
        /// <returns>True, if screen was removed in manager, False, if screen is not managed
        /// by screen manager</returns>
        bool    Remove(GameScreen i_Screen);

        /// <summary>
        /// Adds a new screen to the manager
        /// </summary>
        /// <param name="i_Screen">New Screen</param>
        void Add(GameScreen i_Screen);
    }
}
