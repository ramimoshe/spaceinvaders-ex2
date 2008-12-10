using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvadersGame.Interfaces;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// Represents an enemy bullet game component
    /// </summary>
    public class EnemyBullet : Bullet
    {
        public EnemyBullet(Game i_Game)
            : base(i_Game)
        {
            TintColor = Color.Blue;
        }

        /// <summary>
        /// Check for collision with a given component.        
        /// </summary>
        /// <param name="i_OtherComponent">the component we want to check for collision 
        /// against</param>
        /// <returns>true in case the bullet collides with the given component or false
        /// in case the given component is an invader or there is no collision
        /// between the components </returns>
        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is IEnemy) &&
                   !(i_OtherComponent is EnemyBullet) &&
                   base.CheckForCollision(i_OtherComponent);
        }

        /// <summary>
        /// Implement the collision between the bullet and a game component by 
        /// making the bullet invisible.
        /// In case the colliding component is a space ship bullet, we'll 
        /// randomly decide whether the bullet will be hit.
        /// </summary>
        /// <param name="i_OtherComponent">The component the bullet colided with</param>
        public override void    Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            bool collided = true;

            if (i_OtherComponent is Bullet)
            {
                // Randomly choose whether the bullet will be hit
                collided = new Random().Next(0, 1) == 1;
            }

            if (collided)
            {
                base.Collided(i_OtherComponent);
            }
        }
    }
}
