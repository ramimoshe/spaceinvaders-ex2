using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.Service;
using SpaceInvadersGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    /// <summary>
    /// A parent for all the different screens that the game has
    /// </summary>
    public class SpaceInvadersScreenAbstract : GameScreen
    {
        private readonly Keys r_MuteKey = Keys.M;

        protected SoundManager m_SoundManager;

        public SpaceInvadersScreenAbstract(Game i_Game)
            : base(i_Game)
        {
        }

        /// <summary>
        /// Initialize the screen by setting it's sound manager
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_SoundManager = Game.Services.GetService(typeof(SoundManager)) as SoundManager;
        }

        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(r_MuteKey))
            {
                m_SoundManager.ToggleMute();                
            }
        }

        // TODO: Check if the code should be here

        public override void    Add(IGameComponent i_Component)
        {
            base.Add(i_Component);

            ISoundableGameComponent component = i_Component as ISoundableGameComponent;

            if (component != null)
            {
                component.PlayActionSoundEvent += new PlayActionSoundDelegate(Component_PlayActionSoundEvent);
            }
        }

        /// <summary>
        /// Catch the PlayActionSoundEvent raised by a SoundableGameComponent
        /// and plays the action sound
        /// </summary>
        /// <param name="i_Action">The action that happened in the game that 
        /// should result in playing a sound</param>
        protected void    Component_PlayActionSoundEvent(eSoundActions i_Action)
        {
            PlayActionCue(i_Action);
        }

        /// <summary>
        /// Plays a cue for a given action in the game
        /// </summary>
        /// <param name="i_Action">The action that we need to play a cue for</param>
        protected void  PlayActionCue(eSoundActions i_Action)
        {
            string cue = SoundFactory.GetActionCue(i_Action);
            if (!cue.Equals(String.Empty))
            {
                m_SoundManager.Play(cue, false);
            }
        }
    }
}
