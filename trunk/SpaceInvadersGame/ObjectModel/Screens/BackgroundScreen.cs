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

namespace SpaceInvadersGame.ObjectModel.Screens
{
    /// <summary>
    /// Implements the backgorund screen.
    /// This screen always rests at the bottom of the screen stack in the ScreensManager
    /// </summary>
    public class BackgroundScreen : GameScreen
    {
        private BackGround m_Background;
        private int m_StarsNum;

        /// <summary>
        /// Initializes the background
        /// </summary>
        /// <param name="i_Game">The Hosting Game</param>
        /// <param name="i_StarsNum">The number of stars to be shown on background</param>
        public  BackgroundScreen(Game i_Game, int i_StarsNum)
            : base(i_Game)
        {
            m_Background = new BackGround(i_Game);
            this.Add(m_Background);
            this.IsOverlayed = true;
            this.Visible = true;
            this.Enabled = true;
            m_StarsNum = i_StarsNum;
        }

        /// <summary>
        /// Screen Deactivation is prevented to maintain background in tha background
        /// </summary>
        protected override void Deactivate()
        {
        }

        /// <summary>
        /// Create all stars on background
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            Random rand = new Random();

            Color[] pixels = m_Background.ColorData;

            int starsAdded = 0;
            while (starsAdded < m_StarsNum)
            {
                // generate a random pixel index, see if it is a black one,
                // and generate a new random value for the pixel.
                int index = rand.Next(pixels.Length);
                if (pixels[index] == Color.TransparentBlack ||
                    pixels[index] == Color.Black)
                {
                    Vector2 starPosition = new Vector2(
                        index % m_Background.WidthBeforeScale,
                        index / m_Background.WidthBeforeScale);
                    float value = (float)rand.NextDouble() + .5f;

                    this.Add(new Star(
                            Game,
                            starPosition,
                            rand.Next(1, 4),
                            DrawOrder + 1));

                    starsAdded++;
                }
            }
        }
    }
}
