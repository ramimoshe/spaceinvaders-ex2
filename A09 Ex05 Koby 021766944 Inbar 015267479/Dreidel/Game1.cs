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

namespace DreidelGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {        
        private const int k_DefaultSpinningDreidelsNum = 0;
        private const int k_DreidelsNum = 10;
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
        private Matrix m_PointOfView;
        private int m_SpinningDreidels;

        private Dreidel[] m_Dreidels;

        /// <summary>
        /// Read only property that marks if we can get an input from the user
        /// </summary>
        private bool    CanGetInput
        {
            get { return m_SpinningDreidels == k_DefaultSpinningDreidelsNum; }
        }

        /// <summary>
        /// Static ctor that creates the Dictionary that maps the keyboard keys to the dreidel
        /// letters
        /// </summary>
        static Game1()
        {
            s_DreidelLettersKeys = new Dictionary<Keys, eDreidelLetters>();

            s_DreidelLettersKeys.Add(Keys.B, eDreidelLetters.NLetter);
            s_DreidelLettersKeys.Add(Keys.D, eDreidelLetters.GLetter);
            s_DreidelLettersKeys.Add(Keys.V, eDreidelLetters.HLetter);
            s_DreidelLettersKeys.Add(Keys.P, eDreidelLetters.PLetter);
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            m_InputManager = new InputManager(this);
            this.Components.Add(m_InputManager);
            this.Services.AddService(typeof(InputManager), m_InputManager);

            m_Dreidels = new Dreidel[k_DreidelsNum];
            m_ScoreManager = new ScoreManager(this);
            this.Components.Add(m_ScoreManager);

            for (int i = 1; i <= k_DreidelsNum; ++i)
            {
                Dreidel newDreidel;

                if ((i % 2) == 0)
                {
                    newDreidel = new PositionDreidel(this);
                }
                else
                {
                    newDreidel = new TextureDreidel(this);
                }

                newDreidel.FinishedSpinning += new DreidelEventHandler(dreidel_FinishedSpinning);
                newDreidel.FinishedSpinning += new DreidelEventHandler(m_ScoreManager.Dreidel_FinishedSpinning);
                m_Dreidels[i - 1] = newDreidel;                

            }

            m_SpinningDreidels = k_DefaultSpinningDreidelsNum;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void     Initialize()
        {
            graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            base.Initialize();

            float k_NearPlaneDistance = 0.5f;
            float k_FarPlaneDistance = 1000.0f;
            float k_ViewAngle = MathHelper.PiOver4;

            // we are storing the field-of-view data in a matrix:
            m_ProjectionFieldOfView = Matrix.CreatePerspectiveFieldOfView(
                k_ViewAngle,
                (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height,
                k_NearPlaneDistance,
                k_FarPlaneDistance);

            // we want to shoot the center of the world:
            Vector3 targetPosition = Vector3.Zero;
            // we are standing 50 units in front of our target:
            Vector3 pointOfViewPosition = new Vector3(0, 0, 500);
            // we are not standing on our head:
            Vector3 pointOfViewUpDirection = new Vector3(0, 1, 0);

            // we are storing the point-of-view data in a matrix:
            m_PointOfView = Matrix.CreateLookAt(
                pointOfViewPosition, targetPosition, pointOfViewUpDirection);

            m_BasicEffect = new BasicEffect(GraphicsDevice, null);
            m_BasicEffect.View = m_PointOfView;
            m_BasicEffect.Projection = m_ProjectionFieldOfView;
            m_BasicEffect.VertexColorEnabled = true;

            Services.AddService(typeof(BasicEffect), m_BasicEffect);
        }

        protected override void     Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (CanGetInput)
            {
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
            foreach (Dreidel d in m_Dreidels)
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

            //m_Cube.Draw(gameTime);
            //m_CubeTexture.Draw(gameTime);
            base.Draw(gameTime);
        }

        private void    dreidel_FinishedSpinning(Dreidel i_Dreidel)
        {
            m_SpinningDreidels--;            
        }
    }
}
