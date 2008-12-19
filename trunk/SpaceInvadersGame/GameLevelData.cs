using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame.ObjectModel;

namespace SpaceInvadersGame
{
    /// <summary>
    /// Holds all the configured data for a certain game level (the invaders
    /// columns num in the invaders matrix, the invaders score, etc)
    /// </summary>
    public class GameLevelData
    {
        private int m_BarrierSpeed;
        private int m_InvadersColumnNum;
        private int m_MotherShipScore;
        private Dictionary<eInvadersType, int> m_InvadersScoreMap;

        public GameLevelData(
            int i_BarrierSpeed,
            int i_InvadersColumnNum,
            int i_MotherShipScore,
            Dictionary<eInvadersType, int> i_InvadersScoreMap)
        {
            m_BarrierSpeed = i_BarrierSpeed;
            m_InvadersColumnNum = i_InvadersColumnNum;
            m_MotherShipScore = i_MotherShipScore;
            m_InvadersScoreMap = i_InvadersScoreMap;
        }

        /// <summary>
        /// Read only property that gets the barrier speed in a certain level
        /// </summary>
        public int BarrierSpeed
        {
            get { return m_BarrierSpeed; }
        }

        /// <summary>
        /// Read only property that gets the number of column in the invaders
        /// matrix in a certain level
        /// </summary>
        public int  InvadersColumnNum
        {
            get { return m_InvadersColumnNum; }
        }

        /// <summary>
        /// Read only property that gets the mother ship score in a certain
        /// level
        /// </summary>
        public int  MotherShipScore
        {
            get { return m_MotherShipScore; }
        }

        /// <summary>
        /// Return an invader score according to the current level data
        /// </summary>
        /// <param name="i_Invader">The invader we want to get his score</param>
        /// <returns>The score of the invader in the current level</returns>
        public int  GetInvaderScore(eInvadersType i_Invader)
        {
            int retVal;

            m_InvadersScoreMap.TryGetValue(i_Invader, out retVal);

            return retVal;
        }
    }
}
