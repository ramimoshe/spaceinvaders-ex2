using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A09_Ex02_Koby_021766944_Inbar_015267479
{
    public class YellowEnemy : Enemy
    {
        private const string k_AssetName = @"Sprites\Enemy0301_32x32";
        private const int k_Score = 100;        

        public YellowEnemy(Game i_Game, Vector2 i_Position) 
            : this(i_Game, i_Position, 0)
        {
        }

        public YellowEnemy(Game i_Game, Vector2 i_Position, int i_UpdateOrder)
            : base(k_AssetName, i_Game, i_Position, i_UpdateOrder)
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
