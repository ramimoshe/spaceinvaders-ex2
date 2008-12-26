using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel.Screens;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    public abstract class MenuTypeScreen : GameScreen
    {
        protected List<MenuItem> m_MenuItems = new List<MenuItem>();
        protected SpriteFontComponent m_Title;
        private int m_CurrentMenuItem = -1;

        public MenuTypeScreen(Game i_Game, String i_Title)
            : base(i_Game)
        {
            m_Title = new SpriteFontComponent(i_Game, @"Fonts\David40", i_Title);
            m_Title.TintColor = Color.SlateBlue;
            Add(m_Title);
        }

        public override void Add(IGameComponent i_Component)
        {
            base.Add(i_Component);

            if (i_Component is MenuItem)
            {
                m_MenuItems.Add((MenuItem)i_Component);
                SetItemPosition(m_MenuItems.Count - 1);

                if (m_MenuItems.Count == 1)
                {
                    m_MenuItems[0].IsSelected = true;
                    m_CurrentMenuItem = 0;
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_Title.PositionOfOrigin = new Vector2(m_Title.ViewPortCenter.X, m_Title.HeightAfterScale);
            m_Title.PositionOrigin = m_Title.SpriteCenter;

            for (int i = 0; i < m_MenuItems.Count; ++i )
            {
                SetItemPosition(i);
            }
        }

        private void SetItemPosition(int i_ItemIndex)
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Down))
            {
                ChangeCurrentItem(true);
            }
            else if (InputManager.KeyPressed(Keys.Up))
            {
                ChangeCurrentItem(false);
            }
        }

        private void ChangeCurrentItem(bool i_MoveDown)
        {
            int itemsToMove = i_MoveDown ? 1 : -1;
            int newCurrentItem = (int) MathHelper.Clamp(m_CurrentMenuItem + itemsToMove, 0, m_MenuItems.Count - 1);

            if (newCurrentItem != m_CurrentMenuItem)
            {
                m_MenuItems[m_CurrentMenuItem].IsSelected = false;
                m_MenuItems[newCurrentItem].IsSelected = true;
                m_CurrentMenuItem = newCurrentItem;
            }
        }
    }
}
