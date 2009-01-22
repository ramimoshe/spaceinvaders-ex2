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
        private const float k_MinX = -.5f;
        private const float k_MaxX = .5f;
        private const float k_MinY = 3f;
        private const float k_MaxY = 7;
        private const float k_MinZ = -.5f;
        private const float k_MaxZ = .5f;
        private readonly Color r_SideColor = Color.Brown;

        private Vector3 m_LeftBottomBack = new Vector3(k_MinX, k_MinY, k_MinZ);
        private Vector3 m_LeftBottomFront = new Vector3(k_MinX, k_MinY, k_MaxZ);
        private Vector3 m_LeftTopBack = new Vector3(k_MinX, k_MaxY, k_MinZ);
        private Vector3 m_LeftTopFront = new Vector3(k_MinX, k_MaxY, k_MaxZ);
        private Vector3 m_RightBottomBack = new Vector3(k_MaxX, k_MinY, k_MinZ);
        private Vector3 m_RightBottomFront = new Vector3(k_MaxX, k_MinY, k_MaxZ);
        private Vector3 m_RightTopBack = new Vector3(k_MaxX, k_MaxY, k_MinZ);
        private Vector3 m_RightTopFront = new Vector3(k_MaxX, k_MaxY, k_MaxZ);

        private Vector3[] m_VerticesCoordinates;        

        /// <summary>
        /// CTOR. creates a new instance
        /// </summary>
        /// <param name="i_Game"></param>
        public Box(Game i_Game)
            : base(i_Game)
        {
        }

        /// <summary>
        /// Initialize the box coordinates and adds them to the components list
        /// </summary>
        public override void    Initialize()
        {                        
            // Front face
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_LeftTopFront, r_SideColor),
                            new VertexPositionColor(m_RightTopFront, r_SideColor),
                            new VertexPositionColor(m_RightBottomFront, r_SideColor),
                            new VertexPositionColor(m_LeftBottomFront, r_SideColor)));

            // Back face
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_LeftTopBack, r_SideColor),
                            new VertexPositionColor(m_LeftBottomBack, r_SideColor),
                            new VertexPositionColor(m_RightBottomBack, r_SideColor),
                            new VertexPositionColor(m_RightTopBack, r_SideColor)));

            // Top Face
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_LeftTopFront, r_SideColor),
                            new VertexPositionColor(m_LeftTopBack, r_SideColor),
                            new VertexPositionColor(m_RightTopBack, r_SideColor),
                            new VertexPositionColor(m_RightTopFront, r_SideColor)));

            // Bottom Face
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_RightBottomFront, r_SideColor),
                            new VertexPositionColor(m_RightBottomBack, r_SideColor),
                            new VertexPositionColor(m_LeftBottomBack, r_SideColor),
                            new VertexPositionColor(m_LeftBottomFront, r_SideColor)));

            // Right Face
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_RightTopFront, r_SideColor),
                            new VertexPositionColor(m_RightTopBack, r_SideColor),
                            new VertexPositionColor(m_RightBottomBack, r_SideColor),
                            new VertexPositionColor(m_RightBottomFront, r_SideColor)));

            // Left Face
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_LeftBottomFront, r_SideColor),
                            new VertexPositionColor(m_LeftBottomBack, r_SideColor),
                            new VertexPositionColor(m_LeftTopBack, r_SideColor),
                            new VertexPositionColor(m_LeftTopFront, r_SideColor)));

            base.Initialize();
        }

        /// <summary>
        /// Initialize the box coordinates
        /// </summary>
        private void    initCoordinates()
        {
            m_VerticesCoordinates = new Vector3[8];

            m_VerticesCoordinates[7] = new Vector3(
                k_MinX,
                k_MinY,
                k_MaxZ);
            m_VerticesCoordinates[6] = new Vector3(
                k_MinX,
                k_MaxY,
                k_MaxZ);
            m_VerticesCoordinates[5] = new Vector3(
                k_MaxX,
                k_MaxY,
                k_MaxZ);
            m_VerticesCoordinates[4] = new Vector3(
                k_MaxX,
                k_MinY,
                k_MaxZ);
            m_VerticesCoordinates[3] = new Vector3(
                k_MaxX,
                k_MinY,
                k_MinZ);
            m_VerticesCoordinates[2] = new Vector3(
                k_MaxX,
                k_MaxY,
                k_MinZ);
            m_VerticesCoordinates[1] = new Vector3(
                k_MinX,
                k_MaxY,
                k_MinZ);
            m_VerticesCoordinates[0] = new Vector3(
                k_MinX,
                k_MinY,
                k_MinZ);
        }
    }
}
