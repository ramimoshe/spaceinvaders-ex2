using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;
using SpaceInvadersGame.ObjectModel;

namespace SpaceInvadersGame
{       
    /// <summary>
    /// The game background sprite
    /// </summary>
    public class BackGround : Sprite
    {
        private const string k_AssetName = @"Sprites\BG_Space01_1024x768";
       
        public BackGround(Game i_Game) 
            : base(k_AssetName, i_Game, Int32.MaxValue, Int32.MinValue)
        {
        }

        // TODO: Remove the code

        /// <summary>
        /// Loads the background asset, and creates the backgroung stars
        /// </summary>
      /*  protected override void    LoadContent()
        {
            base.LoadContent();
            Random rand = new Random();

            Color[] pixels = new Color[this.Texture.Width * this.Texture.Height];
            this.Texture.GetData<Color>(pixels);

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
                        index % Texture.Width, 
                        index / Texture.Width);
                    float value = (float)rand.NextDouble() + .5f;                    
                    Star s = new Star(
                            Game,
                            starPosition,
                            rand.Next(1, 4),
                            DrawOrder + 1);

                    starsAdded++;
                }
            }            
        }*/

        /// <summary>
        /// Raise an AddedStar event
        /// </summary>
        /// <param name="i_Star">The new star we created</param>
        /*private void    onAddedStar(IGameComponent i_Star)
        {
            if (AddedStar != null)
            {
                AddedStar(i_Start);
            }
        }*/
    }
}