using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// Represents the dreidel handle
    /// </summary>
    public class Box : CompositeGameComponent
    {
        private const float k_MinXCoordinate = -.5f;
        private const float k_MaxXCoordinate = .5f;
        private const float k_MinYCoordinate = 3;
        private const float k_MaxYCoordinate = 7;
        private const float k_MinZCoordinate = -.5f;
        private const float k_MaxZCoordinate = .5f;
        private readonly Color r_SideColor = Color.Brown;

        private Vector3[] m_VerticesCoordinates;        

        public Box(Game i_Game)
            : base (i_Game)
        {
        }

        /// <summary>
        /// Initialize the box coordinates and adds them to the components list
        /// </summary>
        public override void    Initialize()
        {
            initCoordinates();
                        
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor)
                            ));

            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_SideColor)
                            ));

            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor)
                            ));

            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[7], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor)
                            ));

            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_SideColor)
                            ));

            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_SideColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_SideColor)
                            ));

            base.Initialize();
        }

        /// <summary>
        /// Initialize the box coordinates
        /// </summary>
        private void    initCoordinates()
        {
            m_VerticesCoordinates = new Vector3[8];

            m_VerticesCoordinates[7] = new Vector3(
                k_MinXCoordinate, 
                k_MinYCoordinate, 
                k_MaxZCoordinate);
            m_VerticesCoordinates[6] = new Vector3(
                k_MinXCoordinate,
                k_MaxYCoordinate, 
                k_MaxZCoordinate);
            m_VerticesCoordinates[5] = new Vector3(
                k_MaxXCoordinate, 
                k_MaxYCoordinate, 
                k_MaxZCoordinate);
            m_VerticesCoordinates[4] = new Vector3(
                k_MaxXCoordinate, 
                k_MinYCoordinate, 
                k_MaxZCoordinate);
            m_VerticesCoordinates[3] = new Vector3(
                k_MaxXCoordinate, 
                k_MinYCoordinate, 
                k_MinZCoordinate);
            m_VerticesCoordinates[2] = new Vector3(
                k_MaxXCoordinate, 
                k_MaxYCoordinate, 
                k_MinZCoordinate);
            m_VerticesCoordinates[1] = new Vector3(
                k_MinXCoordinate, 
                k_MaxYCoordinate, 
                k_MinZCoordinate);
            m_VerticesCoordinates[0] = new Vector3(
                k_MinXCoordinate, 
                k_MinYCoordinate, 
                k_MinZCoordinate);
        }
    }
}
