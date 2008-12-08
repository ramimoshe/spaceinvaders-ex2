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
        private const string k_AssetName = @"Content\Sprites\BG_Space01_1024x768";

        private int m_StarsNum;

        public BackGround(Game i_Game, int i_StarsNum) 
            : base(k_AssetName, i_Game, Int32.MaxValue, Int32.MinValue)
        {
            m_StarsNum = i_StarsNum;
        }

        protected override void    LoadContent()
        {
            base.LoadContent();
            Vector4 tint = Color.AntiqueWhite.ToVector4();
            tint.W = 0.8f;
            TintColor = new Color(tint);
            Random rand = new Random();

            Color[] pixels = new Color[this.Texture.Width * this.Texture.Height];
            this.Texture.GetData<Color>(pixels);

            int starsAdded = 0;

            while (starsAdded < m_StarsNum)
            {
                //generate a random pixel index, see if it is a black one,
                //and generate a new random value for the pixel.
                int index = rand.Next(pixels.Length);
                if (pixels[index] == Color.TransparentBlack ||
                    pixels[index] == Color.Black)
                {
                    float value = (float)rand.NextDouble() + .5f;                    
                    Star s = new Star(
                            Game,
                            new Vector2(
                                index % Texture.Width, 
                                index / Texture.Width),
                            rand.Next(1, 4),
                            DrawOrder + 1);

                    starsAdded++;
                }
            }

            // TODO: Remove the code 

            /*this.Texture.GetData<Color>(pixels);

            for (int i = 1; i <= 4; i++)
            {
                pixels[(Game.GraphicsDevice.Viewport.Width) * i] = new Color(new Vector4(1f));
                pixels[(Game.GraphicsDevice.Viewport.Width * i) - 1] = new Color(new Vector4(1f));
                pixels[(Game.GraphicsDevice.Viewport.Width * i) - 2] = new Color(new Vector4(1f));
                pixels[(Game.GraphicsDevice.Viewport.Width * i) - 3] = new Color(new Vector4(1f));
                pixels[(Game.GraphicsDevice.Viewport.Width * i) - 4] = new Color(new Vector4(1f));
                pixels[(Game.GraphicsDevice.Viewport.Width * i) - 5] = new Color(new Vector4(1f));
                pixels[(Game.GraphicsDevice.Viewport.Width * i) - 6] = new Color(new Vector4(1f));
                pixels[(Game.GraphicsDevice.Viewport.Width * i) - 7] = new Color(new Vector4(1f));
            }
            //save the pixels to the texture
            this.Texture.SetData(pixels);*/
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
          
    }
}