using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// The delegate is used by the Bullet to notify that he colided
    /// with another component
    /// </summary>
    /// <param name="i_OtherComponent">The component the bullet colided with</param>
    /// <param name="i_Bullet">The current bullet that coliided with the component</param>
    public delegate void BulletCollisionDelegate(ICollidable i_OtherComponent, SpaceShipBullet i_Bullet);

    /// <summary>
    /// A space ship bullet game component
    /// </summary>
    public class SpaceShipBullet : Bullet
    {
        public event BulletCollisionDelegate BulletCollision; 

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
            return !(i_OtherComponent is SpaceShip) &&
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

        /// <summary>
        /// Raise a collision with a component event
        /// </summary>
        /// <param name="i_Enemy">The component that the bullet colided with</param>
        private void    onBulletCollision(ICollidable i_OtherComponent)
        {
            if (BulletCollision != null)
            {
                BulletCollision(i_OtherComponent, this);
            }
        }        
    }
}
