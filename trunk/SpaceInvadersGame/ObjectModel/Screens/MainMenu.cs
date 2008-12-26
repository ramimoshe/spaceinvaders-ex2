using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    class MainMenuScreen : MenuTypeScreen
    {
        const string k_MainMenuName = "Main Menu";

        public MainMenuScreen(Game i_Game)
            : base(i_Game, k_MainMenuName)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            MenuItem playersMenuItem = new MenuItem(Game, "Players:   {0}");
            playersMenuItem.Executed += new MenuItemEventHandler(playersMenuItemModified);
//            this.Add();

            this.Add(new MenuItem(Game, "nice"));
            this.Add(new MenuItem(Game, "bye"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void playersMenuItemModified()
        {
        }
    }
}
