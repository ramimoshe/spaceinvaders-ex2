using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame.ObjectModel.Screens;

namespace SpaceInvadersGame.Interfaces
{
    /// <summary>
    /// Interface representing a gmae component that has an abillity to shoot
    /// </summary>
    public interface IShoot
    {
        event AddGameComponentDelegate ReleasedShot;

        /// <summary>
        /// Relase a shoot from the component
        /// </summary>
        void    Shoot();
    }
}
