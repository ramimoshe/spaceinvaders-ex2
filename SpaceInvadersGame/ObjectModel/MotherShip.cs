using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.Interfaces;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// Represents the invaders mother ship
    /// </summary>
    public class MotherShip : CollidableSprite, IScorable
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const int k_Score = 500;
        private readonly Vector2 r_MotionVector = new Vector2(-150, 0);
        private Vector2 m_DefaultPosition;

        private readonly TimeSpan r_TimeBetweenMove = TimeSpan.FromSeconds(5.0f);
        private TimeSpan m_RemainingTimeToMove;

        // TODO Remove position from the enemy ctor        

        public MotherShip(Game i_Game) 
            : base(k_AssetName, i_Game)
        {
            TintColor = Color.Red;
        }

        public int Score
        {
            get 
            { 
                return k_Score; 
            }
        }

        protected override void     InitPosition()
        {
            Position = new Vector2(Game.GraphicsDevice.Viewport.Width, 
                                   Texture.Height);

            m_DefaultPosition = Position;
        }        

        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            // If the ship isn't moving we'll start counting until the next 
            // move
            if (MotionVector.X == 0 && MotionVector.Y == 0)
            {
                m_RemainingTimeToMove -= i_GameTime.ElapsedGameTime;

                if (m_RemainingTimeToMove.TotalSeconds <= 0)
                {
                    // Start moving the ship
                    MotionVector = r_MotionVector;
                    Position = m_DefaultPosition;
                    m_RemainingTimeToMove = r_TimeBetweenMove;
                    Visible = true;
                }
            }
            else
            {
                // If the ship reached the end of the screen, we'll make it 
                // invisible and start counting until next move
                if (Bounds.Right == 0)
                {
                    MotionVector = Vector2.Zero;
                    Visible = false;
                }
            }
        }
    }
}
