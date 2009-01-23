using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    // TODO: Remove remark

    /// <summary>
    /// Represents the dreidel bottom component (a pyramid)
    /// </summary>
    public class Pyramid : BaseDrawableComponent//CompositeGameComponent
    {
        const int k_VerticesNum = 5;
        private const float k_MinXCoordinate = -3;
        private const float k_MaxXCoordinate = 3;
        private const float k_YCoordinate = -3;
        private const float k_ZFactorWidth = 6;
        private const float k_ZFactorCoordinate = 3f;
        private readonly Color r_SideColor = Color.BurlyWood;
        private readonly Vector3 r_PyramidHead = new Vector3(0, -6, 0);
        private const int k_TrianglesNum = 6;

        private VertexPositionColor[] m_Vertices;

        /// <summary>
        /// Gets the number of triangles the pyramid has
        /// </summary>
        public override int     TriangleNum
        {
            get { return k_TrianglesNum; }
        }

        // TODO: Remove

        //private Vector3[] m_VerticesCoordinates = new Vector3[5];

        /// <summary>
        /// CTOR. creates a new instance
        /// </summary>
        /// <param name="i_Game">The hosting game</param>
        public Pyramid(Game i_Game)
            : base(i_Game)
        {
        }

        // TODO: Remove the proc

        /// <summary>
        /// Initialize the pyramid coordinates and adds them to the component list
        /// </summary>
       /* public override void    Initialize()
        {
            initCoordinates();            

            // TODO: Remove the remark

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    2,
                    new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));

            base.Initialize();
        }*/

        /// <summary>
        /// Initialize the component vertices and buffers
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            createVertices();
            createIndices();
            InitBuffers();
        }

        /// <summary>
        /// Creates the component vertices
        /// </summary>
        private void    createVertices()
        {           
            m_Vertices = new VertexPositionColor[k_VerticesNum];

            m_Vertices[0] = new VertexPositionColor(new Vector3(
                k_MinXCoordinate,
                k_YCoordinate,
                k_ZFactorCoordinate), r_SideColor);
            
            m_Vertices[1] = new VertexPositionColor(new Vector3(
                k_MaxXCoordinate,
                k_YCoordinate,
                k_ZFactorCoordinate), r_SideColor);

            m_Vertices[2] = new VertexPositionColor(new Vector3(
                k_MaxXCoordinate,
                k_YCoordinate,
                k_ZFactorCoordinate - k_ZFactorWidth), r_SideColor);

            m_Vertices[3] = new VertexPositionColor(new Vector3(
                k_MinXCoordinate,
                k_YCoordinate,
                k_ZFactorCoordinate - k_ZFactorWidth), r_SideColor);

            m_Vertices[4] = new VertexPositionColor(r_PyramidHead, r_SideColor);

            this.VerticesNum = k_VerticesNum;
        }

        /// <summary>
        /// Creates the component indices that the index buffer uses
        /// </summary>
        private void    createIndices()
        {
            int[] indices = new int[18];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;
            indices[6] = 0;
            indices[7] = 1;
            indices[8] = 4;
            indices[9] = 1;
            indices[10] = 2;
            indices[11] = 4;
            indices[12] = 2;
            indices[13] = 3;
            indices[14] = 4;
            indices[15] = 3;
            indices[16] = 0;
            indices[17] = 4;

            BufferIndices = indices;

            // TODO: Remove

            /*Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    2,
                    new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));

            Add(new TriangleHolder<VertexPositionColor>(
                    Game,
                    VertexPositionColor.VertexElements,
                    1,
                    new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor),
                    new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)));*/
        }

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public override void    InitBuffers()
        {
            VertexBuffer vBuffer;
            IndexBuffer iBuffer;

            vBuffer = new VertexBuffer(
                this.GraphicsDevice,
                VertexPositionColor.SizeInBytes * m_Vertices.Length, 
                BufferUsage.WriteOnly);

            vBuffer.SetData<VertexPositionColor>(m_Vertices, 0, m_Vertices.Length);

            iBuffer = new IndexBuffer(
                this.GraphicsDevice,
                typeof(int),
                this.BufferIndices.Length,
                BufferUsage.WriteOnly);

            iBuffer.SetData<int>(this.BufferIndices);

            this.ComponentVertexBuffer = vBuffer;
            this.ComponentIndexBuffer = iBuffer;
        }

        // TODO: Remove

        /// <summary>
        /// Initialize the pyramid coordinates
        /// </summary>
        /*private void    initCoordinates()
        {
            m_VerticesCoordinates = new Vector3[5];

            m_VerticesCoordinates[0] = new Vector3(
                k_MinXCoordinate, 
                k_YCoordinate, 
                k_ZFactorCoordinate);
            m_VerticesCoordinates[1] = new Vector3(
                k_MaxXCoordinate, 
                k_YCoordinate, 
                k_ZFactorCoordinate);
            m_VerticesCoordinates[2] = new Vector3(
                k_MaxXCoordinate, 
                k_YCoordinate, 
                k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[3] = new Vector3(
                k_MinXCoordinate, 
                k_YCoordinate, 
                k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[4] = r_PyramidHead;
        }*/
    }
}
