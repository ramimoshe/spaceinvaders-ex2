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
    public class BackGroundComposite : CompositeDrawableComponent<IGameComponent>
    {
        private BackGround m_BackGround;

        private int m_StarsNum;        

        public BackGroundComposite(
            Game i_Game,             
            int i_StarsNum) 
            : base(i_Game)
        {
            m_BackGround = new BackGround(i_Game);
            m_StarsNum = i_StarsNum;
            this.Add(m_BackGround);
        }

        /// <summary>
        /// Gets the BackGround component
        /// </summary>
        public BackGround BackGround
        {
            get { return m_BackGround; }
        }

        // TODO: Remove the code

       /* protected override void     LoadContent()
        {
            base.LoadContent();

            Random rand = new Random();

            Color[] pixels = new Color[BackGround.Texture.Width * 
                BackGround.Texture.Height];
            this.BackGround.Texture.GetData<Color>(pixels);

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
                        index % BackGround.Texture.Width,
                        index / BackGround.Texture.Width);
                    float value = (float)rand.NextDouble() + .5f;

                    this.Add(new Star(
                            Game,
                            starPosition,
                            rand.Next(1, 4),
                            DrawOrder + 1));

                    starsAdded++;
                }
            }            
        }*/        

        public override void    Initialize()
        {
            base.Initialize();

            Random rand = new Random();

            Color[] pixels = new Color[BackGround.Texture.Width *
                BackGround.Texture.Height];
            this.BackGround.Texture.GetData<Color>(pixels);

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
                        index % BackGround.Texture.Width,
                        index / BackGround.Texture.Width);
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

        // TODO: Remove the code

        /// <summary>
        /// Catch the AddedStar event and adds the new star to the 
        /// components list
        /// </summary>
        /// <param name="i_Component">The new star that was created</param>
       /* private void backGround_AddedStar(IGameComponent i_Component)
        {
            this.Add(i_Component);
        }*/

        // TODO: Check the dispose

        /// <summary>
        /// Catch an space ship disposed event and disposes the current object
        /// also
        /// </summary>
        /// <param name="i_Sender">The disposed space ship</param>
        /// <param name="i_EventArgs">The event arguments</param>
        /*private void    invader_Disposed(object i_Sender, EventArgs i_EventArgs)
        {
            Dispose();
        }*/
    }
}