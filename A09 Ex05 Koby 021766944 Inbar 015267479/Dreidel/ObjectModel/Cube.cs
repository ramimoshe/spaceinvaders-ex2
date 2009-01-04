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
        private const float k_ZFactorWidth = 6;
        private const float k_ZFactorCoordinate = 3f;
        private readonly Color r_BoxColor = Color.BurlyWood;
        private readonly Color r_FrontColor = Color.Yellow;
        private readonly Color r_BackColor = Color.Red;
        private readonly Color r_LeftColor = Color.Green;
        private readonly Color r_RightColor = Color.Blue;
        private readonly Color r_UpDownColor = Color.BurlyWood;

        private Vector3[] m_NLetterCoordinates;
        private VertexPositionColor[] m_NLetterCoordinates1;
        private VertexPositionColor[] m_NLetterCoordinates2;
        private VertexPositionColor[] m_NLetterCoordinates3;
        private VertexPositionColor[] m_NLetterCoordinates4;

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

            // TODO: Change
            initLettersCoordinates();

            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2, 
                            new VertexPositionColor(m_VerticesCoordinates[0], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[3], r_FrontColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2, 
                            new VertexPositionColor(m_VerticesCoordinates[4], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_BackColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2, 
                            new VertexPositionColor(m_VerticesCoordinates[3], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_RightColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[7], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_LeftColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor)
                            ));

            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor)
                            ));

            base.Initialize();
        }

        private void initLettersCoordinates()
        {
            m_NLetterCoordinates = new Vector3[10];
            m_NLetterCoordinates1 = new VertexPositionColor[5];
            m_NLetterCoordinates2 = new VertexPositionColor[4];
            m_NLetterCoordinates3 = new VertexPositionColor[3];
            m_NLetterCoordinates4 = new VertexPositionColor[4];

            m_NLetterCoordinates[0] = new Vector3(-2, -2.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[1] = new Vector3(-2, -1.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[2] = new Vector3(1, -1.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[3] = new Vector3(2, -1.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[4] = new Vector3(2, -2.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[5] = new Vector3(1, 1.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[6] = new Vector3(1, 2.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[7] = new Vector3(2, 2.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[8] = new Vector3(0, 2.5f, k_ZFactorCoordinate + .01f);
            m_NLetterCoordinates[9] = new Vector3(0, 1.5f, k_ZFactorCoordinate + .01f);

            m_NLetterCoordinates1[0] = new VertexPositionColor(m_NLetterCoordinates[0], Color.Black);
            m_NLetterCoordinates1[1] = new VertexPositionColor(m_NLetterCoordinates[1], Color.Black);
            m_NLetterCoordinates1[2] = new VertexPositionColor(m_NLetterCoordinates[2], Color.Black);
            m_NLetterCoordinates1[3] = new VertexPositionColor(m_NLetterCoordinates[3], Color.Black);
            m_NLetterCoordinates1[4] = new VertexPositionColor(m_NLetterCoordinates[4], Color.Black);

            m_NLetterCoordinates2[0] = new VertexPositionColor(m_NLetterCoordinates[2], Color.Black);
            m_NLetterCoordinates2[1] = new VertexPositionColor(m_NLetterCoordinates[5], Color.Black);
            m_NLetterCoordinates2[2] = new VertexPositionColor(m_NLetterCoordinates[7], Color.Black);
            m_NLetterCoordinates2[3] = new VertexPositionColor(m_NLetterCoordinates[4], Color.Black);

            m_NLetterCoordinates3[0] = new VertexPositionColor(m_NLetterCoordinates[5], Color.Black);
            m_NLetterCoordinates3[1] = new VertexPositionColor(m_NLetterCoordinates[6], Color.Black);
            m_NLetterCoordinates3[2] = new VertexPositionColor(m_NLetterCoordinates[7], Color.Black);

            m_NLetterCoordinates4[0] = new VertexPositionColor(m_NLetterCoordinates[9], Color.Black);
            m_NLetterCoordinates4[1] = new VertexPositionColor(m_NLetterCoordinates[8], Color.Black);
            m_NLetterCoordinates4[2] = new VertexPositionColor(m_NLetterCoordinates[6], Color.Black);
            m_NLetterCoordinates4[3] = new VertexPositionColor(m_NLetterCoordinates[5], Color.Black);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// The component change the GraphicsDevice so that it can draw VertexPositionColor.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates1, 0, 3);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates2, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates3, 0, 1);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates4, 0, 2);
        }
    }
}
