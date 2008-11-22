using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectInterfaces
{
    public delegate void PositionChangedEventHandler(ICollidable i_Collidable);

    public interface ICollidable
    {
        bool     CheckForCollision(ICollidable i_OtherComponent);
        
        Rectangle Bounds
        {
            get;
        }

        Vector2 MotionVector
        {
            get;
        }
        
        bool Visible
        {
            get;
        }

        void Collided(ICollidable i_OtherComponent);
        event EventHandler Disposed;
        event PositionChangedEventHandler PositionChanged;
    }
}
