using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A09_Ex02_Koby_021766944_Inbar_015267479.ObjectModel
{
    // Delegate for collision between a bullet and an enemy event
    public delegate void BulletCollidedWithEnemy(Enemy i_Enemy);

    class SpaceShipBullet : Bullet
    {
        public SpaceShipBullet(Game i_Game) : base(i_Game)
        {
            TintColor = Color.Red;
        }

        public event BulletCollidedWithEnemy CollidedWithEnemy; 

        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return ((!(i_OtherComponent is SpaceShip)) &&
                   (base.CheckForCollision(i_OtherComponent)));            
        }

        public override void Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            base.Collided(i_OtherComponent);

            if (i_OtherComponent is Enemy)
            {
                onCollidedWithEnemy((Enemy)i_OtherComponent);
            }
        }

        /// <summary>
        /// Raise a colision with enemy event
        /// </summary>
        /// <param name="i_Enemy">The enemy the bullet colided with</param>
        private void    onCollidedWithEnemy(Enemy i_Enemy)
        {
            if (CollidedWithEnemy != null)
            {
                CollidedWithEnemy(i_Enemy);
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
