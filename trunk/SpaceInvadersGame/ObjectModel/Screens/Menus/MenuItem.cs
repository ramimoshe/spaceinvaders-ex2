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

    /// <summary>
    /// Implements an items which apears in menu screens, and enables execution.
    /// Execution behaviour should be implmented by instantiating menu.
    /// </summary>
    public class MenuItem : SpriteFontComponent
    {
        private readonly Color r_TintWhenSelected = Color.OrangeRed;
        private readonly Color r_TintWhenDeSelected = Color.Silver;
        private const string k_DefaultAssetName = @"Fonts\Tahoma28";

        private bool m_Selected = false;
        protected IInputManager m_InputManager;

        public event MenuItemExecuteEventHandler Executed;

        public event MenuItemSelectedEventHandler Selected;

        /// <summary>
        /// Initialize the item
        /// </summary>
        /// <param name="i_Game">The hosting Game</param>
        /// <param name="i_Text">Text displayed in the item</param>
        /// <param name="executedHandler">Delegate to be invoked when item executes</param>
        public  MenuItem(
            Game i_Game, 
            string i_Text, 
            MenuItemExecuteEventHandler executedHandler)
            : base(i_Game, k_DefaultAssetName, i_Text)
        {
            TintColor = r_TintWhenDeSelected;

            if (executedHandler != null)
            {
                Executed += new MenuItemExecuteEventHandler(executedHandler);
            }
        }

        /// <summary>
        /// Initialize input manager and pulse animation
        /// </summary>
        public override void    Initialize()
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

        /// <summary>
        /// Gets / Sets whether item is currently selected
        /// </summary>
        public bool     IsSelected
        {
            get
            {
                return m_Selected;
            }

            set
            {
                // If value is changed then proper actions are taken
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

        /// <summary>
        /// Actions performed when item is selected
        /// Tint color is set and pulse animation restarts
        /// </summary>
        public void     OnMenuItemSelected()
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

        /// <summary>
        /// Actions performed when item is de-selected
        /// </summary>
        public void     OnMenuItemDeSelected()
        {
            TintColor = r_TintWhenDeSelected;

            if (Animations != null)
            {
                Animations.Reset();
                Animations.Pause();
            }
        }

        /// <summary>
        /// Handles default behaviour for mouse and keyboard actions
        /// </summary>
        /// <param name="i_GameTime">Hosting game elapsed time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            bool mouseOnItem = ScreenBoundsAfterScale.Intersects(
                                        new Rectangle(
                                            m_InputManager.MouseState.X, 
                                            m_InputManager.MouseState.Y, 
                                            1, 
                                            1));

            // Checking if mouse is inside item bounds (makes is selected)
            if (!IsSelected && mouseOnItem)
            {
                IsSelected = true;
            }

            // Checking if button is pressed to execute item
            if ((m_InputManager.KeyPressed(Keys.Enter) ||
                 (m_InputManager.ButtonPressed(eInputButtons.Left) && mouseOnItem))
                && IsSelected)
            {
                Execute();
            }
        }

        /// <summary>
        /// Notifies observer of execution
        /// </summary>
        protected void      Execute()
        {
            if (Executed != null)
            {
                Executed();
            }
        }
    }
}
