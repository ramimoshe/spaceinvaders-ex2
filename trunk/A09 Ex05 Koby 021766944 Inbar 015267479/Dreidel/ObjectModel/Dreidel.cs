using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public class Dreidel : CompositeGameComponent
    {

        public Dreidel(Game i_Game)
            : base(i_Game)
        {
            Add(new Box(i_Game));
            Add(new Cube(i_Game));
            Add(new Pyramid(i_Game));
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
