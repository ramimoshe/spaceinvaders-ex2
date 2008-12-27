using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using XnaGamesInfrastructure.ObjectModel.Screens;
using SpaceInvadersGame.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.Services;
using SpaceInvadersGame.Interfaces;
using SpaceInvadersGame.ObjectModel.Screens.Menus;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    // TODO: Fix the class presentation

    /// <summary>
    /// The class implements the games GameOver screen
    /// </summary>
    public class GameOverScreen : GameScreen
    {
        private readonly Keys r_EndGameKey = Keys.Escape;
        private readonly Keys r_NewGameKey = Keys.R;
        private readonly Keys r_MainMenuKey = Keys.O;

        private readonly Vector2 r_WiningMsgScale = new Vector2(.75f, .75f);

        private readonly string r_GameOverText = "Game Over";
        private readonly string r_KeysText = 
@"Press: 
Esc to exit
R to start a new Game
O to go to the Main Menu";

        private IPlayer[] m_Players;

        private SpriteFontComponent m_GameOverMessage;
        private SpriteFontComponent m_KeysMessage;
        private SpriteFontComponent m_WinningPlayerMessage;

        public GameOverScreen(Game i_Game, IPlayer[] i_Players)
            : base(i_Game)
        {
            m_Players = i_Players;

            m_GameOverMessage = new SpriteFontComponent(
                                    i_Game, 
                                    @"Fonts\David28", 
                                    r_GameOverText);            
            m_KeysMessage = new SpriteFontComponent(
                                i_Game, 
                                @"Fonts\David",
                                r_KeysText);

            m_WinningPlayerMessage = new SpriteFontComponent(
                                i_Game,
                                @"Fonts\David",
                                WinningPlayerMsg);

            this.IsModal = true;

            m_GameOverMessage.TintColor = Color.White;
            m_KeysMessage.TintColor = Color.White;            

            this.Add(m_GameOverMessage);
            this.Add(m_KeysMessage);
            this.Add(m_WinningPlayerMessage);
        }

        /// <summary>
        /// Return a string that states the wining player
        /// </summary>
        private string      WinningPlayerMsg
        {
            get
            {
                string retVal = String.Empty;

                if (m_Players.Length == 2)
                {
                    retVal = "Player {0} Won";

                    if (m_Players[0].Score > m_Players[1].Score)
                    {
                        retVal = String.Format(retVal, 1);
                    }
                    else if (m_Players[0].Score < m_Players[1].Score)
                    {
                        retVal = String.Format(retVal, 2);
                    }
                    else
                    {
                        retVal = "Tie. Players scores are equal.";
                    }
                }

                for (int i = 0; i < m_Players.Length; i++)
                {
                    retVal += "\n player " + (i+1) + "score is: " + m_Players[i].Score;
                }

                return retVal;
            }
        }

        public IPlayer[]    Players
        {
            set { m_Players = value; }
        }

        /// <summary>
        /// Initialize the screen by setting the messages positions
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            Vector2 center = m_GameOverMessage.ViewPortCenter;
            
            m_GameOverMessage.PositionOfOrigin = center;
            m_GameOverMessage.PositionOrigin = m_GameOverMessage.SpriteCenter;

            m_WinningPlayerMessage.Text = WinningPlayerMsg;
            m_WinningPlayerMessage.PositionOfOrigin =
                new Vector2(
                center.X, center.Y + 
                m_GameOverMessage.HeightAfterScale * 2);
            m_WinningPlayerMessage.PositionOrigin =
                m_GameOverMessage.SpriteCenter;
            
            m_WinningPlayerMessage.Scale = r_WiningMsgScale;

            m_KeysMessage.Text = m_KeysMessage.Text;
            m_KeysMessage.PositionOfOrigin = 
                new Vector2(
                center.X,
                center.Y + m_WinningPlayerMessage.HeightAfterScale);
            m_KeysMessage.PositionOrigin = m_WinningPlayerMessage.SpriteCenter;            
        }

        /// <summary>
        /// Update the screen by checking the given key and continue as 
        /// necessary       
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(r_EndGameKey))
            {
                this.Game.Exit();
            }
            else if (InputManager.KeyPressed(r_NewGameKey))
            {
                this.ExitScreen();
                ScreensManager.SetCurrentScreen(
                    new SpaceInvadersGameScreen(this.Game, m_Players.Length));                
            }
            else if (InputManager.KeyPressed(r_MainMenuKey))
            {
                this.ExitScreen();
                ScreensManager.SetCurrentScreen(
                    new MainMenuScreen(this.Game));                
            }
        }
    }
}
