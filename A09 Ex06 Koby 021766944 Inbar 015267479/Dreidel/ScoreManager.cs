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
        private const string k_FontName = @"Fonts/David";
        private const string k_DrawText = "Player Score: {0}";
        private readonly Color r_DrawColor = Color.Black;
        private const int k_DrawOrder = Int32.MaxValue;

        private int m_CurrPlayerScore;
        private eDreidelLetters m_PlayerChosenLetter;
        private Vector2 m_ScorePosition;
        private SpriteBatch m_SpriteBatch;
        private SpriteFont m_Font;
        private string m_Text = String.Empty;

        /// <summary>
        /// Property that gets/sets the letter that the player chose
        /// </summary>
        public eDreidelLetters      PlayerChosenLetter
        {
            get { return m_PlayerChosenLetter; }

            set { m_PlayerChosenLetter = value; }
        }

        /// <summary>
        /// Gets the text we want to print on the screen
        /// </summary>
        public string   DrawString
        {            
            get { return m_Text; }            
        }

        /// <summary>
        /// Gets/sets the player score
        /// </summary>
        public int      PlayerScore
        {
            get { return m_CurrPlayerScore; }

            set
            {
                m_CurrPlayerScore = value;
                m_Text = String.Format(k_DrawText, m_CurrPlayerScore);
            }
        }

        /// <summary>
        /// CTOR. inits a new instance
        /// </summary>
        /// <param name="i_Game">The hosting game</param>
        public ScoreManager(Game i_Game)
            : base(i_Game)
        {
            m_PlayerChosenLetter = eDreidelLetters.None;

            PlayerScore = k_DefaultPlayerScore;
            DrawOrder = k_DrawOrder;
        }

        /// <summary>
        /// Initializes the position where the class will draw the players score
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_ScorePosition = Vector2.Zero;
        }

        /// <summary>
        /// Creates a SpriteBatch and loads the class font
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
            m_Font = Game.Content.Load<SpriteFont>(k_FontName);
        }

        /// <summary>
        /// Draws the players score on the screen
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the games time</param>
        public override void    Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);

            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(m_Font, this.DrawString, m_ScorePosition, r_DrawColor);
            m_SpriteBatch.End();
        }

        /// <summary>
        /// Catch a finished spinning event raised by a dreidel and updates the players score 
        /// in case the dreidel current side letter matches the letter that the player chose        
        /// </summary>
        /// <param name="i_Dreidel">The dreidel that raised the event</param>
        public void     Dreidel_FinishedSpinning(Dreidel i_Dreidel)
        {
            if (i_Dreidel.DreidelFrontLetter.Equals(m_PlayerChosenLetter))
            {
                PlayerScore = PlayerScore + k_PlayerScoreIncrement;
            }
        }
    }
}
