using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel
{
    public class Barrier : Sprite
    {
        private const string k_AssetName = @"Sprites\Barrier_44x32";
        private const int k_XMotionSpeed = 100;

        private bool m_FirstUpdate = true;
        private float m_MaxXValue = 0;
        private float m_MinXValue = 0;

        public Barrier(Game i_Game)
            : base(k_AssetName, i_Game)
        {
            m_BoundCheckType = eSpriteBoundCheckType.ReachBounds;
            MotionVector = new Vector2(k_XMotionSpeed, 0);
        }

        public override void    Update(GameTime i_GameTime)
        {
            // On the first update, we'll set the barrier maximum and
            // minimum bounds 
            if (m_FirstUpdate)
            {
                m_FirstUpdate = false;
                m_MinXValue = Bounds.Left - Texture.Width / 2;
                m_MaxXValue = Bounds.Right + Texture.Width / 2;
            }

            base.Update(i_GameTime);

            // If the barrier reached one of the allowed bounds, we'll switch
            // the movment direction
            if (Bounds.Left <= m_MinXValue || Bounds.Right >= m_MaxXValue)
            {
                MotionVector *= -1;
            }
        }
    }
}
