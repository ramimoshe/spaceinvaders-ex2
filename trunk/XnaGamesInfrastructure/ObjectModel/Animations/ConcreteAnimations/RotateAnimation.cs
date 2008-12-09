using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    public class RotateAnimation : SpriteAnimation
    {
        private float m_TargetRotateAngle;
        private TimeSpan m_RotateLength;
        private float m_AngularVelocity;

        // CTORs
        public RotateAnimation( string i_Name,
                                float i_TargetRotateAngle,
                                TimeSpan i_AnimationLength,
                                bool i_ResetAfterFinish)
            : base(i_Name, i_AnimationLength)
        {
            m_TargetRotateAngle = i_TargetRotateAngle;
            ResetAfterFinish = i_ResetAfterFinish;
            m_RotateLength = i_AnimationLength;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_AngularVelocity = (m_TargetRotateAngle - BoundSprite.Rotation) / (float)m_RotateLength.TotalSeconds;
        }

        protected override void OnFinished()
        {
            BoundSprite.AngularVelocity = m_OriginalSpriteInfo.AngularVelocity;
            base.OnFinished();
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.RotationOrigin = BoundSprite.SpriteCenter;
            //BoundSprite.PositionOrigin = new Vector2(BoundSprite.RotationOrigin.X, -BoundSprite.HeightAfterScale);
            BoundSprite.AngularVelocity = m_AngularVelocity;
        }

        public override void Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);
            BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
            BoundSprite.AngularVelocity = m_OriginalSpriteInfo.AngularVelocity;
            BoundSprite.RotationOrigin = m_OriginalSpriteInfo.RotationOrigin;
            BoundSprite.PositionOrigin = m_OriginalSpriteInfo.PositionOrigin;
        }
    }
}
