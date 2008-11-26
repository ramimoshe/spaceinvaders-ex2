using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvadersGame.Interfaces
{
    /// <summary>
    /// Interface representing a a component that has an abillity to shoot
    /// </summary>
    public interface IShootable
    {    
        /// <summary>
        /// Relase a shoot from the component
        /// </summary>
        void    Shoot();
    }
}
