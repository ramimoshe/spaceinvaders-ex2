using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations
{
    /// <summary>
    /// Handles multiple animations assigned to the same bound sprite
    /// </summary>
    public class CompositeAnimation : SpriteAnimation
    {
        /// <summary>
        /// Enables to get animations by name
        /// </summary>
        private readonly Dictionary<string, SpriteAnimation> m_AnimationsDictionary = 
            new Dictionary<string, SpriteAnimation>();

        /// <summary>
        /// Enables itteration on animations
        /// </summary>
        protected readonly List<SpriteAnimation> m_AnimationsList = new List<SpriteAnimation>();

        /// <summary>
        /// Initializes a new animation for the specified sprite with default name 
        /// ("AnimationManager") and length (infinite)
        /// </summary>
        /// <param name="i_BoundSprite">The sprite which is animated</param>
        public  CompositeAnimation(Sprite i_BoundSprite)
            : this("AnimationsManager", TimeSpan.Zero, i_BoundSprite, new SpriteAnimation[] { })
        {
            this.Enabled = false;
        }
        
        /// <summary>
        /// Creates a new composite animations from an instantiated list of animations
        /// </summary>
        /// <param name="i_Name">Animation name</param>
        /// <param name="i_AnimationLength">Animation length</param>
        /// <param name="i_BoundSprite">The animated sprite</param>
        /// <param name="i_Animations">List of animations (must be initialized)</param>
        public  CompositeAnimation(
            string i_Name,
            TimeSpan i_AnimationLength,
            Sprite i_BoundSprite,
            params SpriteAnimation[] i_Animations)
            : base(i_Name, i_AnimationLength)
        {
            this.BoundSprite = i_BoundSprite;

            foreach (SpriteAnimation animation in i_Animations)
            {
                this.Add(animation);
            }
        }

        /// <summary>
        /// Adds a new animations to list and dictionary
        /// </summary>
        /// <param name="i_Animation">New animations to be added. Name must be unique</param>
        public void     Add(SpriteAnimation i_Animation)
        {
            i_Animation.BoundSprite = this.BoundSprite;
            i_Animation.Enabled = true;
            m_AnimationsDictionary.Add(i_Animation.Name, i_Animation);
            m_AnimationsList.Add(i_Animation);

            // Registers itself as an observer for child animations
            i_Animation.Finished += new AnimationFinishedEventHandler(childAnimation_Finished);
        }

        /// <summary>
        /// Initiated by each observerd child animations, to check whether all child
        /// animations are done. Then composite animation will notify it is done.
        /// </summary>
        /// <param name="i_ChildAnimation"></param>
        private void    childAnimation_Finished(SpriteAnimation i_ChildAnimation)
        {
            bool isFinished = true;

            // Checking if all animations are done
            foreach (SpriteAnimation animation in this.m_AnimationsList)
            {
                if (!animation.IsFinished)
                {
                    isFinished = false;
                    break;
                }
            }

            // If all animations are done then setting composite animation as done too
            if (isFinished)
            {
                this.IsFinished = true;
            }
        }

        /// <summary>
        /// Removes animations from composite animation
        /// </summary>
        /// <param name="i_AnimationName">The removed animation name</param>
        public void     Remove(string i_AnimationName)
        {
            SpriteAnimation animationToRemove;
            m_AnimationsDictionary.TryGetValue(i_AnimationName, out animationToRemove);

            // Validating animation exists in lists
            if (animationToRemove != null)
            {
                m_AnimationsDictionary.Remove(i_AnimationName);
                m_AnimationsList.Remove(animationToRemove);
            }
        }

        /// <summary>
        /// Gets a specific animations from animation list
        /// </summary>
        /// <param name="i_Name">The specified animation name</param>
        /// <returns>The requested animation</returns>
        public SpriteAnimation  this[string i_Name]
        {
            get
            {
                SpriteAnimation retVal = null;
                m_AnimationsDictionary.TryGetValue(i_Name, out retVal);
                return retVal;
            }            
        }

        /// <summary>
        /// Restarts all child animations
        /// </summary>
        public override void    Restart(TimeSpan i_AnimationLength)
        {
            base.Restart(i_AnimationLength);

            // each child animation is restarted
            foreach (SpriteAnimation animation in m_AnimationsList)
            {
                animation.Restart();
            }
        }

        /// <summary>
        /// Resets all child animations
        /// </summary>
        /// <param name="i_AnimationLength"></param>
        public override void    Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            foreach (SpriteAnimation animation in m_AnimationsList)
            {
                animation.Reset();
            }
        }

        /// <summary>
        /// Creates a clone of bound sprite initial state in all animations
        /// </summary>
        protected override void     CloneSpriteInfo()
        {
            base.CloneSpriteInfo();

            foreach (SpriteAnimation animation in m_AnimationsList)
            {
                animation.m_OriginalSpriteInfo = m_OriginalSpriteInfo;
            }
        }

        /// <summary>
        /// Animates all child animations
        /// </summary>
        /// <param name="i_GameTime">Game time since last run</param>
        protected override void     DoFrame(GameTime i_GameTime)
        {
            foreach (SpriteAnimation animation in m_AnimationsList)
            {
                animation.Animate(i_GameTime);
            }
        }
    }
}
