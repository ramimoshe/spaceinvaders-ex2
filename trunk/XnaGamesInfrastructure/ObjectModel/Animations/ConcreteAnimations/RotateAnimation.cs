using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /// <summary>
    /// Animates a sprite's rotation
    /// </summary>
    public class RotateAnimation : SpriteAnimation
    {
        private float m_TargetRotateAngle;
        private TimeSpan m_RotateLength;
        private float m_AngularVelocity;

        /// <summary>
        /// Sets a new rotation animation
        /// </summary>
        /// <param name="i_Name">Animation name</param>
        /// <param name="i_TargetRotateAngle">Sets the target rotation angle in radians</param>
        /// <param name="i_AnimationLength">Animation time</param>
        /// <param name="i_ResetAfterFinish">Specifies whther to reset animation when done</param>
        public  RotateAnimation( 
                                string i_Name,
                                float i_TargetRotateAngle,
                                TimeSpan i_AnimationLength,
                                bool i_ResetAfterFinish)
            : base(i_Name, i_AnimationLength)
        {
            m_TargetRotateAngle = i_TargetRotateAngle;
            ResetAfterFinish = i_ResetAfterFinish;
            m_RotateLength = i_AnimationLength;
        }

        /// <summary>
        /// Overriden to calculate angular velocity for bound sprite
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();
            m_AngularVelocity = (m_TargetRotateAngle - BoundSprite.Rotation) / (float)m_RotateLength.TotalSeconds;
        }

        /// <summary>
        /// Animates the rotation
        /// </summary>
        /// <param name="i_GameTime">Time since last run</param>
        protected override void     DoFrame(GameTime i_GameTime)
        {
            BoundSprite.RotationOrigin = BoundSprite.SpriteCenter;
            BoundSprite.PositionOrigin = BoundSprite.RotationOrigin;
            BoundSprite.AngularVelocity = m_AngularVelocity;
        }

        /// <summary>
        /// Resets animation to initial state
        /// </summary>
        /// <param name="i_AnimationLength">Animation time</param>
        public override void    Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);
            BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
            BoundSprite.AngularVelocity = m_OriginalSpriteInfo.AngularVelocity;
            BoundSprite.RotationOrigin = m_OriginalSpriteInfo.RotationOrigin;
            BoundSprite.PositionOrigin = m_OriginalSpriteInfo.PositionOrigin;
        }
    }
}
