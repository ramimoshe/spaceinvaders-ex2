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
    /// The class is the games pause screen
    /// </summary>
    public class PauseScreen : GameScreen
    {
        private readonly Keys r_ContinueGameKey = Keys.R;
        private readonly string r_PauseText = "PAUSE";
        private readonly string r_GameContinueText = 
            "Press R To Continue Playing";

        private SpriteFontComponent m_PauseMessage;
        private SpriteFontComponent m_ContinuePlayMessage;

        public PauseScreen(Game i_Game)
            : base(i_Game)
        {
            m_PauseMessage = new SpriteFontComponent(
                i_Game, 
                @"Fonts\David28", 
                r_PauseText);            
            m_ContinuePlayMessage = new SpriteFontComponent(
                i_Game, 
                @"Fonts\David28", 
                r_GameContinueText);

            m_PauseMessage.TintColor = Color.White;
            m_ContinuePlayMessage.TintColor = Color.White;
            // TODO: Change the parameter to a constant

            this.Add(m_PauseMessage);
            this.Add(m_ContinuePlayMessage);
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = .7f;
        }

        /// <summary>
        /// Initialize the screen by setting the messages positions
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();
            
            Vector2 center = new    Vector2(
                                    GraphicsDevice.Viewport.Width / 2,
                                    GraphicsDevice.Viewport.Height / 2);
            
            m_PauseMessage.PositionOfOrigin = center;
            m_PauseMessage.PositionOrigin = m_PauseMessage.SpriteCenter;

            m_ContinuePlayMessage.Text = m_ContinuePlayMessage.Text;
            m_ContinuePlayMessage.Scale = Vector2.One * 0.5f;
            m_ContinuePlayMessage.PositionOfOrigin = new Vector2(center.X, center.Y + m_PauseMessage.HeightAfterScale);
            m_ContinuePlayMessage.PositionOrigin = m_ContinuePlayMessage.SpriteCenter;
        }

        /// <summary>
        /// Update the screen by checking if the key to continue the game had
        /// been pressed and exit if it had
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(r_ContinueGameKey))
            {
                ExitScreen();
            }
        }
    }
}
