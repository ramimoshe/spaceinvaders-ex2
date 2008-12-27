using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// A pink invader in the invaders matrix
    /// </summary>
    public class PinkInvader : Invader
    {
        private const int k_TextureYPositionVal = 0;

        public PinkInvader(
            Game i_Game, 
            int i_UpdateOrder, 
            int i_InvaderListNum)
            : base(i_Game, i_UpdateOrder, i_InvaderListNum)
        {
            TintColor = Color.Pink;
            this.SourcePosition = new Vector2(0, k_TextureYPositionVal);
        }

        /// <summary>
        /// Returns the invader type (PinkInvader) 
        /// </summary>
        public override eInvadersType InvaderType
        {
            get { return eInvadersType.PinkInvader; }
        }

        /// <summary>
        /// Read only property that gets the pink invader hit action value
        /// </summary>
        protected override eSoundActions HitAction
        {
            get { return eSoundActions.PinkInvaderHit; }
        }        
    }
}
