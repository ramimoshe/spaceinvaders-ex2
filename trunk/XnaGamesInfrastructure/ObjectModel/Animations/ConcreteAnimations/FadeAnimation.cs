using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /// <summary>
    /// Fades the bound sprite according to specified parameters
    /// </summary>
    public class FadeAnimation : SpriteAnimation
    {
        private bool m_ReverseFade = false;
        private TimeSpan m_FadeLength;
        private float m_MinOpacity = 0;
        private float m_MaxOpacity = 1;
        private bool m_FadeOut = true;

        /// <summary>
        /// Creates a new fade animation
        /// </summary>
        /// <param name="i_Name">Animation name</param>
        /// <param name="i_ReverseFade">Specifies whether fade should be reversed when 
        /// reaching min/max opacity value</param>
        /// <param name="i_MinOpacity">Minimum opacity value</param>
        /// <param name="i_MaxOpacity">Maximum opacity value</param>
        /// <param name="i_FadeOut">Specifies whether to begin animation by fading out/in.
        /// Fade out means opacity is decreased.</param>
        /// <param name="i_FadeLength">Length for fade in/out).
        /// This is specified to set animation speed</param>
        /// <param name="i_AnimationLength">Length for entire animation</param>
        /// <param name="i_ResetAfterFinish">Specifies whether animations will be reset when done</param>
        public  FadeAnimation(  
                                string i_Name, 
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

        /// <summary>
        /// Creates a new fade animation with default opacity values
        /// </summary>
        /// <param name="i_Name">Animation name</param>
        /// <param name="i_FadeOut">Specifies whether to begin animation by fading out/in.
        /// Fade out means opacity is decreased.</param>
        /// <param name="i_FadeLength">Length for fade in/out).
        /// This is specified to set animation speed</param>
        /// <param name="i_AnimationLength">Length for entire animation</param>
        public  FadeAnimation(   
                                string i_Name,
                                TimeSpan i_FadeLength, 
                                TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            m_FadeLength = i_FadeLength;
        }

        /// <summary>
        /// Gets the opacity change per second
        /// </summary>
        private float   OpacityPerSecond 
        {
            get
            {
                return (m_FadeOut ? -1 : 1) * (m_MaxOpacity - m_MinOpacity) / 
                    (float)m_FadeLength.TotalSeconds;
            }
        }

        /// <summary>
        /// Animates the fade
        /// </summary>
        /// <param name="i_GameTime">Game time since last run</param>
        protected override void     DoFrame(GameTime i_GameTime)
        {
            // Adding the opacity delta in bound sprite's tint color
            Vector4 tint = BoundSprite.TintColor.ToVector4();
            float opacity = tint.W;
            opacity += OpacityPerSecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            tint.W = opacity;
            BoundSprite.TintColor = new Color(tint);

            // Checking if opacity reached is bounding limits
            if (opacity < m_MinOpacity || opacity > m_MaxOpacity ||
                opacity < 0 || opacity > 1)
            {
                // Checking if animation is to be ended or fade is reversed
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

        /// <summary>
        /// Resets the fade animation.
        /// </summary>
        public override void    Reset()
        {
            base.Reset();
            BoundSprite.TintColor = m_OriginalSpriteInfo.TintColor;
        }
    }
}
