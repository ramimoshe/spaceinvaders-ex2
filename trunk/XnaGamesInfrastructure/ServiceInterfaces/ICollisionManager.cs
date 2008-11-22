using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace XnaGamesInfrastructure.ServiceInterfaces
{
    public interface ICollisionManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
}
