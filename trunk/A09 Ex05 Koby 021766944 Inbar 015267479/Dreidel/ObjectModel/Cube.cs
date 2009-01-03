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
    public class Cube// : DrawableGameComponent
    {
        private const int k_ZFactor = 8;

        private Vector3 m_Position = new Vector3(0, 0, 0);
        private Vector3 m_Rotations = Vector3.Zero;
        private Vector3 m_Scales = Vector3.One;
        private Matrix m_WorldMatrix = Matrix.Identity;

        private const float k_ZFactorWidth = 7;
        private const float k_ZFactorCoordinate = 3.5f;

        private readonly Color r_BoxColor = Color.BurlyWood;
        private readonly Color r_FrontColor = Color.Yellow;
        private readonly Color r_BackColor = Color.Red;
        private readonly Color r_LeftColor = Color.Green;
        private readonly Color r_RightColor = Color.Blue;
        private readonly Color r_UpDownColor = Color.BurlyWood;

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

        private GraphicsDevice device;

        public GraphicsDevice GraphicDevice
        {
            set { device = value; }
        }

        public Cube(GraphicsDevice i_Device)// : base(i_Game)
        {
            // TODO: Enable
            //i_Game.Components.Add(this);
            device = i_Device;
        }

        // TODO: Remove
        //public override void    Initialize()
        private void     createEffectData()
        {
            float k_NearPlaneDistance = 0.5f;
            float k_FarPlaneDistance = 1000.0f;
            float k_ViewAngle = MathHelper.PiOver4;

            // we are storing the field-of-view data in a matrix:
            m_ProjectionFieldOfView = Matrix.CreatePerspectiveFieldOfView(
                k_ViewAngle,
                (float)device.Viewport.Width / device.Viewport.Height,
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
        }

        private void createCubeVertices()
        {
            m_FrontVertices = new VertexPositionColor[4];
            m_BackVertices = new VertexPositionColor[4];
            m_LeftSideVertices = new VertexPositionColor[4];
            m_RightSideVertices = new VertexPositionColor[4];
            m_UpVertices = new VertexPositionColor[4];
            m_DownVertices = new VertexPositionColor[4];

            createCubeCoordinates();


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
        }

        private void createCubeCoordinates()
        {
            m_VerticesCoordinates = new Vector3[8];

            m_VerticesCoordinates[0] = new Vector3(-3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[1] = new Vector3(-3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[2] = new Vector3(3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[3] = new Vector3(3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[4] = new Vector3(3, -3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[5] = new Vector3(3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[6] = new Vector3(-3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[7] = new Vector3(-3, -3, k_ZFactorCoordinate - k_ZFactorWidth);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void    LoadGraphicContent()
        {
            createEffectData();

            m_BasicEffect = new BasicEffect(device, null);
            m_BasicEffect.View = m_PointOfView;
            m_BasicEffect.Projection = m_ProjectionFieldOfView;
            m_BasicEffect.VertexColorEnabled = true;

            // we are working with colored vertices
    /*        this.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                this.GraphicsDevice, VertexPositionColor.VertexElements);

            // we did not use certain clockwise ordering in our vertex buffer
            // and we don't want antthing to be culled away..
            this.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace; */

            createCubeCoordinates();
            createCubeVertices();

            // TODO: Remove

           /* m_FrontVertices = new VertexPositionColor[4];
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
            m_DownVertices[3] = new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor);*/

            // TODO: Remove
//            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        //protected override void UnloadContent()
        public void     UnloadContent()
        {
            if (m_BasicEffect != null)
            {
                m_BasicEffect.Dispose();
                m_BasicEffect = null;
            }
        }

        private void BuildWorldMatrix()
        {
            m_WorldMatrix =
                /*I*/ Matrix.Identity *
                /*S*/ Matrix.CreateScale(m_Scales) *
                /*R*/ Matrix.CreateRotationX(m_Rotations.X) *
                        Matrix.CreateRotationY(m_Rotations.Y) *
                        Matrix.CreateRotationZ(m_Rotations.Z) *
                /* No Orbit */
                /*T*/ Matrix.CreateTranslation(m_Position);
        }   

        //public override void    Update(GameTime gameTime)
        public void    Update(GameTime gameTime)
        {                 
            m_Rotations.Y += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            BuildWorldMatrix();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //public override void    Draw(GameTime gameTime)
        public void    Draw(GameTime gameTime)
        {
            device.VertexDeclaration = new VertexDeclaration(device,
                VertexPositionColor.VertexElements);
            m_BasicEffect.World = m_WorldMatrix;

            //graphics.GraphicsDevice.Clear(Color.White);            

            m_BasicEffect.Begin();
            foreach (EffectPass pass in m_BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                drawCube();

                pass.End();
            }
            m_BasicEffect.End();

            // TODO: Remove
            //base.Draw(gameTime);
        }

        private void    drawCube()
        {
            device.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleFan, m_FrontVertices, 0, 2);

            device.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_RightSideVertices, 0, 2);

            device.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_LeftSideVertices, 0, 2);

            device.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_UpVertices, 0, 2);

            device.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_DownVertices, 0, 2);

            device.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.TriangleFan, m_BackVertices, 0, 2);

            /*device.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.PointList, m_Vertices, 0, 8);*/
        }

    }
}
