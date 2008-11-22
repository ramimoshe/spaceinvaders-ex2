using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel
{
    public abstract class CollidableSprite : Sprite, ICollidable
    {

        public  CollidableSprite(string i_AssetName, Game i_Game,
                                 int i_UpdateOrder,
                                 int i_DrawOrder)
            : base (i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        #region ICollidable Members

        /// <summary>
        /// Check if a given component colides with the current one
        /// </summary>
        /// <param name="i_OtherComponent">The component that we want to check collision against</param>
        /// <returns>true if the given component colides the current one or false otherwise</returns>
        public bool     CheckForCollision(Sprite i_OtherComponent)
        {
            return IsColiding(i_OtherComponent);
        }

        public abstract void    Colided(Sprite i_OtherComponent);
        
        #endregion

        /// <summary>
        /// Performs a rectangle collision detection between the component 
        /// bounds and the given component bounds
        /// </summary>
        /// <param name="i_OtherComponent">The component that we want to check collision against</param>
        /// <returns>true if the given component colides the current one or false otherwise</returns>
        protected bool  IsColiding(Sprite i_OtherComponent)
        {
            return this.Bounds.Intersects(i_OtherComponent.Bounds);
        }
    }
}
