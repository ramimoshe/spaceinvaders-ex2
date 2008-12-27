using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.ObjectModel;
using SpaceInvadersGame.Interfaces;

namespace SpaceInvadersGame
{
    /// <summary>
    /// Holds all the barriers in the game and manages them
    /// </summary>
    public class BarriersHolder : CompositeDrawableComponent<Barrier>
    {
        private const int k_BarriersNum = 4;
        private GameLevelData m_GameLevelData;

        public event PlayActionSoundDelegate PlayActionSoundEvent;

        public BarriersHolder(Game i_Game)
            : base(i_Game)
        {
            createBarriers();
        }

        /// <summary>
        /// Sets the component game level data and change the invaders matrix
        /// column num according to the given level data
        /// </summary>
        public GameLevelData LevelData
        {
            set
            {
                m_GameLevelData = value;
                onSettingGameLevelData();
            }
        }

        /// <summary>
        /// Create the barriers components
        /// </summary>
        private void    createBarriers()
        {
            for (int i = 0; i < k_BarriersNum; i++)
            {
                Barrier bar = new Barrier(this.Game);
                bar.PlayActionSoundEvent += new PlayActionSoundDelegate(barrier_PlayActionSoundEvent);

                this.Add(bar);
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
                    currBarrier.DefaultPosition = currPosition;
                    currBarrier.CalcBarrierBounds();

                    currPosition += new Vector2(
                        (firstBarrier.Texture.Width * 1.5f) +
                        firstBarrier.Texture.Width,
                        0);
                }
                while (barriersEnumeration.MoveNext());
            }
        }

        /// <summary>
        /// Changing the motion speed for all the barriers
        /// </summary>
        private void    onSettingGameLevelData()
        {                        
            IEnumerator<Barrier> barriersEnumeration = GetEnumerator();

            // Update all the barriers speed
            while (barriersEnumeration.MoveNext())
            {
                barriersEnumeration.Current.ResetBarrier();
                barriersEnumeration.Current.MotionVector = new Vector2(
                    m_GameLevelData.BarrierSpeed,
                    0);                
            }
        }

        /// <summary>
        /// Catch a PlayActionSound event raised by a barrier and raised
        /// it to the listeners
        /// </summary>
        /// <param name="i_Action">The action that cause the event</param>
        private void barrier_PlayActionSoundEvent(eSoundActions i_Action)
        {
            onPlayActionSound(i_Action);
        }

        /// <summary>
        /// Raise a PlayActionSoundEvent that was raised by a barrier
        /// </summary>
        /// <param name="i_Action">The action we want to put in the raised
        /// event</param>
        private void onPlayActionSound(eSoundActions i_Action)
        {
            if (PlayActionSoundEvent != null)
            {
                PlayActionSoundEvent(i_Action);
            }
        }
    }
}
