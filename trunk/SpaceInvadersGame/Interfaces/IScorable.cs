using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvadersGame.Interfaces
{
    public interface IScorable
    {
        /// <summary>
        /// A property that holds the score of the component
        /// </summary>
        int Score
        {
            get;
        }

    }
}
