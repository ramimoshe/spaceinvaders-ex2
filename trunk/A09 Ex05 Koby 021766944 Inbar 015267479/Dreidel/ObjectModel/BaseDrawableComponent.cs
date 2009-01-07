using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public abstract class BaseDrawableComponent : DrawableGameComponent
    {
        protected Vector3 m_Position = Vector3.Zero;
        private Vector3 m_Rotations = Vector3.Zero;
        protected Vector3 m_Scales = Vector3.One;
        protected Matrix m_WorldMatrix = Matrix.Identity;
        protected VertexElement[] m_VertexElements = null;
        private VertexDeclaration m_VertexDeclaration = null;
        private Texture2D m_Texture = null;

        protected float m_RotationsPerSecond = 0;

        private bool m_SpinComponent;

        public virtual Vector3 Rotations
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
        public virtual bool SpinComponent
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

        public virtual float RotationsPerSecond
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

        public virtual Vector3 Position
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

        public virtual Vector3 Scales
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

        public virtual Texture2D Texture
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
            : this(i_Game, null)
        {
        }

        public BaseDrawableComponent(Game i_Game, VertexElement[] i_VertexElements)
            : base(i_Game)
        {
            m_VertexElements = i_VertexElements;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (m_VertexElements != null)
            {
                m_VertexDeclaration = new VertexDeclaration(GraphicsDevice, m_VertexElements);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (SpinComponent)
            {
                m_Rotations.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * m_RotationsPerSecond;
            }

            m_WorldMatrix =
                /*I*/ Matrix.Identity *
                /*S*/ Matrix.CreateScale(m_Scales) *
                /*R*/ Matrix.CreateRotationX(m_Rotations.X) *
                        Matrix.CreateRotationY(m_Rotations.Y) *
                        Matrix.CreateRotationZ(m_Rotations.Z) *
                /* No Orbit */
                /*T*/ Matrix.CreateTranslation(m_Position);
        }

        public abstract void DoDraw(GameTime i_GameTime);

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

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
                DoDraw(gameTime);
                pass.End();
            }

            effect.End();
        }
    }
}
