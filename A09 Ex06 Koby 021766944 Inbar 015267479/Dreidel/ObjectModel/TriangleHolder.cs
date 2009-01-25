using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// Holds an array of triangles and draw them
    /// </summary>
    /// <typeparam name="VectorPrimitiveType">Marks the Generic type that is sent to
    /// GraphicsDevice.DrawUserPrimitives when drawing the triangles
    /// </typeparam>
    public class TriangleHolder<VectorPrimitiveType> : BaseDrawableComponent
        where VectorPrimitiveType : struct
    {
        private int m_TrianglesNum;

        /// <summary>
        /// Gets the number of triangles the pyramid has
        /// </summary>
        public override int     TriangleNum
        {
            get { return m_TrianglesNum; }
        }

        public TriangleHolder(
            Game i_Game,
            VertexElement[] i_VertexElements,
            int i_PrimitivesNum,
            int i_VerticesNum,
            bool i_NeedTexture,
            VertexBuffer i_VBuffer,
            IndexBuffer i_IBuffer,
            short[] i_Indices)
                : base(i_Game, i_VertexElements, i_NeedTexture)
        {
            this.BufferIndices = i_Indices;
            this.ComponentIndexBuffer = i_IBuffer;
            this.ComponentVertexBuffer = i_VBuffer;
            this.VerticesNum = i_VerticesNum;
            m_TrianglesNum = i_PrimitivesNum;
        }          

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public override void    InitBuffers()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
