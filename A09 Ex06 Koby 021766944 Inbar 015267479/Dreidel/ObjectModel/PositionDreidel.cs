using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A position color dreidel component including manually generated letters
    /// </summary>
    public class PositionDreidel : Dreidel
    {
        private const float k_LetterSpace = .01f;
        protected const float k_ZFactorCoordinate = 3;

        private Vector3[] m_NLetterCoordinates;
        private Vector3[] m_HLetterCoordinates;
        private Vector3[] m_PLetterCoordinates;
        private Vector3[] m_GLetterCoordinates;

        public PositionDreidel(Game i_Game)
            : base(i_Game)
        {
        }

        /// <summary>
        /// Gets a CubePosition component
        /// </summary>
        protected override Cube     DreidelCube
        {
            get { return new CubePosition(Game); }
        }

        /// <summary>
        /// Initialize the dreidel by creating the letters that appear on the cube sides
        /// </summary>
        public override void    Initialize()
        {           
            createLetters();

            base.Initialize();
        }

        /// <summary>
        /// Initialize all the letters coordinates and adds them to the cube elements
        /// </summary>
        private void createLetters()
        {
            createNLetter();
            createHLetter();
            createPLetter();
            createGLetter();
        }

        /// <summary>
        /// Initialize the "ð" letter and adds it to the cube elements
        /// </summary>
        private void createNLetter()
        {
            initNLetterCoordinate();

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                3,
                new VertexPositionColor(m_NLetterCoordinates[0], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[3], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[4], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_NLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[7], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[4], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                1,
                new VertexPositionColor(m_NLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[6], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[7], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_NLetterCoordinates[9], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[8], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[6], Color.Black),
                new VertexPositionColor(m_NLetterCoordinates[5], Color.Black)));
        }

        /// <summary>
        /// Initialize the "ð" letter coordinates
        /// </summary>
        private void initNLetterCoordinate()
        {
            m_NLetterCoordinates = new Vector3[10];

            m_NLetterCoordinates[0] = new Vector3(-2, -2.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[1] = new Vector3(-2, -1.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[2] = new Vector3(1, -1.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[3] = new Vector3(2, -1.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[4] = new Vector3(2, -2.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[5] = new Vector3(1, 1.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[6] = new Vector3(1, 2.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[7] = new Vector3(2, 2.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[8] = new Vector3(0, 2.5f, k_ZFactorCoordinate + k_LetterSpace);
            m_NLetterCoordinates[9] = new Vector3(0, 1.5f, k_ZFactorCoordinate + k_LetterSpace);
        }

        /// <summary>
        /// Initialize the "ä" letter and adds it to the cube elements
        /// </summary>
        private void createHLetter()
        {
            initHLetterCoordinate();

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_HLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[4], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[0], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_HLetterCoordinates[6], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[4], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[3], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_HLetterCoordinates[10], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[8], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[7], Color.Black),
                new VertexPositionColor(m_HLetterCoordinates[9], Color.Black)));
        }

        /// <summary>
        /// Initialize the "ä" letter coordinates
        /// </summary>
        private void initHLetterCoordinate()
        {
            m_HLetterCoordinates = new Vector3[11];

            m_HLetterCoordinates[0] = new Vector3(-2, -2.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[1] = new Vector3(-1, -2.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[2] = new Vector3(-2, 2.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[3] = new Vector3(-1, 1.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[4] = new Vector3(-1, 2.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[5] = new Vector3(2, 2.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[6] = new Vector3(2, 1.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[7] = new Vector3(1, 0, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[8] = new Vector3(2, 0, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[9] = new Vector3(1, -2.5f, -k_ZFactorCoordinate - k_LetterSpace);
            m_HLetterCoordinates[10] = new Vector3(2, -2.5f, -k_ZFactorCoordinate - k_LetterSpace);
        }

        /// <summary>
        /// Initialize the "â" letter and adds it to the cube elements
        /// </summary>
        private void createGLetter()
        {
            initGLetterCoordinates();

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[0], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[4], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[3], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[2], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[7], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[6], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[11], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[12], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[13], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_GLetterCoordinates[9], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[10], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[11], Color.Black),
                new VertexPositionColor(m_GLetterCoordinates[8], Color.Black)));
        }

        /// <summary>
        /// Initialize the "â" letter coordinates
        /// </summary>
        private void initGLetterCoordinates()
        {
            m_GLetterCoordinates = new Vector3[14];

            m_GLetterCoordinates[0] = new Vector3(-3 - k_LetterSpace, -2.5f, -2);
            m_GLetterCoordinates[1] = new Vector3(-3 - k_LetterSpace, 0, -2);
            m_GLetterCoordinates[2] = new Vector3(-3 - k_LetterSpace, 0, -1);
            m_GLetterCoordinates[3] = new Vector3(-3 - k_LetterSpace, -1, -1);
            m_GLetterCoordinates[4] = new Vector3(-3 - k_LetterSpace, -2.5f, -1);
            m_GLetterCoordinates[5] = new Vector3(-3 - k_LetterSpace, -2.5f, 1);
            m_GLetterCoordinates[6] = new Vector3(-3 - k_LetterSpace, -1, 1);
            m_GLetterCoordinates[7] = new Vector3(-3 - k_LetterSpace, 0, 1);
            m_GLetterCoordinates[8] = new Vector3(-3 - k_LetterSpace, 1.5f, 1);
            m_GLetterCoordinates[9] = new Vector3(-3 - k_LetterSpace, 1.5f, -2);
            m_GLetterCoordinates[10] = new Vector3(-3 - k_LetterSpace, 2.5f, -2);
            m_GLetterCoordinates[11] = new Vector3(-3 - k_LetterSpace, 2.5f, 1);
            m_GLetterCoordinates[12] = new Vector3(-3 - k_LetterSpace, 2.5f, 2);
            m_GLetterCoordinates[13] = new Vector3(-3 - k_LetterSpace, -2.5f, 2);
        }

        /// <summary>
        /// Initialize the "ô" letter and adds it to the cube elements
        /// </summary>
        private void createPLetter()
        {
            initPLetterCoordinate();

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_PLetterCoordinates[0], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[5], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[6], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_PLetterCoordinates[1], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[7], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[8], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[2], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_PLetterCoordinates[11], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[8], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[9], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[10], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_PLetterCoordinates[4], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[16], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[12], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[5], Color.Black)));

            Add(new TriangleHolder<VertexPositionColor>(
                Game,
                VertexPositionColor.VertexElements,
                2,
                new VertexPositionColor(m_PLetterCoordinates[14], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[13], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[16], Color.Black),
                new VertexPositionColor(m_PLetterCoordinates[15], Color.Black)));
        }

        /// <summary>
        /// Initialize the "ô" letter coordinates
        /// </summary>
        private void initPLetterCoordinate()
        {
            m_PLetterCoordinates = new Vector3[17];

            m_PLetterCoordinates[0] = new Vector3(3 + k_LetterSpace, -2.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[1] = new Vector3(3 + k_LetterSpace, -1.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[2] = new Vector3(3 + k_LetterSpace, -1.5f, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[3] = new Vector3(3 + k_LetterSpace, -2.5f, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[4] = new Vector3(3 + k_LetterSpace, -1.5f, -k_ZFactorCoordinate + 2);
            m_PLetterCoordinates[5] = new Vector3(3 + k_LetterSpace, -1.5f, -k_ZFactorCoordinate + 1);
            m_PLetterCoordinates[6] = new Vector3(3 + k_LetterSpace, -2.5f, -k_ZFactorCoordinate + 1);
            m_PLetterCoordinates[7] = new Vector3(3 + k_LetterSpace, .5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[8] = new Vector3(3 + k_LetterSpace, .5f, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[9] = new Vector3(3 + k_LetterSpace, .5f, 0);
            m_PLetterCoordinates[10] = new Vector3(3 + k_LetterSpace, 0, 0);
            m_PLetterCoordinates[11] = new Vector3(3 + k_LetterSpace, 0, k_ZFactorCoordinate - 2);
            m_PLetterCoordinates[12] = new Vector3(3 + k_LetterSpace, 2.5f, -k_ZFactorCoordinate + 1);
            m_PLetterCoordinates[13] = new Vector3(3 + k_LetterSpace, 2.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[14] = new Vector3(3 + k_LetterSpace, 1.5f, k_ZFactorCoordinate - 1);
            m_PLetterCoordinates[15] = new Vector3(3 + k_LetterSpace, 1.5f, -k_ZFactorCoordinate + 2);
            m_PLetterCoordinates[16] = new Vector3(3 + k_LetterSpace, 2.5f, -k_ZFactorCoordinate + 2);
        }
    }
}
