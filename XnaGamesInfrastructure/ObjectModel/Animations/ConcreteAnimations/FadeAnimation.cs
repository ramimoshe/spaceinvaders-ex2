using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    public class FadeAnimation : SpriteAnimation
    {
        private bool m_ReverseFade = true;
        private TimeSpan m_FadeLength;
        private float m_MinOpacity = 0;
        private float m_MaxOpacity = 1;
        private bool m_FadeOut = true;

        // CTORs
        public  FadeAnimation(  string i_Name, 
                                bool  i_ReverseFade,
                                float i_MinOpacity,
                                float i_MaxOpacity,
                                bool i_FadeOut, 
                                TimeSpan i_FadeLength, 
                                TimeSpan i_AnimationLength,
                                bool i_ResetAfterFinish)
            : this(i_Name, i_FadeLength, i_AnimationLength)
        {
            m_MinOpacity = i_MinOpacity;
            m_MaxOpacity = i_MaxOpacity;
            m_ReverseFade = i_ReverseFade;
            m_FadeOut = i_FadeOut;
            m_ResetAfterFinish = i_ResetAfterFinish;
        }

        public FadeAnimation(   string i_Name,
                                TimeSpan i_FadeLength, 
                                TimeSpan i_AnimationLength)
            :base( i_Name, i_AnimationLength)
        {
            m_FadeLength = i_FadeLength;
        }

        private float OpacityPerSecond 
        {
            get
            {
                return (m_MaxOpacity - m_MinOpacity) / (float)m_FadeLength.TotalSeconds;
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            Vector4 tint = BoundSprite.TintColor.ToVector4();
            float opacity = tint.W;

            opacity += OpacityPerSecond * (m_FadeOut ? -1 : 1) * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            tint.W = opacity;
            BoundSprite.TintColor = new Color(tint);

            if (opacity < m_MinOpacity || opacity > m_MaxOpacity ||
                opacity < 0 || opacity > 1)
            {
                if (m_ReverseFade)
                {
                    m_FadeOut = !m_FadeOut;
                }
                else
                {
                    IsFinished = true;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            BoundSprite.TintColor = m_OriginalSpriteInfo.TintColor;
        }
    }
}
