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

            RotationsPerSecond = (float)m_Rand.NextDouble() + m_Rand.Next(5, 20);
            m_StartRotationsPerSecond = RotationsPerSecond;
            Scales = Vector3.One * (float)(0.5 + m_Rand.NextDouble());
            m_SpinTime = i_SpinTime;
        }

        protected override void AfterLoadContent()
        {
            base.AfterLoadContent();
            Position = new Vector3(
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 15),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 15),
                ((-1) + (m_Rand.Next(2)) * 2) * (float)m_Rand.Next(1, 10));
        }

        private float rotationDegregationPerSecond
        {
            get
            {
                return m_StartRotationsPerSecond / (float)m_SpinTime.TotalSeconds;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            RotationsPerSecond -= rotationDegregationPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (RotationsPerSecond <= 0)
            {
                OnDreidelFinished();
                m_StartRotationsPerSecond = 0;
                Dispose();
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
