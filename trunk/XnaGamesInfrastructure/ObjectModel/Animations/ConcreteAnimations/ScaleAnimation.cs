using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /// <summary>
    /// Performs a scaling animation that changes a sprite scale
    /// factor
    /// </summary>
    public class ScaleAnimation : SpriteAnimation
    {
        protected TimeSpan m_ScaleLength;
        protected Vector2 m_TargetScaleSize;

        /// <summary>
        /// Creates a new scale animation
        /// </summary>
        /// <param name="i_Name">Animation name</param>
        /// <param name="i_TargetScaleSize">The scale to be reached by the bounding sprite</param>
        /// <param name="i_AnimationLength">Animation time</param>
        /// <param name="i_ResetAfterFinish">Specifies if reset should be done after animation is done.</param>
        public ScaleAnimation(
            string i_Name,
            Vector2 i_TargetScaleSize,
            TimeSpan i_AnimationLength,
            bool i_ResetAfterFinish)
            : base(i_Name, i_AnimationLength)
        {
            m_TargetScaleSize = i_TargetScaleSize;
            m_ScaleLength = i_AnimationLength;
            m_ResetAfterFinish = i_ResetAfterFinish;
        }

        /// <summary>
        /// Overrided to perform calculation on original bound sprite
        /// </summary>
        public override void     Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Gets the original sprite scale
        /// </summary>
        virtual protected Vector2   OriginalSpriteScale
        {
            get
            {
                return m_OriginalSpriteInfo.Scale;
            }
        }

        /// <summary>
        /// Returns the scale change per second
        /// </summary>
        protected virtual Vector2 ScalePerSecond
        {
            get
            {
                return new Vector2(
                    (m_TargetScaleSize.X - OriginalSpriteScale.X) / 
                    (float)m_ScaleLength.TotalSeconds,
                    (m_TargetScaleSize.Y - OriginalSpriteScale.Y) /
                    (float)m_ScaleLength.TotalSeconds);
            }
        }

        /// <summary>
        /// Returns the position shift per second to maintain sprite centered
        /// </summary>
        private Vector2     PositionShiftPerSecond
        {
            get
            {
                return new Vector2(
                    m_OriginalSpriteInfo.WidthAfterScale * ScalePerSecond.X,
                    m_OriginalSpriteInfo.HeightAfterScale * ScalePerSecond.Y);
            }
        }

        /// <summary>
        /// Animates the scale
        /// </summary>
        /// <param name="i_GameTime">Time since last run</param>
        protected override void     DoFrame(GameTime i_GameTime)
        {
            float totalSeconds = (float) i_GameTime.ElapsedGameTime.TotalSeconds;

            // Sprite's position origin and scale are modified according to modification rates
            BoundSprite.Scale += ScalePerSecond * totalSeconds;
            BoundSprite.PositionOrigin += PositionShiftPerSecond * totalSeconds;
        }

        /// <summary>
        /// Resets the animation to original state
        /// </summary>
        /// <param name="i_AnimationLength">Animation length</param>
        public override void    Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);
            BoundSprite.Scale = m_OriginalSpriteInfo.Scale;
            BoundSprite.PositionOrigin = m_OriginalSpriteInfo.PositionOrigin;
        }
    }
}
