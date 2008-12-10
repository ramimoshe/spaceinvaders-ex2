using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.Interfaces
{
    /// <summary>
    /// Interface that defines a player main keys: action key (fire key) 
    /// and movement keys (left & right)    
    /// </summary>
    public interface IPlayerControls
    {
        /// <summary>
        /// Read only property that states the player action key
        /// </summary>
        Keys    ActionKey
        {
            get;
        }

        /// <summary>
        /// Read only property that states the player right
        /// movement key
        /// </summary>
        Keys    RightMovmentKey
        {
            get;
        }

        /// <summary>
        /// Read only property that states the player left 
        /// movement key
        /// </summary>
        Keys    LeftMovmentKey
        {
            get;
        }

        /// <summary>
        /// Read only property that states whether the player can
        /// use the mouse input
        /// </summary>
        bool    UseMouse
        {
            get;
        }
    }
}
