using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// A blue invader in the invaders matrix
    /// </summary>
    public class BlueInvader : Invader
    {
        private const string k_AssetName = @"Sprites\Enemy0201_32x32";
        private const int k_Score = 200;

        #region CTOR's

        public BlueInvader(Game i_Game) 
            : this(i_Game, 0)
        {
        }

        public BlueInvader(Game i_Game, int i_UpdateOrder)
            : base(k_AssetName, i_Game, i_UpdateOrder)
        {
            TintColor = Color.Blue;
        }

        #endregion

        /// <summary>
        /// A property for the invader score
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
