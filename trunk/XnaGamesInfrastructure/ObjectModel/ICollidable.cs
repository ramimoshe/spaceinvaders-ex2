using System;
using System.Collections.Generic;
using System.Text;

namespace XnaGamesInfrastructure.ObjectModel
{
    public interface ICollidable
    {
        bool     CheckForCollision(Sprite i_OtherComponent);

        void     Colided(Sprite i_OtherComponent);
    }
}
