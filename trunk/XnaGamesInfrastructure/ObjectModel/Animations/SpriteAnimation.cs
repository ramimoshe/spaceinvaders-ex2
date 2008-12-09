using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations
{
    public delegate void AnimationFinishedEventHandler (SpriteAnimation i_Animation);

    public abstract class SpriteAnimation
    {
        protected internal Sprite m_OriginalSpriteInfo;

        private Sprite      m_BoundSprite;
        private bool        m_IsFinished = false;
        private bool        m_Enabled = true;
        private TimeSpan    m_TimeLeft;
        private TimeSpan    m_AnimationLength;
        private bool        m_IsFinite = true;
        private string      m_Name;
        private bool        m_Initialized = false;
        protected bool      m_ResetAfterFinish = true;

        public bool ResetAfterFinish
        {
            get { return m_ResetAfterFinish; }
            set { m_ResetAfterFinish = value; }
        }

        public event AnimationFinishedEventHandler Finished;

        protected virtual void OnFinished()
        {
            if (m_ResetAfterFinish)
            {
               Reset();
               this.m_IsFinished = true;
            }

            if (Finished != null)
            {
                Finished(this);
            }
        }

        protected SpriteAnimation(string i_Name, TimeSpan i_AnimationLength)
        {
            m_Name = i_Name;
            m_AnimationLength = i_AnimationLength;
        }

        protected internal Sprite BoundSprite
        {
            get { return m_BoundSprite; }
            set { m_BoundSprite = value; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public bool Enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        public bool IsFinite
        {
            get { return this.m_IsFinite; }
        }

        public virtual void Initialize()
        {
            if (!m_Initialized)
            {
                m_Initialized = true;

                CloneSpriteInfo();

                Reset();
            }
        }

        protected virtual void CloneSpriteInfo()
        {
            if (m_OriginalSpriteInfo == null)
            {
                m_OriginalSpriteInfo = m_BoundSprite.ShallowClone();
            }
        }

        public virtual void Reset()
        {
            Reset(m_AnimationLength);
        }

        public virtual void Reset(TimeSpan i_AnimationLength)
        {
            if (!m_Initialized)
            {
                Initialize();
            }

            m_AnimationLength = i_AnimationLength;
            m_TimeLeft = m_AnimationLength;
            this.IsFinished = false;
            m_IsFinite = i_AnimationLength != TimeSpan.Zero;
        }

        public void Pause()
        {
            this.Enabled = false;
        }

        public void ReleasePause()
        {
            m_Enabled = true;
        }

        public virtual void Restart()
        {
            Restart(m_AnimationLength);
        }

        public virtual void Restart(TimeSpan i_AnimationLength)
        {
            Reset(i_AnimationLength);
            ReleasePause();
        }

        public virtual bool IsFinished
        {
            get { return this.m_IsFinished; }
            protected set
            {
                if (value != m_IsFinished)
                {
                    m_IsFinished = value;
                    if (m_IsFinished == true)
                    {
                        OnFinished();
                    }
                }
            }
        }

        public void Animate(GameTime i_GameTime)
        {
            if (!m_Initialized)
            {
                Initialize();
            }

            if (this.Enabled && !this.IsFinished)
            {
                if (this.IsFinite)
                {
                    // check if we should stop animating:
                    m_TimeLeft -= i_GameTime.ElapsedGameTime;

                    if (m_TimeLeft.TotalSeconds < 0)
                    {
                        this.IsFinished = true;
                    }
                }

                if (!this.IsFinished)
                {
                    // we are still required to animate:
                    DoFrame(i_GameTime);
                }
            }
        }

        protected abstract void DoFrame(GameTime i_GameTime);
    }
}
