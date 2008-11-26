using System;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// This class implements a 2Dimensional sprite, which generalize the use of
    /// 2D texture and implements the required methods for collision detection.
    /// </summary>
    public abstract class Sprite : DrawableLoadableComponent
    {
        /// <summary>
        /// The constructor intiates the base constructor 
        /// (DrawableLoadableComponent)
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        public  Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game)
        {
        }

        /// <summary>
        /// Calls Base constructor
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        /// <param name="i_UpdateOrder">Number defining the order in which update 
        /// of all game components is called</param>
        /// <param name="i_DrawOrder">Number defining the order in which draw
        /// of all game components is called</param>
        public  Sprite(
            string i_AssetName,
            Game i_Game,
            int i_UpdateOrder,
            int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        #region Data members & Properties

        /// <summary>
        /// Defines whether component's SpriteBatch is a shared batch
        /// </summary>
        private bool m_UseSharedBatch = false;

        /// <summary>
        /// Component's SpriteBatch (responsible for drawing the sprite)
        /// </summary>
        private SpriteBatch m_SpriteBatch;

        /// <summary>
        /// Gets/sets the SpriteBatch
        /// </summary>
        public SpriteBatch  SpriteBatch
        {
            get
            {
                return m_SpriteBatch;
            }
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        /// <summary>
        /// Defines the 2D texture
        /// </summary>
        private Texture2D   m_Texture;

        /// <summary>
        /// Gets/sets the 2D texture
        /// </summary>
        public Texture2D    Texture
        {
            get
            {
                return m_Texture;
            }

            set
            {
                m_Texture = value;
            }
        }

        /// <summary>
        /// Gets object bounds according to sprite size and position
        /// </summary>
        public Rectangle    Bounds
        {
            get
            {
                return new Rectangle(
                    (int)m_Position.X,
                    (int)m_Position.Y,
                    m_Texture.Width,
                    m_Texture.Height);
            }
        }


        /// <summary>
        /// Defines object 2Dimensional position
        /// </summary>
        protected Vector2   m_Position;

        /// <summary>
        /// Get/Sets object position
        /// </summary>
        public Vector2  Position
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
        /// Sprite's tint-color
        /// </summary>
        protected Color m_TintColor = Color.White;

        /// <summary>
        /// Get/Sets tint-color
        /// </summary>
        public Color    TintColor
        {
            get
            {
                return m_TintColor;
            }

            set
            {
                m_TintColor = value;
            }
        }

        /// <summary>
        /// Sprite motion vector, as pixels per second.
        /// </summary>
        protected Vector2 m_MotionVector = Vector2.Zero;

        /// <summary>
        /// Gets/Sets sprite's motion vector regarding both axis' (X/Y)
        /// Motion in both directions is defined in pixels per second
        /// X value - Positive (negative) value defines right (left) movement
        /// Y value - Positive (negative) value defines down (up) movement
        /// </summary>
        public Vector2  MotionVector
        {
            get
            {
                return m_MotionVector;
            }

            set
            {
                m_MotionVector = value;
            }
        }

        #endregion

        /// <summary>
        /// Loads sprite's asset into content manager, and initializes spriteBatch
        /// </summary>
        protected override void     LoadContent()
        {
            m_Texture = m_ContentManager.Load<Texture2D>(m_AssetName);

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
        /// Updates game position according to motion vector
        /// </summary>
        /// <param name="gameTime">Elapsed time since last call</param>
        public override void Update(GameTime gameTime)
        {
            m_Position += MotionVector * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// Initializes sprite's position to default location (zero)
        /// </summary>
        protected override void InitPosition()
        {
            m_Position = Vector2.Zero;
        }

        /// <summary>
        /// Default draw behaviour. Sprite is drawn using private or shared
        /// spriteBatch.
        /// </summary>
        /// <param name="gameTime">Elapsed time since last call</param>
        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            m_SpriteBatch.Draw(Texture, Position, TintColor);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
