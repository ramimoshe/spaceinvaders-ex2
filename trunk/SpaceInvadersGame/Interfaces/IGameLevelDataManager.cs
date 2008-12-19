using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame;

namespace SpaceInvadersGame.Interfaces
{
    public interface IGameLevelDataManager
    {
        GameLevelData   this[int i_LevelNum]
        {
            get;
        }
    }
}
