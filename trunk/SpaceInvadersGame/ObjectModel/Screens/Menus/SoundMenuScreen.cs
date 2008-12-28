using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ServiceInterfaces;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
    /// <summary>
    /// Implements the sound options menu screen
    /// </summary>
    public class SoundMenuScreen : MenuTypeScreen
    {
        private const string k_SoundMenuName = "Sound Options";
        private const int k_VolumeChangeValue = 10;
        private const int k_VolumeMin = 0;
        private const int k_VolumeMax = 100;
        private string[] k_ToggleSoundText = { "Toggle Sound: On", "Toggle Sound: Off" };

        private List<string> m_EffectVolumeTexts;
        private List<string> m_MusicVolumeTexts;

        private OptionsMenuItem m_SoundEffetsVolumeItem;
        private OptionsMenuItem m_MusicVolumeItem;
        private OptionsMenuItem m_ToggleSoundItem;
        private MenuItem m_DoneItem;

        /// <summary>
        /// Initializes the sound menu
        /// </summary>
        /// <param name="i_Game">Hosting game</param>
        public  SoundMenuScreen(Game i_Game)
            : base(i_Game, k_SoundMenuName)
        {
            // Generating 2 volume string list 
            m_EffectVolumeTexts = generateTexts("Set Sound Effects Volume");
            m_MusicVolumeTexts = generateTexts("Set Music Volume");
        }

        /// <summary>
        /// Initializing the screen (adding all items, and registering as observer)
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();
            int currentSoundFXVolume = (int)(m_SoundManager.SoundFXVolume * k_VolumeChangeValue);
            int currentMusicVolume = (int)(m_SoundManager.MusicVolume * k_VolumeChangeValue);

            m_SoundEffetsVolumeItem = new OptionsMenuItem(
                                            Game,
                                            m_EffectVolumeTexts,
                                            currentSoundFXVolume,
                                            m_SoundEffetsVolumeItem_Modified);
            m_MusicVolumeItem = new OptionsMenuItem(
                                    Game,
                                    m_MusicVolumeTexts,
                                    currentMusicVolume,
                                    m_MusicVolumeItem_Modified);
            m_ToggleSoundItem = new OptionsMenuItem(
                                    Game,
                                    new List<string>(k_ToggleSoundText),
                                    m_ToggleSoundItem_Modified);
            m_DoneItem = new MenuItem(Game, "Done", m_DoneItem_Executed);

            Add(m_SoundEffetsVolumeItem);
            Add(m_MusicVolumeItem);
            Add(m_ToggleSoundItem);
            Add(m_DoneItem);
        }

        /// <summary>
        /// Sets the current volume according to menu item
        /// </summary>
        /// <param name="i_CurrentVolume">Current Volume</param>
        private void    m_SoundEffetsVolumeItem_Modified(int i_CurrentVolume)
        {
            m_SoundManager.SoundFXVolume = (float)i_CurrentVolume * (float)k_VolumeChangeValue / (float)k_VolumeMax;
        }

        /// <summary>
        /// Sets the current volume according to menu item
        /// </summary>
        /// <param name="i_CurrentVolume">Current Volume</param>
        private void    m_MusicVolumeItem_Modified(int i_CurrentVolume)
        {
            m_SoundManager.MusicVolume = (float)i_CurrentVolume * (float)k_VolumeChangeValue / (float)k_VolumeMax;
        }

        /// <summary>
        /// Toggling Sound mute
        /// </summary>
        /// <param name="i_Dummy">Text id</param>
        private void    m_ToggleSoundItem_Modified(int i_Dummy)
        {
            m_SoundManager.ToggleMute();
        }

        /// <summary>
        /// Generating text list according to default volume settings
        /// </summary>
        /// <param name="i_Prefix">Prefix for all volumns</param>
        /// <returns>A list containing all generated texts</returns>
        private List<string>    generateTexts(string i_Prefix)
        {
            List<string> textList = new List<string>();

            for (int i = k_VolumeMin; i <= k_VolumeMax; i += k_VolumeChangeValue)
            {
                textList.Add(i_Prefix + ":  " + i);
            }

            return textList;
        }

        /// <summary>
        /// Exiting screen
        /// </summary>
        private void    m_DoneItem_Executed()
        {
            ExitScreen();
        }
    }
}
