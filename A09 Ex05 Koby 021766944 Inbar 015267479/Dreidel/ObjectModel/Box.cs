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
        private Vector3[] m_VerticesCoordinates;
        private readonly Color r_SideColor = Color.Brown;

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

            // TODO: Move the coordinates to constants

            m_VerticesCoordinates[7] = new Vector3(-.5f, 3, .5f);
            m_VerticesCoordinates[6] = new Vector3(-.5f, 7, .5f);
            m_VerticesCoordinates[5] = new Vector3(.5f, 7, .5f);
            m_VerticesCoordinates[4] = new Vector3(.5f, 3, .5f);
            m_VerticesCoordinates[3] = new Vector3(.5f, 3, -.5f);
            m_VerticesCoordinates[2] = new Vector3(.5f, 7, -.5f);
            m_VerticesCoordinates[1] = new Vector3(-.5f, 7, -.5f);
            m_VerticesCoordinates[0] = new Vector3(-.5f, 3, -.5f);
        }
    }
}
