using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{

    // TODO: Remove the class

    /// <summary>
    /// Holds an array of triangles and draw them
    /// </summary>
    /// <typeparam name="VectorPrimitiveType">Marks the Generic type that is sent to
    /// GraphicsDevice.DrawUserPrimitives when drawing the triangles
    /// </typeparam>
    public class TriangleHolder<VectorPrimitiveType> : BaseDrawableComponent
        where VectorPrimitiveType : struct
    {
        private VectorPrimitiveType[] m_VectorArray;
        private int m_PrimitivesNum;
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
            int[] i_Indices) : base(i_Game, i_VertexElements, i_NeedTexture)
        {
            this.BufferIndices = i_Indices;
            this.ComponentIndexBuffer = i_IBuffer;
            this.ComponentVertexBuffer = i_VBuffer;
            this.VerticesNum = i_VerticesNum;
            m_TrianglesNum = i_PrimitivesNum;
        }

        // TODO: Remove the CTORS

        /// <summary>
        /// CTOR. Creates a new instance
        /// </summary>
        /// <param name="i_Game">hosting game</param>
        /// <param name="i_VertexElements">The vertex elements defining the component.
        /// This is in order to load VertexDeclaration.
        /// Valid values are: null, VertexPositionTexture.VertexElements,
        /// VertexPositionColor</param>
        /// <param name="i_Vectors">List of vectors</param>
        /// <param name="i_PrimitivesNum">The number of primitives to draw</param>
        public TriangleHolder(
            Game i_Game,
            VertexElement[] i_VertexElements,
            int i_PrimitivesNum,
            params VectorPrimitiveType[] i_Vectors)
            : base(i_Game, i_VertexElements)
        {
            m_VectorArray = i_Vectors;
            m_PrimitivesNum = i_PrimitivesNum;            
        }

        /// <summary>
        /// CTOR. Creates a new instance
        /// </summary>
        /// <param name="i_Game">hosting game</param>
        /// <param name="i_VertexElements">The vertex elements defining the component.
        /// This is in order to load VertexDeclaration.
        /// Valid values are: null, VertexPositionTexture.VertexElements,
        /// VertexPositionColor</param>
        /// <param name="i_Vectors">List of vectors</param>
        /// <param name="i_NeedTexture">Specifies if holder uses a texture</param>
        /// <param name="i_PrimitivesNum">The number of primitives to draw</param>
        public TriangleHolder(
            Game i_Game,
            VertexElement[] i_VertexElements, 
            int i_PrimitivesNum,
            bool i_NeedTexture,
            params VectorPrimitiveType[] i_Vectors)           
            : base(i_Game, i_VertexElements, i_NeedTexture)
        {
            m_VectorArray = i_Vectors;
            m_PrimitivesNum = i_PrimitivesNum;
        }

        /// <summary>
        /// Draws all the triangles that the component holds.
        /// 
        /// Notice that the triangles are drawn using the GraphicsDevice DrawUserPrimitives
        /// method while drawing them as a TriangleFan
        /// </summary>
        /// <param name="gameTime"></param>
        /*public override void    DoDraw(GameTime gameTime)
        {
            GraphicsDevice.DrawUserPrimitives<VectorPrimitiveType>(
                            PrimitiveType.TriangleFan, m_VectorArray, 0, m_PrimitivesNum);
        }*/       

        /// <summary>
        /// Initialize the VertexBuffer and IndexBuffer components.
        /// </summary>
        public override void InitBuffers()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
