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
        private const int k_VerticesNum = 16;
        protected const int k_TrianglesNum = 10;

        private VertexPositionTexture[] m_Vertices;        

        /// <summary>
        /// Gets the number of triangles the pyramid has
        /// </summary>
        public override int     TriangleNum
        {
            get { return k_TrianglesNum; }
        }

        public CubeTexture(Game i_Game) : base(i_Game)
        {
        }

        /// <summary>
        /// Creates the cube sides as VertexPositionTexture
        /// </summary>
        protected override void     CreateSides()
        {
            createVertices();
            createIndices();
            InitBuffers();
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
            m_Vertices = new VertexPositionTexture[k_VerticesNum];                                               

            // Create front vertices
            m_Vertices[0] = new VertexPositionTexture(
                m_VerticesCoordinates[0],
                new Vector2(0, .5f));
            m_Vertices[1] = new VertexPositionTexture(
                m_VerticesCoordinates[1],
                new Vector2(0, 0));
            m_Vertices[2] = new VertexPositionTexture(
                m_VerticesCoordinates[2],
                new Vector2(.5f, 0));
            m_Vertices[3] = new VertexPositionTexture(
                m_VerticesCoordinates[3],
                new Vector2(.5f, .5f));

            // Creating the back side
            m_Vertices[4] = new VertexPositionTexture(
                m_VerticesCoordinates[4],
                new Vector2(0, 1));
            m_Vertices[5] = new VertexPositionTexture(
                m_VerticesCoordinates[5],
                new Vector2(0, .5f));
            m_Vertices[6] = new VertexPositionTexture(
                m_VerticesCoordinates[6],
                new Vector2(.5f, .5f));
            m_Vertices[7] = new VertexPositionTexture(
                m_VerticesCoordinates[7],
                new Vector2(.5f, 1));

            // Creating the right side
            m_Vertices[8] = new VertexPositionTexture(
                m_VerticesCoordinates[3],
                new Vector2(.5f, 1));
            m_Vertices[9] = new VertexPositionTexture(
                m_VerticesCoordinates[2],
                new Vector2(.5f, .5f));
            m_Vertices[10] = new VertexPositionTexture(
                m_VerticesCoordinates[5],
                new Vector2(1, .5f));
            m_Vertices[11] = new VertexPositionTexture(
                m_VerticesCoordinates[4],
                new Vector2(1, 1));

            // Creating the left side
            m_Vertices[12] = new VertexPositionTexture(
                m_VerticesCoordinates[7],
                new Vector2(.5f, .5f));
            m_Vertices[13] = new VertexPositionTexture(
                m_VerticesCoordinates[6],
                new Vector2(.5f, 0));
            m_Vertices[14] = new VertexPositionTexture(
                m_VerticesCoordinates[1],
                new Vector2(1, 0));
            m_Vertices[15] = new VertexPositionTexture(
                m_VerticesCoordinates[0],
                new Vector2(1, .5f));

            // TODO: Add top and bottom sides

            // Creating the top side
        /*    m_Vertices[16] = new VertexPositionColor(
                m_VerticesCoordinates[1],
                r_UpDownColor);
            m_Vertices[17] = new VertexPositionColor(
                m_VerticesCoordinates[6],
                r_UpDownColor);
            m_Vertices[18] = new VertexPositionColor(
                m_VerticesCoordinates[5],
                r_UpDownColor);
            m_Vertices[19] = new VertexPositionColor(
                m_VerticesCoordinates[2],
                r_UpDownColor);

            // Creating the bottom side
            m_Vertices[20] = new VertexPositionColor(
                m_VerticesCoordinates[3],
                r_UpDownColor);
            m_Vertices[21] = new VertexPositionColor(
                m_VerticesCoordinates[4],
                r_UpDownColor);
            m_Vertices[22] = new VertexPositionColor(
                m_VerticesCoordinates[7],
                r_UpDownColor);
            m_Vertices[23] = new VertexPositionColor(
                m_VerticesCoordinates[0],
                r_UpDownColor);*/

            this.VerticesNum = k_VerticesNum;
        }

        /// <summary>
        /// Creates the component indices that the index buffer uses
        /// </summary>
        private void createIndices()
        {
            int[] indices = new int[36];

            // Front face
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            // Back face
            indices[6] = 4;
            indices[7] = 5;
            indices[8] = 6;
            indices[9] = 4;
            indices[10] = 6;
            indices[11] = 7;

            // Right face
            indices[12] = 8;
            indices[13] = 9;
            indices[14] = 10;
            indices[15] = 8;
            indices[16] = 10;
            indices[17] = 11;

            // Left face
            indices[18] = 12;
            indices[19] = 13;
            indices[20] = 14;
            indices[21] = 12;
            indices[22] = 14;
            indices[23] = 15;

            // Top face
    /*        indices[24] = 16;
            indices[25] = 17;
            indices[26] = 18;
            indices[27] = 16;
            indices[28] = 18;
            indices[29] = 19;

            // Bottom face
            indices[30] = 20;
            indices[31] = 21;
            indices[32] = 22;
            indices[33] = 20;
            indices[34] = 22;
            indices[35] = 23;*/

            BufferIndices = indices;
        }

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public override void InitBuffers()
        {
            VertexBuffer vBuffer;
            IndexBuffer iBuffer;

            vBuffer = new VertexBuffer(
                this.GraphicsDevice,
                VertexPositionTexture.SizeInBytes * m_Vertices.Length,
                BufferUsage.WriteOnly);

            vBuffer.SetData<VertexPositionTexture>(m_Vertices, 0, m_Vertices.Length);

            iBuffer = new IndexBuffer(
                this.GraphicsDevice,
                typeof(int),
                this.BufferIndices.Length,
                BufferUsage.WriteOnly);

            iBuffer.SetData<int>(this.BufferIndices);

            this.ComponentVertexBuffer = vBuffer;
            this.ComponentIndexBuffer = iBuffer;
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
