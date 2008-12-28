using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceInvadersGame;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    /// <summary>
    /// Menu for display settings
    /// </summary>
    public class DisplayMenuScreen : MenuTypeScreen
    {
        private const string k_DisplayMenuName = "Display Options";

        private string[] m_FullScreenText = { "Full Screen Mode: Off", "Full Screen Mode: On" };
        private string[] m_ResizeText = { "Allow Window Resizing: Off", "Allow Window Resizing: On" };
        private string[] m_MouseText = { "Mouse Visibility: Visible", "Mouse Visibility: InVisible" };

        private OptionsMenuItem m_FullScreenItem;
        private OptionsMenuItem m_ResizeItem;
        private OptionsMenuItem m_MouseItem;
        private MenuItem m_DoneItem;

        /// <summary>
        /// Initializes all menu items
        /// </summary>
        /// <param name="i_Game">Hosting game</param>
        public  DisplayMenuScreen(Game i_Game)
            : base(i_Game, k_DisplayMenuName)
        {
            m_FullScreenItem = new OptionsMenuItem(
                                    Game,
                                    new List<string>(m_FullScreenText),
                                    m_FullScreenItem_Modified);
            m_ResizeItem = new OptionsMenuItem(
                                    Game,
                                    new List<string>(m_ResizeText),
                                    m_ResizeItem_Modified);
            m_MouseItem = new OptionsMenuItem(
                        Game,
                        new List<string>(m_MouseText),
                        m_MouseItem_Modified);
            m_DoneItem = new MenuItem(Game, "Done", m_DoneItem_Executed);

            Add(m_FullScreenItem);
            Add(m_ResizeItem);
            Add(m_MouseItem);
            Add(m_DoneItem);
        }

        /// <summary>
        /// Toggles full screen mode
        /// </summary>
        /// <param name="i_Dummy">Text of menu item</param>
        private void    m_FullScreenItem_Modified(int i_Dummy)
        {
            ((GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager))).ToggleFullScreen();
        }

        /// <summary>
        /// Toggles window resize mode
        /// </summary>
        /// <param name="i_Dummy">Text of menu item</param>
        private void    m_ResizeItem_Modified(int i_Dummy)
        {
            Game.Window.AllowUserResizing = !Game.Window.AllowUserResizing;
        }

        /// <summary>
        /// Toggles mouse visibility
        /// </summary>
        /// <param name="i_Dummy">Text of menu item</param>
        private void    m_MouseItem_Modified(int i_Dummy)
        {
            Game.IsMouseVisible = !Game.IsMouseVisible;
        }

        /// <summary>
        /// Exits screen
        /// </summary>
        private void    m_DoneItem_Executed()
        {
            ExitScreen();
        }   
    }
}
