using System;
using System.Collections.Generic;
using System.Text;

namespace DreidelGame.Interfaces
{
    /// <summary>
    /// An enum containing the available dreidel letters
    /// </summary>
    public enum eDreidelLetters
    {
        NLetter,
        HLetter,
        PLetter,
        GLetter,
        None
    }

    /// <summary>
    /// This class represents the contant class for all letter
    /// </summary>
    public static class DreidelLettersContainer
    {
        private static readonly eDreidelLetters[] r_Letters = 
            {eDreidelLetters.NLetter,
            eDreidelLetters.GLetter,
            eDreidelLetters.HLetter,
            eDreidelLetters.PLetter};

        public static eDreidelLetters[] Letters
        {
            get 
            { 
                return r_Letters; 
            }
        } 
    }

    /// <summary>
    /// Event handler for dreidel events
    /// </summary>
    /// <param name="i_Dreidel">Notifying dreidel</param>
    public delegate void DreidelEventHandler(IDreidel i_Dreidel);

    /// <summary>
    /// Basic interface for all dreidels
    /// </summary>
    public interface IDreidel
    {
        /// <summary>
        /// Notifies observer uppon finishing dreidel spin
        /// </summary>
        event DreidelEventHandler FinishedSpinning;

        /// <summary>
        /// Activates the 
        /// </summary>
        void SpinDreidel();

        eDreidelLetters DreidelFrontLetter
        {
            get;
        }

        int DreidelScore
        {
            get;
        }
    }
}
