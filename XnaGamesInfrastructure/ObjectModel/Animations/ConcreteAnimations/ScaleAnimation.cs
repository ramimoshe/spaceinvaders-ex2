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
        private TimeSpan m_ScaleLength;
        private Vector2 m_TargetScaleSize;

        // CTORs
        public ScaleAnimation(  string i_Name,
                                Vector2 i_TargetScaleSize,
                                TimeSpan i_AnimationLength,
                                bool i_ResetAfterFinish)
            : base(i_Name, i_AnimationLength)
        {
            m_TargetScaleSize = i_TargetScaleSize;
            m_ScaleLength = i_AnimationLength;
            m_ResetAfterFinish = i_ResetAfterFinish;
        }

        private Vector2 ScalePerSecond
        {
            get
            {
                return new Vector2( (m_TargetScaleSize.X - m_OriginalSpriteInfo.Scale.X) / (float) m_ScaleLength.TotalSeconds,
                                    (m_TargetScaleSize.Y - m_OriginalSpriteInfo.Scale.Y) / (float)m_ScaleLength.TotalSeconds);
            }
        }

        private Vector2 PositionShiftPerSecond
        {
            get
            {
                float targetWidth = m_OriginalSpriteInfo.WidthBeforeScale * ScalePerSecond.X;
                float targetHeight = m_OriginalSpriteInfo.HeightBeforeScale * ScalePerSecond.Y;
                return new Vector2(targetWidth / 2, targetHeight / 2);
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
            Vector2 position = BoundSprite.PositionOrigin;
            BoundSprite.Scale += (ScalePerSecond * (float) i_GameTime.ElapsedGameTime.TotalSeconds);
            BoundSprite.PositionOrigin += PositionShiftPerSecond * (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if ((XScaleOut && BoundSprite.Scale.X >= m_TargetScaleSize.X ||
                 !XScaleOut && BoundSprite.Scale.X <= m_TargetScaleSize.X ) &&
                (YScaleOut && BoundSprite.Scale.Y >= m_TargetScaleSize.Y ||
                 !YScaleOut && BoundSprite.Scale.Y <= m_TargetScaleSize.Y))
            {
                IsFinished = true;
            }
        }

        public override void Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);
            BoundSprite.Scale = m_OriginalSpriteInfo.Scale;
            BoundSprite.PositionOrigin = m_OriginalSpriteInfo.PositionOrigin;
        }
    }
}
