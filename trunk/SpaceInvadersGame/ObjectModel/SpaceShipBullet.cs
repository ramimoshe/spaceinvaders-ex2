using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;
using SpaceInvadersGame.Interfaces;

namespace SpaceInvadersGame.ObjectModel
{  
    /// <summary>
    /// A space ship bullet game component
    /// </summary>
    public class SpaceShipBullet : Bullet
    {        
        public SpaceShipBullet(Game i_Game) : base(i_Game)
        {
            TintColor = Color.Red;
        }

        /// <summary>
        /// Check for collision with a given component.        
        /// </summary>
        /// <param name="i_OtherComponent">the component we want to check for collision 
        /// against</param>        
        /// <returns>true in case the bullet collides with the given component 
        /// or false in case the given component is a space ship or there is 
        /// no collision between the components </returns>
        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is IPlayer) &&
                   !(i_OtherComponent is SpaceShipBullet) &&
                   base.CheckForCollision(i_OtherComponent);            
        }

        /// <summary>
        /// Implement the collision between the bullet and a game component by
        /// changing the bullet to invisible and raising a BulletCollision event
        /// <param name="i_OtherComponent">The component the ship colided with</param>
        public override void    Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            base.Collided(i_OtherComponent);

            onBulletCollision(i_OtherComponent);            
        }     
    }
}
