using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    /// <summary>
    /// Implements main menu
    /// </summary>
    public class MainMenuScreen : MenuTypeScreen
    {
        private const string k_MainMenuName = "Main Menu";
        private const int k_PlayersNumMin = 1;
        private const int k_PlayersNumMax = 2;
        private int m_PlayersNum = k_PlayersNumMin;
        private string[] m_PlayersText = { "Players: One", "Players: Two" };

        private OptionsMenuItem m_PlayersMenuItem;
        private MenuItem m_DisplayOptionsItem;
        private MenuItem m_SoundOptionsItem;
        private MenuItem m_PlayItem;
        private MenuItem m_QuitItem;

        /// <summary>
        /// Inits the menu
        /// </summary>
        /// <param name="i_Game"></param>
        public  MainMenuScreen(Game i_Game)
            : base(i_Game, k_MainMenuName)
        {
        }

        /// <summary>
        /// Adds all items to menu
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();
            m_PlayersMenuItem = new OptionsMenuItem(
                                    Game, 
                                    new List<string>(m_PlayersText), 
                                    m_PlayersItem_Modified);

            m_DisplayOptionsItem = new MenuItem(Game, "Screen Options", displayOptionsItem_Executed);
            m_SoundOptionsItem = new MenuItem(Game, "Sound Options", soundOptionsItem_Executed);
            m_PlayItem = new MenuItem(Game, "Play", playItem_Executed);
            m_QuitItem = new MenuItem(Game, "Quit", quitItem_Executed);

            Add(m_PlayersMenuItem);
            Add(m_DisplayOptionsItem);
            Add(m_SoundOptionsItem);
            Add(m_PlayItem);
            Add(m_QuitItem);
        }

        /// <summary>
        /// Mdifies number of players
        /// </summary>
        /// <param name="i_PlayersNum">The number of players according to item</param>
        private void    m_PlayersItem_Modified(int i_PlayersNum)
        {
            m_PlayersNum = i_PlayersNum + 1;
        }

        /// <summary>
        /// Invokes Display Options screen
        /// </summary>
        private void    displayOptionsItem_Executed()
        {
            ScreensManager.SetCurrentScreen(new DisplayMenuScreen(Game));
        }

        /// <summary>
        /// Invokes Sound Options screen
        /// </summary>
        private void    soundOptionsItem_Executed()
        {
            ScreensManager.SetCurrentScreen(new SoundMenuScreen(Game));
        }

        /// <summary>
        /// Starts the gane
        /// </summary>
        private void    playItem_Executed()
        {
            ExitScreen();
            ScreensManager.SetCurrentScreen(new SpaceInvadersGameScreen(
                                                Game, 
                                                m_PlayersNum));
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        private void    quitItem_Executed()
        {
            Game.Exit();
        }
    }
}
