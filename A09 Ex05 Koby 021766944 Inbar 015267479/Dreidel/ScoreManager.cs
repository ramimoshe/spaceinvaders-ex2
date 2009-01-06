using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DreidelGame.ObjectModel;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame
{
    /// <summary>
    /// Responsible for presenting the player score on the screen
    /// </summary>
    public class ScoreManager : DrawableGameComponent
    {
        private const int k_PlayerScoreIncrement = 1;
        private const int k_DefaultPlayerScore = 0;

        private int m_CurrPlayerScore;
        private eDreidelLetters m_PlayerChosenLetter;
        private Vector2 m_ScorePosition;
        private SpriteBatch m_SpriteBatch;
        private SpriteFont m_Font;

        /// <summary>
        /// Property that gets/sets the letter that the player chose
        /// </summary>
        public eDreidelLetters      PlayerChosenLetter
        {
            get { return m_PlayerChosenLetter; }

            set { m_PlayerChosenLetter = value; }
        }

        public ScoreManager(Game i_Game)
            : base(i_Game)
        {
            m_PlayerChosenLetter = eDreidelLetters.None;
            m_CurrPlayerScore = k_DefaultPlayerScore;
        }

        /// <summary>
        /// Initializes the position where the class will draw the players score
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_ScorePosition = Vector2.Zero;
        }

        protected override void     LoadContent()
        {
            base.LoadContent();

            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            m_Font = Game.Content.Load<SpriteFont>("");
        }

        /// <summary>
        /// Draws the players score on the screen
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the games time</param>
        public override void    Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);

            m_SpriteBatch.Begin();

            m_SpriteBatch.End();
        }
    }
}
