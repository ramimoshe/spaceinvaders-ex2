using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.Services;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    /// <summary>
    /// Notifies when item needs to execute something
    /// </summary>
    public delegate void MenuItemExecuteEventHandler();

    /// <summary>
    /// Notifies when a non selected menu item was selected
    /// </summary>
    /// <param name="i_Item">Selected item</param>
    public delegate void MenuItemSelectedEventHandler(MenuItem i_Item);

    public class MenuItem : SpriteFontComponent
    {
        private bool m_Selected = false;
        readonly private Color r_TintWhenSelected = Color.OrangeRed;
        readonly private Color r_TintWhenDeSelected = Color.Silver;
        public event MenuItemExecuteEventHandler Executed;
        public event MenuItemSelectedEventHandler Selected;
        private const string k_DefaultAssetName = @"Fonts\Tahoma28";
        protected IInputManager m_InputManager;

        public MenuItem(Game i_Game, string i_Text, MenuItemExecuteEventHandler executedHandler)
            : base(i_Game, k_DefaultAssetName, i_Text)
        {
            TintColor = r_TintWhenDeSelected;

            if (executedHandler != null)
            {
                Executed += new MenuItemExecuteEventHandler(executedHandler);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_InputManager = Game.Services.GetService(typeof(InputManager)) as IInputManager;

            Animations.Add(new PulseAnimation(
                    "MenuItem_selected",
                    Vector2.One * 0.97f,
                    Vector2.One * 1.03f,
                    TimeSpan.Zero,
                    false,
                    TimeSpan.FromSeconds(0.35f)));
        }

        public bool IsSelected
        {
            get
            {
                return m_Selected;
            }

            set
            {
                if (value != m_Selected)
                {
                    m_Selected = value;
                    if (m_Selected)
                    {
                        OnMenuItemSelected();
                    }
                    else
                    {
                        OnMenuItemDeSelected();
                    }
                }
            }
        }

        public void OnMenuItemSelected()
        {
            TintColor = r_TintWhenSelected;
            
            if (Animations != null)
            {
                Animations.Restart(TimeSpan.Zero);
            }

            if (Selected != null)
            {
                Selected(this);
            }
        }

        public void OnMenuItemDeSelected()
        {
            TintColor = r_TintWhenDeSelected;

            if (Animations != null)
            {
                Animations.Reset();
                Animations.Pause();
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            bool mouseOnItem = ScreenBoundsAfterScale.Intersects(
                                        new Rectangle(
                                            m_InputManager.MouseState.X, 
                                            m_InputManager.MouseState.Y, 
                                            1, 
                                            1));

            if (!IsSelected && mouseOnItem)
            {
                IsSelected = true;
            }

            if ((m_InputManager.KeyPressed(Keys.Enter) ||
                 (m_InputManager.ButtonPressed(eInputButtons.Left) && mouseOnItem))
                && IsSelected)
            {
                Execute();
            }
        }

        protected void Execute()
        {
            if (Executed != null)
            {
                Executed();
            }
        }
    }
}
