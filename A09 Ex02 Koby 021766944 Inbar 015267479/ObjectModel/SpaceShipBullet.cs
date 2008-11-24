using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace A09_Ex02_Koby_021766944_Inbar_015267479.ObjectModel
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

        public override void Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            base.Collided(i_OtherComponent);

            // TODO Remove remark

            //if (i_OtherComponent is IScorable)
            //{
                onBulletCollision(i_OtherComponent);

                // TODO check what todo with dispose
//                Dispose();
            //}
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

        /*public override void    Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            if (!(i_OtherComponent is SpaceShip))
            {
                base.Collided(i_OtherComponent);
            }
        }*/        
    }
}
