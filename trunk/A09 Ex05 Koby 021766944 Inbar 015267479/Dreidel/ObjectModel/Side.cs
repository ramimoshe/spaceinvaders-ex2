using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public class Side <VectorPrimitiveType> : DrawableGameComponent
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
            int i_PrimitivesNum,
            params VectorPrimitiveType[] vectors)
            : base(i_Game)
        {
            m_VectorArray = vectors;
            m_PrimitivesNum = i_PrimitivesNum;
        }

        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            GraphicsDevice.DrawUserPrimitives<VectorPrimitiveType>(
                            PrimitiveType.TriangleFan, m_VectorArray, 0, m_PrimitivesNum);
        }
    }
}
