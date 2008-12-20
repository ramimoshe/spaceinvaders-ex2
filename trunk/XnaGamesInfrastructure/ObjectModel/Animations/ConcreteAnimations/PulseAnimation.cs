using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /*
    public class PulseAnimation : SpriteAnimation
    {
        protected Vector2 m_Scale;
        public Vector2 Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        protected float m_PulsePerSecond;
        
        public float PulsePerSecond
        {
            get { return m_PulsePerSecond; }
            set { m_PulsePerSecond = value; }
        }

        private bool m_Shrinking;
        private float m_TargetScale;
        private float m_SourceScale;
        private float m_DeltaScale;

        public PulseAnimation(string i_Name, TimeSpan i_AnimationLength, float i_TargetScale, float i_PulsePerSecond)
            : base(i_Name, i_AnimationLength)
        {
            m_Scale = i_TargetScale;
            m_PulsePerSecond = i_PulsePerSecond;
        }

        public override void Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            this.BoundSprite.Scale = m_OriginalSpriteInfo.Scale;

            m_SourceScale = m_OriginalSpriteInfo.Scale;
            m_TargetScale = m_Scale;
            m_DeltaScale = m_TargetScale - m_SourceScale;
            m_Shrinking = m_DeltaScale < 0;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_Shrinking)
            {
                if (this.BoundSprite.Scale > m_TargetScale)
                {
                    this.BoundSprite.Scale -= totalSeconds * 2*m_PulsePerSecond * m_DeltaScale;
                }
                else
                {
                    this.BoundSprite.Scale = m_TargetScale;
                    m_Shrinking = false;
                    m_TargetScale = m_SourceScale;
                    m_SourceScale = this.BoundSprite.Scale;
                }
            }
            else 
            {
                if (this.BoundSprite.Scale < m_TargetScale)
                {
                    this.BoundSprite.Scale += totalSeconds * 2*m_PulsePerSecond * m_DeltaScale;
                }
                else
                {
                    this.BoundSprite.Scale = m_TargetScale;
                    m_Shrinking = true;
                    m_TargetScale = m_SourceScale;
                    m_SourceScale = this.BoundSprite.Scale;
                }
            }
        }
    }
     */
}
