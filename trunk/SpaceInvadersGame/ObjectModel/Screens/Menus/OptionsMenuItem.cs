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
    /// Used to notify menu item option 
    /// </summary>
    public delegate void MenuOptionChangedEventHandler(int i_CurrentOptionIndex);

    public class OptionsMenuItem : MenuItem
    {
        private List<string> m_OptionsText;
        private int m_CurrentOptionIndex = 0;

        public  OptionsMenuItem(
                Game i_Game,
                List<string> i_OptionsText, 
                MenuOptionChangedEventHandler modificationHandler)
            : this(i_Game, i_OptionsText, 0, modificationHandler)
        {
        }

        public OptionsMenuItem(
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

        public List<string> OptionsText
        {
            get
            {
                return m_OptionsText;
            }
        }

        public int CurrentOptionIndex
        {
            get
            {
                return m_CurrentOptionIndex;
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            int changeIndex = 0;

            if (IsSelected)
            {                
                if (m_InputManager.KeyPressed(Keys.PageDown) || 
                    m_InputManager.ScrollWheelDelta < 0)
                {
                    changeIndex = -1;
                }
                else if (   m_InputManager.KeyPressed(Keys.PageUp) ||
                            m_InputManager.ScrollWheelDelta > 0 ||
                            m_InputManager.ButtonPressed(eInputButtons.Right))
                {
                    changeIndex = 1;
                }

                if (changeIndex != 0)
                {
                    m_CurrentOptionIndex = (m_CurrentOptionIndex + m_OptionsText.Count + changeIndex) % m_OptionsText.Count;
                    Text = m_OptionsText[m_CurrentOptionIndex];
                    OnOptionModified();
                }

            }
        }

        public event MenuOptionChangedEventHandler Modified;

        private void OnOptionModified()
        {
            if (Modified != null)
            {
                Modified(m_CurrentOptionIndex);
            }
        }
    }
}
