using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ObjectModel;

namespace XnaGamesInfrastructure.Services
{
    /// <summary>
    /// Defines a manager handling with ICollidable components collisions
    /// </summary>
    public class CollisionManager : GameService, ICollisionManager
    {
        /// <summary>
        /// List of observed collidable objects
        /// </summary>
        protected List<ICollidable> m_Collidables = new List<ICollidable>();

        /// <summary>
        /// Sets a manager with default maximum update order
        /// </summary>
        /// <param name="i_Game">Game holding the service</param>
        public CollisionManager(Game i_Game)
            : this(i_Game, Int32.MinValue)
        {
        }

        /// <summary>
        /// Sets a new manager
        /// </summary>
        /// <param name="i_Game">Game holding the service</param>
        public CollisionManager(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
        }

        /// <summary>
        /// Registers itself as service in game
        /// </summary>
        protected override void     RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ICollisionManager), this);
        }

        /// <summary>
        /// adds a new ocmponent to monitor
        /// </summary>
        /// <param name="i_Collidable">new component</param>
        public void     AddObjectToMonitor(ICollidable i_Collidable)
        {
            // Checking if object does not exists in observed list
            if (!m_Collidables.Contains(i_Collidable))
            {
                m_Collidables.Add(i_Collidable);
                i_Collidable.PositionChanged += collidable_PositionChanged;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        /// <summary>
        /// Handling disposal of an observed component
        /// </summary>
        /// <param name="sender">The observed component</param>
        /// <param name="e">Event arguments</param>
        private void    collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            // Verifying if component is in observed list
            if (collidable != null
                &&
                this.m_Collidables.Contains(collidable))
            {
                m_Collidables.Remove(collidable);
            }
        }

        /// <summary>
        /// Checking if a collision occured for specified component
        /// </summary>
        /// <param name="i_Collidable">specified component</param>
        private void    collidable_PositionChanged(object i_Collidable)
        {
            if (i_Collidable is ICollidable)
            {
                checkCollision(i_Collidable as ICollidable);
            }
        }

        /// <summary>
        /// Checking if a collision occured for specified component
        /// </summary>
        /// <param name="i_Source">specified component</param>
        private void    checkCollision(ICollidable i_Source)
        {
            // Checking for collision with each of the observed components
            foreach (ICollidable target in m_Collidables)
            {
                // Validating both collidables are visible, differ and
                // not in a dying state
                if (i_Source.Visible && 
                    i_Source != target && 
                    target.Visible &&
                    !i_Source.Dying &&
                    !target.Dying)
                {
                    // Checking if collision occured
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
