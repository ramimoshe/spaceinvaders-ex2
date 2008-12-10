using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// Implements a SpriteFont component that can be used to write on the
    /// screen with
    /// </summary>
    public class SpriteFontComponent : DrawableLoadableComponent
    {
        private string m_Text = String.Empty;

        private SpriteFont m_Font;

        private Color m_TintColor = Color.White;

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        /// <summary>
        /// Defines the position where we want to draw the string
        /// </summary>
        private Vector2 m_Position;

        /// <summary>
        /// Defines whether component's SpriteBatch is a shared batch
        /// </summary>
        private bool m_UseSharedBatch = false;

        /// <summary>
        /// Component's SpriteBatch (responsible for drawing the sprite)
        /// </summary>
        private SpriteBatch m_SpriteBatch;

        public SpriteFontComponent(
            Game i_Game, 
            string i_FontAssetName)
            : base(i_FontAssetName, i_Game)
        {
        }

        /// <summary>
        /// Gets/sets the text we want to write on the screen
        /// </summary>
        public string   Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        /// <summary>
        /// Gets/sets the position we want to write in the screen
        /// </summary>
        public Vector2  Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }	

        /// <summary>
        /// Initialize the component bounds
        /// </summary>
        protected override void     InitBounds()
        {
            m_Position = Vector2.Zero;
        }        

        /// <summary>
        /// Loads the desire SpriteFont
        /// </summary>
        protected override void     LoadContent()
        {
            m_Font = m_ContentManager.Load<SpriteFont>(m_AssetName);

            // Checking if spritebatch already exists
            if (m_SpriteBatch == null)
            {
                // Trying to receive game's sprite batch
                m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(this.GraphicsDevice);
                }
                else
                {
                    m_UseSharedBatch = true;
                }
            }

            base.LoadContent();
        }

        /// <summary>
        /// Draws the given text in the screen according to the component
        /// font and position
        /// </summary>
        /// <param name="i_GameTime">A snapshot to the game time</param>
        public override void    Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            m_SpriteBatch.DrawString(
                m_Font,
                Text,
                m_Position,
                TintColor);                

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(i_GameTime);
        }       
    }
}
