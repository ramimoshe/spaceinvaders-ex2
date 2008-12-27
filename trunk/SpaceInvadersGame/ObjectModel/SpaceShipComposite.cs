using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.Services;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaGamesInfrastructure.ObjectInterfaces;
using SpaceInvadersGame.Interfaces;
using SpaceInvadersGame.ObjectModel.Screens;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace SpaceInvadersGame.ObjectModel
{
   /// <summary>
    /// A composite class that holds a SpaceShip and her bullets
    /// </summary>
    public class SpaceShipComposite : CompositeDrawableComponent<IGameComponent>
    {
        private SpaceShip m_SpaceShip;

        #region CTOR's             

        public SpaceShipComposite(Game i_Game, SpaceShip i_SpaceShip)
            : base(i_Game)
        {
            m_SpaceShip = i_SpaceShip;
            m_SpaceShip.ReleasedShot += new AddGameComponentDelegate(spaceShip_ReleasedShot);

            this.Add(m_SpaceShip);
        }

        #endregion

        /// <summary>
        /// Gets the SpaceShip component
        /// </summary>
        public SpaceShip    SpaceShip
        {
            get { return m_SpaceShip; }
        }

        /// <summary>
        /// Catch the ReleasedShot event and adds the new bullet to the 
        /// components list
        /// </summary>
        /// <param name="i_Component">The bullet that the space ship shot</param>
        private void    spaceShip_ReleasedShot(IGameComponent i_Component)
        {
            this.Add(i_Component);
        }
    }
}