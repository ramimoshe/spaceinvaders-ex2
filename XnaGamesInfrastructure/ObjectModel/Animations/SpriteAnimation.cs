using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations
{
    /// <summary>
    /// Used to notify observer that animation ended
    /// </summary>
    /// <param name="i_Animation">The animation which ended</param>
    public delegate void    AnimationFinishedEventHandler(SpriteAnimation i_Animation);

    /// <summary>
    /// Base class for all sprite animations
    /// </summary>
    public abstract class SpriteAnimation
    {
        // Data members
        protected internal Sprite m_OriginalSpriteInfo;

        private Sprite      m_BoundSprite;
        private bool        m_IsFinished = false;
        private bool        m_Enabled = true;
        private TimeSpan    m_TimeLeft;
        
        /// <summary>
        /// Defines animations length. If set to TimeSpan.Zero then animation is inifinite.
        /// </summary>
        private TimeSpan    m_AnimationLength;
        
        private bool        m_IsFinite = true;
        private string      m_Name;
        private bool        m_Initialized = false;
        protected bool      m_ResetAfterFinish = true;

        /// <summary>
        /// Gets/Sets whether animation will reset itself when done executing
        /// </summary>
        public bool ResetAfterFinish
        {
            get { return m_ResetAfterFinish; }
            set { m_ResetAfterFinish = value; }
        }

        /// <summary>
        /// Observers for this event will be notified by event is finished
        /// </summary>
        public event    AnimationFinishedEventHandler Finished;

        /// <summary>
        /// Performed when IsFinished is marked true
        /// </summary>
        protected virtual void  OnFinished()
        {
            // Animation is reset if marked
            if (m_ResetAfterFinish)
            {
               Reset();
               this.m_IsFinished = true;
            }

            // observers are notified here
            if (Finished != null)
            {
                Finished(this);
            }
        }

        /// <summary>
        /// Create a new animation
        /// </summary>
        /// <param name="i_Name">Animation name</param>
        /// <param name="i_AnimationLength">Length of animation</param>
        protected   SpriteAnimation(string i_Name, TimeSpan i_AnimationLength)
        {
            m_Name = i_Name;
            m_AnimationLength = i_AnimationLength;
        }

        /// <summary>
        /// Gets/Sets animation's bound sprite
        /// </summary>
        protected internal Sprite   BoundSprite
        {
            get { return m_BoundSprite; }
            set { m_BoundSprite = value; }
        }

        /// <summary>
        /// Gets animation name
        /// </summary>
        public string   Name
        {
            get { return m_Name; }
        }

        /// <summary>
        /// Gets/Sets whether animation is enabled (running)
        /// </summary>
        public bool     Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        /// <summary>
        /// Gets whethen animation length is finite
        /// </summary>
        public bool     IsFinite
        {
            get { return this.m_IsFinite; }
        }

        /// <summary>
        /// Initializes the animation for the first time by storing bound's sprite 
        /// initial state
        /// </summary>
        public virtual void     Initialize()
        {
            // Initialize is called only once
            if (!m_Initialized)
            {
                m_Initialized = true;

                CloneSpriteInfo();

                Reset();
            }
        }

        /// <summary>
        /// Stores original bound sprite state
        /// </summary>
        protected virtual void  CloneSpriteInfo()
        {
            if (m_OriginalSpriteInfo == null)
            {
                m_OriginalSpriteInfo = m_BoundSprite.ShallowClone();
            }
        }

        /// <summary>
        /// Reset the animation to it's initial state
        /// </summary>
        public virtual void  Reset()
        {
            Reset(m_AnimationLength);
        }

        /// <summary>
        /// Reset the animation to it's initial state
        /// </summary>
        /// <param name="i_AnimationLength">new animation time</param>
        public virtual void  Reset(TimeSpan i_AnimationLength)
        {
            // initialize is called only once
            if (!m_Initialized)
            {
                Initialize();
            }

            // Reseting animation length and status
            m_AnimationLength = i_AnimationLength;
            m_TimeLeft = m_AnimationLength;
            this.IsFinished = false;
            m_IsFinite = i_AnimationLength != TimeSpan.Zero;
        }

        /// <summary>
        /// Stops the amination (Enabled = false)
        /// </summary>
        public void     Pause()
        {
            this.Enabled = false;
        }

        /// <summary>
        /// Starts the animation (Enabled = true)
        /// </summary>
        public void     ReleasePause()
        {
            m_Enabled = true;
        }

        /// <summary>
        /// Restarts the animation
        /// </summary>
        public virtual void     Restart()
        {
            Restart(m_AnimationLength);
        }

        /// <summary>
        /// Resets the animation and starts it
        /// </summary>
        /// <param name="i_AnimationLength">new time for animation</param>
        public virtual void     Restart(TimeSpan i_AnimationLength)
        {
            Reset(i_AnimationLength);
            ReleasePause();
        }

        /// <summary>
        /// Gets/Sets whether animation is done
        /// </summary>
        public bool     IsFinished
        {
            get 
            { 
                return this.m_IsFinished; 
            }

            protected set
            {
                // value is changed only if differs
                if (value != m_IsFinished)
                {
                    m_IsFinished = value;

                    // If state is changed to finished, then OnFinish is called
                    if (m_IsFinished == true)
                    {
                        OnFinished();
                    }
                }
            }
        }

        /// <summary>
        /// Runs the animation
        /// </summary>
        /// <param name="i_GameTime">Game time since last run</param>
        public void     Animate(GameTime i_GameTime)
        {
            // Validating animation is initialized
            if (!m_Initialized)
            {
                Initialize();
            }

            // Validating animations is enabled and not done
            if (this.Enabled && !this.IsFinished)
            {
                // handlind a time limited animation
                if (this.IsFinite)
                {
                    // check if we should stop animating:
                    m_TimeLeft -= i_GameTime.ElapsedGameTime;

                    if (m_TimeLeft.TotalSeconds < 0)
                    {
                        this.IsFinished = true;
                    }
                }

                // Animation is done only when it has not finished
                if (!this.IsFinished)
                {
                    // we are still required to animate:
                    DoFrame(i_GameTime);
                }
            }
        }

        /// <summary>
        /// This method performs the actual animation for the bound sprite
        /// </summary>
        /// <param name="i_GameTime">Time since last run</param>
        protected abstract void     DoFrame(GameTime i_GameTime);
    }
}
