using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        /// Check if the bullet colides with another component
        /// </summary>
        /// <param name="i_OtherComponent">The component we want to check the collision
        /// against</param>
        /// <returns>True if the components collides or false otherwise</returns>
        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is Invader) && 
                base.CheckForCollision(i_OtherComponent);
        }       
    }
}
