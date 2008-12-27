using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace XnaGamesInfrastructure.ServiceInterfaces
{
    /// <summary>
    /// Encapsulates CollisionManager (to be used by Game.Services)
    /// </summary>
    public interface ICollisionManager : IDisposable
    {
        /// <summary>
        /// Add's a new ICollidable to CollisionManager's private monitored 
        /// objects list.
        /// </summary>
        /// <param name="i_Collidable">The newle added</param>
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
}
