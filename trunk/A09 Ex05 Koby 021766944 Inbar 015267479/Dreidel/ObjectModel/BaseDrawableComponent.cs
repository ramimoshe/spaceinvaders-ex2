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
        protected Vector3 m_Rotations = Vector3.Zero;
        protected Vector3 m_Scales = Vector3.One;
        protected Matrix m_WorldMatrix = Matrix.Identity;
        protected VertexElement[] m_VertexElements = null;
        
        private bool m_SharedGraphicsDevice = false;

        private BasicEffect m_BasicEffect;
        private Matrix m_ProjectionFieldOfView;
        private Matrix m_PointOfView;

        public BasicEffect Effect
        {
            get
            {
                return m_BasicEffect;
            }

            set
            {
                m_BasicEffect = value;
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

            float k_NearPlaneDistance = 0.5f;
            float k_FarPlaneDistance = 1000.0f;
            float k_ViewAngle = MathHelper.PiOver4;

            // we are storing the field-of-view data in a matrix:
            m_ProjectionFieldOfView = Matrix.CreatePerspectiveFieldOfView(
                k_ViewAngle,
                (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height,
                k_NearPlaneDistance,
                k_FarPlaneDistance);

            // we want to shoot the center of the world:
            Vector3 targetPosition = Vector3.Zero;
            // we are standing 50 units in front of our target:
            Vector3 pointOfViewPosition = new Vector3(0, 0, 50);
            // we are not standing on our head:
            Vector3 pointOfViewUpDirection = new Vector3(0, 1, 0);

            // we are storing the point-of-view data in a matrix:
            m_PointOfView = Matrix.CreateLookAt(
                pointOfViewPosition, targetPosition, pointOfViewUpDirection);

            m_BasicEffect = new BasicEffect(GraphicsDevice, null);
            m_BasicEffect.View = m_PointOfView;
            m_BasicEffect.Projection = m_ProjectionFieldOfView;
            m_BasicEffect.VertexColorEnabled = true;

            AfterLoadContent();
        }

        protected virtual void AfterLoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_Rotations.Y += (float)gameTime.ElapsedGameTime.TotalSeconds;

            m_WorldMatrix =
                /*I*/ Matrix.Identity *
                /*S*/ Matrix.CreateScale(m_Scales) *
                /*R*/ Matrix.CreateRotationX(m_Rotations.X) *
                        Matrix.CreateRotationY(m_Rotations.Y) *
                        Matrix.CreateRotationZ(m_Rotations.Z) *
                /* No Orbit */
                /*T*/ Matrix.CreateTranslation(m_Position);
        }

        public bool SharedGraphicsDevice
        {
            get
            {
                return m_SharedGraphicsDevice;
            }

            set
            {
                m_SharedGraphicsDevice = value;
            }
        }

        public abstract void DoDraw(GameTime i_GameTime);

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (m_VertexElements != null)
            {
                GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, m_VertexElements);
            }

            if (SharedGraphicsDevice)
            {
                DoDraw(gameTime);
            }
            else
            {
                m_BasicEffect.World = m_WorldMatrix;

                m_BasicEffect.Begin();
                foreach (EffectPass pass in m_BasicEffect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    DoDraw(gameTime);
                    pass.End();
                }

                m_BasicEffect.End();
            }
        }
    }
}
