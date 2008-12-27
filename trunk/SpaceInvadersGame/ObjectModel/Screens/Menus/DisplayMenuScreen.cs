using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    public class DisplayMenuScreen : MenuTypeScreen
    {
        private const string k_DisplayMenuName = "Sound Options";

        public DisplayMenuScreen(Game i_Game)
            : base(i_Game, k_DisplayMenuName)
        {
        }
    }
}
