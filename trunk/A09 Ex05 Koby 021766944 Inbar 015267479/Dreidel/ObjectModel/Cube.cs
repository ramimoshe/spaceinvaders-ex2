using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;

namespace DreidelGame.ObjectModel
{
    public class Cube : DrawableGameComponent
    {
        private const int k_ZFactor = 8;

        private readonly Color r_FrontColor = Color.Yellow;
        private readonly Color r_BackColor = Color.Red;
        private readonly Color r_LeftColor = Color.Green;
        private readonly Color r_RightColor = Color.Blue;
        private readonly Color r_UpDownColor = Color.Purple;

        private BasicEffect m_BasicEffect;
        private VertexDeclaration m_VertDeclaration;
        private Matrix m_ProjectionFieldOfView;
        private Matrix m_PointOfView;
        private VertexPositionColor[] m_FrontVertices;
        private VertexPositionColor[] m_BackVertices;
        private VertexPositionColor[] m_LeftSideVertices;
        private VertexPositionColor[] m_RightSideVertices;
        private VertexPositionColor[] m_UpVertices;
        private VertexPositionColor[] m_DownVertices;
        private VertexPositionColor[] m_Vertices;
        private Vector3[] m_VerticesCoordinates;

        public Cube(Game i_Game) : base(i_Game)
        {
            // TODO: Enable
            //i_Game.Components.Add(this);
        }

        // TODO: Remove
       /* public override void Initialize()
        {
            float k_NearPlaneDistance = 0.5f;
            float k_FarPlaneDistance = 1000.0f;
            float k_ViewAngle = MathHelper.PiOver4;

            // we are storing the field-of-view data in a matrix:
            m_ProjectionFieldOfView = Matrix.CreatePerspectiveFieldOfView(
                k_ViewAngle,
                GraphicsDevice.Viewport.AspectRatio,
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
            base.Initialize();
        }*/

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            m_BasicEffect = new BasicEffect(this.GraphicsDevice, null);
            m_BasicEffect.View = m_PointOfView;
            m_BasicEffect.Projection = m_ProjectionFieldOfView;
            m_BasicEffect.VertexColorEnabled = true;

            // TODO: Remove

            // we are working with colored vertices
            /*this.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                this.GraphicsDevice, VertexPositionColor.VertexElements);

            // we did not use certain clockwise ordering in our vertex buffer
            // and we don't want antthing to be culled away..
            this.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;*/

            // lets create our 6 colored vertices:
            /*m_Vertices = new VertexPositionColor[6];
            m_Vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
            m_Vertices[1] = new VertexPositionColor(new Vector3(-11, 7, 0), Color.Red);
            m_Vertices[2] = new VertexPositionColor(new Vector3(0, 14, 0), Color.Blue);
            m_Vertices[3] = new VertexPositionColor(new Vector3(10, 10, -14), Color.Blue);
            m_Vertices[4] = new VertexPositionColor(new Vector3(16, 2, -2), Color.Green);
            m_Vertices[5] = new VertexPositionColor(new Vector3(15, -8, 2), Color.Green);*/

            m_FrontVertices = new VertexPositionColor[4];
            m_BackVertices = new VertexPositionColor[4];
            m_LeftSideVertices = new VertexPositionColor[4];
            m_RightSideVertices = new VertexPositionColor[4];
            m_UpVertices = new VertexPositionColor[4];
            m_DownVertices = new VertexPositionColor[4];
            m_VerticesCoordinates = new Vector3[8];

            m_VerticesCoordinates[0] = new Vector3(0, 0, 0);
            m_VerticesCoordinates[1] = new Vector3(0, 7, 0);
            m_VerticesCoordinates[2] = new Vector3(7, 7, 0);
            m_VerticesCoordinates[3] = new Vector3(7, 0, 0);
            m_VerticesCoordinates[4] = new Vector3(9, 0, k_ZFactor);
            m_VerticesCoordinates[5] = new Vector3(9, 7, k_ZFactor);
            m_VerticesCoordinates[6] = new Vector3(2, 7, k_ZFactor);
            m_VerticesCoordinates[7] = new Vector3(2, 0, k_ZFactor);

            m_Vertices = new VertexPositionColor[8];
            int count = 0;

            foreach (Vector3 vertix in m_VerticesCoordinates)
            {
                m_Vertices[count] = new VertexPositionColor(vertix, Color.Black);
                count++;
            }

            m_FrontVertices[0] = new VertexPositionColor(m_VerticesCoordinates[0], r_FrontColor);
            m_FrontVertices[1] = new VertexPositionColor(m_VerticesCoordinates[1], r_FrontColor);
            m_FrontVertices[2] = new VertexPositionColor(m_VerticesCoordinates[2], r_FrontColor);
            m_FrontVertices[3] = new VertexPositionColor(m_VerticesCoordinates[3], r_FrontColor);

            m_BackVertices[0] = new VertexPositionColor(m_VerticesCoordinates[4], r_BackColor);
            m_BackVertices[1] = new VertexPositionColor(m_VerticesCoordinates[5], r_BackColor);
            m_BackVertices[2] = new VertexPositionColor(m_VerticesCoordinates[6], r_BackColor);
            m_BackVertices[3] = new VertexPositionColor(m_VerticesCoordinates[7], r_BackColor);

            m_RightSideVertices[0] = new VertexPositionColor(m_VerticesCoordinates[3], r_RightColor);
            m_RightSideVertices[1] = new VertexPositionColor(m_VerticesCoordinates[2], r_RightColor);
            m_RightSideVertices[2] = new VertexPositionColor(m_VerticesCoordinates[5], r_RightColor);
            m_RightSideVertices[3] = new VertexPositionColor(m_VerticesCoordinates[4], r_RightColor);

            m_LeftSideVertices[0] = new VertexPositionColor(m_VerticesCoordinates[7], r_LeftColor);
            m_LeftSideVertices[1] = new VertexPositionColor(m_VerticesCoordinates[6], r_LeftColor);
            m_LeftSideVertices[2] = new VertexPositionColor(m_VerticesCoordinates[1], r_LeftColor);
            m_LeftSideVertices[3] = new VertexPositionColor(m_VerticesCoordinates[0], r_LeftColor);

            m_UpVertices[0] = new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor);
            m_UpVertices[1] = new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor);
            m_UpVertices[2] = new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor);
            m_UpVertices[3] = new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor);

            m_DownVertices[0] = new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor);
            m_DownVertices[1] = new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor);
            m_DownVertices[2] = new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor);
            m_DownVertices[3] = new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (m_BasicEffect != null)
            {
                m_BasicEffect.Dispose();
                m_BasicEffect = null;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            //graphics.GraphicsDevice.Clear(Color.White);

            m_BasicEffect.Begin();
            foreach (EffectPass pass in m_BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();
                this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleFan, m_FrontVertices, 0, 2);
                //pass.Begin();                
                /*this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleFan, m_LeftSideVertices, 0, 2);*/
                this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                    PrimitiveType.TriangleFan, m_RightSideVertices, 0, 2);

                this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                    PrimitiveType.TriangleFan, m_LeftSideVertices, 0, 2);

                this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                    PrimitiveType.TriangleFan, m_UpVertices, 0, 2);

                this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                    PrimitiveType.TriangleFan, m_DownVertices, 0, 2);

                this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleFan, m_BackVertices, 0, 2);

                this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                    PrimitiveType.PointList, m_Vertices, 0, 8);
                pass.End();
            }
            m_BasicEffect.End();

            base.Draw(gameTime);
        }

    }
}
