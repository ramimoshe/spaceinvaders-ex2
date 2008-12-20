using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using XnaGamesInfrastructure.ObjectModel.Screens;
using SpaceInvadersGame.ObjectModel;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    public class WelcomeScreen : GameScreen
    {
        private BackGround m_Background;
        private SpriteFontComponent m_WelcomeMessage;

        public  WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            m_WelcomeMessage = new SpriteFontComponent(i_Game, @"Fonts\David", "Welcome");
            m_WelcomeMessage.TintColor = Color.White;
            m_WelcomeMessage.Scale = new Vector2(3, 3);

            m_Background = new BackGround(i_Game, 100);
            this.Add(m_Background);
            this.Add(m_WelcomeMessage);   
        }

        public override void Initialize()
        {
            base.Initialize();
            m_WelcomeMessage.PositionOfOrigin = new Vector2(
                                                    GraphicsDevice.Viewport.Width / 2,
                                                    GraphicsDevice.Viewport.Height / 2);
            m_WelcomeMessage.PositionOrigin = m_WelcomeMessage.SpriteCenter;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
