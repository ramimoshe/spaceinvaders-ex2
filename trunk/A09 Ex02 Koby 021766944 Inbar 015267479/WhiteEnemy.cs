using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A09_Ex02_Koby_021766944_Inbar_015267479
{
    public class WhiteEnemy : Enemy
    {
        private const string k_AssetName = @"Sprites\Enemy0301_32x32";
        private const int k_Score = 100;

        public WhiteEnemy(Game i_Game, Vector2 i_Position) 
            : base(k_AssetName, i_Game, i_Position)
        {
            TintColor = Color.White;
        }

        public override int     Score
        {
            get { return k_Score; }
        }        
    }
}
