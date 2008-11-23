using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace A09_Ex02_Koby_021766944_Inbar_015267479
{
    public class BackGround : Sprite
    {

        private const string k_AssetName = @"Sprites\BG_Space01_1024x768";

        public BackGround(Game i_Game) : base(k_AssetName, i_Game,
                                              Int32.MaxValue, Int32.MinValue)
        {
        }

        public override bool    CheckForCollision(ICollidable i_OtherComponent)
        {
            return false;
        }

    }
}
