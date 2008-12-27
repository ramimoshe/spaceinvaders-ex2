using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceInvadersGame;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
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

        public DisplayMenuScreen(Game i_Game)
            : base(i_Game, k_DisplayMenuName)
        {
            m_FullScreenItem = new OptionsMenuItem(
                                    Game,
                                    new List<string>(m_FullScreenText),
                                    m_FullScreenItem_Changed,
                                    m_FullScreenItem_Changed);
            m_ResizeItem = new OptionsMenuItem(
                                    Game,
                                    new List<string>(m_ResizeText),
                                    m_ResizeItem_Changed,
                                    m_ResizeItem_Changed);
            m_MouseItem = new OptionsMenuItem(
                        Game,
                        new List<string>(m_MouseText),
                        m_MouseItem_Changed,
                        m_MouseItem_Changed);
            m_DoneItem = new MenuItem(Game, "Done", m_DoneItem_Executed);

            Add(m_FullScreenItem);
            Add(m_ResizeItem);
            Add(m_MouseItem);
            Add(m_DoneItem);
        }

        private void m_FullScreenItem_Changed()
        {
            ((GraphicsDeviceManager)Game.Services.GetService(typeof(GraphicsDeviceManager))).ToggleFullScreen();
        }

        private void m_ResizeItem_Changed()
        {
            Game.Window.AllowUserResizing = !Game.Window.AllowUserResizing;
        }

        private void m_MouseItem_Changed()
        {
            Game.IsMouseVisible = !Game.IsMouseVisible;
        }

        private void m_DoneItem_Executed()
        {
            ExitScreen();
        }   
    }
}
