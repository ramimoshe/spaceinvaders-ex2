using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public class Side <VectorPrimitiveType> : BaseDrawableComponent
        where VectorPrimitiveType : struct
    {
        private VectorPrimitiveType[] m_VectorArray;
        private int m_PrimitivesNum;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_Game"></param>
        /// <param name="i_VectorsNum"></param>
        public Side(
            Game i_Game,
            VertexElement[] i_VertexElements, 
            int i_PrimitivesNum,
            params VectorPrimitiveType[] vectors)
            : base(i_Game, i_VertexElements)
        {
            m_VectorArray = vectors;
            m_PrimitivesNum = i_PrimitivesNum;
        }

        public override void DoDraw(GameTime gameTime)
        {
            GraphicsDevice.DrawUserPrimitives<VectorPrimitiveType>(
                            PrimitiveType.TriangleFan, m_VectorArray, 0, m_PrimitivesNum);
        }
    }
}
