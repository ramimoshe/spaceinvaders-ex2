using System;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace XnaGamesInfrastructure.ObjectModel
{
    public abstract class Sprite : DrawableLoadableComponent, ICollidable
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

        public Rectangle Bounds
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

        protected Vector2 m_MotionVector = Vector2.Zero;

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

        #endregion

        protected ICollisionManager m_CollisionManager;

        public override void Initialize()
        {
            base.Initialize();

            m_CollisionManager = (ICollisionManager)this.Game.Services.GetService(typeof(ICollisionManager));
            
            if (m_CollisionManager != null)
            {
                m_CollisionManager.AddObjectToMonitor(this);
            }
        }

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
            m_Position += m_MotionVector * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (m_MotionVector != Vector2.Zero)
            {
                OnPositionChanged();
            }

            base.Update(gameTime);
        }

        protected override void InitBounds()
        {
            
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

        #region ICollidable Members

        /// <summary>
        /// Check if a given component colides with the current one
        /// </summary>
        /// <param name="i_OtherComponent">The component that we want to check collision against</param>
        /// <returns>true if the given component colides the current one or false otherwise</returns>
        public virtual bool CheckForCollision(ICollidable i_OtherComponent)
        {
            return this.Bounds.Intersects(i_OtherComponent.Bounds);
        }

        public virtual void Collided(ICollidable i_OtherComponent)
        {
            this.Visible = false ;
            //MotionVector *= -1;
        }

        public event PositionChangedEventHandler PositionChanged;
        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this);
            }
        }
        
        #endregion
    }
}
