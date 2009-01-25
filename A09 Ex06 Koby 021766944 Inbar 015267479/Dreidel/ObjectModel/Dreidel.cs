using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DreidelGame.Interfaces;

namespace DreidelGame.ObjectModel
{

    // TODO: Change to inherit from Base

    /// <summary>
    /// Represents a dreidel in the game
    /// </summary>
    public abstract class Dreidel : CompositeGameComponent, IDreidel
    {
        private const int k_DreidelSidesNum = 4;
        private const int k_DefaultDreidelSide = 0;
        private const int k_TrianglesNum = 0;
        
        private static Random m_Rand = new Random();

        private static eDreidelLetters[] s_DreidelLetters;
        private TimeSpan m_SpinTime;
        private float m_StartRotationsPerSecond;
        private bool m_IsAlligning = false;
        private float m_TargetRotation = -1;
        private int m_CurrSide;

        //private List<BaseDrawableComponent> m_Components;

        public event DreidelEventHandler FinishedSpinning;

        // Initializing a random position for the dreidel
        private Vector3 m_RandomPosition = new Vector3(
                    ((-1) + (m_Rand.Next(2) * 2)) * (float)m_Rand.Next(1, 200),
                    ((-1) + (m_Rand.Next(2) * 2)) * (float)m_Rand.Next(1, 150),
                    ((-1) + (m_Rand.Next(2) * 2)) * (float)m_Rand.Next(1, 20));

        /// <summary>
        /// Gets the number of triangles the dreidel has
        /// </summary>
        public override int     TriangleNum
        {
            get { return k_TrianglesNum; }
        }

        /// <summary>
        /// Gets the dreidels cube
        /// </summary>
        protected abstract Cube     DreidelCube
        {
            get;
        }

        /// <summary>
        /// Static ctor that creates the dreidel sides array
        /// </summary>
        static Dreidel()
        {
            s_DreidelLetters = new eDreidelLetters[k_DreidelSidesNum];

            s_DreidelLetters[0] = eDreidelLetters.NLetter;
            s_DreidelLetters[1] = eDreidelLetters.GLetter;
            s_DreidelLetters[2] = eDreidelLetters.HLetter;
            s_DreidelLetters[3] = eDreidelLetters.PLetter;
        }

        /// <summary>
        /// Initializes the dreidel components and random factors 
        /// (scale, position, rotation)
        /// </summary>
        /// <param name="i_Game"></param>
        public  Dreidel(Game i_Game)
            : base(i_Game)
        {
            addDreidelComponents(Game); 
            Scales = Vector3.One * (float)(0.5 + m_Rand.Next(12) + m_Rand.NextDouble());
            SpinComponent = false;
            m_CurrSide = k_DefaultDreidelSide;
            reset();
        }

        /// <summary>
        /// Adds the components that construct the dreidel
        /// </summary>
        /// <param name="i_Game">The game component</param>
        private void    addDreidelComponents(Game i_Game)
        {            
            Add(this.DreidelCube);
            Add(new Box(Game));
            Add(new Pyramid(Game));
        }        

        /// <summary>
        /// Gets the current letter that faces to the player position
        /// </summary>
        public eDreidelLetters      DreidelFrontLetter
        {
            get 
            { 
                return s_DreidelLetters[m_CurrSide]; 
            }
        }

        /// <summary>
        /// Initialize the dreidel position transformation values
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            Position = m_RandomPosition;
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
            RotationsPerSecond = m_StartRotationsPerSecond;
            SpinComponent = true;
        }

        /// <summary>
        /// Resets the dreidel spin values
        /// </summary>
        private void    reset()
        {
            m_SpinTime = TimeSpan.FromSeconds(3 + m_Rand.NextDouble() + m_Rand.Next(6));
            m_IsAlligning = false;
            RotationsPerSecond = (MathHelper.TwoPi * (float)m_Rand.NextDouble()) + m_Rand.Next(1, 8);
            m_StartRotationsPerSecond = RotationsPerSecond;
        }

        /// <summary>
        /// Updates the dreidel status
        /// </summary>
        /// <param name="gameTime">A snapshoit to the current game time</param>
        public override void    Update(GameTime gameTime)
        {            
            base.Update(gameTime);

            // All updates are done only when dreidel is spinning
            if (SpinComponent)
            {
                // Changing rotation speed when dreidel has not finished spinning
                if (!m_IsAlligning)
                {
                    RotationsPerSecond -= rotationDiffPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                // Checking if rotation finished in order to calculate the result dreidel
                // side
                if (RotationsPerSecond <= 0 && !m_IsAlligning)
                {
                    m_TargetRotation = (float)Math.Ceiling(Rotations.Y / MathHelper.PiOver2);
                    m_CurrSide = (int)m_TargetRotation % 4;
                    m_TargetRotation *= MathHelper.PiOver2;
                    m_IsAlligning = true;
                    RotationsPerSecond = (m_TargetRotation - Rotations.Y);
                }

                // Norifying observers uppon finish
                if (m_IsAlligning && Rotations.Y >= m_TargetRotation)
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
            Vector3 rotation = Rotations;
            rotation.Y = m_TargetRotation;
            Rotations = rotation;
            SpinComponent = false;
            if (FinishedSpinning != null)
            {
                FinishedSpinning(this);
            }
        }

        // tODO: Remove the proc

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public override void InitBuffers()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
