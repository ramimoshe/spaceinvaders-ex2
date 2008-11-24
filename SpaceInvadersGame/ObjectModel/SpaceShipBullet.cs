using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace SpaceInvadersGame.ObjectModel
{
    // Delegate for collision between a bullet and an enemy event
    public delegate void BulletCollisionDelegate(ICollidable i_OtherComponent, SpaceShipBullet i_Bullet);

    class SpaceShipBullet : Bullet
    {
        public SpaceShipBullet(Game i_Game) : base(i_Game)
        {
            TintColor = Color.Red;
        }

        public event BulletCollisionDelegate BulletCollision; 

        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return ((!(i_OtherComponent is SpaceShip)) &&
                   (base.CheckForCollision(i_OtherComponent)));            
        }

        public override void    Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            base.Collided(i_OtherComponent);

            onBulletCollision(i_OtherComponent);            
        }      

        /// <summary>
        /// Raise a colision with component event
        /// </summary>
        /// <param name="i_Enemy">The component the bullet colided with</param>
        private void    onBulletCollision(ICollidable i_OtherComponent)
        {
            if (BulletCollision != null)
            {
                BulletCollision(i_OtherComponent, this);
            }
        }        
    }
}
