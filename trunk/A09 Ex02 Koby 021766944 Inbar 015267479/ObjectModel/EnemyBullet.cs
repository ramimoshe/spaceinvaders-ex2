using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A09_Ex02_Koby_021766944_Inbar_015267479.ObjectModel
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
        /// Check if the bullet colides with another component
        /// </summary>
        /// <param name="i_OtherComponent">The component we want to check the collision
        /// against</param>
        /// <returns>True if the components collides or false otherwise</returns>
        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is Enemy) &&
                    (base.CheckForCollision(i_OtherComponent));
        }       
    }
}
