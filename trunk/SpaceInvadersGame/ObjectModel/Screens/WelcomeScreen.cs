using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using XnaGamesInfrastructure.ObjectModel.Screens;
using SpaceInvadersGame.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.Services;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    public class WelcomeScreen : GameScreen
    {
        private SpriteFontComponent m_WelcomeMessage;

        public  WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            m_WelcomeMessage = new SpriteFontComponent(i_Game, @"Fonts\David28", "Welcome");
            m_WelcomeMessage.TintColor = Color.White;

            // TODO: Change the parameter to a constant

            this.Add(m_WelcomeMessage);
            this.HasFocus = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_WelcomeMessage.PositionOfOrigin = new Vector2(
                                                    GraphicsDevice.Viewport.Width / 2,
                                                    GraphicsDevice.Viewport.Height / 2);
            m_WelcomeMessage.PositionOrigin = m_WelcomeMessage.SpriteCenter;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Enter))
            {
                ExitScreen();
            }
        }
    }
}
