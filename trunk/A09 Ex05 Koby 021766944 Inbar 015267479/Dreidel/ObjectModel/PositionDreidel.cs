using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A position color dreidel component
    /// </summary>
    public class PositionDreidel : Dreidel
    {
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
    }
}
