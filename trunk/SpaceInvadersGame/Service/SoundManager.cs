using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpaceInvadersGame.Service
{    

    /// <summary>
    /// The class manages all the games sound
    /// </summary>
    public class SoundManager : GameService
    {
        // TODO: Change the wave bank and sound bank names

        private const string k_ContentFolder = @"Content\Audio\";
        private const string k_SoundBankName = "SpaceInvaders.xsb";
        private const string k_WaveBankName = "SpaceInvaders.xwb";
        private const string k_ProjectName = "SpaceInvaders.xgs";
        private const string k_MusicCategoryName = "Music";
        private const float k_DefaultMusicVolume = 1f;
        private const float k_DefaultSoundFXVolume = 1f;
        private const bool k_DefaultToggleMute = true;

        private float m_MusicVolume;
        private float m_SoundFXVolume;
        private bool m_MusicEnabled = true;

        private AudioEngine m_AudioEngine;
        private WaveBank m_WaveBank;
        private SoundBank m_SoundBank;

        private Dictionary<string, Cue> m_Cues;

        public SoundManager(Game i_Game)
            : base(i_Game, Int32.MinValue)
        {
            m_MusicVolume = k_DefaultMusicVolume;
            m_SoundFXVolume = k_DefaultSoundFXVolume;
            m_Cues = new Dictionary<string, Cue>();
        }

        /// <summary>
        /// Initialize the manager by creating the AudioEngine, WaveBank and
        /// SoundBank members
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_AudioEngine = new AudioEngine(k_ContentFolder + k_ProjectName);
            m_WaveBank = new WaveBank(m_AudioEngine, k_ContentFolder + k_WaveBankName);
            m_SoundBank = new SoundBank(m_AudioEngine, k_ContentFolder + k_SoundBankName);
        }

        // TODO: Enable the code

        /// <summary>
        /// Updates the audio engine data
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
       /* public override void    Update(GameTime i_GameTime)            
        {
            m_AudioEngine.Update();
        }*/

        /// <summary>
        /// Play a given cue
        /// </summary>
        /// <param name="i_CueName">The name of the cue that we want to play</param>
        public void     Play(string i_CueName, bool i_SaveCue)
        {
            Cue cue;

            // TODO: Check the if

            if (!(m_Cues.TryGetValue(i_CueName, out cue)))
            {

                cue = m_SoundBank.GetCue(i_CueName);

                if (i_SaveCue)
                {
                    m_Cues.Add(i_CueName, cue);
                }

                cue.Play();
            }
        }

        public void     ToggleMute()
        {
            this.ToggleMute(k_DefaultToggleMute);
        }

        public void     ToggleMute(bool i_EnableSound)
        {
            m_MusicEnabled = !m_MusicEnabled && i_EnableSound;

            m_AudioEngine.GetCategory(k_MusicCategoryName).SetVolume(
                !m_MusicEnabled ?  0 : m_MusicVolume);   
        }

        public void     StopCue(string i_CueName)
        {
            Cue cue;

            if (m_Cues.TryGetValue(i_CueName, out cue) && cue.IsPlaying)
            {            
                cue.Stop(AudioStopOptions.Immediate);
                m_Cues.Remove(i_CueName);
            }
        }


        // TODO: Remove the code
        /*public void     StopMusic()
        {
            m_AudioEngine.GetCategory(k_MusicCategoryName).Pause();
        }

        public void     EnableMusic()
        {
            m_AudioEngine.GetCategory(k_MusicCategoryName).Resume();
        }*/
    }
}
