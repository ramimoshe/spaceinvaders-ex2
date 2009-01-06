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
    public class Dreidel : CompositeGameComponent
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

        // TODO: Remove the spin parameter

        public Dreidel(Game i_Game, TimeSpan i_SpinTime)
            : base(i_Game)
        {
            Add(new Box(i_Game));
            Add(new Cube(i_Game));
            Add(new Pyramid(i_Game));

            Scales = Vector3.One * (float)(m_Rand.Next(20) + m_Rand.NextDouble());
            m_SpinTime = i_SpinTime;
            SpinComponent = false;
            m_CurrSide = k_DefaultDreidelSide;
            Reset();
        }

        public eDreidelLetters      DreidelFrontLetter
        {
            get { return s_DreidelLetters[m_CurrSide]; }
        }

        protected override void AfterLoadContent()
        {
            base.AfterLoadContent();
            Position = new Vector3(
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 300),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 200),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 20));
        }

        private float rotationDiffPerSecond
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

        private void Reset()
        {
            m_IsAlligning = false;
            RotationsPerSecond = MathHelper.TwoPi * (float)m_Rand.NextDouble() + m_Rand.Next(1, 8);
            m_StartRotationsPerSecond = RotationsPerSecond;
        }

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
                    m_TargetRotation = (float)Math.Ceiling(m_Rotations.Y / MathHelper.PiOver2);
                    m_CurrSide = (int)m_TargetRotation % 4;
                    m_TargetRotation *= MathHelper.PiOver2;
                    m_IsAlligning = true;
                    RotationsPerSecond = (m_TargetRotation - m_Rotations.Y);
                }

                if (m_IsAlligning && m_Rotations.Y >= m_TargetRotation)
                {
                    OnDreidelFinished();
                }
            }
        }

        public void     OnDreidelFinished()
        {
            m_IsAlligning = false;
            m_Rotations.Y = m_TargetRotation;
            SpinComponent = false;
            if (FinishedSpinning != null)
            {
                FinishedSpinning(this);
            }
        }
    }
}
