using System;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;

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

        public override void Initialize()
        {
            base.Initialize();

            m_Animations = new CompositeAnimation(this);
            m_ViewportBounds = new Rectangle(
                        Game.GraphicsDevice.Viewport.X,
                        Game.GraphicsDevice.Viewport.Y,
                        Game.GraphicsDevice.Viewport.Width,
                        Game.GraphicsDevice.Viewport.Height);
        }

        #region Data members & Properties        

        private Rectangle m_ViewportBounds;

        // TODO: Check if the variable is nessecary

        protected Color[] m_ColorData;

        public Color[] ColorData
        {
            get { return m_ColorData; }
            set { m_ColorData = value; }
        }

        /// <summary>
        /// Defines whether component's SpriteBatch is a shared batch
        /// </summary>
        private bool m_UseSharedBatch = false;

        /// <summary>
        /// Component's SpriteBatch (responsible for drawing the sprite)
        /// </summary>
        private SpriteBatch m_SpriteBatch;

        /// <summary>
        /// Defines the draw layer depth
        /// </summary>
        protected float m_LayerDepth;

        /// <summary>
        /// Gets/sets the draw layer depth
        /// </summary>
        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        /// <summary>
        /// Defines the draw rotation
        /// </summary>
        protected float m_Rotation = 0;

        /// <summary>
        /// Gets/sets the draw rotation value
        /// </summary>
        public float    Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        /// <summary>
        /// Defines the draw scale factor
        /// </summary>
        protected Vector2 m_Scale = Vector2.One;

        /// <summary>
        /// Gets/sets the draw scale factor
        /// </summary>
        public Vector2 Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        public float WidthAfterScale
        {
            get
            {
                return m_WidthBeforeScale * m_Scale.X;
            }
            set
            {
                m_WidthBeforeScale = (int) (value / m_Scale.X);
            }
        }

        public float HeightAfterScale
        {
            get
            {
                return m_HeightBeforeScale * m_Scale.X;
            }
            set
            {
                m_HeightBeforeScale = (int) (value / m_Scale.Y);
            }
        }

        public float WidthBeforeScale
        {
            get
            {
                return m_WidthBeforeScale;
            }
            set
            {
                m_WidthBeforeScale = (int)value;
            }
        }

        public float HeightBeforeScale
        {
            get
            {
                return m_HeightBeforeScale;
            }
            set
            {
                m_HeightBeforeScale = (int) value;
            }
        }

        protected Vector2 m_PositionOfOrigin = Vector2.Zero;
        /// <summary>
        /// Represents the location of the sprite's origin point in screen coorinates
        /// </summary>
        public Vector2 PositionOfOrigin
        {
            get
            {
                return m_PositionOfOrigin;
            }
            set
            {
                if (m_PositionOfOrigin != value)
                {
                    m_PositionOfOrigin = value;
                    //OnPositionChanged();
                }
            }
        }

        public Vector2 m_PositionOrigin;
        public Vector2 PositionOrigin
        {
            get
            {
                return m_PositionOrigin;
            }
            set
            {
                m_PositionOrigin = value;
            }
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;
        public Vector2 RotationOrigin
        {
            get
            {
                return m_RotationOrigin;
            }// r_SpriteParameters.RotationOrigin; }
            set
            {
                m_RotationOrigin = value;
            }
        }

        public Vector2 TopLeftPosition
        {
            get
            {
                return this.PositionOfOrigin - this.PositionOrigin;
            }
            set
            {
                this.PositionOfOrigin = value + this.PositionOrigin;
            }
        }

        public Rectangle ScreenBoundsAfterScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthAfterScale,
                    (int)this.HeightAfterScale);
            }
        }

        public Rectangle ScreenBoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        public virtual Rectangle ColliadbleBounds
        {
            get
            {
                return ScreenBoundsAfterScale;
            }
        }


        /// <summary>
        /// Defines the source rectangle position
        /// </summary>
        protected Vector2 m_sourceRectanglePosition = Vector2.Zero;

        /// <summary>
        /// Gets/sets the position of the draw source rectangle
        /// </summary>
        public Vector2 SourcePosition
        {
            get 
            { 
                return m_sourceRectanglePosition; 
            }
            
            set 
            { 
                m_sourceRectanglePosition = value;

                SourceRectangle = new Rectangle(
                    (int)SourcePosition.X,
                    (int)SourcePosition.Y,
                    m_WidthBeforeScale,
                    m_HeightBeforeScale);
            }
        }

        /// <summary>
        /// The source rectangle for the draw method. defines the bounds
        /// of the image we want to draw in the loaded texture.
        /// </summary>
        protected Rectangle? m_SourceRectangle = null;             

        /// <summary>
        /// Gets/sets the source textures rectangle
        /// </summary>
        public Rectangle? SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value;}
        }

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

        protected CompositeAnimation m_Animations;

        public CompositeAnimation Animations
        {
            get
            {
                return m_Animations;
            }
            set
            {
                m_Animations = value;
            }
        }

        /// <summary>
        /// Defines the 2D texture
        /// </summary>
        protected Texture2D   m_Texture;

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
        /// Defines the original texture width
        /// </summary>
        protected int m_WidthBeforeScale;

        /// <summary>
        /// Defines the original texture height
        /// </summary>
        protected int m_HeightBeforeScale;

        /// <summary>
        /// Gets object bounds according to sprite size and position
        /// </summary>
        public Rectangle    Bounds
        {
            get
            {
                return new Rectangle(
                    (int)PositionForDraw.X,
                    (int)PositionForDraw.Y,
                    (int)WidthBeforeScale,
                    (int)HeightBeforeScale);
            }
        }

        // TODO: Remove the variable

        /// <summary>
        /// Defines object 2Dimensional position
        /// </summary>
