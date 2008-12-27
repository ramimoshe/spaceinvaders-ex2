using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    public class MainMenuScreen : MenuTypeScreen
    {
        const string k_MainMenuName = "Main Menu";
        const int k_PlayersNumMin = 1;
        const int k_PlayersNumMax = 2;
        int m_PlayersNum = k_PlayersNumMin;

        OptionsMenuItem m_PlayersMenuItem;
        MenuItem m_ScreenOptionsItem;
        MenuItem m_SoundOptionsItem;
        MenuItem m_PlayItem;
        MenuItem m_QuitItem;

        public MainMenuScreen(Game i_Game)
            : base(i_Game, k_MainMenuName)
        {
        }

        public override void Initialize()
        {
            string[] players = {"Players: One", "Players: Two"};

            base.Initialize();
            m_PlayersMenuItem = new OptionsMenuItem(Game, new List<string>(players));
            m_PlayersMenuItem.Decreased += new MenuOptionChangedEventHandler(playersItem_Decreased);
            m_PlayersMenuItem.Increased += new MenuOptionChangedEventHandler(playersItem_Increased);

            m_ScreenOptionsItem = new MenuItem(Game, "Screen Options");
            m_ScreenOptionsItem.Executed += new MenuItemEventHandler(screenOptionsItem_Executed);
            m_SoundOptionsItem = new MenuItem(Game, "Sound Options");
            m_SoundOptionsItem.Executed += new MenuItemEventHandler(soundOptionsItem_Executed);
            m_PlayItem = new MenuItem(Game, "Play");
            m_PlayItem.Executed += new MenuItemEventHandler(playItem_Executed);
            m_QuitItem = new MenuItem(Game, "Quit");
            m_QuitItem.Executed += new MenuItemEventHandler(quitItem_Executed);

            Add(m_PlayersMenuItem);
            Add(m_ScreenOptionsItem);
            Add(m_SoundOptionsItem);
            Add(m_PlayItem);
            Add(m_QuitItem);
        }

        private void playersItem_Increased()
        {
            changeNumberOfPlayers(-1);
        }

        private void playersItem_Decreased()
        {
            changeNumberOfPlayers(1);
        }

        private void changeNumberOfPlayers(int numDelta)
        {
            m_PlayersNum = (int) MathHelper.Clamp(m_PlayersNum + numDelta, k_PlayersNumMin, k_PlayersNumMax);
        }

        private void screenOptionsItem_Executed()
        {
        }

        private void soundOptionsItem_Executed()
        {
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
