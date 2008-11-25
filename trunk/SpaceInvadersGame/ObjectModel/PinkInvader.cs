using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// A pink enemy in the enemies matrix
    /// </summary>
    public class PinkInvader : Invader
    {
        private const string k_AssetName = @"Sprites\Enemy0101_32x32";
        private const int k_Score = 300;

        public PinkInvader(Game i_Game) 
            : this(i_Game, 0)
        {            
        }

        public PinkInvader(Game i_Game, int i_UpdateOrder)
            : base(k_AssetName, i_Game, i_UpdateOrder)
        {
            TintColor = Color.Pink;
        }

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
