using System;
using System.Collections.Generic;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel
{
    public abstract class GameService : RegisteredComponent 
    {
        public GameService(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
            RegisterAsService();
        }

        protected virtual void RegisterAsService()
        {
            this.Game.Services.AddService(this.GetType(), this);
        }
    }
}
