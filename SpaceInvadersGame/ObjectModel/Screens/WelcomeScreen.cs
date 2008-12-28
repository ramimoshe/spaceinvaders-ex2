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
    /// <summary>
    /// Implements the game welcome screen
    /// </summary>
    public class WelcomeScreen : SpaceInvadersScreenAbstract
    {
        private SpriteFontComponent m_WelcomeMessage;
        private SpriteFontComponent m_HitEnterMessage;

        /// <summary>
        /// Initializes the screen
        /// </summary>
        /// <param name="i_Game">the hosting game</param>
        public  WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            m_WelcomeMessage = new SpriteFontComponent(
                i_Game, 
                @"Fonts\David28", 
                "Welcome");
            m_HitEnterMessage = new SpriteFontComponent(
                                    i_Game, 
                                    @"Fonts\David", 
                                    "Hit Enter to continue");
            m_WelcomeMessage.TintColor = Color.White;
            m_HitEnterMessage.TintColor = Color.PowderBlue;

            this.Add(m_WelcomeMessage);
            this.Add(m_HitEnterMessage);
            this.HasFocus = true;
        }

        /// <summary>
        /// Setting screen message positions 
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            Vector2 center = m_WelcomeMessage.ViewPortCenter;

            m_WelcomeMessage.PositionOfOrigin = center;
            m_WelcomeMessage.PositionOrigin = m_WelcomeMessage.SpriteCenter;
            m_HitEnterMessage.PositionOfOrigin = new Vector2(center.X, center.Y + m_WelcomeMessage.HeightAfterScale);
            m_HitEnterMessage.PositionOrigin = m_HitEnterMessage.SpriteCenter;
        }

        /// <summary>
        /// Exiting the screen when key is pressed
        /// </summary>
        /// <param name="gameTime"></param>
        public override void    Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Enter))
            {
                ExitScreen();
            }
        }
    }
}
