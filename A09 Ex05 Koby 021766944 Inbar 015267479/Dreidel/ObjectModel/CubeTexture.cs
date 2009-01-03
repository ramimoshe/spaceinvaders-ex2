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
    public class CubeTexture// : DrawableGameComponent
    {
        private const int k_ZFactor = 8;
        private readonly Color r_UpDownColor = Color.BurlyWood;

        private Texture2D m_Texture;

        private Vector3 m_Position = new Vector3(0, 0, 0);
        private Vector3 m_Rotations = Vector3.Zero;
        private Vector3 m_Scales = Vector3.One;
        private Matrix m_WorldMatrix = Matrix.Identity;

        private const float k_ZFactorWidth = 7;
        private const float k_ZFactorCoordinate = 3.5f;        

        private BasicEffect m_BasicEffect;
        private VertexDeclaration m_VertDeclaration;
        private Matrix m_ProjectionFieldOfView;
        private Matrix m_PointOfView;
        private VertexPositionTexture[] m_FrontVerticesTexture;
        private VertexPositionTexture[] m_BackVerticesTexture;
        private VertexPositionTexture[] m_LeftSideVerticesTexture;
        private VertexPositionTexture[] m_RightSideVerticesTexture;
        private VertexPositionColor[] m_UpVertices;
        private VertexPositionColor[] m_DownVertices;
        private Vector3[] m_VerticesCoordinates;        

        private GraphicsDevice m_Device;

        public GraphicsDevice GraphicDevice
        {
            set { m_Device = value; }
        }

        public CubeTexture(GraphicsDevice i_Device)// : base(i_Game)
        {
            // TODO: Enable
            //i_Game.Components.Add(this);
            m_Device = i_Device;
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
                (float)m_Device.Viewport.Width / m_Device.Viewport.Height,
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
            m_FrontVerticesTexture = new VertexPositionTexture[4];
            m_BackVerticesTexture = new VertexPositionTexture[4];
            m_LeftSideVerticesTexture = new VertexPositionTexture[4];
            m_RightSideVerticesTexture = new VertexPositionTexture[4];
            m_UpVertices = new VertexPositionColor[4];
            m_DownVertices = new VertexPositionColor[4];

            m_FrontVerticesTexture[0] = new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(0, .5f));
            m_FrontVerticesTexture[1] = new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(0, 0));
            m_FrontVerticesTexture[2] = new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, 0));
            m_FrontVerticesTexture[3] = new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, .5f));

            m_BackVerticesTexture[0] = new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(0, 1));
            m_BackVerticesTexture[1] = new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(0, .5f));
            m_BackVerticesTexture[2] = new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, .5f));
            m_BackVerticesTexture[3] = new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, 1));

            m_RightSideVerticesTexture[0] = new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, 1));
            m_RightSideVerticesTexture[1] = new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, .5f));
            m_RightSideVerticesTexture[2] = new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(1, .5f));
            m_RightSideVerticesTexture[3] = new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(1, 1));

            m_LeftSideVerticesTexture[0] = new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, .5f));
            m_LeftSideVerticesTexture[1] = new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, 0));
            m_LeftSideVerticesTexture[2] = new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(1, 0));
            m_LeftSideVerticesTexture[3] = new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(1, .5f));

            m_UpVertices[0] = new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor);
            m_UpVertices[1] = new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor);
            m_UpVertices[2] = new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor);
            m_UpVertices[3] = new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor);

            m_DownVertices[0] = new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor);
            m_DownVertices[1] = new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor);
            m_DownVertices[2] = new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor);
            m_DownVertices[3] = new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor);
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
        public void    LoadGraphicContent(ContentManager i_ContentManager)
        {
            createEffectData();

            m_Texture = i_ContentManager.Load<Texture2D>(@"Sprites\Dreidel");

            // we are working with the out-of-the box shader that comes with XNA:
            m_BasicEffect = new BasicEffect(m_Device, null);
            m_BasicEffect.View = m_PointOfView;
            m_BasicEffect.Projection = m_ProjectionFieldOfView;

            // we pass the texture to the shader, so it can sample it to paint the triangle 
            m_BasicEffect.Texture = m_Texture;
            m_BasicEffect.TextureEnabled = true;            

            createCubeCoordinates();
            createCubeVertices();
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
            // we are working with textured vertices
            m_Device.VertexDeclaration = new VertexDeclaration(
                m_Device, VertexPositionTexture.VertexElements);

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
            m_Device.DrawUserPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleFan, m_FrontVerticesTexture, 0, 2);

            m_Device.DrawUserPrimitives<VertexPositionTexture>(
                                PrimitiveType.TriangleFan, m_RightSideVerticesTexture, 0, 2);

            m_Device.DrawUserPrimitives<VertexPositionTexture>(
                                PrimitiveType.TriangleFan, m_LeftSideVerticesTexture, 0, 2);

            m_Device.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_UpVertices, 0, 2);

            m_Device.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_DownVertices, 0, 2);

            m_Device.DrawUserPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleFan, m_BackVerticesTexture, 0, 2);   
        }

    }
}
