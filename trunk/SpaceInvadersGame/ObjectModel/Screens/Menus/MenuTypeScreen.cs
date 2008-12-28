using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel.Screens;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    /// <summary>
    /// A parent to all the menu screens in the game
    /// </summary>
    public abstract class MenuTypeScreen : SpaceInvadersScreenAbstract
    {
        protected List<MenuItem> m_MenuItems = new List<MenuItem>();
        protected SpriteFontComponent m_Title;
        private int m_CurrentMenuItem = -1;

        /// <summary>
        /// Inits the menu
        /// </summary>
        /// <param name="i_Game">Hosting game</param>
        /// <param name="i_Title">Menu Title</param>
        public  MenuTypeScreen(Game i_Game, string i_Title)
            : base(i_Game)
        {
            m_Title = new SpriteFontComponent(i_Game, @"Fonts\David40", i_Title);
            m_Title.TintColor = Color.SlateBlue;
            Add(m_Title);
        }

        /// <summary>
        /// Adds a new component to the screen. 
        /// In case it's a MenuItem, we'll save it in a dedicated list in
        /// addition to the parent list
        /// </summary>
        /// <param name="i_Component">The component we want to add to the screen</param>
        public override void    Add(IGameComponent i_Component)
        {
            base.Add(i_Component);

            MenuItem item = i_Component as MenuItem;

            // Adding item and registering item selected event
            if (item != null)
            {
                m_MenuItems.Add(item);
                setItemPosition(m_MenuItems.Count - 1);
                item.Selected += new MenuItemSelectedEventHandler(item_Selected);

                if (m_MenuItems.Count == 1)
                {
                    m_MenuItems[0].IsSelected = true;
                    m_CurrentMenuItem = 0;
                }
            }
        }

        /// <summary>
        /// Setting current item according to selected item
        /// </summary>
        /// <param name="i_Item"></param>
        private void    item_Selected(MenuItem i_Item)
        {
            int newSelectedItem = m_MenuItems.IndexOf(i_Item);

            if (newSelectedItem != m_CurrentMenuItem)
            {
                setCurrentItem(newSelectedItem);
            }
        }

        /// <summary>
        /// Initialize the menu by setting the position of all the menu items
        /// in the screen
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_Title.PositionOfOrigin = new Vector2(m_Title.ViewPortCenter.X, m_Title.HeightAfterScale);
            m_Title.PositionOrigin = m_Title.SpriteCenter;

            for (int i = 0; i < m_MenuItems.Count; ++i )
            {
                setItemPosition(i);
            }
        }

        /// <summary>
        /// Setting item position on screen according to it's index
        /// </summary>
        /// <param name="i_ItemIndex">Item's index in menu items list</param>
        private void    setItemPosition(int i_ItemIndex)
        {
            Vector2 position;

            if (i_ItemIndex == 0)
            {
                position = m_Title.PositionOfOrigin;
                position.Y += m_Title.HeightAfterScale * 1.2f;
            }
            else
            {
                MenuItem prevItem = m_MenuItems[i_ItemIndex - 1];
                position = prevItem.PositionOfOrigin;
                position.Y += prevItem.HeightAfterScale * 1.2f;
            }

            MenuItem item = m_MenuItems[i_ItemIndex];
            item.PositionOfOrigin = position;
            m_MenuItems[i_ItemIndex].PositionOrigin = item.SpriteCenter;
        }

        /// <summary>
        /// Updates the current selected menu item according to the player input
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Keys.Down))
            {
                changeCurrentItem(true);
            }
            else if (InputManager.KeyPressed(Keys.Up))
            {
                changeCurrentItem(false);
            }
        }

        /// <summary>
        /// Changes the current menu item according to the player input
        /// </summary>
        /// <param name="i_MoveDown">Mark if we want to get the next item
        /// or the previous one (true - get the next menu item, false - get
        /// the previous menu item)</param>
        private void    changeCurrentItem(bool i_MoveDown)
        {
            int itemsToMove = i_MoveDown ? 1 : -1;
            int newCurrentItem = m_CurrentMenuItem + itemsToMove + m_MenuItems.Count;
            newCurrentItem %= m_MenuItems.Count;

            setCurrentItem(newCurrentItem);
        }

        /// <summary>
        /// Sets the new active menu item
        /// </summary>
        /// <param name="i_NewItemIndex">The index of the new chosen menu item 
        /// </param>
        private void    setCurrentItem(int i_NewItemIndex)
        {
            if (i_NewItemIndex != m_CurrentMenuItem && m_CurrentMenuItem != -1)
            {
                m_MenuItems[m_CurrentMenuItem].IsSelected = false;
                m_MenuItems[i_NewItemIndex].IsSelected = true;
                m_CurrentMenuItem = i_NewItemIndex;

                // Play the change menu item cue
                PlayActionCue(eSoundActions.MenuItemChanged);
            }
        }
    }
}
