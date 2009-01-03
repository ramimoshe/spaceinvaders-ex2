using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public abstract class CompositeGameComponent : DrawableGameComponent
    {
        private List<DrawableGameComponent> m_Drawables = new List<DrawableGameComponent>();
        private const int k_ZFactor = 8;

        protected Vector3 m_Position = new Vector3(0, 0, 0);
        protected Vector3 m_Rotations = Vector3.Zero;
        protected Vector3 m_Scales = Vector3.One;
        protected Matrix m_WorldMatrix = Matrix.Identity;

        private BasicEffect m_BasicEffect;
        private Matrix m_ProjectionFieldOfView;
        private Matrix m_PointOfView;

        public CompositeGameComponent(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
        }

        protected override void  LoadGraphicsContent(bool loadAllContent)
        {
         	 base.LoadGraphicsContent(loadAllContent);

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
        }

        public void Add(DrawableGameComponent i_Drawable)
        {
            if (m_Drawables.IndexOf(i_Drawable) < 0)
            {
                m_Drawables.Add(i_Drawable);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (DrawableGameComponent drawable in m_Drawables)
            {
                drawable.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (DrawableGameComponent drawable in m_Drawables)
            {
                drawable.Update(gameTime);
            }
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            m_BasicEffect.World = m_WorldMatrix;

            m_BasicEffect.Begin();
            foreach (EffectPass pass in m_BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                foreach (DrawableGameComponent drawable in m_Drawables)
                {
                    drawable.Draw(gameTime);
                }

                pass.End();
            }

            m_BasicEffect.End();
        }
    }
}
