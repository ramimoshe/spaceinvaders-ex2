using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.ObjectModel;

namespace SpaceInvadersGame
{
    /// <summary>
    /// Holds all the barriers in the game and manages them
    /// </summary>
    public class BarriersHolder : CompositeDrawableComponent<Barrier>
    {
        private const int k_BarriersNum = 4;

        public BarriersHolder(Game i_Game)
            : base(i_Game)
        {
            createBarriers();
        }

        /// <summary>
        /// Create the barriers components
        /// </summary>
        private void    createBarriers()
        {
            for (int i = 0; i < k_BarriersNum; i++)
            {
                this.Add(new Barrier(this.Game));
            }
        }        

        /// <summary>
        /// Update all the barriers position to be above the player position
        /// in the y coordinate
        /// </summary>
        /// <param name="i_PlayerTop">The players Top bound value</param>
        /// <param name="i_PlayerHeight">The players texture height</param>
        public void     UpdateBarriersPossition(
            int i_PlayerTop, 
            int i_PlayerHeight)
        {
            IEnumerator<Barrier> barriersEnumeration = GetEnumerator();
            
            if (barriersEnumeration.MoveNext())
            {
                Barrier firstBarrier = barriersEnumeration.Current;

                // Calculate the first barrier position when we need to make
                // sure that the positions will be symetric in relation to the
                // screen middle and that the space between the barriers will
                // be equal to 1.5 * (barrier texture width). 
                Vector2 startingPosition = new Vector2(
                   (Game.GraphicsDevice.Viewport.Width / 2) - // getting the screen middle position
                    (firstBarrier.Texture.Width * .75f) - // making sure the positions will be symetric
                 (((k_BarriersNum / 2) - 1) * firstBarrier.Texture.Width * 1.5f) - // getting to the first barrier position
                  ((k_BarriersNum / 2) * firstBarrier.Texture.Width), // barriers draw offset
                    i_PlayerTop - i_PlayerHeight - firstBarrier.Texture.Height);             

                Vector2 currPosition = startingPosition;
                Barrier currBarrier;

                // Move on all the barriers and change their position
                do
                {
                    currBarrier = barriersEnumeration.Current;

                    currBarrier.PositionForDraw = currPosition;

                    currPosition += new Vector2(
                        (firstBarrier.Texture.Width * 1.5f) +
                        firstBarrier.Texture.Width,
                        0);
                }
                while (barriersEnumeration.MoveNext());
            }
        }
    }
}
