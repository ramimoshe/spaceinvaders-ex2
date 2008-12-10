using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /// <summary>
    /// Run animations one after the other
    /// </summary>
    public class SequencialAnimation : CompositeAnimation
    {
        public SequencialAnimation(
            string i_Name,
            TimeSpan i_AnimationLength,
            Sprite i_BoundSprite,
            params SpriteAnimation[] i_Animations)
            : base(i_Name, i_AnimationLength, i_BoundSprite, i_Animations)
        {
        }

        /// <summary>
        /// Perform the current animation update
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the game time</param>
        protected override void DoFrame(GameTime i_GameTime)
        {
            bool allFinished = true;
            foreach (SpriteAnimation animation in m_AnimationsList)
            {
                if (!animation.IsFinished)
                {
                    animation.Animate(i_GameTime);
                    allFinished = false;
                    break; // we are not going to call all our animations until this one is done
                }
            }

            if (allFinished)
            {
                this.IsFinished = true;
            }
        }
    }
}
