using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel
{
    public class CollidableSprite : Sprite, ICollidable
    {
        public  CollidableSprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game)
        {
        }

        public  CollidableSprite(
                string i_AssetName,
                Game i_Game,
                int i_UpdateOrder,
                int i_DrawOrder)
                : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        protected ICollisionManager m_CollisionManager;

        public override void Initialize()
        {
            base.Initialize();

            m_CollisionManager = (ICollisionManager)this.Game.Services.GetService(typeof(ICollisionManager));

            if (m_CollisionManager != null)
            {
                m_CollisionManager.AddObjectToMonitor(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_MotionVector != Vector2.Zero)
            {
                OnPositionChanged();
            }
        }

        #region ICollidable Members

        /// <summary>
        /// Check if a given component colides with the current one
        /// </summary>
        /// <param name="i_OtherComponent">The component that we want to check collision against</param>
        /// <returns>true if the given component colides the current one or false otherwise</returns>
        public virtual bool CheckForCollision(ICollidable i_OtherComponent)
        {
            return this.Bounds.Intersects(i_OtherComponent.Bounds);
        }

        public virtual void Collided(ICollidable i_OtherComponent)
        {
            this.Visible = false;            
        }

        public event PositionChangedDelegate PositionChanged;

        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this);
            }
        }

        #endregion
    }
}
