using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public class Cube : CompositeGameComponent
    {        
        private Vector3[] m_VerticesCoordinates = new Vector3[8];
        private const float k_ZFactorWidth = 7;
        private const float k_ZFactorCoordinate = 3.5f;
        private readonly Color r_BoxColor = Color.BurlyWood;
        private readonly Color r_FrontColor = Color.Yellow;
        private readonly Color r_BackColor = Color.Red;
        private readonly Color r_LeftColor = Color.Green;
        private readonly Color r_RightColor = Color.Blue;
        private readonly Color r_UpDownColor = Color.BurlyWood;

        public Cube(Game i_Game)
            : base (i_Game)
        {
        }

        public override void Initialize()
        {
            m_VerticesCoordinates[0] = new Vector3(-3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[1] = new Vector3(-3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[2] = new Vector3(3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[3] = new Vector3(3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[4] = new Vector3(3, -3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[5] = new Vector3(3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[6] = new Vector3(-3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[7] = new Vector3(-3, -3, k_ZFactorCoordinate - k_ZFactorWidth);

            Add(new Side<VertexPositionColor>(
                            Game, 
                            2, 
                            new VertexPositionColor(m_VerticesCoordinates[0], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[3], r_FrontColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game, 
                            2, 
                            new VertexPositionColor(m_VerticesCoordinates[4], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_BackColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game, 
                            2,                             
                            new VertexPositionColor(m_VerticesCoordinates[3], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_RightColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game, 
                            2,   
                            new VertexPositionColor(m_VerticesCoordinates[7], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_LeftColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor)
                            ));

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

        public override void Update(GameTime gameTime)
        {
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// The component change the GraphicsDevice so that it can draw VertexPositionColor.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // we are working with textured vertices
            GraphicsDevice.VertexDeclaration = new VertexDeclaration(
                GraphicsDevice, VertexPositionColor.VertexElements);

            base.Draw(gameTime);
        }
    }
}
