using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DreidelGame.Services;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A parent for all the drawable components in the game
    /// </summary>
    public abstract class BaseDrawableComponent : DrawableGameComponent
    {
        private const bool k_NeedTextureDefault = false;
        protected Vector3 m_Position = Vector3.Zero;
        private Vector3 m_Rotations = Vector3.Zero;
        protected Vector3 m_Scales = Vector3.One;
        protected Matrix m_WorldMatrix = Matrix.Identity;
        protected VertexElement[] m_VertexElements = null;
        private VertexDeclaration m_VertexDeclaration = null;
        private Texture2D m_Texture = null;
        private VertexBuffer m_VertexBuffer;
        private IndexBuffer m_IndexBuffer;
        private int m_VerticesNum;
        private short[] m_BufferIndices;
        private Camera m_Camera;

        protected float m_RotationsPerSecond = 0;

        private bool m_SpinComponent;

        private bool m_NeedTexture;

        /// <summary>
        /// Marks if the current component is a Texture primitive and needs a 
        /// texture in draw
        /// </summary>
        public bool      NeedTexture
        {
            get { return m_NeedTexture; }
            protected set { m_NeedTexture = value; }
        }

        /// <summary>
        /// Gets/sets the component rotation transformation values
        /// </summary>
        public virtual Vector3      Rotations
        {
            get
            {
                return m_Rotations;
            }

            set
            {
                m_Rotations = value;
            }
        }

        /// <summary>
        /// Mark if we want to spin the current component
        /// </summary>
        public virtual bool     SpinComponent
        {
            get
            {
                return m_SpinComponent;
            }

            set
            {
                m_SpinComponent = value;
            }
        }

        /// <summary>
        /// Gets/sets the number of rotations the component make in a second
        /// </summary>
        public virtual float    RotationsPerSecond
        {
            get
            {
                return m_RotationsPerSecond;
            }

            set
            {
                m_RotationsPerSecond = value;
            }
        }

        /// <summary>
        /// Gets/sets the components position transformation values
        /// </summary>
        public virtual Vector3      Position
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
        /// Gets/sets the components scale transformation values
        /// </summary>
        public virtual Vector3      Scales
        {
            get
            {
                return m_Scales;
            }

            set
            {
                m_Scales = value;
            }
        }

        /// <summary>
        /// Gets/sets the components texture
        /// </summary>
        public virtual Texture2D    Texture
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
        /// Gets/sets the components VertexBuffer
        /// </summary>
        public VertexBuffer     ComponentVertexBuffer
        {
            get { return m_VertexBuffer; }

            set { m_VertexBuffer = value; }
        }

        /// <summary>
        /// Gets/sets the components VertexBuffer
        /// </summary>
        public IndexBuffer     ComponentIndexBuffer
        {
            get { return m_IndexBuffer; }

            set { m_IndexBuffer = value; }
        }        

        /// <summary>
        /// Gets/sets the number of vertices the component has
        /// </summary>
        public int      VerticesNum
        {
            get { return m_VerticesNum; }
            set { m_VerticesNum = value; }
        }        

        /// <summary>
        /// Gets/sets the indecies used for the IndexBuffer
        /// </summary>
        public short[]    BufferIndices
        {
            get { return m_BufferIndices; }
            set { m_BufferIndices = value; }
        }

        /// <summary>
        /// Gets the number of triangles the component has
        /// </summary>
        public abstract int     TriangleNum
        {
            get;
        }

        /// <summary>
        /// CTOR. Creates a new instance
        /// </summary>
        /// <param name="i_Game">hosting game</param>
        public BaseDrawableComponent(Game i_Game)
            : this(i_Game, null, k_NeedTextureDefault)
        {
        }

        /// <summary>
        /// CTOR. Creates a new instance
        /// </summary>
        /// <param name="i_Game">hosting game</param>
        /// <param name="i_VertexElements">The vertex elements defining the component.
        /// This is in order to load VertexDeclaration.
        /// Valid values are: null, VertexPositionTexture.VertexElements,
        /// VertexPositionColor</param>
        public BaseDrawableComponent(Game i_Game, VertexElement[] i_VertexElements)
            : this(i_Game, i_VertexElements, k_NeedTextureDefault)
        {
        }

        /// <summary>
        /// CTOR. Creates a new instance
        /// </summary>
        /// <param name="i_Game">hosting game</param>
        /// <param name="i_VertexElements">The vertex elements defining the component.
        /// This is in order to load VertexDeclaration.
        /// Valid values are: null, VertexPositionTexture.VertexElements,
        /// VertexPositionColor</param>
        /// <param name="i_NeedTexture">Defines if a 2D texture is needed by the component</param>
        public BaseDrawableComponent(
            Game i_Game,
            VertexElement[] i_VertexElements,
            bool i_NeedTexture) : base(i_Game)
        {
            m_VertexElements = i_VertexElements;
            NeedTexture = i_NeedTexture;
        }

        /// <summary>
        /// Initialize the camera member from the games services list
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_Camera = Game.Services.GetService(typeof(Camera)) as Camera;
        }

        /// <summary>
        /// Creates the components vertex decleration
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            if (m_VertexElements != null)
            {
                m_VertexDeclaration = new VertexDeclaration(GraphicsDevice, m_VertexElements);
            }
        }

        /// <summary>
        /// Update the component by changing the rotation transformation (only if SpinComponent is
        /// true), and creates a new WorldMatrix represents the new state
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (SpinComponent)
            {
                m_Rotations.Y += (float)i_GameTime.ElapsedGameTime.TotalSeconds * m_RotationsPerSecond;
            }

            m_WorldMatrix =
                Matrix.Identity *
                Matrix.CreateScale(m_Scales) *
                Matrix.CreateRotationX(m_Rotations.X) *
                Matrix.CreateRotationY(m_Rotations.Y) *
                Matrix.CreateRotationZ(m_Rotations.Z) *
                Matrix.CreateTranslation(m_Position);
        }

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public abstract void    InitBuffers();

        // TODO: Remove the method

        /// <summary>
        /// Drawing the component
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the game time</param>
        //public abstract void    DoDraw(GameTime i_GameTime);

        /// <summary>
        /// Draws the component. 
        /// first we'll search ffor a shared effect, creates if none exists, updates the effect
        /// world matrix and draw parameters according to the component values, and than draws
        /// the component
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the game time</param>
        public override void    Draw(GameTime i_GameTime)
        {            
            // Getting effect from services
            BasicEffect effect = Game.Services.GetService(typeof(BasicEffect)) as BasicEffect;

            // Validating effect is initialized
            if (effect == null)
            {
                effect = new BasicEffect(GraphicsDevice, null);
            }

            effect.World = m_WorldMatrix;
            effect.View = m_Camera.ViewMatrix;

            // Setting vertex declaration for graphics device
            if (m_VertexElements != null)
            {
                GraphicsDevice.VertexDeclaration = m_VertexDeclaration;
            }

            // Setting effects behaviour for Texture\Color
            if (Texture != null)
            {
                effect.Texture = Texture;
                effect.TextureEnabled = true;
                effect.VertexColorEnabled = false;

                // TODO: Remove

                /*this.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                    this.GraphicsDevice, 
                    VertexPositionTexture.VertexElements);*/

                this.GraphicsDevice.Vertices[0].SetSource(
                    m_VertexBuffer, 
                    0, 
                    VertexPositionTexture.SizeInBytes);
            }
            else
            {
                effect.TextureEnabled = false;
                effect.VertexColorEnabled = true;

                // TODO: Remove

                /*this.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                    this.GraphicsDevice,
                    VertexPositionColor.VertexElements);*/

                this.GraphicsDevice.Vertices[0].SetSource(
                    m_VertexBuffer, 
                    0, 
                    VertexPositionColor.SizeInBytes);
            }
            
            effect.Begin();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                // TODO: Remove the remark
                //DoDraw(i_GameTime);

                /*if (Texture != null)
                {
                    this.GraphicsDevice.Vertices[0].SetSource(m_VertexBuffer, 0, VertexPositionTexture.SizeInBytes);
                }
                else
                {
                    this.GraphicsDevice.Vertices[0].SetSource(m_VertexBuffer, 0, VertexPositionColor.SizeInBytes);
                }*/

                this.GraphicsDevice.Indices = m_IndexBuffer;
                this.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    0,
                    0,
                    this.VerticesNum,
                    0,
                    this.TriangleNum);

                pass.End();
            }

            effect.End();

            base.Draw(i_GameTime);
        }
    }
}
