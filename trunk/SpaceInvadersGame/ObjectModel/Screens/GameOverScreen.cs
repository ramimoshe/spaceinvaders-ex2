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
using SpaceInvadersGame;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    /// <summary>
    /// The class implements the games GameOver screen
    /// </summary>
    public class GameOverScreen : GameScreen
    {
        public event GameOverDelegate ExitGame;

        private readonly Keys r_EndGameKey = Keys.Escape;
        private readonly string r_GameOverText = "Game Over";
        private readonly string r_KeysText = 
            "Press Esc to exit, R to start a new Game, O to go to the Main Menu";

        private IPlayer[] m_Players;

        private SpriteFontComponent m_GameOverMessage;
        private SpriteFontComponent m_KeysMessage;

        private bool m_EndGame = false;

        public GameOverScreen(Game i_Game, IPlayer[] i_Players)
            : base(i_Game)
        {
            m_GameOverMessage = new SpriteFontComponent(
                i_Game, 
                @"Fonts\David28", 
                r_GameOverText);            
            m_KeysMessage = new SpriteFontComponent(
                i_Game, 
                @"Fonts\David28",
                r_KeysText);

            this.IsModal = true;

            m_GameOverMessage.TintColor = Color.White;
            m_KeysMessage.TintColor = Color.White;
            m_Players = i_Players;
            // TODO: Change the parameter to a constant

            this.Add(m_GameOverMessage);
            this.Add(m_KeysMessage);
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
            
            Vector2 center = new    Vector2(
                                    GraphicsDevice.Viewport.Width / 2,
                                    GraphicsDevice.Viewport.Height / 2);
            
            m_GameOverMessage.Spacing = 20;
            m_GameOverMessage.PositionOfOrigin = center;
            m_GameOverMessage.PositionOrigin = m_GameOverMessage.SpriteCenter;

            m_KeysMessage.Text = m_KeysMessage.Text;
            m_KeysMessage.Scale = Vector2.One * 0.5f;
            m_KeysMessage.PositionOfOrigin = new Vector2(center.X, center.Y + m_GameOverMessage.HeightAfterScale);
            m_KeysMessage.PositionOrigin = m_KeysMessage.SpriteCenter;
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
                m_EndGame = true;
                ExitScreen();
            }
        }

        /// <summary>
        /// Called when the screen is deactivated.
        /// we'll check if the user chose to exit the game, and if so we'll
        /// raise the ExitGame event
        /// </summary>
        protected override void     OnDeactivated()
        {
            if (m_EndGame)
            {
                onExitGame();
            }
        }

        /// <summary>
        /// Raise a GameOver event when the user press the end game key
        /// </summary>
        private void    onExitGame()
        {
            if (ExitGame != null)
            {
                ExitGame();
            }
        }
    }
}
