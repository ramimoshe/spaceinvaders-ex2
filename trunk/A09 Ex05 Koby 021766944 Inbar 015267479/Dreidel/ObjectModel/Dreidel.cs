using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public delegate void DreidelEventHandler(Dreidel i_Dreidel);

    /// <summary>
    /// An enum containing the available dreidel letters
    /// </summary>
    public enum eDreidelLetters
    {
        NLetter,
        HLetter,
        PLetter,
        GLetter,
        None
    }

    /// <summary>
    /// Represents a dreidel in the game
    /// </summary>
    public abstract class Dreidel : CompositeGameComponent
    {
        private const int k_DreidelSidesNum = 4;
        private const int k_DefaultDreidelSide = 0;
        private static eDreidelLetters[] s_DreidelLetters;

        public event DreidelEventHandler FinishedSpinning;
        private TimeSpan m_SpinTime;
        private float m_StartRotationsPerSecond;
        private bool m_IsAlligning = false;
        private float m_TargetRotation = -1;
        private int m_CurrSide;

        private static Random m_Rand = new Random();

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

        public Dreidel(Game i_Game)
            : base(i_Game)
        {
            addDreidelComponents(i_Game);

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
            Add(new Box(i_Game));
            Add(this.DreidelCube);
            Add(new Pyramid(i_Game));
        }        

        /// <summary>
        /// Gets the current letter that faces to the player position
        /// </summary>
        public eDreidelLetters      DreidelFrontLetter
        {
            get { return s_DreidelLetters[m_CurrSide]; }
        }

        protected override void     LoadContent()
        {
            base.LoadContent();
            Position = new Vector3(
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 300),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 200),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 20));
        }

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
            RotationsPerSecond = MathHelper.TwoPi * (float)m_Rand.NextDouble() + m_Rand.Next(1, 8);
            m_StartRotationsPerSecond = RotationsPerSecond;
        }

        /// <summary>
        /// Updates the dreidel status
        /// </summary>
        /// <param name="gameTime">A snapshoit to the current game time</param>
        public override void    Update(GameTime gameTime)
        {            
            base.Update(gameTime);

            if (SpinComponent)
            {
                if (!m_IsAlligning)
                {
                    RotationsPerSecond -= rotationDiffPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (RotationsPerSecond <= 0 && !m_IsAlligning)
                {
                    m_TargetRotation = (float)Math.Ceiling(Rotations.Y / MathHelper.PiOver2);
                    m_CurrSide = (int)m_TargetRotation % 4;
                    m_TargetRotation *= MathHelper.PiOver2;
                    m_IsAlligning = true;
                    RotationsPerSecond = (m_TargetRotation - Rotations.Y);
                }

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
    }
}
