using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A texture dreidel component
    /// </summary>
    public class TextureDreidel : Dreidel
    {
        public TextureDreidel(Game i_Game)
            : base(i_Game)
        {
        }

        /// <summary>
        /// Gets a CubeTexture component
        /// </summary>
        protected override Cube     DreidelCube
        {
            get { return new CubeTexture(Game); }
        }
    }
}
