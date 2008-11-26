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
        /// Check for collision with a given component.        
        /// </summary>
        /// <param name="i_OtherComponent">the component we want to check for collision 
        /// against</param>
        /// <returns>true in case the bullet collides with the given component or false
        /// in case the given component is an invader or there is no collision
        /// between the components </returns>
        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is Invader) && 
                base.CheckForCollision(i_OtherComponent);
        }       
    }
}
