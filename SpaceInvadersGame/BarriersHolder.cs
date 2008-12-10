using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.ObjectModel;

namespace SpaceInvadersGame
{
    public class BarriersHolder : RegisteredComponent
    {
        private const int k_BarriersNum = 4;

        private List<Barrier> m_Barriers = new List<Barrier>();

        public BarriersHolder(Game i_Game)
            : base(i_Game, Int32.MaxValue)
        {
            createBarriers();
        }

        private void    createBarriers()
        {
            for (int i = 0; i < k_BarriersNum; i++)
            {
                m_Barriers.Add(new Barrier(this.Game));
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
            // Calculate the first barrier position when we need to make
            // sure that the positions will be symetric in relation to the
            // screen middle and that the space between the barriers will
            // be equal to 1.5 * (barrier texture width). 
            Vector2 startingPosition = new Vector2(
               (m_Barriers[0].Game.GraphicsDevice.Viewport.Width / 2) - // getting the screen middle position
                (m_Barriers[0].Texture.Width * .75f) - // making sure the positions will be symetric
             (((k_BarriersNum / 2) - 1) * m_Barriers[0].Texture.Width * 1.5f) - // getting to the first barrier position
              ((k_BarriersNum / 2) * m_Barriers[0].Texture.Width), // barriers draw offset
                i_PlayerTop - i_PlayerHeight - m_Barriers[0].Texture.Height);

            Vector2 currPosition = startingPosition;

            for (int i = 0; i < k_BarriersNum; i++)
            {
                m_Barriers[i].PositionForDraw = currPosition;

                currPosition += new Vector2(
                    (m_Barriers[0].Texture.Width * 1.5f) + 
                    m_Barriers[0].Texture.Width,
                    0);
            }
        }
    }
}
