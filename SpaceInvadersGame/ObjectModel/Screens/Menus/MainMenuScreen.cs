using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
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

        public MainMenuScreen(Game i_Game)
            : base(i_Game, k_MainMenuName)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_PlayersMenuItem = new OptionsMenuItem(
                                    Game, 
                                    new List<string>(m_PlayersText), 
                                    m_PlayersItem_Decreased, 
                                    m_PlayersItem_Increased);

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

        private void m_PlayersItem_Increased()
        {
            changeNumberOfPlayers(-1);
        }

        private void m_PlayersItem_Decreased()
        {
            changeNumberOfPlayers(1);
        }

        private void changeNumberOfPlayers(int numDelta)
        {
            m_PlayersNum = (int) MathHelper.Clamp(m_PlayersNum + numDelta, k_PlayersNumMin, k_PlayersNumMax);
        }

        private void displayOptionsItem_Executed()
        {
            ScreensManager.SetCurrentScreen(new DisplayMenuScreen(Game));
        }

        private void soundOptionsItem_Executed()
        {
            ScreensManager.SetCurrentScreen(new SoundMenuScreen(Game));
        }

        private void playItem_Executed()
        {
            ExitScreen();
            ScreensManager.SetCurrentScreen(new SpaceInvadersGameScreen(
                                                Game, 
                                                m_PlayersNum));
        }

        private void quitItem_Executed()
        {
            // TODO: verify Code
            Game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void playersMenuItemModified()
        {
        }
    }
}
