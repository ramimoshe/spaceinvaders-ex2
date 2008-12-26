using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame;

namespace SpaceInvadersGame.Interfaces
{
    public delegate void PlayActionSoundDelegate(eSoundActions i_Action);

    public interface ISoundableGameComponent
    {
        event PlayActionSoundDelegate PlayActionSoundEvent;
    }
}
