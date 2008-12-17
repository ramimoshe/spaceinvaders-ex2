using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.Services;

namespace XnaGamesInfrastructure.ObjectModel.Screens
{
    /// <summary>
    /// Implements a base class for game screens
    /// </summary>
    public abstract class GameScreen
        : CompositeDrawableComponent<IGameComponent>
    {
        /// <summary>
        /// CTOR. Initializes Enables and Visible as false
        /// </summary>
        /// <param name="i_Game">Hosting game</param>
        public GameScreen(Game i_Game)
            : base(i_Game)
        {
            this.Enabled = false;
            this.Visible = false;
        }

        protected IScreensMananger m_ScreensManager;

        /// <summary>
        /// Gets / Sets ScreenManager for this GameScreen
        /// </summary>
        public IScreensMananger ScreensManager
        {
            get
            {
                return m_ScreensManager;
            }

            set
            {
                m_ScreensManager = value;
            }
        }

        protected bool m_IsModal = true;

        /// <summary>
        /// Gets / Set whether background screen should not be updated
        /// </summary>
        public bool IsModal
        {
            get
            {
                return m_IsModal;
            }

            set
            {
                m_IsModal = value;
            }
        }

        protected bool m_IsOverlayed;

        /// <summary>
        /// Gets / Sets whether background screen should be drawn
        /// </summary>
        public bool IsOverlayed
        {
            get
            {
                return m_IsOverlayed;
            }

            set
            {
                m_IsOverlayed = value;
            }
        }

        protected GameScreen m_PreviousScreen;

        /// <summary>
        /// Gets / Sets previous screen
        /// </summary>
        public GameScreen   PreviousScreen
        {
            get
            {
                return m_PreviousScreen;
            }

            set
            {
                m_PreviousScreen = value;
            }
        }

        protected bool m_HasFocus;

        /// <summary>
        /// Gets / Sets whether GameScreen should handle input
        /// </summary>
        public bool HasFocus
        {
            get
            {
                return m_HasFocus;
            }

            set
            {
                m_HasFocus = value;
            }
        }

        protected float m_BlackTintAlpha = 0;

        /// <summary>
        /// Gets / Sets opacity value
        /// </summary>
        public float    BlackTintAlpha
        {
            get
            {
                return m_BlackTintAlpha;
            }

            set
            {
                if (m_BlackTintAlpha < 0 || m_BlackTintAlpha > 1)
                {
                    throw new ArgumentException("value must be between 0 and 1", "BackgroundDarkness");
                }

                m_BlackTintAlpha = value;
            }
        }

        private IInputManager m_InputManager;

        private IInputManager m_DummyInputManager = new DummyInputManager();

        /// <summary>
        /// Gets / Sets GameScreen's input manager
        /// </summary>
        public IInputManager    InputManager
        {
            get
            {
                return this.HasFocus ? m_InputManager : m_DummyInputManager;
            }
        }

        /// <summary>
        /// Initialize GameScreen with input manager
        /// </summary>
        public override void    Initialize()
        {
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            if (m_InputManager == null)
            {
                m_InputManager = m_DummyInputManager;
            }

            base.Initialize();
        }

        /// <summary>
        /// Activates GameScreen (Enabled, Visible & HasFocus set to true)
        /// </summary>
        public void Activate()
        {
            this.Enabled = true;
            this.Visible = true;
            this.HasFocus = true;

            OnActivated();
        }

        /// <summary>
        /// Default behaviour - previous screen's focus is changed.
        /// </summary>
        protected virtual void OnActivated()
        {
            if (PreviousScreen != null)
            {
                PreviousScreen.HasFocus = !this.HasFocus;
            }
        }

        /// <summary>
        /// Deactivates & Closes screen
        /// </summary>
        protected void  ExitScreen()
        {
            Deactivate();
            OnClosed();
        }

        public event EventHandler Closed;

        /// <summary>
        /// Notifies observers screen has been closed
        /// </summary>
        protected virtual void  OnClosed()
        {
            if (Closed != null)
            {
                Closed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// DeActivates GameScreen (Enabled, Visible & HasFocus set to false)
        /// </summary>
        public void Deactivate()
        {
            this.Enabled = false;
            this.Visible = false;
            this.HasFocus = false;
        }

        /// <summary>
        /// Updates Previous screen if exists
        /// </summary>
        /// <param name="gameTime"></param>
        public override void    Update(GameTime gameTime)
        {
            if (PreviousScreen != null && !this.IsModal)
            {
                PreviousScreen.Update(gameTime);
            }

            base.Update(gameTime);
        }

        #region Faded Background Support

        private Texture2D m_GradientTexture;
        private Texture2D m_BlankTexture;

        /// <summary>
        /// Loads screen background's textures
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            m_GradientTexture = this.ContentManager.Load<Texture2D>(@"Screens\gradient");
            m_BlankTexture = this.ContentManager.Load<Texture2D>(@"Screens\blank");
        }

        /// <summary>
        /// Draw's background screen if exists with background tint values
        /// </summary>
        /// <param name="gameTime"></param>
        public override void    Draw(GameTime gameTime)
        {
            if (PreviousScreen != null
                && IsOverlayed)
            {
                PreviousScreen.Draw(gameTime);

                if (BlackTintAlpha > 0 || UseGradientBackground)
                {
                    FadeBackBufferToBlack((byte)(m_BlackTintAlpha * byte.MaxValue));
                }
            }

            base.Draw(gameTime);
        }

        protected bool m_UseGradientBackground = false;

        /// <summary>
        /// Gets / Sets whther to use gradient background pattern
        /// </summary>
        public bool UseGradientBackground
        {
            get
            {
                return m_UseGradientBackground;
            }

            set
            {
                m_UseGradientBackground = value;
            }
        }

        /// <summary>
        /// Draws background with specified opacity value
        /// </summary>
        /// <param name="i_Alpha">Opacity value (between 0 and 1)</param>
        public void FadeBackBufferToBlack(byte i_Alpha)
        {
            Viewport viewport = this.GraphicsDevice.Viewport;

            Texture2D background = UseGradientBackground ? m_GradientTexture : m_BlankTexture;

            SpriteBatch.Begin();
            SpriteBatch.Draw(
                        background,
                        new Rectangle(0, 0, viewport.Width, viewport.Height),
                        new Color(0, 0, 0, i_Alpha));
            SpriteBatch.End();
        }
        #endregion Faded Background Support
    }
}
