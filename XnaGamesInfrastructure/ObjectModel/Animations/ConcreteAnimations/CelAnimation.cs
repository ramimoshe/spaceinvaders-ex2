
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    public class CelAnimation : SpriteAnimation
    {
        private TimeSpan m_FrameLength;
        private TimeSpan m_TimeLeftForFrame;
        private bool m_Loop = true;
        private int m_CurrCell = 0;
        private int m_NumOfCels = 1;

        // CTORs
        public CelAnimation(TimeSpan i_FrameLength, int i_NumOfCels, TimeSpan i_AnimationLength)
            : base("CelAnimation", i_AnimationLength)
        {
            this.m_FrameLength = i_FrameLength;
            this.m_TimeLeftForFrame = i_FrameLength;
            this.m_NumOfCels = i_NumOfCels;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        public void NextFrame()
        {
            m_CurrCell++;
            if (m_CurrCell >= m_NumOfCels)
            {
                if (m_Loop)
                {
                    m_CurrCell = 0;
                }
                else
                {
                    m_CurrCell = m_NumOfCels - 1; // lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        public override void Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_FrameLength != TimeSpan.Zero)
            {
                m_TimeLeftForFrame -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForFrame.TotalSeconds <= 0)
                {
                    // we have elapsed, so blink
                    NextFrame();
                    m_TimeLeftForFrame = m_FrameLength;
                }
            }

            if (this.BoundSprite.SourceRectangle != null)
            {
                Rectangle r = (Rectangle)this.BoundSprite.SourceRectangle;
                this.BoundSprite.SourceRectangle = new Rectangle(
                    m_CurrCell * r.Width,
                    r.Top,
                    r.Width,
                    r.Height);
            }
        }
    }
}