//        protected Vector2   m_PositionForDraw;

        /// <summary>
        /// Get/Sets object position
        /// </summary>
        public Vector2  PositionForDraw
        {
            get
            {
                return this.PositionOfOrigin - this.PositionOrigin + this.RotationOrigin;
            }

            set
            {
                PositionOfOrigin = value;
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


        private float m_AngularVelocity = 0;
        /// <summary>
        /// Radians per Second on X Axis
        /// </summary>
        public float AngularVelocity
        {
            get
            {
                return m_AngularVelocity;
            }
            set
            {
                m_AngularVelocity = value;
            }
        }
        #endregion

        /// <summary>
        /// Loads sprite's asset into content manager, and initializes spriteBatch
        /// </summary>
        protected override void     LoadContent()
        {
            m_Texture = m_ContentManager.Load<Texture2D>(m_AssetName);                        

            // TODO: Check if i need this here

            /*Game.GraphicsDevice.Indices = null;
            Game.GraphicsDevice.Vertices[0].SetSource(null, 0, 0);*/
            
            // TODO: Add remark

            Game.GraphicsDevice.Textures[0] = null;

            m_ColorData = new Color[m_Texture.Width * m_Texture.Height];
            Texture.GetData(m_ColorData, 0, Texture.Width * Texture.Height);

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
        public override void    Update(GameTime i_GameTime)
        {
            float totalSeconds = (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            PositionOfOrigin += MotionVector * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;

            base.Update(i_GameTime);

            Animations.Animate(i_GameTime);
        }

        /// <summary>
        /// Initializes sprite's position to default location (zero)
        /// </summary>
        protected override void     InitBounds()
        {
            m_PositionOfOrigin = Vector2.Zero;

            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;

            InitSourceRectangle();
            InitOrigins();
        }

        protected virtual void InitOrigins()
        {
        }

        /// <summary>
        /// Initialize the source rectangle for the draw only if the source 
        /// position had been set (isn't (0,0))
        /// </summary>
        protected virtual void  InitSourceRectangle()
        {
            if (SourcePosition != Vector2.Zero)
            {
                SourceRectangle = new Rectangle(
                    (int)SourcePosition.X,
                    (int)SourcePosition.Y,
                    m_WidthBeforeScale,
                    m_HeightBeforeScale);
            }
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

            // TODO: Add rotation origin

            m_SpriteBatch.Draw(
                m_Texture, 
                this.PositionForDraw,
                this.SourceRectangle, 
                this.TintColor,
                this.Rotation, 
                this.RotationOrigin, 
                this.Scale,
                SpriteEffects.None, 
                this.LayerDepth);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }


        /// <summary>
        /// Creates a memberwise clone of sprite
        /// </summary>
        /// <returns>A copy of this sprite</returns>
        public Sprite   ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }      
    }
}
