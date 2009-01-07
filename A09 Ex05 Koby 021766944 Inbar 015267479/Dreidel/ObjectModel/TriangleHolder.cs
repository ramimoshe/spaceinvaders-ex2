using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public class TriangleHolder <VectorPrimitiveType> : BaseDrawableComponent
        where VectorPrimitiveType : struct
    {
        private VectorPrimitiveType[] m_VectorArray;
        private int m_PrimitivesNum;
        
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

        public override void DoDraw(GameTime gameTime)
        {
            GraphicsDevice.DrawUserPrimitives<VectorPrimitiveType>(
                            PrimitiveType.TriangleFan, m_VectorArray, 0, m_PrimitivesNum);
        }
    }
}
