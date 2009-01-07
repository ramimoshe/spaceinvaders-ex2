using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A cube that contains ColorPosition vertices
    /// </summary>
    public class CubePosition : Cube
    {                
        private readonly Color r_BoxColor = Color.BurlyWood;
        private readonly Color r_FrontColor = Color.Yellow;
        private readonly Color r_BackColor = Color.Red;
        private readonly Color r_LeftColor = Color.Green;
        private readonly Color r_RightColor = Color.Blue;        

        public CubePosition(Game i_Game)
            : base (i_Game)
        {
        }

        /// <summary>
        /// Create the cube sides as VertexPositionColor, and in addition creates all the letters
        /// that are presented in each cube side
        /// </summary>
        protected override void     CreateSides()
        {                        
            // Creating the front side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[0], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_FrontColor),
                            new VertexPositionColor(m_VerticesCoordinates[3], r_FrontColor)));

            // Creating the back side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2, 
                            new VertexPositionColor(m_VerticesCoordinates[4], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_BackColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_BackColor)));

            // Creating the right side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2, 
                            new VertexPositionColor(m_VerticesCoordinates[3], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_RightColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_RightColor)));

            // Creating the left side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[7], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[1], r_LeftColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_LeftColor)));

            // Creating the top side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor)));

            // Creating the bottom side
            Add(new TriangleHolder<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor)));            
        }
    }
}
