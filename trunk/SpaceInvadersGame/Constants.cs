using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame
{
    /// <summary>
    /// The class holds variables for constants that are used throughout
    /// the game
    /// </summary>
    public class Constants
    {
        // Read only variables that holds the default player keys
        public static readonly Keys r_DefaultActionKey = Keys.Enter;
        public static readonly Keys r_DefaultLeftMovmentKey = Keys.Left;
        public static readonly Keys r_DefaultRightMovmentKey = Keys.Right;
        public const bool k_DeafultActionUsingMouse = true;

        public const string k_Player1AssetName = @"Sprites\Ship01_32x32";
        public const string k_Player2AssetName = @"Sprites\Ship02_32x32";
    }
}
