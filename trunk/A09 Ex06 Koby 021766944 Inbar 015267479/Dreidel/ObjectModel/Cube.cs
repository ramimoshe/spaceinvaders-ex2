using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A parent class for the different cube classes
    /// </summary>
    public abstract class Cube : CompositeGameComponent
    {
        protected const float k_ZFactorWidth = 6;
        protected const float k_ZFactorCoordinate = 3f;
        protected const float k_MinXCoordinate = -3;
        protected const float k_MaxXCoordinate = 3;
        protected const float k_MinYCoordinate = -3;
        protected const float k_MaxYCoordinate = 3;
        protected readonly Color r_UpDownColor = Color.BurlyWood;

        protected Vector3[] m_VerticesCoordinates = new Vector3[8];

        /// <summary>
        /// CTOR. Inits a new instance
        /// </summary>
        /// <param name="i_Game">The hosting game</param>
        public Cube(Game i_Game)
            : base(i_Game)
        {
        }
        
        /// <summary>
        /// Initialize the cube by initializing the coordinates and create all the cube sides
        /// </summary>
        public override void    Initialize()
        {
            initStartCoordinates();
            CreateSides();

            base.Initialize();
        }

        /// <summary>
        /// Initialize the cube starting vertices coordinates
        /// </summary>
        private void    initStartCoordinates()
        {
            m_VerticesCoordinates[0] =
                new Vector3(k_MinXCoordinate, k_MinYCoordinate, k_ZFactorCoordinate);
            m_VerticesCoordinates[1] =
                new Vector3(k_MinXCoordinate, k_MaxYCoordinate, k_ZFactorCoordinate);
            m_VerticesCoordinates[2] =
                new Vector3(k_MaxXCoordinate, k_MaxYCoordinate, k_ZFactorCoordinate);
            m_VerticesCoordinates[3] =
                new Vector3(k_MaxXCoordinate, k_MinYCoordinate, k_ZFactorCoordinate);

            m_VerticesCoordinates[4] =
                new Vector3(
                k_MaxXCoordinate,
                k_MinYCoordinate,
                k_ZFactorCoordinate - k_ZFactorWidth);

            m_VerticesCoordinates[5] =
                new Vector3(
                k_MaxXCoordinate,
                k_MaxYCoordinate,
                k_ZFactorCoordinate - k_ZFactorWidth);

            m_VerticesCoordinates[6] =
                new Vector3(
                k_MinXCoordinate,
                k_MaxYCoordinate,
                k_ZFactorCoordinate - k_ZFactorWidth);

            m_VerticesCoordinates[7] =
                new Vector3(
                k_MinXCoordinate,
                k_MinYCoordinate,
                k_ZFactorCoordinate - k_ZFactorWidth);
        }

        /// <summary>
        /// Creates the cube sides
        /// </summary>
        protected abstract void     CreateSides();        
    }
}
