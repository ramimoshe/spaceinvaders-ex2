using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.Interfaces;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// Represents the invaders mother ship
    /// </summary>
    public class MotherShip : Enemy
    {
        private readonly TimeSpan r_TimeBetweenMove = TimeSpan.FromSeconds(5.0f);
        private readonly Vector2 r_MotionVector = new Vector2(-150, 0);
        private const string k_AssetName = @"Sprites\MotherShip_32x120";

        private GameLevelData m_LevelData;

        // TODO: Remove the code
        //private const int k_Score = 500;

        private bool m_MotherShipScaled = false;
        
        // The initialized position
        private Vector2 m_DefaultPosition;

        private TimeSpan m_RemainingTimeToMove;

        public MotherShip(Game i_Game) 
            : base(k_AssetName, i_Game)
        {
            TintColor = Color.Red;
        }

        /// <summary>
        /// Sets the component level game data and change the score according
        /// to the given level data
        /// </summary>
        public GameLevelData    LevelData
        {
            set 
            { 
                m_LevelData = value;
                Score = m_LevelData.MotherShipScore;
            } 
        }

        // TODO: Remove the code

        /// <summary>
        /// A property for the space ship score
        /// </summary>
        /*public override int     Score
        {
            get 
            { 
                return k_Score; 
            }
        }*/

        /// <summary>
        /// Initialize the ship position to be (Viewport.Width, ShipHeight), 
        /// meaning that the ship will be out of the screen, when the Y 
        /// axis value will be equal to the ships height
        /// </summary>
        protected override void     InitBounds()
        {
            PositionForDraw = new Vector2(
                Game.GraphicsDevice.Viewport.Width, 
                Texture.Height);

            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;

            m_DefaultPosition = PositionForDraw;
        }

        /// <summary>
        /// Catch the end event raised by the component animation
        /// </summary>
        /// <param name="i_Animation">the animation that ended</param>
        protected override void     ScaleAnimation_Finished(SpriteAnimation i_Animation)
        {
            base.ScaleAnimation_Finished(i_Animation);
            m_MotherShipScaled = true;
        }

        /// <summary>
        /// Update the ships state according to the current game time, when
        /// every couple of seconds will move the ship accros the screen to 
        /// the other side
        /// </summary>
        /// <param name="i_GameTime">The current game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            // If the ship isn't moving we'll start counting until the next 
            // move
            if ((MotionVector.X == 0 && MotionVector.Y == 0) || m_MotherShipScaled)
            {
                m_RemainingTimeToMove -= i_GameTime.ElapsedGameTime;

                if (m_RemainingTimeToMove.TotalSeconds <= 0)
                {
                    m_MotherShipScaled = false;
                    Animations[k_ScaleAnimationName].Reset();

                    // Start moving the ship in the next update
                    MotionVector = r_MotionVector;
                    PositionForDraw = m_DefaultPosition;
                    m_RemainingTimeToMove = r_TimeBetweenMove;
                    Visible = true;
                }
            }
            else
            {
                // If the ship reached the end of the screen, we'll make it 
                // invisible and start counting until next move again
                if (Bounds.Right == 0)
                {
                    MotionVector = Vector2.Zero;
                    Visible = false;
                }
            }
        }
    }
}
