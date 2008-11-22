using System;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel
{
    public abstract class Sprite : DrawableLoadableComponent
    {

        public Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game)
        {
        }

        public Sprite(
            string i_AssetName,
            Game i_Game,
            int i_UpdateOrder,
            int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        #region Data members & Properties

        private bool m_UseSharedBatch = false;
        private SpriteBatch m_SpriteBatch;

        public SpriteBatch SpriteBatch
        {
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        private Texture2D m_Texture;

        public Texture2D Texture
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

        Rectangle m_Bounds;

        public Rectangle Bounds
        {
            get
            {
                return m_Bounds;
            }

            set
            {
                m_Bounds = value;
            }
        }

        protected Vector2 m_Position;

        public Vector2 Position
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;

                // Update the bounds value
                m_Bounds.X = (int)m_Position.X;
                m_Bounds.Y = (int)m_Position.Y;
            }
        }

        protected Color m_TintColor = Color.White;

        public Color TintColor
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

        private Vector2 m_MotionVector = Vector2.Zero;

        public Vector2 MotionVector
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

        private Vector2 m_Velocity = Vector2.One;

        public Vector2  Velocity
        {
            get
            {
                return m_Velocity;
            }

            set
            {
                m_Velocity = value;
            }
        }

        #endregion

        protected override void LoadContent()
        {
            m_Texture = m_ContentManager.Load<Texture2D>(m_AssetName);

            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                    m_SpriteBatch = new SpriteBatch(m_GraphicsDevice);
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {                        
            Position += (m_MotionVector * m_Velocity) * (float)gameTime.ElapsedGameTime.TotalSeconds;                        
            base.Update(gameTime);
        }

        protected override void     InitBounds()
        {
            this.Bounds = new Rectangle(0, 0, m_Texture.Width, m_Texture.Height);
        }

        protected override void InitPosition()
        {
            m_Position = Vector2.Zero;
        }

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
