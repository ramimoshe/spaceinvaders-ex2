using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.Interfaces
{
    /// <summary>
    /// Interface that defines the players main keys: action key (fire key) 
    /// and movement keys (left & right)    
    /// </summary>
    public interface IPlayerControls
    {
        Keys ActionKey
        {
            get;
        }

        Keys RightMovmentKey
        {
            get;
        }

        Keys LeftMovmentKey
        {
            get;
        }

        bool UseMouse
        {
            get;
        }
    }
}
