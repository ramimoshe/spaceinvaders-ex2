using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.Interfaces
{
    public delegate void PlayerIsDeadDelegate(IPlayer i_Player);

    public delegate void PlayerWasHitDelegate();
    
    public delegate void PlayerScoreChangedDelegate();

    public interface IPlayer
    {
        event PlayerWasHitDelegate PlayerWasHitEvent;

        event PlayerScoreChangedDelegate PlayerScoreChangedEvent;

        /// <summary>
        /// Property that gets the player remaining lives num
        /// </summary>
        int RemainingLives
        {
            get;
        }

        /// <summary>
        /// Property that gets the player score
        /// </summary>
        int Score
        {
            get;
        }

        /// <summary>
        /// Property that gets the players texture
        /// </summary>
        Texture2D PlayerTexture
        {
            get;
        }

        /// <summary>
        /// Property that gets the player number
        /// </summary>
        int PlayerNum
        {
            get;
        }
    }
}
