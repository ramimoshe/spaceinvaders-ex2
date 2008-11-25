using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// A yellow enemy in the enemies matrix
    /// </summary>
    public class YellowInvader : Invader
    {
        private const string k_AssetName = @"Sprites\Enemy0301_32x32";
        private const int k_Score = 100;        

        public YellowInvader(Game i_Game) 
            : this(i_Game, 0)
        {
        }

        public YellowInvader(Game i_Game, int i_UpdateOrder)
            : base(k_AssetName, i_Game, i_UpdateOrder)
        {
            TintColor = Color.LightYellow;
        }

        /// <summary>
        /// A property to the score that the current enemy adds to the player
        /// </summary>
        public override int Score
        {
            get 
            { 
                return k_Score; 
            }
        }        
    }
}
