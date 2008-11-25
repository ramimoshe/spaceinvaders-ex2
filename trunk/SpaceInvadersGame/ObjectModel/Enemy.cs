using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel
{
    public abstract class Enemy : CollidableSprite
    {
        #region CTORs

        public Enemy(string i_AssetName, Game i_Game)
            : this(i_AssetName, i_Game, 0, 0)
        {
        }

        public Enemy(string i_AssetName, Game i_Game, int i_UpdateOrder)
            : this(i_AssetName, i_Game, i_UpdateOrder, 0)
        {
        }

        public Enemy(
            string i_AssetName, 
            Game i_Game, 
            int i_UpdateOrder,
            int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {                        
        }
#endregion

        /// <summary>
        /// A property that return the enemy score
        /// </summary>
        public abstract int     Score
        {
            get;
        }

        protected override void     InitPosition()
        {
            // We override the method but do nothing cause the matrix already
            // set the position from outside when it creates the invaders
        }
    }
}
