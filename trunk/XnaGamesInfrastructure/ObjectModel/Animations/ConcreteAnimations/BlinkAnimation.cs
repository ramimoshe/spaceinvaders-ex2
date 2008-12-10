using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /// <summary>
    /// A blinking animation that changes a sprite visible property
    /// </summary>
    public class BlinkAnimation : SpriteAnimation
    {
        private TimeSpan m_BlinkLength;
        private TimeSpan m_TimeLeftForNextBlink;
        private bool m_IsVisible = true;

        /// <summary>
        /// Property that gets the animation length
        /// </summary>
        public TimeSpan     BlinkLength
        {
            get { return m_BlinkLength; }
            set { m_BlinkLength = value; }
        }

        public BlinkAnimation(
            string i_Name, 
            TimeSpan i_BlinkLength, 
            TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.m_BlinkLength = i_BlinkLength;
            this.m_TimeLeftForNextBlink = i_BlinkLength;
        }

        public BlinkAnimation(
            TimeSpan i_BlinkLength, 
            TimeSpan i_AnimationLength)
            : this("Blink", i_BlinkLength, i_AnimationLength)
        {
            this.m_BlinkLength = i_BlinkLength;
            this.m_TimeLeftForNextBlink = i_BlinkLength;
        }

        /// <summary>
        /// Perform the animation next move
        /// </summary>
        /// <param name="i_GameTime">A snapshot to the game time</param>
        protected override void     DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextBlink -= i_GameTime.ElapsedGameTime;
            if (m_TimeLeftForNextBlink.TotalSeconds < 0)
            {
                // we have elapsed, so blink
                m_IsVisible = !m_IsVisible;
                m_TimeLeftForNextBlink = m_BlinkLength;
            }

            this.BoundSprite.Visible = m_IsVisible;
        }

        /// <summary>
        /// Resets the animation to original state
        /// </summary>
        /// <param name="i_AnimationLength">Animation length</param>
        public override void    Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
