using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.Services;
using Microsoft.Xna.Framework.Input;
using XnaGamesInfrastructure.ServiceInterfaces;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    /// <summary>
    /// Used to notify menu item option was modified, with the current index value
    /// </summary>
    public delegate void MenuOptionChangedEventHandler(int i_CurrentOptionIndex);

    /// <summary>
    /// Class is a sub-class of MenuItem which enables the modification of options.
    /// Class owns multiple string, which could be changed according to user selection,
    /// and notify the menu that a change has been made.
    /// </summary>
    public class OptionsMenuItem : MenuItem
    {
        private List<string> m_OptionsText;
        private int m_CurrentOptionIndex = 0;

        /// <summary>
        /// Init's the Item
        /// </summary>
        /// <param name="i_Game">Hosting game</param>
        /// <param name="i_OptionsText">Text list as options for item</param>
        /// <param name="modificationHandler">Subscriber for Modified event</param>
        public  OptionsMenuItem(
                Game i_Game,
                List<string> i_OptionsText, 
                MenuOptionChangedEventHandler modificationHandler)
            : this(i_Game, i_OptionsText, 0, modificationHandler)
        {
        }

        /// <summary>
        /// Init's the Item, and enables setting a specific initial string for the item
        /// </summary>
        /// <param name="i_Game">Hosting game</param>
        /// <param name="i_OptionsText">Text list as options for item</param>
        /// <param name="modificationHandler">Subscriber for Modified event</param>
        /// <param name="i_StartingTextIndex">Initial string index</param>
        public  OptionsMenuItem(
                  Game i_Game,
                  List<string> i_OptionsText,
                  int i_StartingTextIndex,
                  MenuOptionChangedEventHandler modificationHandler)
            : base(i_Game, i_OptionsText[i_StartingTextIndex], null)
        {
            m_OptionsText = i_OptionsText;
            m_CurrentOptionIndex = i_StartingTextIndex;

            if (modificationHandler != null)
            {
                Modified += new MenuOptionChangedEventHandler(modificationHandler);
            }
        }

        /// <summary>
        /// Handles default behaviour for option modification by keyboard and mouse
        /// </summary>
        /// <param name="i_GameTime">Hosting game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            int changeIndex = 0;

            // Validating item is selected
            if (IsSelected)
            {         
                // Checking if modification is required by input devices
                if (m_InputManager.KeyPressed(Keys.PageDown) || 
                    m_InputManager.ScrollWheelDelta < 0)
                {
                    changeIndex = -1;
                }
                else if (m_InputManager.KeyPressed(Keys.PageUp) ||
                         m_InputManager.ScrollWheelDelta > 0 ||
                         m_InputManager.ButtonPressed(eInputButtons.Right))
                {
                    changeIndex = 1;
                }

                // Change current index according to required change
                if (changeIndex != 0)
                {
                    m_CurrentOptionIndex = (m_CurrentOptionIndex + m_OptionsText.Count + changeIndex) % m_OptionsText.Count;
                    Text = m_OptionsText[m_CurrentOptionIndex];
                    OnOptionModified();
                }
            }
        }

        public event MenuOptionChangedEventHandler Modified;

        /// <summary>
        /// Notifies observer a modification was made
        /// </summary>
        private void    OnOptionModified()
        {
            if (Modified != null)
            {
                Modified(m_CurrentOptionIndex);
            }
        }
    }
}
