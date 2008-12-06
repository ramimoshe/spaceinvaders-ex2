using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    public class FadeAnimation : SpriteAnimation
    {
        private TimeSpan m_FadeLength;
        private bool m_ReverseFade = false;
        private TimeSpan m_TimeLeftForNextFade;
        private float m_MinOpacity = 0;
        private float m_MaxOpacity = 1;
        private float m_OpacityPerSecond;
        private bool m_FadeOut = true;

        // CTORs
        public  FadeAnimation(  string i_Name, 
                                TimeSpan i_FadeLength, 
                                bool i_ReverseFade,
                                float i_MinOpacity,
                                float i_MaxOpacity,
                                bool i_FadeOut, 
                                TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_FadeLength = i_FadeLength;
            m_MinOpacity = i_MinOpacity;
            m_MaxOpacity = i_MaxOpacity;
            m_ReverseFade = i_ReverseFade;
            m_FadeOut = i_FadeOut;
            m_TimeLeftForNextFade = i_FadeLength;
            m_OpacityPerSecond = (m_MaxOpacity - m_MinOpacity) / (float)m_FadeLength.TotalSeconds;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForNextFade -= i_GameTime.ElapsedGameTime;
            Vector4 tint = BoundSprite.TintColor.ToVector4();
            float opacity = tint.W;

            opacity += m_OpacityPerSecond * (m_FadeOut ? -1 : 1) * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            tint.W = opacity;
            BoundSprite.TintColor = new Color(tint);

            if (opacity < 0 || opacity > 1 || 
                m_TimeLeftForNextFade.TotalSeconds < 0)
            {
                if (m_ReverseFade)
                {
                    m_FadeOut = !m_FadeOut;
                    m_TimeLeftForNextFade = m_FadeLength;
                }
                else
                {
                    this.IsFinished = true;
                }
            }
        }

        public override void Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            //this.BoundSprite.Visible = m_OriginalSpriteInfo.Visible;
        }
    }
}
