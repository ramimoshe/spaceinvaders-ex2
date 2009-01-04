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

namespace DreidelGame.ObjectModel
{
    public class CubeTexture : CompositeGameComponent
    {
        private Vector3[] m_VerticesCoordinates = new Vector3[8];
        private const float k_ZFactorWidth = 6;
        private const float k_ZFactorCoordinate = 3f;     
        private readonly Color r_UpDownColor = Color.BurlyWood;

        private const int k_ZFactor = 8;

        private Texture2D m_Texture;

        public CubeTexture(Game i_Game) : base(i_Game)
        {
        }

        // TODO: Remove

        // TODO: Remove
        //public override void    Initialize()
       /* private void     createEffectData()
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
        }*/

        public override void Initialize()
        {
            // TODO: Move to a parent class
            m_VerticesCoordinates[0] = new Vector3(-3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[1] = new Vector3(-3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[2] = new Vector3(3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[3] = new Vector3(3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[4] = new Vector3(3, -3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[5] = new Vector3(3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[6] = new Vector3(-3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[7] = new Vector3(-3, -3, k_ZFactorCoordinate - k_ZFactorWidth);                                  

            

            // Creating the front side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(0, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, .5f))
                            ));

            // Creating the back side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(0, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, 1))
                            ));

            // Creating the right side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(1, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(1, 1))
                            ));

            // Creating the left side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(1, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(1, .5f))
                            ));

            // Creating the top side
            Add(new Side<VertexPositionColor>(
                            Game,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor)
                            ));

            // Creating the bottom side
            Add(new Side<VertexPositionColor>(
                            Game,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor)
                            ));

            base.Initialize();
        }

        // TODO: Remove

       /* private void createCubeVertices()
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
        }*/

        // TODO: Remove

      /*  private void createCubeCoordinates()
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
        }*/

        protected override void  AfterLoadContent()
        {           
            m_Texture = Game.Content.Load<Texture2D>(@"Sprites\Dreidel");

            BasicEffect effect = this.Effect;

            effect.Texture = m_Texture;
            effect.TextureEnabled = true;


            // TODO: Check if needed
            effect.VertexColorEnabled = false;

            this.Effect = effect;
        }

        // TODO: Remove

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
       /* public void    LoadGraphicContent(ContentManager i_ContentManager)
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
        }*/

        // TODO: Remove

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        //protected override void UnloadContent()
       /* public void     UnloadContent()
        {
            if (m_BasicEffect != null)
            {
                m_BasicEffect.Dispose();
                m_BasicEffect = null;
            }
        }*/

        // TODO: Move to a parent class

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
        public override void  Update(GameTime gameTime)
        { 	        
            m_Rotations.Y += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            BuildWorldMatrix();

            base.Update(gameTime);        
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// The component change the GraphicsDevice so that it can draw VertexPositionTexture.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void    Draw(GameTime gameTime)
        {
            // we are working with textured vertices
            GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                GraphicsDevice, VertexPositionTexture.VertexElements);      

            base.Draw(gameTime);
        }
    }
}
