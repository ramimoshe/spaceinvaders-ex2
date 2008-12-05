using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.Interfaces
{
    public interface IPlayer
    {
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
    }
}
