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
    /// <summary>
    /// A cube that contains Texture vertices
    /// </summary>
    public class CubeTexture : Cube
    {
        private const int k_TextureVerticesNum = 16;
        private const int k_ColorVerticesNum = 8;
        protected const int k_TextureTrianglesNum = 10;
        protected const int k_ColorTrianglesNum = 4;

        private VertexPositionTexture[] m_TextureVertices;
        private VertexPositionColor[] m_ColorVertices;

        /// <summary>
        /// Gets the number of triangles the pyramid has
        /// </summary>
        public override int     TriangleNum
        {
            get { return k_TextureTrianglesNum + k_ColorTrianglesNum; }
        }

        public CubeTexture(Game i_Game) : base(i_Game)
        {
        }

        /// <summary>
        /// Creates the cube sides as VertexPositionTexture
        /// </summary>
        protected override void     CreateSides()
        {
            int[] textureIndices;
            int[] colorIndices;
            VertexBuffer textureVBuffer, colorVBuffer;
            IndexBuffer textureIBuffer, colorIBuffer;

            createVertices();
            createIndices(out colorIndices, out textureIndices);
            InitBuffers(
                colorIndices, 
                textureIndices, 
                out colorVBuffer, 
                out colorIBuffer, 
                out textureVBuffer, 
                out textureIBuffer);

            Add(new TriangleHolder<VertexPositionTexture>(
                    this.Game,
                    VertexPositionTexture.VertexElements,
                    k_TextureTrianglesNum,
                    k_TextureVerticesNum,
                    true,
                    textureVBuffer,
                    textureIBuffer,
                    textureIndices));

            Add(new TriangleHolder<VertexPositionColor>(
                    this.Game,
                    VertexPositionColor.VertexElements,
                    k_ColorTrianglesNum,
                    k_ColorVerticesNum,
                    false,
                    colorVBuffer,
                    colorIBuffer,
                    colorIndices));

            // TODO: Remove remark

            // Creating the front side
/*            Add(new TriangleHolder<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements, 
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(0, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, .5f))));

            // Creating the back side
            Add(new TriangleHolder<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(0, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, 1))));

            // Creating the right side
            Add(new TriangleHolder<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(1, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(1, 1))));

            // Creating the left side
            Add(new TriangleHolder<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(1, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(1, .5f))));

            // Creating the top side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            false,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor)));

            // Creating the bottom side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            false,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor)));*/
        }

        /// <summary>
        /// Creates the component vertices
        /// </summary>
        private void createVertices()
        {
            m_TextureVertices = new VertexPositionTexture[k_TextureVerticesNum];
            m_ColorVertices = new VertexPositionColor[k_ColorVerticesNum];

            // Create front vertices
            m_TextureVertices[0] = new VertexPositionTexture(
                m_VerticesCoordinates[0],
                new Vector2(0, .5f));
            m_TextureVertices[1] = new VertexPositionTexture(
                m_VerticesCoordinates[1],
                new Vector2(0, 0));
            m_TextureVertices[2] = new VertexPositionTexture(
                m_VerticesCoordinates[2],
                new Vector2(.5f, 0));
            m_TextureVertices[3] = new VertexPositionTexture(
                m_VerticesCoordinates[3],
                new Vector2(.5f, .5f));

            // Creating the back side
            m_TextureVertices[4] = new VertexPositionTexture(
                m_VerticesCoordinates[4],
                new Vector2(0, 1));
            m_TextureVertices[5] = new VertexPositionTexture(
                m_VerticesCoordinates[5],
                new Vector2(0, .5f));
            m_TextureVertices[6] = new VertexPositionTexture(
                m_VerticesCoordinates[6],
                new Vector2(.5f, .5f));
            m_TextureVertices[7] = new VertexPositionTexture(
                m_VerticesCoordinates[7],
                new Vector2(.5f, 1));

            // Creating the right side
            m_TextureVertices[8] = new VertexPositionTexture(
                m_VerticesCoordinates[3],
                new Vector2(.5f, 1));
            m_TextureVertices[9] = new VertexPositionTexture(
                m_VerticesCoordinates[2],
                new Vector2(.5f, .5f));
            m_TextureVertices[10] = new VertexPositionTexture(
                m_VerticesCoordinates[5],
                new Vector2(1, .5f));
            m_TextureVertices[11] = new VertexPositionTexture(
                m_VerticesCoordinates[4],
                new Vector2(1, 1));

            // Creating the left side
            m_TextureVertices[12] = new VertexPositionTexture(
                m_VerticesCoordinates[7],
                new Vector2(.5f, .5f));
            m_TextureVertices[13] = new VertexPositionTexture(
                m_VerticesCoordinates[6],
                new Vector2(.5f, 0));
            m_TextureVertices[14] = new VertexPositionTexture(
                m_VerticesCoordinates[1],
                new Vector2(1, 0));
            m_TextureVertices[15] = new VertexPositionTexture(
                m_VerticesCoordinates[0],
                new Vector2(1, .5f));

            // TODO: Add top and bottom sides

            // Creating the top side
            m_ColorVertices[0] = new VertexPositionColor(
                m_VerticesCoordinates[1],
                r_UpDownColor);
            m_ColorVertices[1] = new VertexPositionColor(
                m_VerticesCoordinates[6],
                r_UpDownColor);
            m_ColorVertices[2] = new VertexPositionColor(
                m_VerticesCoordinates[5],
                r_UpDownColor);
            m_ColorVertices[3] = new VertexPositionColor(
                m_VerticesCoordinates[2],
                r_UpDownColor);

            // Creating the bottom side
            m_ColorVertices[4] = new VertexPositionColor(
                m_VerticesCoordinates[3],
                r_UpDownColor);
            m_ColorVertices[5] = new VertexPositionColor(
                m_VerticesCoordinates[4],
                r_UpDownColor);
            m_ColorVertices[6] = new VertexPositionColor(
                m_VerticesCoordinates[7],
                r_UpDownColor);
            m_ColorVertices[7] = new VertexPositionColor(
                m_VerticesCoordinates[0],
                r_UpDownColor);            

            // TODO: Remove

            //this.VerticesNum = k_TextureVerticesNum;
        }

        /// <summary>
        /// Creates the component indices that the index buffer uses
        /// </summary>
        private void    createIndices(out int[] o_ColorIndices, out int[] o_TextureIndices)
        {
            o_TextureIndices = new int[36];
            o_ColorIndices = new int[12];

            // Front face
            o_TextureIndices[0] = 0;
            o_TextureIndices[1] = 1;
            o_TextureIndices[2] = 2;
            o_TextureIndices[3] = 0;
            o_TextureIndices[4] = 2;
            o_TextureIndices[5] = 3;

            // Back face
            o_TextureIndices[6] = 4;
            o_TextureIndices[7] = 5;
            o_TextureIndices[8] = 6;
            o_TextureIndices[9] = 4;
            o_TextureIndices[10] = 6;
            o_TextureIndices[11] = 7;

            // Right face
            o_TextureIndices[12] = 8;
            o_TextureIndices[13] = 9;
            o_TextureIndices[14] = 10;
            o_TextureIndices[15] = 8;
            o_TextureIndices[16] = 10;
            o_TextureIndices[17] = 11;

            // Left face
            o_TextureIndices[18] = 12;
            o_TextureIndices[19] = 13;
            o_TextureIndices[20] = 14;
            o_TextureIndices[21] = 12;
            o_TextureIndices[22] = 14;
            o_TextureIndices[23] = 15;

            // Top face
            o_ColorIndices[0] = 0;
            o_ColorIndices[1] = 1;
            o_ColorIndices[2] = 2;
            o_ColorIndices[3] = 0;
            o_ColorIndices[4] = 2;
            o_ColorIndices[5] = 3;

            // Bottom face
            o_ColorIndices[6] = 4;
            o_ColorIndices[7] = 5;
            o_ColorIndices[8] = 6;
            o_ColorIndices[9] = 4;
            o_ColorIndices[10] = 6;
            o_ColorIndices[11] = 7;

            // TODO: Remove

            //BufferIndices = indices;
        }

        // TODO: Check if it's ok

        public override void    InitBuffers()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public void     InitBuffers(
            int[] i_ColorIndices, 
            int[] i_TextureIndices,
            out VertexBuffer o_ColorVBuffer,
            out IndexBuffer o_ColorIBuffer,
            out VertexBuffer o_TextureVBuffer,
            out IndexBuffer o_TextureIBuffer)
        {
            // Create the texture buffers
            o_TextureVBuffer = new VertexBuffer(
                this.GraphicsDevice,
                VertexPositionTexture.SizeInBytes * m_TextureVertices.Length,
                BufferUsage.WriteOnly);

            o_TextureVBuffer.SetData<VertexPositionTexture>(m_TextureVertices, 0, m_TextureVertices.Length);

            o_TextureIBuffer = new IndexBuffer(
                this.GraphicsDevice,
                typeof(int),
                i_TextureIndices.Length,
                BufferUsage.WriteOnly);

            o_TextureIBuffer.SetData<int>(i_TextureIndices);

            // Create the color buffers
            o_ColorVBuffer = new VertexBuffer(
                this.GraphicsDevice,
                VertexPositionColor.SizeInBytes * m_ColorVertices.Length,
                BufferUsage.WriteOnly);

            o_ColorVBuffer.SetData<VertexPositionColor>(m_ColorVertices, 0, m_ColorVertices.Length);

            o_ColorIBuffer = new IndexBuffer(
                this.GraphicsDevice,
                typeof(int),
                i_ColorIndices.Length,
                BufferUsage.WriteOnly);

            o_ColorIBuffer.SetData<int>(i_ColorIndices);
            
            // TODO: Remove
            /*this.ComponentVertexBuffer = vBuffer;
            this.ComponentIndexBuffer = iBuffer;*/
        }

        /// <summary>
        /// Loads the texture we want to draw on the cube sides
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            Texture = Game.Content.Load<Texture2D>(@"Sprites\Dreidel");
        }
    }
}
