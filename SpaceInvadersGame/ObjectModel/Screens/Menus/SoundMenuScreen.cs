using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ServiceInterfaces;

namespace SpaceInvadersGame.ObjectModel.Screens.Menus
{
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

        public SoundMenuScreen(Game i_Game)
            : base(i_Game, k_SoundMenuName)
        {
            m_EffectVolumeTexts = generateTexts("Set Sound Effects Volume");
            m_MusicVolumeTexts = generateTexts("Set Music Volume");
        }

        public override void Initialize()
        {
            base.Initialize();
            int currentSoundFXVolume = (int)(m_SoundManager.SoundFXVolume * k_VolumeChangeValue);
            int currentMusicVolume = (int)(m_SoundManager.MusicVolume * k_VolumeChangeValue);

            m_SoundEffetsVolumeItem = new OptionsMenuItem(
                                            Game,
                                            m_EffectVolumeTexts,
                                            currentSoundFXVolume,
                                            m_SoundEffetsVolumeItem_Increased,
                                            m_SoundEffetsVolumeItem_Decreased);
            m_MusicVolumeItem = new OptionsMenuItem(
                                    Game,
                                    m_MusicVolumeTexts,
                                    currentMusicVolume,
                                    m_MusicVolumeItem_Increased,
                                    m_MusicVolumeItem_Decreased);
            m_ToggleSoundItem = new OptionsMenuItem(
                                    Game,
                                    new List<string>(k_ToggleSoundText),
                                    m_ToggleSoundItem_Increased,
                                    m_ToggleSoundItem_Decreased);
            m_DoneItem = new MenuItem(Game, "Done", m_DoneItem_Executed);

            Add(m_SoundEffetsVolumeItem);
            Add(m_MusicVolumeItem);
            Add(m_ToggleSoundItem);
            Add(m_DoneItem);
        }

        void m_SoundEffetsVolumeItem_Increased()
        {
            m_SoundManager.SoundFXVolume += (float)k_VolumeChangeValue / (float) k_VolumeMax;
        }

        void m_SoundEffetsVolumeItem_Decreased()
        {
            m_SoundManager.SoundFXVolume -= (float)k_VolumeChangeValue / (float)k_VolumeMax;
        }

        void m_MusicVolumeItem_Increased()
        {
            m_SoundManager.MusicVolume += (float)k_VolumeChangeValue / (float)k_VolumeMax;
        }

        void m_MusicVolumeItem_Decreased()
        {
            m_SoundManager.MusicVolume -= (float)k_VolumeChangeValue / (float)k_VolumeMax;
        }

        void m_ToggleSoundItem_Increased()
        {
            m_SoundManager.ToggleMute(true);
        }

        void m_ToggleSoundItem_Decreased()
        {
            m_SoundManager.ToggleMute(true);
        }

        private List<string> generateTexts(string i_Prefix)
        {
            List<string> textList = new List<string>();

            for (int i = k_VolumeMin; i <= k_VolumeMax; i += k_VolumeChangeValue)
            {
                textList.Add(i_Prefix + ":  " + i);
            }

            return textList;
        }

        private void m_DoneItem_Executed()
        {
            ExitScreen();
        }
    }
}
