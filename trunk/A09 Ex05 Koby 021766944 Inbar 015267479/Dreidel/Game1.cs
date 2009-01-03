using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using DreidelGame.ObjectModel;
using DreidelGame.Services;

namespace DreidelGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        InputManager m_InputManager;

        // TODO: Remove

        bool m_LeftPressed = true;
        bool m_RightPressed = false;
        bool m_DownPressed = false;
        bool m_UpPressed = false;
        
        // TODO: Remove
        //SpriteBatch spriteBatch;

        // TODO: Remove

        private const float k_ZFactorWidth = 7;
        private const float k_ZFactorCoordinate = 3.5f;

        private readonly Color r_BoxColor = Color.BurlyWood;
        private readonly Color r_FrontColor = Color.Yellow;
        private readonly Color r_BackColor = Color.Red;
        private readonly Color r_LeftColor = Color.Green;
        private readonly Color r_RightColor = Color.Blue;
        private readonly Color r_UpDownColor = Color.BurlyWood;

        private Texture2D m_Texture;
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

        private VertexPositionTexture[] m_FrontVerticesTexture;
        private VertexPositionTexture[] m_BackVerticesTexture;
        private VertexPositionTexture[] m_LeftSideVerticesTexture;
        private VertexPositionTexture[] m_RightSideVerticesTexture;
        private VertexPositionTexture[] m_UpVerticesTexture;
        private VertexPositionTexture[] m_DownVerticesTexture;

        private Vector3 m_PiramidHead;
        private VertexPositionColor[] m_PiramidVertices;

        // Box vertices
        private VertexPositionColor[] m_BoxFrontVertices;
        private VertexPositionColor[] m_BoxBackVertices;
        private VertexPositionColor[] m_BoxLeftSideVertices;
        private VertexPositionColor[] m_BoxRightSideVertices;
        private VertexPositionColor[] m_BoxUpVertices;
        private VertexPositionColor[] m_BoxDownVertices;
        private VertexPositionColor[] m_BoxVertices;
        private Vector3[] m_BoxVerticesCoordinates;

        private Vector3 m_Position = new Vector3(0, 0, 0);
        private Vector3 m_Rotations = Vector3.Zero;
        private Vector3 m_Scales = Vector3.One;
        private Matrix m_WorldMatrix = Matrix.Identity;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_InputManager = new InputManager(this);

            this.Components.Add(m_InputManager);

            //Cube c = new Cube(this);
        }

        // TODO: Remove

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // we are working with the out-of-the box shader that comes with XNA:
            /*m_BasicEffect = new BasicEffect(this.GraphicsDevice, null);
            m_BasicEffect.View = m_PointOfView;
            m_BasicEffect.Projection = m_ProjectionFieldOfView;
            m_BasicEffect.VertexColorEnabled = true;

            // we are working with colored vertices
            this.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                this.GraphicsDevice, VertexPositionColor.VertexElements);*/

            m_Texture = Content.Load<Texture2D>(@"Sprites\Dreidel");

            // we are working with the out-of-the box shader that comes with XNA:
            m_BasicEffect = new BasicEffect(this.GraphicsDevice, null);
            m_BasicEffect.View = m_PointOfView;
            m_BasicEffect.Projection = m_ProjectionFieldOfView;

            // we pass the texture to the shader, so it can sample it to paint the triangle 
            m_BasicEffect.Texture = m_Texture;
            m_BasicEffect.TextureEnabled = true;

            this.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Clamp;
            this.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Clamp;

            // we are working with textured vertices
            this.GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                this.GraphicsDevice, VertexPositionTexture.VertexElements);

            // we did not use certain clockwise ordering in our vertex buffer
            // and we don't want antthing to be culled away..
            this.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
            
            createCubeVerticesTexture();
            createBoxVertices();
            createPiramid();
        }

        private void    createCubeVerticesTexture()
        {           
            createCubeCoordinates();

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

            /*m_BackVerticesTexture[0] = new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(0, 1));
            m_BackVerticesTexture[1] = new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(0, 0));
            m_BackVerticesTexture[2] = new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(1, 0));
            m_BackVerticesTexture[3] = new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(1, 1));

            m_RightSideVerticesTexture[0] = new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(0, 1));
            m_RightSideVerticesTexture[1] = new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(0, 0));
            m_RightSideVerticesTexture[2] = new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(1, 0));
            m_RightSideVerticesTexture[3] = new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(1, 1));

            m_LeftSideVerticesTexture[0] = new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(0, 1));
            m_LeftSideVerticesTexture[1] = new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(0, 0));
            m_LeftSideVerticesTexture[2] = new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(1, 0));
            m_LeftSideVerticesTexture[3] = new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(1, 1));*/

            m_UpVertices[0] = new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor);
            m_UpVertices[1] = new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor);
            m_UpVertices[2] = new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor);
            m_UpVertices[3] = new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor);

            m_DownVertices[0] = new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor);
            m_DownVertices[1] = new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor);
            m_DownVertices[2] = new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor);
            m_DownVertices[3] = new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor);
        }

        private void    createCubeCoordinates()
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

        private void    createBoxVertices()
        {
            m_BoxFrontVertices = new VertexPositionColor[4];
            m_BoxBackVertices = new VertexPositionColor[4];
            m_BoxLeftSideVertices = new VertexPositionColor[4];
            m_BoxRightSideVertices = new VertexPositionColor[4];
            m_BoxUpVertices = new VertexPositionColor[4];
            m_BoxDownVertices = new VertexPositionColor[4];
            m_BoxVerticesCoordinates = new Vector3[8];

            // TODO: Change to constants

            m_BoxVerticesCoordinates[0] = new Vector3(-.5f, 3, 0);
            m_BoxVerticesCoordinates[1] = new Vector3(-.5f, 7, 0);
            m_BoxVerticesCoordinates[2] = new Vector3(.5f, 7, 0);
            m_BoxVerticesCoordinates[3] = new Vector3(.5f, 3, 0);
            m_BoxVerticesCoordinates[4] = new Vector3(.5f, 3, 1);
            m_BoxVerticesCoordinates[5] = new Vector3(.5f, 7, 1);
            m_BoxVerticesCoordinates[6] = new Vector3(-.5f, 7, 1);
            m_BoxVerticesCoordinates[7] = new Vector3(-.5f, 3, 1);

            m_BoxFrontVertices[0] = new VertexPositionColor(m_BoxVerticesCoordinates[0], r_BoxColor);
            m_BoxFrontVertices[1] = new VertexPositionColor(m_BoxVerticesCoordinates[1], r_BoxColor);
            m_BoxFrontVertices[2] = new VertexPositionColor(m_BoxVerticesCoordinates[2], r_BoxColor);
            m_BoxFrontVertices[3] = new VertexPositionColor(m_BoxVerticesCoordinates[3], r_BoxColor);

            m_BoxBackVertices[0] = new VertexPositionColor(m_BoxVerticesCoordinates[4], r_BoxColor);
            m_BoxBackVertices[1] = new VertexPositionColor(m_BoxVerticesCoordinates[5], r_BoxColor);
            m_BoxBackVertices[2] = new VertexPositionColor(m_BoxVerticesCoordinates[6], r_BoxColor);
            m_BoxBackVertices[3] = new VertexPositionColor(m_BoxVerticesCoordinates[7], r_BoxColor);

            m_BoxRightSideVertices[0] = new VertexPositionColor(m_BoxVerticesCoordinates[3], r_BoxColor);
            m_BoxRightSideVertices[1] = new VertexPositionColor(m_BoxVerticesCoordinates[2], r_BoxColor);
            m_BoxRightSideVertices[2] = new VertexPositionColor(m_BoxVerticesCoordinates[5], r_BoxColor);
            m_BoxRightSideVertices[3] = new VertexPositionColor(m_BoxVerticesCoordinates[4], r_BoxColor);

            m_BoxLeftSideVertices[0] = new VertexPositionColor(m_BoxVerticesCoordinates[7], r_BoxColor);
            m_BoxLeftSideVertices[1] = new VertexPositionColor(m_BoxVerticesCoordinates[6], r_BoxColor);
            m_BoxLeftSideVertices[2] = new VertexPositionColor(m_BoxVerticesCoordinates[1], r_BoxColor);
            m_BoxLeftSideVertices[3] = new VertexPositionColor(m_BoxVerticesCoordinates[0], r_BoxColor);

            m_BoxUpVertices[0] = new VertexPositionColor(m_BoxVerticesCoordinates[1], r_BoxColor);
            m_BoxUpVertices[1] = new VertexPositionColor(m_BoxVerticesCoordinates[6], r_BoxColor);
            m_BoxUpVertices[2] = new VertexPositionColor(m_BoxVerticesCoordinates[5], r_BoxColor);
            m_BoxUpVertices[3] = new VertexPositionColor(m_BoxVerticesCoordinates[2], r_BoxColor);

            m_BoxDownVertices[0] = new VertexPositionColor(m_BoxVerticesCoordinates[3], r_BoxColor);
            m_BoxDownVertices[1] = new VertexPositionColor(m_BoxVerticesCoordinates[4], r_BoxColor);
            m_BoxDownVertices[2] = new VertexPositionColor(m_BoxVerticesCoordinates[7], r_BoxColor);
            m_BoxDownVertices[3] = new VertexPositionColor(m_BoxVerticesCoordinates[0], r_BoxColor);
        }

        private void    createCubeVertices()
        {
            m_FrontVertices = new VertexPositionColor[4];
            m_BackVertices = new VertexPositionColor[4];
            m_LeftSideVertices = new VertexPositionColor[4];
            m_RightSideVertices = new VertexPositionColor[4];
            m_UpVertices = new VertexPositionColor[4];
            m_DownVertices = new VertexPositionColor[4];            

            /*m_VerticesCoordinates[0] = new Vector3(0, 0, 0);
            m_VerticesCoordinates[1] = new Vector3(0, 7, 0);
            m_VerticesCoordinates[2] = new Vector3(7, 7, 0);
            m_VerticesCoordinates[3] = new Vector3(7, 0, 0);
            m_VerticesCoordinates[4] = new Vector3(7, 0, k_ZFactor);
            m_VerticesCoordinates[5] = new Vector3(7, 7, k_ZFactor);
            m_VerticesCoordinates[6] = new Vector3(0, 7, k_ZFactor);
            m_VerticesCoordinates[7] = new Vector3(0, 0, k_ZFactor);*/

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

          /*  m_UpVertices[0] = new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor);
            m_UpVertices[1] = new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor);
            m_UpVertices[2] = new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor);
            m_UpVertices[3] = new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor);

            m_DownVertices[0] = new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor);
            m_DownVertices[1] = new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor);
            m_DownVertices[2] = new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor);
            m_DownVertices[3] = new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor);*/
        }

        private void    createPiramid()
        {
            // TODO: Change to constants

            m_PiramidHead = new Vector3(0, -6, 0);
            m_PiramidVertices = new VertexPositionColor[12];

            m_PiramidVertices[0] = new VertexPositionColor(m_VerticesCoordinates[0], r_BoxColor);
            m_PiramidVertices[1] = new VertexPositionColor(m_VerticesCoordinates[3], r_BoxColor);
            m_PiramidVertices[2] = new VertexPositionColor(m_PiramidHead, r_BoxColor);

            m_PiramidVertices[3] = new VertexPositionColor(m_VerticesCoordinates[3], r_BoxColor);
            m_PiramidVertices[4] = new VertexPositionColor(m_VerticesCoordinates[4], r_BoxColor);
            m_PiramidVertices[5] = new VertexPositionColor(m_PiramidHead, r_BoxColor);

            m_PiramidVertices[6] = new VertexPositionColor(m_VerticesCoordinates[4], r_BoxColor);
            m_PiramidVertices[7] = new VertexPositionColor(m_VerticesCoordinates[7], r_BoxColor);
            m_PiramidVertices[8] = new VertexPositionColor(m_PiramidHead, r_BoxColor);

            m_PiramidVertices[9] = new VertexPositionColor(m_VerticesCoordinates[7], r_BoxColor);
            m_PiramidVertices[10] = new VertexPositionColor(m_VerticesCoordinates[0], r_BoxColor);
            m_PiramidVertices[11] = new VertexPositionColor(m_PiramidHead, r_BoxColor);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
     /*   protected override void UnloadContent()
        {
            if (m_BasicEffect != null)
            {
                m_BasicEffect.Dispose();
                m_BasicEffect = null;
            }
        }*/

        // TODO: Remove

        private void    checkMoveDirection()
        {
            if (m_InputManager.KeyPressed(Keys.Right))
            {
                m_RightPressed = true;
                m_LeftPressed = false;
                m_UpPressed = false;
                m_DownPressed = false;
            }
            else if (m_InputManager.KeyPressed(Keys.Left))
            {
                m_RightPressed = false;
                m_LeftPressed = true;
                m_UpPressed = false;
                m_DownPressed = false;
            }
            else if (m_InputManager.KeyPressed(Keys.Up))
            {
                m_RightPressed = false;
                m_LeftPressed = false;
                m_UpPressed = true;
                m_DownPressed = false;
            }
            else if (m_InputManager.KeyPressed(Keys.Down))
            {
                m_RightPressed = false;
                m_LeftPressed = false;
                m_UpPressed = false;
                m_DownPressed = true;
            }            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            checkMoveDirection();

            // TODO: Remove
//            m_Position.X += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_LeftPressed)
            {
                m_Rotations.Y += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (m_RightPressed)
            {
                m_Rotations.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (m_UpPressed)
            {
                m_Rotations.X -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (m_DownPressed)
            {
                m_Rotations.X += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            BuildWorldMatrix();

            base.Update(gameTime);
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

        // TODO: Remove

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

            m_BasicEffect.World = m_WorldMatrix;

            m_BasicEffect.Begin();
            foreach (EffectPass pass in m_BasicEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                // Draws all the dreidel elements
                //drawCube();
                drawCubeTexture();
                //drawBox();
                //drawPiramid();
                
                pass.End();
            }
            m_BasicEffect.End();

            base.Draw(gameTime);
        }

        private void    drawCubeTexture()
        {
            this.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleFan, m_FrontVerticesTexture, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                                PrimitiveType.TriangleFan, m_RightSideVerticesTexture, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                                PrimitiveType.TriangleFan, m_LeftSideVerticesTexture, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_UpVertices, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_DownVertices, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleFan, m_BackVerticesTexture, 0, 2);            
        }

        private void    drawCube()
        {
            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleFan, m_FrontVertices, 0, 2);

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
        }

        private void    drawBox()
        {
            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleFan, m_BoxFrontVertices, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_BoxRightSideVertices, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_BoxLeftSideVertices, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_BoxUpVertices, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                                PrimitiveType.TriangleFan, m_BoxDownVertices, 0, 2);

            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.TriangleFan, m_BoxBackVertices, 0, 2);
        }        

        private void    drawPiramid()
        {
            this.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                PrimitiveType.TriangleList, m_PiramidVertices, 0, 4);
        }
    }
}
