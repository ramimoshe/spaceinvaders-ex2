using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A cube that contains Texture vertices
    /// </summary>
    public class CubeTexture : Cube
    {       
        public CubeTexture(Game i_Game) : base(i_Game)
        {
        }

        /// <summary>
        /// Creates the cube sides as VertexPositionTexture
        /// </summary>
        protected override void     CreateSides()
        {          
            // Creating the front side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements, 
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(0, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, .5f))));

            // Creating the back side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(0, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, 1))));

            // Creating the right side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(1, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(1, 1))));

            // Creating the left side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            true,
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(1, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(1, .5f))));

            // Creating the top side
            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            false,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor)));

            // Creating the bottom side
            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionColor.VertexElements,
                            2,
                            false,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor)));
        }

        /// <summary>
        /// Loads the texture we want to draw on the cube sides
        /// </summary>
        protected override void     LoadContent()
        {           
            Texture = Game.Content.Load<Texture2D>(@"Sprites\Dreidel");
        }
    }
}
