using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// Class implements a sprite capable of participating in collision managemnt
    /// </summary>
    public class CollidableSprite : Sprite, ICollidable
    {
        /// <summary>
        /// The constructor intiates the base constructor 
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        public  CollidableSprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game)
        {
        }

        /// <summary>
        /// Calls Base constructor
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        /// <param name="i_UpdateOrder">Number defining the order in which update 
        /// of all game components is called</param>
        /// <param name="i_DrawOrder">Number defining the order in which draw
        /// of all game components is called</param>
        public  CollidableSprite(
                string i_AssetName,
                Game i_Game,
                int i_UpdateOrder,
                int i_DrawOrder)
                : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        /// <summary>
        /// Collision manager (handling collision)
        /// </summary>
        protected ICollisionManager m_CollisionManager;

        /// <summary>
        /// Initialize collision manager, and registers itself as an ICollidable
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_CollisionManager = Game.Services.GetService(typeof(ICollisionManager)) as ICollisionManager;

            if (m_CollisionManager != null)
            {
                m_CollisionManager.AddObjectToMonitor(this);
            }
        }

        /// <summary>
        /// Raises position-changed event when position was changed
        /// </summary>
        /// <param name="gameTime">Elapsed time since last call</param>
        public override void    Update(GameTime gameTime)
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
        /// <param name="i_OtherComponent">The component that we want to check 
        /// collision against</param>
        /// <returns>true if the given component colides the current one or false 
        /// otherwise</returns>
        public virtual bool CheckForCollision(ICollidable i_OtherComponent)
        {
            return this.Bounds.Intersects(i_OtherComponent.Bounds);
        }

        /// <summary>
        /// Default behaviour in case of collision - hides the sprite
        /// </summary>
        /// <param name="i_OtherComponent"></param>
        public virtual void Collided(ICollidable i_OtherComponent)
        {
            this.Visible = false;            
        }

        /// <summary>
        /// Raised when sprite's position is modified in current update
        /// </summary>
        public event PositionChangedDelegate PositionChanged;

        /// <summary>
        /// Raises PositionChanged event if there are registered observers.
        /// </summary>
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
