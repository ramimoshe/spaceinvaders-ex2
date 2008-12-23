using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame.ObjectModel;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.Interfaces;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame
{
    public class GameLevelDataManager : GameService, IGameLevelDataManager
    {
        private const int k_LevelsNum = 5;
        private const int k_IncreaseLevelScoreVal = 50;
        private const int k_MotherShipScore = 500;
        private const int k_YellowInvaderScore = 100;
        private const int k_BlueInvaderScore = 200;
        private const int k_PinkInvaderScore = 300;
        private const int k_Barrier1LevelSpeed = 40;
        private const int k_Barrier2LevelSpeed = 40;
        private const float k_IncreaseBarrierSpeed = .25f;
        private const int k_InvadersColumnNum = 9;        

        private GameLevelData[] m_LevelsData;

        public GameLevelDataManager(Game i_Game) 
            : base(i_Game, Int32.MinValue)
        {
            m_LevelsData = new GameLevelData[k_LevelsNum];
            initLevelsData();
        }

        private Dictionary<eInvadersType, int>    createNextLevelScoreMap(
            Dictionary<eInvadersType, int> i_CurrentLevelInvadersMap)
        {
            Dictionary<eInvadersType, int> retVal = new Dictionary<eInvadersType, int>();

            foreach (KeyValuePair<eInvadersType, int> key in i_CurrentLevelInvadersMap)
            {
                retVal[key.Key] = key.Value + k_IncreaseLevelScoreVal;
            }

            return retVal;
        }

   /*     private void    initGameData()
        {
            m_FirstLevelData = new GameLevelData(
                k_Barrier1LevelSpeed,
                k_InvadersColumnNum,
                k_MotherShipScore,
                r_InvadersDefaultScores);

        }*/

        private Dictionary<eInvadersType, int>  getFirstLevelScoreMap()
        {
            Dictionary<eInvadersType, int> retVal = 
                new Dictionary<eInvadersType, int>();

            retVal.Add(eInvadersType.YellowInvader, k_YellowInvaderScore);
            retVal.Add(eInvadersType.BlueInvader, k_BlueInvaderScore);
            retVal.Add(eInvadersType.PinkInvader, k_PinkInvaderScore);

            return retVal;
        }

        private void    initLevelsData()
        {
            Dictionary<eInvadersType, int> currLevelInvadersScore =
                getFirstLevelScoreMap();

            // Create first level game data
            m_LevelsData[0] = new GameLevelData(
                k_Barrier1LevelSpeed,
                k_InvadersColumnNum,
                k_MotherShipScore,
                getFirstLevelScoreMap());

            // Create all the rest game levels data
            for (int i = 1; i < k_LevelsNum; i++)
            {
                currLevelInvadersScore =
                    createNextLevelScoreMap(currLevelInvadersScore);

                m_LevelsData[i] = new GameLevelData(
                    k_Barrier2LevelSpeed + (int)(k_IncreaseBarrierSpeed * (i-1)),
                    k_InvadersColumnNum + i,
                    k_MotherShipScore + (i * k_IncreaseLevelScoreVal),
                    currLevelInvadersScore);
            }
        }

        /// <summary>
        /// Read only indexer that returns a game level data
        /// </summary>
        /// <param name="i_LevelNum">The level number that we want to get
        /// the data for</param>
        /// <returns>The game data of the given level num</returns>
        public GameLevelData    this[int i_LevelNum]
        {
            get 
            {
                int levelNum = i_LevelNum % (k_LevelsNum + 1);
                levelNum = (i_LevelNum > k_LevelsNum) ?
                    levelNum : levelNum - 1;

                return m_LevelsData[levelNum];
            }
        }
    }
}
