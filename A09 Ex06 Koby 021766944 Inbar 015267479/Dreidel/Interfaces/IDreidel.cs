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

    public delegate void DreidelEventHandler(IDreidel i_Dreidel);

    public interface IDreidel
    {
        event DreidelEventHandler FinishedSpinning;

        void SpinDreidel();

        eDreidelLetters DreidelFrontLetter
        {
            get;
        }
    }
}
