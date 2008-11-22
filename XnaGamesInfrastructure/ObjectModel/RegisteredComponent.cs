using System;
using System.Collections.Generic;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel
{
    public abstract class RegisteredComponent : GameComponent
    {
        public RegisteredComponent(Game i_Game, int i_UpdateOrder)
            : base(i_Game)
        {
            this.UpdateOrder = i_UpdateOrder;
            Game.Components.Add(this);
        }
    }
}
