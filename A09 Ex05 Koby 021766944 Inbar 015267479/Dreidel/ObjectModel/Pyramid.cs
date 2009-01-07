using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// Represents the dreidel bottom component (a pyramid)
    /// </summary>
    public class Pyramid : CompositeGameComponent
    {
        private Vector3[] m_VerticesCoordinates = new Vector3[5];
        private const float k_ZFactorWidth = 6;
        private const float k_ZFactorCoordinate = 3f;
        private readonly Color r_SideColor = Color.BurlyWood;

        public Pyramid(Game i_Game)
            : base (i_Game)
        {
        }

        /// <summary>
        /// Initialize the pyramid coordinates and adds them to the component list
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
        }

        /// <summary>
        /// Initialize the pyramid coordinates
        /// </summary>
        private void    initCoordinates()
        {
            m_VerticesCoordinates = new Vector3[5];

            // TODO: Move the values to constants

            m_VerticesCoordinates[0] = new Vector3(-3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[1] = new Vector3(3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[2] = new Vector3(3, -3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[3] = new Vector3(-3, -3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[4] = new Vector3(0, -6, 0);
        }
    }
}
