using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ObjectModel;

namespace XnaGamesInfrastructure.Services
{
    public class CollisionManager : GameService, ICollisionManager
    {
        protected List<ICollidable> m_Collidables = new List<ICollidable>();

        public CollisionManager(Game i_Game)
            : this(i_Game, Int32.MinValue)
        {
        }

        public CollisionManager(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionManager), this);
        }

        public void AddObjectToMonitor(ICollidable i_Collidable)
        {
            if (!m_Collidables.Contains(i_Collidable))
            {
                m_Collidables.Add(i_Collidable);
                i_Collidable.PositionChanged += collidable_PositionChanged;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            if (collidable != null
                &&
                this.m_Collidables.Contains(collidable))
            {
                m_Collidables.Remove(collidable);
            }
        }

        private void collidable_PositionChanged(object i_Collidable)
        {
            if (i_Collidable is ICollidable)
            {
                CheckCollision(i_Collidable as ICollidable);
            }
        }

        private void CheckCollision(ICollidable i_Source)
        {
            foreach (ICollidable target in m_Collidables)
            {
                if (i_Source.Visible && i_Source != target && target.Visible)
                {
                    if (target.CheckForCollision(i_Source))
                    {
                        target.Collided(i_Source);
                        i_Source.Collided(target);
                    }
                }
            }
        }
    }
}
