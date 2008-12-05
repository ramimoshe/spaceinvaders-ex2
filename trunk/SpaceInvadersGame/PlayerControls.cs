using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame
{
    /// <summary>
    /// The class defines a player input keys
    /// </summary>
    public class PlayerControls : IPlayerControls
    {        
        private Keys m_ActionKey;
        private Keys m_RightKey;
        private Keys m_LeftKey;
        private bool m_ActionUsingMouse;

        // TODO: Check if the bool constant is ok

        /// <summary>
        /// An empty constructor that creates the class using all the default 
        /// keys from the Constants class
        /// </summary>
        public PlayerControls()
            : this(
            Constants.r_DefaultActionKey,
            Constants.r_DefaultLeftMovmentKey,
            Constants.r_DefaultRightMovmentKey,
            Constants.k_DeafultActionUsingMouse)
        {
        }
       
        public PlayerControls(
            Keys i_ActionKey,
            Keys i_LeftKey,
            Keys i_RightKey,
            bool i_ActionUsingMouse)
        {
            m_ActionKey = i_ActionKey;
            m_LeftKey = i_LeftKey;
            m_RightKey = i_RightKey;
            m_ActionUsingMouse = i_ActionUsingMouse;
        }

        #region IPlayerControls Members        

        /// <summary>
        /// Read only property that gets the players fire key
        /// </summary>
        public Keys     ActionKey
        {
            get 
            {
                return m_ActionKey;
            }
        }

        /// <summary>
        /// Read only property that gets the players right movment key
        /// </summary>
        public Keys     RightMovmentKey
        {
            get 
            {
                return m_RightKey;
            }
        }

        /// <summary>
        /// Read only property that gets the players left movment key 
        /// </summary>
        public Keys     LeftMovmentKey
        {
            get 
            {
                return m_LeftKey;
            }
        }
             
        /// <summary>
        /// Read only property that gets an indication wether the player can
        /// use the mouse button as an action key
        /// </summary>
        public bool ActionUsingMouse
        {
            get 
            {
                return m_ActionUsingMouse;
            }
        }

        #endregion
    }
}
