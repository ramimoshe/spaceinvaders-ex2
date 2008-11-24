using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.ObjectModel
{    
    /// <summary>
    /// A blue enemy in the enemies matrix
    /// </summary>
    public class BlueEnemy : Enemy
    {
        private const string k_AssetName = @"Sprites\Enemy0201_32x32";
        private const int k_Score = 200;

        #region CTOR's

        public BlueEnemy(Game i_Game, Vector2 i_Position) 
            : this(i_Game, i_Position, 0)
        {
        }

        public BlueEnemy(Game i_Game, Vector2 i_Position, int i_UpdateOrder)
            : base(k_AssetName, i_Game, i_Position, i_UpdateOrder)
        {
            TintColor = Color.Blue;
        }

        #endregion

        /// <summary>
        /// A property for the enemy score
        /// </summary>
        public override int     Score
        {
            get 
            { 
                return k_Score; 
            }
        }        
    }
}
