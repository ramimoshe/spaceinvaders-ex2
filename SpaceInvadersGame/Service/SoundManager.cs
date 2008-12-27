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
    /// The class manages all the games sounds
    /// </summary>
    public class SoundManager : GameService
    {
        private const string k_ContentFolder = @"Content\Audio\";
        private const string k_SoundBankName = "SpaceInvaders.xsb";
        private const string k_WaveBankName = "SpaceInvaders.xwb";
        private const string k_ProjectName = "SpaceInvaders.xgs";
        private const string k_MusicCategoryName = "Music";
        private const string k_FXCategoryName = "SoundFX";
        private const float k_DefaultMusicVolume = 1f;
        private const float k_DefaultSoundFXVolume = 1f;
        private const bool k_DefaultMuteFX = false;

        private float m_MusicVolume;
        private float m_SoundFXVolume;
        private bool m_MusicEnabled = true;
        private bool m_FXEnabled = true;

        private AudioEngine m_AudioEngine;
        private WaveBank m_WaveBank;
        private SoundBank m_SoundBank;

        private Dictionary<string, Cue> m_Cues;

        /// <summary>
        /// Sets/gets the current sound fx category volume
        /// </summary>
        public float    SoundFXVolume
        {
            get { return m_SoundFXVolume; }
            set 
            {
                m_SoundFXVolume = MathHelper.Clamp(value, 0f, 1f);
                changeCategoryVolume(k_FXCategoryName, m_SoundFXVolume);
            }
        }

        /// <summary>
        /// Sets/gets the current music category volume
        /// </summary>
        public float MusicVolume
        {
            get { return m_MusicVolume; }
            set 
            {
                m_MusicVolume = MathHelper.Clamp(value , 0f, 1f);
                
                changeCategoryVolume(k_MusicCategoryName, m_MusicVolume);
            }
        }

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

        // TODO: Remove the code

        /// <summary>
        /// Mute the sound category only
        /// </summary>
     /*   public void     ToggleMute()
        {
            this.ToggleMute(k_DefaultMuteFX);
        }*/

        /// <summary>
        /// Mute the sound category and the fx category if needed
        /// </summary>
        /// <param name="i_ToggleFXMute">Mark if we want to mute the sound
        /// fx also</param>
        public void     ToggleMute(/*bool i_ForceMute*/)
        {
            m_MusicEnabled = !m_MusicEnabled;

            changeCategoryVolume(
                k_MusicCategoryName, 
                !m_MusicEnabled ?  0 : m_MusicVolume);   

            // TODO: Remove the remarked code

            // Check if we need to mute the FX also
           // if (i_ToggleFXMute)
           // {
                m_FXEnabled = !m_FXEnabled;

                changeCategoryVolume(
                    k_FXCategoryName,
                    !m_FXEnabled ? 0 : m_SoundFXVolume);   
           // }
        }

        /// <summary>
        /// Change a given category volume
        /// </summary>
        /// <param name="i_CategoryName">The name of the category that we want
        /// to change</param>
        /// <param name="i_Volume">The new category volume we want to set</param>
        private void    changeCategoryVolume(
            string i_CategoryName, 
            float i_Volume)
        {
            m_AudioEngine.GetCategory(i_CategoryName).SetVolume(i_Volume);   
        }

        /// <summary>
        /// Stops a playing cue
        /// </summary>
        /// <param name="i_CueName">The name of the cue that we want to stop</param>
        public void     StopCue(string i_CueName)
        {
            Cue cue;

            if (m_Cues.TryGetValue(i_CueName, out cue) && cue.IsPlaying)
            {            
                cue.Stop(AudioStopOptions.Immediate);
                m_Cues.Remove(i_CueName);
            }
        }   
    }
}
