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

        private Vector3[] m_HLetterCoordinates;
        private VertexPositionColor[] m_HLetterCoordinates1;
        private VertexPositionColor[] m_HLetterCoordinates2;
        private VertexPositionColor[] m_HLetterCoordinates3;

        private Vector3[] m_PLetterCoordinates;
        private VertexPositionColor[] m_PLetterCoordinates1;
        private VertexPositionColor[] m_PLetterCoordinates2;
        private VertexPositionColor[] m_PLetterCoordinates3;
        private VertexPositionColor[] m_PLetterCoordinates4;
        private VertexPositionColor[] m_PLetterCoordinates5;

        private Vector3[] m_GLetterCoordinates;
        private VertexPositionColor[] m_GLetterCoordinates1;
        private VertexPositionColor[] m_GLetterCoordinates2;
        private VertexPositionColor[] m_GLetterCoordinates3;
        private VertexPositionColor[] m_GLetterCoordinates4;

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

            initHLetterCoordinate();
            initPLetterCoordinate();
            initGLetterCoordinate();
        }

        private void initHLetterCoordinate()
        {
            m_HLetterCoordinates = new Vector3[11];
            m_HLetterCoordinates1 = new VertexPositionColor[4];
            m_HLetterCoordinates2 = new VertexPositionColor[4];
            m_HLetterCoordinates3 = new VertexPositionColor[4];            

            m_HLetterCoordinates[0] = new Vector3(-2, -2.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[1] = new Vector3(-1, -2.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[2] = new Vector3(-2, 2.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[3] = new Vector3(-1, 1.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[4] = new Vector3(-1, 2.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[5] = new Vector3(2, 2.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[6] = new Vector3(2, 1.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[7] = new Vector3(1, 0, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[8] = new Vector3(2, 0, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[9] = new Vector3(1, -2.5f, -k_ZFactorCoordinate - .01f);
            m_HLetterCoordinates[10] = new Vector3(2, -2.5f, -k_ZFactorCoordinate - .01f);

            m_HLetterCoordinates1[0] = new VertexPositionColor(m_HLetterCoordinates[1], Color.Black);
            m_HLetterCoordinates1[1] = new VertexPositionColor(m_HLetterCoordinates[4], Color.Black);
            m_HLetterCoordinates1[2] = new VertexPositionColor(m_HLetterCoordinates[2], Color.Black);
            m_HLetterCoordinates1[3] = new VertexPositionColor(m_HLetterCoordinates[0], Color.Black);

            m_HLetterCoordinates2[0] = new VertexPositionColor(m_HLetterCoordinates[6], Color.Black);
            m_HLetterCoordinates2[1] = new VertexPositionColor(m_HLetterCoordinates[5], Color.Black);
            m_HLetterCoordinates2[2] = new VertexPositionColor(m_HLetterCoordinates[4], Color.Black);
            m_HLetterCoordinates2[3] = new VertexPositionColor(m_HLetterCoordinates[3], Color.Black);

            m_HLetterCoordinates3[0] = new VertexPositionColor(m_HLetterCoordinates[10], Color.Black);
            m_HLetterCoordinates3[1] = new VertexPositionColor(m_HLetterCoordinates[8], Color.Black);
            m_HLetterCoordinates3[2] = new VertexPositionColor(m_HLetterCoordinates[7], Color.Black);
            m_HLetterCoordinates3[3] = new VertexPositionColor(m_HLetterCoordinates[9], Color.Black);
        }

        private void    initGLetterCoordinate()
        {
            m_GLetterCoordinates = new Vector3[14];

            m_GLetterCoordinates1 = new VertexPositionColor[4];
            m_GLetterCoordinates2 = new VertexPositionColor[4];
            m_GLetterCoordinates3 = new VertexPositionColor[4];
            m_GLetterCoordinates4 = new VertexPositionColor[4];

            m_GLetterCoordinates[0] = new Vector3(-3 - .01f, -2.5f, -2);
            m_GLetterCoordinates[1] = new Vector3(-3 - .01f, 0, -2);
            m_GLetterCoordinates[2] = new Vector3(-3 - .01f, 0, -1);
            m_GLetterCoordinates[3] = new Vector3(-3 - .01f, -1, -1);
            m_GLetterCoordinates[4] = new Vector3(-3 - .01f, -2.5f, -1);
            m_GLetterCoordinates[5] = new Vector3(-3 - .01f, -2.5f, 1);
            m_GLetterCoordinates[6] = new Vector3(-3 - .01f, -1, 1);
            m_GLetterCoordinates[7] = new Vector3(-3 - .01f, 0, 1);
            m_GLetterCoordinates[8] = new Vector3(-3 - .01f, 1.5f, 1);
            m_GLetterCoordinates[9] = new Vector3(-3 - .01f, 1.5f, -2);
            m_GLetterCoordinates[10] = new Vector3(-3 - .01f, 2.5f, -2);
            m_GLetterCoordinates[11] = new Vector3(-3 - .01f, 2.5f, 1);
            m_GLetterCoordinates[12] = new Vector3(-3 - .01f, 2.5f, 2);
            m_GLetterCoordinates[13] = new Vector3(-3 - .01f, -2.5f, 2);

            m_GLetterCoordinates1[0] = new VertexPositionColor(m_GLetterCoordinates[0], Color.Black);
            m_GLetterCoordinates1[1] = new VertexPositionColor(m_GLetterCoordinates[1], Color.Black);
            m_GLetterCoordinates1[2] = new VertexPositionColor(m_GLetterCoordinates[2], Color.Black);
            m_GLetterCoordinates1[3] = new VertexPositionColor(m_GLetterCoordinates[4], Color.Black);

            m_GLetterCoordinates2[0] = new VertexPositionColor(m_GLetterCoordinates[3], Color.Black);
            m_GLetterCoordinates2[1] = new VertexPositionColor(m_GLetterCoordinates[2], Color.Black);
            m_GLetterCoordinates2[2] = new VertexPositionColor(m_GLetterCoordinates[7], Color.Black);
            m_GLetterCoordinates2[3] = new VertexPositionColor(m_GLetterCoordinates[6], Color.Black);

            m_GLetterCoordinates3[0] = new VertexPositionColor(m_GLetterCoordinates[5], Color.Black);
            m_GLetterCoordinates3[1] = new VertexPositionColor(m_GLetterCoordinates[11], Color.Black);
            m_GLetterCoordinates3[2] = new VertexPositionColor(m_GLetterCoordinates[12], Color.Black);
            m_GLetterCoordinates3[3] = new VertexPositionColor(m_GLetterCoordinates[13], Color.Black);

            m_GLetterCoordinates4[0] = new VertexPositionColor(m_GLetterCoordinates[9], Color.Black);
            m_GLetterCoordinates4[1] = new VertexPositionColor(m_GLetterCoordinates[10], Color.Black);
            m_GLetterCoordinates4[2] = new VertexPositionColor(m_GLetterCoordinates[11], Color.Black);
            m_GLetterCoordinates4[3] = new VertexPositionColor(m_GLetterCoordinates[8], Color.Black);
        }

        private void    initPLetterCoordinate()
        {
            m_PLetterCoordinates = new Vector3[17];

            m_PLetterCoordinates1 = new VertexPositionColor[4];
            m_PLetterCoordinates2 = new VertexPositionColor[4];
            m_PLetterCoordinates3 = new VertexPositionColor[4];
            m_PLetterCoordinates4 = new VertexPositionColor[4];
            m_PLetterCoordinates5 = new VertexPositionColor[4];

            m_PLetterCoordinates[0] = new Vector3(3 + .01f, -2.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[1] = new Vector3(3 + .01f, -1.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[2] = new Vector3(3 + .01f, -1.5f, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[3] = new Vector3(3 + .01f, -2.5f, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[4] = new Vector3(3 + .01f, -1.5f, -k_ZFactorCoordinate + 2);
            m_PLetterCoordinates[5] = new Vector3(3 + .01f, -1.5f, -k_ZFactorCoordinate + 1);
            m_PLetterCoordinates[6] = new Vector3(3 + .01f, -2.5f, -k_ZFactorCoordinate + 1);
            m_PLetterCoordinates[7] = new Vector3(3 + .01f, .5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[8] = new Vector3(3 + .01f, .5f, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[9] = new Vector3(3 + .01f, .5f, 0);
            m_PLetterCoordinates[10] = new Vector3(3 + .01f, 0, 0);
            m_PLetterCoordinates[11] = new Vector3(3 + .01f, 0, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[12] = new Vector3(3 + .01f, 2.5f, -k_ZFactorCoordinate + 1);
            m_PLetterCoordinates[13] = new Vector3(3 + .01f, 2.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[14] = new Vector3(3 + .01f, 1.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[15] = new Vector3(3 + .01f, 1.5f, -k_ZFactorCoordinate + 2);
            m_PLetterCoordinates[16] = new Vector3(3 + .01f, 2.5f, -k_ZFactorCoordinate + 2);
            

            m_PLetterCoordinates1[0] = new VertexPositionColor(m_PLetterCoordinates[0], Color.Black);
            m_PLetterCoordinates1[1] = new VertexPositionColor(m_PLetterCoordinates[1], Color.Black);
            m_PLetterCoordinates1[2] = new VertexPositionColor(m_PLetterCoordinates[5], Color.Black);
            m_PLetterCoordinates1[3] = new VertexPositionColor(m_PLetterCoordinates[6], Color.Black);

            m_PLetterCoordinates2[0] = new VertexPositionColor(m_PLetterCoordinates[1], Color.Black);
            m_PLetterCoordinates2[1] = new VertexPositionColor(m_PLetterCoordinates[7], Color.Black);
            m_PLetterCoordinates2[2] = new VertexPositionColor(m_PLetterCoordinates[8], Color.Black);
            m_PLetterCoordinates2[3] = new VertexPositionColor(m_PLetterCoordinates[2], Color.Black);

            m_PLetterCoordinates3[0] = new VertexPositionColor(m_PLetterCoordinates[11], Color.Black);
            m_PLetterCoordinates3[1] = new VertexPositionColor(m_PLetterCoordinates[8], Color.Black);
            m_PLetterCoordinates3[2] = new VertexPositionColor(m_PLetterCoordinates[9], Color.Black);
            m_PLetterCoordinates3[3] = new VertexPositionColor(m_PLetterCoordinates[10], Color.Black);

            m_PLetterCoordinates4[0] = new VertexPositionColor(m_PLetterCoordinates[4], Color.Black);
            m_PLetterCoordinates4[1] = new VertexPositionColor(m_PLetterCoordinates[16], Color.Black);
            m_PLetterCoordinates4[2] = new VertexPositionColor(m_PLetterCoordinates[12], Color.Black);
            m_PLetterCoordinates4[3] = new VertexPositionColor(m_PLetterCoordinates[5], Color.Black);

            m_PLetterCoordinates5[0] = new VertexPositionColor(m_PLetterCoordinates[14], Color.Black);
            m_PLetterCoordinates5[1] = new VertexPositionColor(m_PLetterCoordinates[13], Color.Black);
            m_PLetterCoordinates5[2] = new VertexPositionColor(m_PLetterCoordinates[16], Color.Black);
            m_PLetterCoordinates5[3] = new VertexPositionColor(m_PLetterCoordinates[15], Color.Black);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// The component change the GraphicsDevice so that it can draw VertexPositionColor.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void    Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            drawNLetter();
            drawHLetter();
            drawPLetter();
            drawGLetter();

        }

        private void    drawNLetter()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates1, 0, 3);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates2, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates3, 0, 1);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_NLetterCoordinates4, 0, 2);
        }

        private void    drawHLetter()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_HLetterCoordinates1, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_HLetterCoordinates2, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_HLetterCoordinates3, 0, 2);
        }

        private void    drawPLetter()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_PLetterCoordinates1, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_PLetterCoordinates2, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_PLetterCoordinates3, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_PLetterCoordinates4, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_PLetterCoordinates5, 0, 2);
        }

        private void    drawGLetter()
        {
            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_GLetterCoordinates1, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_GLetterCoordinates2, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_GLetterCoordinates3, 0, 2);

            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                  PrimitiveType.TriangleFan, m_GLetterCoordinates4, 0, 2);
        }
    }
}
