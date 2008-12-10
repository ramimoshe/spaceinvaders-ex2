using System;
using System.Collections.Generic;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// This class a game service to be registered under Game.Services
    /// </summary>
    public abstract class GameService : RegisteredComponent 
    {
        /// <summary>
        /// Calls base class CTOR and registers itself as a game service.
        /// </summary>
        /// <param name="i_Game">The game containing the component</param>
        /// <param name="i_UpdateOrder">Number defining the order in which update 
        /// of all game components is called</param>
        public GameService(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
            RegisterAsService();
        }

        /// <summary>
        /// Default GameService behaviour : 
        /// Registers the component as a service under game;
        /// </summary>
        protected virtual void  RegisterAsService()
        {
            this.Game.Services.AddService(this.GetType(), this);
        }
    }
}
