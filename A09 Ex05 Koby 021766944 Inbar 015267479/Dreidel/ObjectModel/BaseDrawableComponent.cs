using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public BaseDrawableComponent(Game i_Game)
            : this(i_Game, null, k_NeedTextureDefault)
        {
        }

        public BaseDrawableComponent(Game i_Game, VertexElement[] i_VertexElements)
            : this(i_Game, i_VertexElements, k_NeedTextureDefault)
        {            
        }

        public BaseDrawableComponent(
            Game i_Game,
            VertexElement[] i_VertexElements,
            bool i_NeedTexture) : base(i_Game)
        {
            m_VertexElements = i_VertexElements;
            NeedTexture = i_NeedTexture;
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
        /// <param name="gameTime"></param>
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
        /// Drawing the component
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the game time</param>
        public abstract void    DoDraw(GameTime i_GameTime);

        /// <summary>
        /// Draws the component. 
        /// first we'll search ffor a shared effect, creates if none exists, updates the effect
        /// world matrix and draw parameters according to the component values, and than draws
        /// the component
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the game time</param>
        public override void    Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);

            if (m_VertexElements != null)
            {
                GraphicsDevice.VertexDeclaration = m_VertexDeclaration;
            }

            BasicEffect effect = Game.Services.GetService(typeof(BasicEffect)) as BasicEffect;

            if (effect == null)
            {
                effect = new BasicEffect(GraphicsDevice, null);
            }

            effect.World = m_WorldMatrix;

            if (Texture != null)
            {
                effect.Texture = Texture;
                effect.TextureEnabled = true;
                effect.VertexColorEnabled = false;
            }
            else
            {
                effect.TextureEnabled = false;
                effect.VertexColorEnabled = true;
            }
            
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                DoDraw(i_GameTime);
                pass.End();
            }

            effect.End();
        }
    }
}
