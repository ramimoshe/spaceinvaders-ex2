using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvadersGame.Interfaces
{
    /// <summary>
    /// Interface representing a gmae component that has an abillity to shoot
    /// </summary>
    public interface IShoot
    {    
        /// <summary>
        /// Relase a shoot from the component
        /// </summary>
        void    Shoot();
    }
}
