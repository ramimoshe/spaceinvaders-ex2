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

        private float m_MusicVolume;
        private float m_SoundFXVolume;
        private bool m_MusicEnabled = true;

        private AudioEngine m_AudioEngine;
        private WaveBank m_WaveBank;
        private SoundBank m_SoundBank;

        public SoundManager(Game i_Game)
            : base(i_Game, Int32.MinValue)
        {
            m_MusicVolume = k_DefaultMusicVolume;
            m_SoundFXVolume = k_DefaultSoundFXVolume;
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

        /// <summary>
        /// Play a given cue
        /// </summary>
        /// <param name="i_CueName">The name of the cue that we want to play</param>
        public void     Play(string i_CueName)
        {
            m_SoundBank.PlayCue(i_CueName);
        }

        public void     ToggleMute()
        {
            m_MusicEnabled = !m_MusicEnabled;

            m_AudioEngine.GetCategory(k_MusicCategoryName).SetVolume(
                !m_MusicEnabled ?  0 : m_MusicVolume);   
        }        
    }
}
