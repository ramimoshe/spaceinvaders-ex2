using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    public class ScaleAnimation : SpriteAnimation
    {
        private TimeSpan m_TimeLeftForScale;
        private TimeSpan m_ScaleLength;
        private Vector2 m_TargetScaleSize;

        // CTORs
        public ScaleAnimation(  string i_Name,
                                TimeSpan i_ScaleLength,
                                Vector2 i_TargetScaleSize,
                                TimeSpan i_AnimationLength,
                                bool i_ResetAfterFinish)
            : base(i_Name, i_AnimationLength)
        {
            m_TargetScaleSize = i_TargetScaleSize;
            m_ScaleLength = i_ScaleLength;
            m_TimeLeftForScale = i_ScaleLength;
            m_ResetAfterFinish = i_ResetAfterFinish;
        }

        private Vector2 ScalePerSecond
        {
            get
            {
                return new Vector2( (m_TargetScaleSize.X - BoundSprite.Scale.X) / (float) m_ScaleLength.TotalSeconds,
                                    (m_TargetScaleSize.Y - BoundSprite.Scale.Y) / (float) m_ScaleLength.TotalSeconds);
            }
        }

        private bool XScaleOut
        {
            get
            {
                return m_TargetScaleSize.X > m_OriginalSpriteInfo.Scale.X;
            }
        }

        private bool YScaleOut
        {
            get
            {
                return m_TargetScaleSize.Y > m_OriginalSpriteInfo.Scale.Y;
            }
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            m_TimeLeftForScale -= i_GameTime.ElapsedGameTime;
            BoundSprite.Scale += (ScalePerSecond * (float) i_GameTime.ElapsedGameTime.TotalSeconds);

            if ((XScaleOut && BoundSprite.Scale.X >= m_TargetScaleSize.X ||
                 !XScaleOut && BoundSprite.Scale.X <= m_TargetScaleSize.X ) &&
                (YScaleOut && BoundSprite.Scale.Y >= m_TargetScaleSize.Y ||
                 !YScaleOut && BoundSprite.Scale.Y >= m_TargetScaleSize.Y))
            {
                IsFinished = true;
            }
       }
    }
}
