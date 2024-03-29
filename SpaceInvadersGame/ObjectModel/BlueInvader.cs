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
        /// Returns the invader type (BlueInvader) 
        /// </summary>
        public override eInvadersType InvaderType
        {
            get { return eInvadersType.BlueInvader; }
        }

        /// <summary>
        /// Read only property that gets the blue invader hit action value
        /// </summary>
        protected override eSoundActions HitAction
        {
            get { return eSoundActions.BlueInvaderHit; }
        }        
    }
}
