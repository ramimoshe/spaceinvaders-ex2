using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public delegate void DreidelEventHandler(Dreidel i_Dreidel);
    public class Dreidel : CompositeGameComponent
    {
        public event DreidelEventHandler FinishedSpinning;
        TimeSpan m_SpinTime;
        float m_StartRotationsPerSecond;
        static Random m_Rand = new Random();

        public Dreidel(Game i_Game, TimeSpan i_SpinTime)
            : base(i_Game)
        {
            Add(new Box(i_Game));
            Add(new Cube(i_Game));
            Add(new Pyramid(i_Game));

            RotationsPerSecond = MathHelper.TwoPi * (float)m_Rand.NextDouble() + m_Rand.Next(1, 8);
            m_StartRotationsPerSecond = RotationsPerSecond;
            Scales = Vector3.One * (float)(m_Rand.Next(20) + m_Rand.NextDouble());
            m_SpinTime = i_SpinTime;
        }

        protected override void AfterLoadContent()
        {
            base.AfterLoadContent();
            Position = new Vector3(
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 300),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 200),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 20));
        }

        private float rotationDegregationPerSecond
        {
            get
            {
                return m_StartRotationsPerSecond / (float)m_SpinTime.TotalSeconds;
            }
        }

        private bool m_IsAlligning = false;
        private bool m_IsFinished = false;
        private float m_TargetRotation = -1;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!m_IsAlligning && !m_IsFinished)
            {
                RotationsPerSecond -= rotationDegregationPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            int lastSide;

            if (RotationsPerSecond <= 0 && !m_IsAlligning && !m_IsFinished)
            {
                OnDreidelFinished();
                m_TargetRotation = (float)Math.Ceiling(m_Rotations.Y / MathHelper.PiOver2);
                lastSide = (int)m_TargetRotation % 4;
                m_TargetRotation *= MathHelper.PiOver2;
                m_IsAlligning = true;
                RotationsPerSecond = (m_TargetRotation - m_Rotations.Y);

            }

            if (m_IsAlligning && m_Rotations.Y >= m_TargetRotation && !m_IsFinished)
            {
                m_IsFinished = true;
                m_IsAlligning = false;
                m_Rotations.Y = m_TargetRotation;
                RotationsPerSecond = 0;
            }
        }

        public void OnDreidelFinished()
        {
            if (FinishedSpinning != null)
            {
                FinishedSpinning(this);
            }
        }
    }
}
