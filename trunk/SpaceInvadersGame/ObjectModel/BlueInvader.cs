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
        private const int k_Score = 200;

        private const int k_TextureYPositionVal = 32;

        #region CTOR's

        public BlueInvader(
            Game i_Game,
            int i_UpdateOrder,
            int i_InvaderListNum)
            : base(i_Game, i_UpdateOrder, i_InvaderListNum)
        {
            TintColor = Color.Blue;
            this.SourcePosition = new Vector2(0, k_TextureYPositionVal);
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
