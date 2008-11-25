using System;
using System.Collections.Generic;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// Implements a base class for non-drawble game components, which registers
    /// itself under Game.Components
    /// </summary>
    public abstract class RegisteredComponent : GameComponent
    {
        /// <summary>
        /// Calls GameComponent to initialize game property, and initialize update 
        /// order
        /// </summary>
        /// <param name="i_Game">The game containing the component</param>
        /// <param name="i_UpdateOrder">Number defining the order in which update 
        /// of all game components is called</param>
        public RegisteredComponent(Game i_Game, int i_UpdateOrder)
            : base(i_Game)
        {
            this.UpdateOrder = i_UpdateOrder;

            // Component is added under game components
            Game.Components.Add(this);
        }
    }
}
