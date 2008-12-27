using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.Services;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    public delegate void MenuOptionChangedEventHandler();

    public class OptionsMenuItem : MenuItem
    {
        private List<string> m_OptionsText;
        private int m_CurrentOptionIndex = 0;

        public  OptionsMenuItem(
                Game i_Game, 
                List<string> i_OptionsText,
                MenuOptionChangedEventHandler decreasedHandler,
                MenuOptionChangedEventHandler increasedHandler)
            : this(i_Game, i_OptionsText, 0, decreasedHandler, increasedHandler)
        {
        }

        public OptionsMenuItem(
                  Game i_Game,
                  List<string> i_OptionsText,
                  int i_StartingTextIndex,
                  MenuOptionChangedEventHandler decreasedHandler,
                  MenuOptionChangedEventHandler increasedHandler)
            : base(i_Game, i_OptionsText[i_StartingTextIndex], null)
        {
            m_OptionsText = i_OptionsText;
            m_CurrentOptionIndex = i_StartingTextIndex;

            if (decreasedHandler != null)
            {
                Decreased += new MenuOptionChangedEventHandler(decreasedHandler);
            }

            if (increasedHandler != null)
            {
                Increased += new MenuOptionChangedEventHandler(increasedHandler);
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

            if (IsSelected)
            {
                int changeIndex = 0;
                
                if (m_InputManager.KeyPressed(Keys.PageDown))
                {
                    changeIndex = 1;
                }
                else if (m_InputManager.KeyPressed(Keys.PageUp))
                {
                    changeIndex = -1;
                }

                int nextIndex = (int) MathHelper.Clamp(
                                m_CurrentOptionIndex + changeIndex, 
                                0, 
                                m_OptionsText.Count - 1);

                if (nextIndex != m_CurrentOptionIndex)
                {
                    m_CurrentOptionIndex = nextIndex;
                    Text = m_OptionsText[nextIndex];

                    if (changeIndex == 1)
                    {
                        OnDecreased();
                    }
                    else
                    {
                        OnIncreased();
                    }
                }
            }
        }

        public event MenuOptionChangedEventHandler Decreased;
        public event MenuOptionChangedEventHandler Increased;

        private void OnDecreased()
        {
            if (Decreased != null)
            {
                Decreased();
            }
        }

        private void OnIncreased()
        {
            if (Increased != null)
            {
                Increased();
            }
        }
    }
}
