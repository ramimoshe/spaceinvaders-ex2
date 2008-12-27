using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    public class PulseAnimation : ScaleAnimation
    {
        TimeSpan m_TimeLeftForPulse;
        TimeSpan m_PulseTime;
        Vector2 m_MinScale;

        /// <summary>
        /// Creates a new scale animation
        /// </summary>
        /// <param name="i_Name">Animation name</param>
        /// <param name="i_MinScale">Minimal scale factor</param>
        /// <param name="i_MaxScale">Maximal scale factor</param>
        /// <param name="i_AnimationLength">Animation time</param>
        /// <param name="i_ResetAfterFinish">Specifies if reset should be done after animation is done.</param>
        /// <param name="i_PulseTime">Specifies the time between each pulse
        /// as 1 pulse=1 Scale in\out</param>
        public PulseAnimation(
            string i_Name,
            Vector2 i_MinScale,
            Vector2 i_MaxScale,
            TimeSpan i_AnimationLength,
            bool i_ResetAfterFinish,
            TimeSpan i_PulseTime) 
            : base(i_Name, i_MaxScale, i_AnimationLength, i_ResetAfterFinish)
        {
            m_TimeLeftForPulse = i_PulseTime;
            m_PulseTime = i_PulseTime;
            m_MinScale = i_MinScale;
            m_ScaleLength = m_PulseTime;
        }

        public override void Initialize()
        {
            base.Initialize();
            BoundSprite.Scale = m_MinScale;
        }

        protected override Vector2 ScalePerSecond
        {
            get
            {
                return (m_TargetScaleSize - m_MinScale) / (float)m_ScaleLength.TotalSeconds;
            }
        }

        /// <summary>
        /// Returns the position shift per second to maintain sprite centered
        /// </summary>
        protected override Vector2 PositionShiftPerSecond
        {
            get
            {
                return Vector2.Zero;
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            base.DoFrame(i_GameTime);

            m_TimeLeftForPulse -= i_GameTime.ElapsedGameTime;

            if (m_TimeLeftForPulse <= TimeSpan.Zero)
            {
                m_TimeLeftForPulse = m_PulseTime;
                Reset();
                ReverseScale();
            }
        }

        private void ReverseScale()
        {
            BoundSprite.Scale = m_TargetScaleSize;
            Vector2 temp = m_MinScale;
            m_MinScale = m_TargetScaleSize;
            m_TargetScaleSize = temp;
            m_PositionShiftPerSecond = new Vector2(
                        BoundSprite.WidthAfterScale * ScalePerSecond.X / 2,
                        BoundSprite.HeightAfterScale * ScalePerSecond.Y / 2);
        }
    }
}
