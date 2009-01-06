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
    public class CubeTexture : CompositeGameComponent
    {
        private Vector3[] m_VerticesCoordinates = new Vector3[8];
        private const float k_ZFactorWidth = 6;
        private const float k_ZFactorCoordinate = 3f;     
        private readonly Color r_UpDownColor = Color.BurlyWood;

        private const int k_ZFactor = 8;

        public CubeTexture(Game i_Game) : base(i_Game)
        {
        }

        public override void Initialize()
        {
            // TODO: Move to a parent class
            m_VerticesCoordinates[0] = new Vector3(-3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[1] = new Vector3(-3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[2] = new Vector3(3, 3, k_ZFactorCoordinate);
            m_VerticesCoordinates[3] = new Vector3(3, -3, k_ZFactorCoordinate);
            m_VerticesCoordinates[4] = new Vector3(3, -3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[5] = new Vector3(3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[6] = new Vector3(-3, 3, k_ZFactorCoordinate - k_ZFactorWidth);
            m_VerticesCoordinates[7] = new Vector3(-3, -3, k_ZFactorCoordinate - k_ZFactorWidth);            

            // Creating the front side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements, 
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(0, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, .5f))
                            ));

            // Creating the back side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(0, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(0, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, 1))
                            ));

            // Creating the right side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[3], new Vector2(.5f, 1)),
                            new VertexPositionTexture(m_VerticesCoordinates[2], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[5], new Vector2(1, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[4], new Vector2(1, 1))
                            ));

            // Creating the left side
            Add(new Side<VertexPositionTexture>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            new VertexPositionTexture(m_VerticesCoordinates[7], new Vector2(.5f, .5f)),
                            new VertexPositionTexture(m_VerticesCoordinates[6], new Vector2(.5f, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[1], new Vector2(1, 0)),
                            new VertexPositionTexture(m_VerticesCoordinates[0], new Vector2(1, .5f))
                            ));

            // Creating the top side
            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[1], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[6], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[5], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[2], r_UpDownColor)
                            ));

            // Creating the bottom side
            Add(new Side<VertexPositionColor>(
                            Game,
                            VertexPositionTexture.VertexElements,
                            2,
                            new VertexPositionColor(m_VerticesCoordinates[3], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[4], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[7], r_UpDownColor),
                            new VertexPositionColor(m_VerticesCoordinates[0], r_UpDownColor)
                            ));

            base.Initialize();
        }

        protected override void  LoadContent()
        {           
            Texture = Game.Content.Load<Texture2D>(@"Sprites\Dreidel");
        }
    }
}
