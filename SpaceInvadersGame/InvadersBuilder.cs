using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame
{
    /// <summary>
    /// Responsible for creating invaders in the game.
    /// The class implements the singelton pattern so that only one instance 
    /// of the class can be created.
    /// </summary>
    public class InvadersBuilder
    {
        private static InvadersBuilder m_Instance = null;

        private InvadersBuilder()
        {
        }        

        /// <summary>
        /// Part of the singelton pattern, gets the songle instance of the 
        /// class
        /// </summary>        
        /// <returns>The class single instance</returns>
        public static InvadersBuilder   GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new InvadersBuilder();
            }

            return m_Instance;
        }

        /// <summary>
        /// Creates a new invader according to the given invader data
        /// </summary>
        /// <param name="i_InvaderType">The invader type that we want to
        /// create</param>
        /// <param name="i_Game">The game component needed for the invader 
        /// creation</param>
        /// <param name="i_UpdateOrder">The invader update order</param>
        /// <param name="i_InvaderListNum">The invader list number in the
        /// invaders matrix</param>
        /// <returns></returns>
        public Invader  CreateInvader(
            eInvadersType i_InvaderType,
            Game i_Game,
            int i_UpdateOrder,
            int i_InvaderListNum)
        {
            Invader retVal = null;

            switch (i_InvaderType)
            {
                case eInvadersType.YellowInvader:
                    retVal = new YellowInvader(
                        i_Game, 
                        i_UpdateOrder, 
                        i_InvaderListNum);
                    break;
                case eInvadersType.BlueInvader:
                    retVal = new BlueInvader(
                        i_Game,
                        i_UpdateOrder,
                        i_InvaderListNum);
                    break;
                case eInvadersType.PinkInvader:
                    retVal = new PinkInvader(
                        i_Game,
                        i_UpdateOrder,
                        i_InvaderListNum);
                    break;
            }

            return retVal;
        }
    }
}
