using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using DreidelGame.ObjectModel;
using DreidelGame.Services;
using DreidelGame.Interfaces;

namespace DreidelGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DreidelGame : Microsoft.Xna.Framework.Game
    {
        private const float k_NearPlaneDistance = 0.5f;
        private const float k_FarPlaneDistance = 5000.0f;
        private const float k_ViewAngle = MathHelper.PiOver4;

        private const int k_DefaultSpinningDreidelsNum = 0;
        private const int k_DreidelsNum = 13;

        private const float k_ZFactorWidth = 7;
        private const float k_ZFactorCoordinate = 3.5f;
        private readonly Keys r_StartGameKey = Keys.Space;

        private static Dictionary<Keys, eDreidelLetters> s_DreidelLettersKeys;

        private GraphicsDeviceManager graphics;
        private InputManager m_InputManager;
        private Vector3 m_Position = new Vector3(0, 0, 0);
        private Vector3 m_Rotations = Vector3.Zero;
        private Vector3 m_Scales = Vector3.One;
        private Matrix m_WorldMatrix = Matrix.Identity;
        private ScoreManager m_ScoreManager;
        private BasicEffect m_BasicEffect;
        private Matrix m_ProjectionFieldOfView;
        private Camera m_Camera;

        private int m_SpinningDreidels;

        private IDreidel[] m_Dreidels = new IDreidel[k_DreidelsNum];

        /// <summary>
        /// Read only property that marks if we can get an input from the user
        /// </summary>
        private bool CanGetInput
        {
            get 
            { 
                return m_SpinningDreidels == k_DefaultSpinningDreidelsNum; 
            }
        }

        /// <summary>
        /// Static ctor that creates the Dictionary that maps the keyboard keys to the dreidel
        /// letters
        /// </summary>
        static DreidelGame()
        {
            s_DreidelLettersKeys = new Dictionary<Keys, eDreidelLetters>();

            s_DreidelLettersKeys.Add(Keys.B, eDreidelLetters.NLetter);
            s_DreidelLettersKeys.Add(Keys.D, eDreidelLetters.GLetter);
            s_DreidelLettersKeys.Add(Keys.V, eDreidelLetters.HLetter);
            s_DreidelLettersKeys.Add(Keys.P, eDreidelLetters.PLetter);
        }

        /// <summary>
        /// Default CTOR. Initializes all dreidels
        /// </summary>
        public DreidelGame()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            
            m_InputManager = new InputManager(this);
            this.Components.Add(m_InputManager);
            this.Services.AddService(typeof(InputManager), m_InputManager);

            m_ScoreManager = new ScoreManager(this);
            this.Components.Add(m_ScoreManager);
          
            // Initialing dreidels
            IDreidel newDreidel;

            for (int i = 0; i < k_DreidelsNum; ++i)
            {
                if (i < k_DreidelsNum - 3)
                {
                    // Every second dreidel will be Texture\Position dreidel
                    newDreidel = (i % 2) == 0 ? 
                        (IDreidel) new PositionDreidel(this) :
                        (IDreidel) new TextureDreidel(this);
                }
                else 
                {
                    newDreidel = (IDreidel) new ModelDreidel(this);
                }

                newDreidel.FinishedSpinning += new DreidelEventHandler(dreidel_FinishedSpinning);
                newDreidel.FinishedSpinning += new DreidelEventHandler(m_ScoreManager.Dreidel_FinishedSpinning);
                m_Dreidels[i] = newDreidel;                
            }
 
            m_SpinningDreidels = k_DefaultSpinningDreidelsNum;

            m_Camera = new Camera(this);
            this.Components.Add(m_Camera);
            this.Services.AddService(typeof(Camera), m_Camera);
        }

        /// <summary>
        /// Initialize the mouse to be in the center of the screen
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            Mouse.SetPosition(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2);

            m_Camera.DefaultMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void     Initialize()
        {
            GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            base.Initialize();            

            // we are storing the field-of-view data in a matrix:
            m_ProjectionFieldOfView = Matrix.CreatePerspectiveFieldOfView(
                k_ViewAngle,
                (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height,
                k_NearPlaneDistance,
                k_FarPlaneDistance);

            m_BasicEffect = new BasicEffect(GraphicsDevice, null);

            m_BasicEffect.Projection = m_ProjectionFieldOfView;

            Services.AddService(typeof(BasicEffect), m_BasicEffect);
        }

        /// <summary>
        /// Updates the game state by getting an input from the player 
        /// (only if CanGetInput is true)
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
        protected override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (CanGetInput)
            {
                // Spining the dreidels when space is entered
                if (m_InputManager.KeyPressed(r_StartGameKey))
                {
                    spinDreidels();
                }
                else
                {
                    checkIfDreidelLettersPressed();
                }
            }
        }

        /// <summary>
        /// Check if the player chose one of the dreidel letters and update the score
        /// manager with the chosen letter
        /// </summary>
        private void    checkIfDreidelLettersPressed()
        {
            foreach (Keys key in s_DreidelLettersKeys.Keys)
            {
                if (m_InputManager.KeyPressed(key))
                {
                    m_ScoreManager.PlayerChosenLetter = s_DreidelLettersKeys[key];
                }
            }
        }

        /// <summary>
        /// Spin all the game dreidels
        /// </summary>
        private void    spinDreidels()
        {
            foreach (IDreidel d in m_Dreidels)
            {
                d.SpinDreidel();
                m_SpinningDreidels++;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void     Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);
            GraphicsDevice.RenderState.DepthBufferEnable = true;
            base.Draw(gameTime);
        }

        /// <summary>
        /// Catch the FinishedSpinning event raised by a dreidel and decrease the number of
        /// spinning dreidels
        /// </summary>
        /// <param name="i_Dreidel">The dreidel the raised the event</param>
        private void    dreidel_FinishedSpinning(IDreidel i_Dreidel)
        {
            m_SpinningDreidels--;            
        }
    }
}
