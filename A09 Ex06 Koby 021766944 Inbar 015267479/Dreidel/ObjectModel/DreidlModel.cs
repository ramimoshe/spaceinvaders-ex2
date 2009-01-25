using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using DreidelGame.Services;
using DreidelGame.Interfaces;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// Class implements a dreidel which is implemented using a model
    /// </summary>
    public class ModelDreidel : DrawableGameComponent, IDreidel
    {
        private const int k_DreidelSidesNum = 4;
        private const int k_DefaultDreidelSide = 0;
        private const int k_DreidelScore = 1;

        private static Random m_Rand = new Random();
        protected Vector3 m_Position = Vector3.Zero;
        private Vector3 m_Rotations = Vector3.Zero;
        protected Vector3 m_Scales = Vector3.One;
        protected Matrix m_WorldMatrix = Matrix.Identity;
        private TimeSpan m_SpinTime;
        private float m_StartRotationsPerSecond;
        private bool m_IsAlligning = false;
        private float m_TargetRotation = -1;
        private int m_CurrSide;
        protected float m_RotationsPerSecond = 0;
        private bool m_SpinComponent;

        Model m_DreidelModel;
        ContentManager m_ContentManager;
        Matrix[] m_ModelTransformations;

        /// <summary>
        /// Initializes the dreidel components and random factors 
        /// (scale, position, rotation)
        /// </summary>
        /// <param name="i_Game">The hosting game</param>
        public  ModelDreidel(Game i_Game)
            : base(i_Game)
        {
            Game.Components.Add(this);
            m_ContentManager = Game.Content;

            m_Scales = Vector3.One * (float)(10 + m_Rand.Next(50));
            m_SpinComponent = false;
            m_CurrSide = k_DefaultDreidelSide;
            reset();

        }

        /// <summary>
        /// Returns the score for the dreidel
        /// </summary>
        public int  DreidelScore
        {
            get 
            { 
                return k_DreidelScore; 
            }
        } 

        /// <summary>
        /// Loads the dreidel's model
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            m_DreidelModel = m_ContentManager.Load<Model>(@"XSI\Dreidel\Models\dreidel");
            m_ModelTransformations = new Matrix[m_DreidelModel.Bones.Count];
            m_DreidelModel.CopyAbsoluteBoneTransformsTo(m_ModelTransformations);
        }

        public event DreidelEventHandler    FinishedSpinning;

        // Initializing a random position for the dreidel
        private Vector3 m_RandomPosition = new Vector3(
                    ((-1) + (m_Rand.Next(2) * 2)) * (float)m_Rand.Next(1, 100),
                    ((-1) + (m_Rand.Next(2) * 2)) * (float)m_Rand.Next(1, 80),
                    ((-1) + (m_Rand.Next(2) * 2)) * (float)m_Rand.Next(1, 20));

        /// <summary>
        /// Gets the current letter that faces to the player position
        /// </summary>
        public eDreidelLetters     DreidelFrontLetter
        {
            get
            {
                return DreidelLettersContainer.Letters[m_CurrSide];
            }
        }

        /// <summary>
        /// Initialize the dreidel position transformation values
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_Position = m_RandomPosition;
        }

        /// <summary>
        /// Gets the change per second in rotation speed
        /// </summary>
        private float   rotationDiffPerSecond
        {
            get
            {
                return m_StartRotationsPerSecond / (float)m_SpinTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Start spinning the dreidel
        /// </summary>
        public void     SpinDreidel()
        {
            reset();
            m_SpinComponent = true;
        }

        /// <summary>
        /// Resets the dreidel spin values
        /// </summary>
        private void    reset()
        {
            m_SpinTime = TimeSpan.FromSeconds(3 + m_Rand.NextDouble() + m_Rand.Next(6));
            m_IsAlligning = false;
            m_RotationsPerSecond = (MathHelper.TwoPi * (float)m_Rand.NextDouble()) + m_Rand.Next(1, 8);
            m_StartRotationsPerSecond = m_RotationsPerSecond;
        }

        /// <summary>
        /// Updates the dreidel status
        /// </summary>
        /// <param name="gameTime">A snapshoit to the current game time</param>
        public override void    Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // All updates are done only when dreidel is spinning
            if (m_SpinComponent)
            {
                m_Rotations.Y += m_RotationsPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Changing rotation speed when dreidel has not finished spinning
                if (!m_IsAlligning)
                {
                    m_RotationsPerSecond -= rotationDiffPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                // Checking if rotation finished in order to calculate the result dreidel
                // side
                if (m_RotationsPerSecond <= 0 && !m_IsAlligning)
                {
                    m_TargetRotation = (float)Math.Ceiling(m_Rotations.Y / MathHelper.PiOver2);
                    m_CurrSide = (int)m_TargetRotation % 4;
                    m_TargetRotation *= MathHelper.PiOver2;
                    m_IsAlligning = true;
                    m_RotationsPerSecond = (m_TargetRotation - m_Rotations.Y);
                }

                // Norifying observers uppon finish
                if (m_IsAlligning && m_Rotations.Y >= m_TargetRotation)
                {
                    OnDreidelFinished();
                }
            }
        }

        /// <summary>
        /// Raise an event that states the dreidel finished spinning
        /// </summary>
        public void     OnDreidelFinished()
        {
            m_IsAlligning = false;
            m_Rotations.Y = m_TargetRotation;
            m_TargetRotation = -1;

            m_SpinComponent = false;
            if (FinishedSpinning != null)
            {
                FinishedSpinning(this);
            }
        }

        /// <summary>
        /// Performs draw of all model meshes
        /// </summary>
        /// <param name="gameTime"></param>
        public override void    Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in m_DreidelModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                        m_ModelTransformations[mesh.ParentBone.Index] *
                                        Matrix.CreateScale(m_Scales) *
                                        Matrix.CreateRotationY(m_Rotations.Y) * 
                                        Matrix.CreateTranslation(m_Position);

                    Camera cam = (Camera) Game.Services.GetService(typeof(Camera));
                    BasicEffect basicEffect = (BasicEffect)Game.Services.GetService(typeof(BasicEffect));

                    effect.View = cam.ViewMatrix;
                    effect.Projection = basicEffect.Projection;
                    effect.EnableDefaultLighting();
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
