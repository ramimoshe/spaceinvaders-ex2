using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel;

namespace SpaceInvadersGame
{    
    // TODO: Change the class to sprite

    /// <summary>
    /// A drawable game component that is responsible to draw the 
    /// players lives on the screen
    /// </summary>
    public class PlayerLivesDrawer : DrawableGameComponent
    {        
        private const int k_SpaceBetweenLives = 25;

        private IPlayer m_Player;
        private Vector2 m_Position;
        private List<Texture2D> m_DrawTextures;
        private SpriteBatch m_SpriteBatch;
        private Color m_TintColor;

        public PlayerLivesDrawer(Game i_Game, IPlayer i_Player)
            : base(i_Game)
        {
            m_Player = i_Player;
        }

        /// <summary>
        /// Property that gets/sets the position where the component will
        /// draw the textures
        /// </summary>
        public Vector2 DrawPosition
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// Initializes the component draw position and textures
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            // TODO: Check if i need the sprite batch

            m_SpriteBatch = new SpriteBatch(GraphicsDevice);

            m_Position = new Vector2(
                Game.GraphicsDevice.Viewport.Width -
                (m_Player.PlayerTexture.Width * Constants.k_LivesDrawScaleValue),
                m_Player.PlayerTexture.Height * Constants.k_LivesDrawScaleValue);

            m_DrawTextures = new List<Texture2D>();

            for (int i = 0; i < m_Player.RemainingLives; i++)
            {
                m_DrawTextures.Add(m_Player.PlayerTexture);
            }

            Vector4 color = Color.White.ToVector4();
            color.W = Constants.k_LivesDrawTransparentValue;
            m_TintColor = new Color(color);

        }

        // TODO: Change it here to observer that notifies when the ship 
        // was hit

        public override void    Update(GameTime gameTime)
        {
            if (m_DrawTextures.Count != m_Player.RemainingLives)
            {
                int texturesNum = m_DrawTextures.Count;

                for (int i = 0 ;
                     i < m_DrawTextures.Count - m_Player.RemainingLives; 
                     i++)
                 {
                        m_DrawTextures.RemoveAt(m_DrawTextures.Count - 1);
                 }   
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the players lives on the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void    Draw(GameTime gameTime)
        {
            Vector2 positionToDraw = m_Position;

            m_SpriteBatch.Begin();

            for (int i = 0; i < m_DrawTextures.Count; i++)
            {
                // TODO: Move all the the variables to constants

                m_SpriteBatch.Draw(
                    m_DrawTextures[i],
                    m_Position,
                    null,
                    m_TintColor,
                    0,
                    Vector2.Zero,
                    new Vector2(Constants.k_LivesDrawScaleValue),
                    SpriteEffects.None,
                    1f);

                m_Position.X -= k_SpaceBetweenLives;
            }

            m_SpriteBatch.End();

            m_Position = positionToDraw;

            base.Draw(gameTime);
        }
    }
}
