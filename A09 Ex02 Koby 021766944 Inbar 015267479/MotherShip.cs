using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A09_Ex02_Koby_021766944_Inbar_015267479
{
    /// <summary>
    /// Represents the invaders mother ship
    /// </summary>
    public class MotherShip : Enemy
    {
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const int k_Score = 500;

        public MotherShip(Game i_Game, Vector2 i_Position) 
            : this(i_Game, i_Position, 0)
        {            
        }

        public MotherShip(Game i_Game, Vector2 i_Position, int i_UpdateOrder)
            : base(k_AssetName, i_Game, i_Position, i_UpdateOrder)
        {
            TintColor = Color.Red;
        }     

        public override int Score
        {
            get 
            { 
                return k_Score; 
            }
        }  
    }
}
