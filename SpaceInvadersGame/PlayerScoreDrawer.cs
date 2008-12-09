using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.Interfaces;

namespace SpaceInvadersGame
{
    public class PlayerScoreDrawer : SpriteFontComponent
    {
        private const string k_FontAssetName = @"Fonts\David";        

        private IPlayer m_Player;
        private string m_TextPrefix;

        public PlayerScoreDrawer(
            Game i_Game, 
            IPlayer i_Player, 
            string i_TextPrefix)
            : base(i_Game, k_FontAssetName)
        {
            m_Player = i_Player;
            m_Player.PlayerScoreChangedEvent += new PlayerScoreChangedDelegate(player_PlayerScoreChangedEvent);
            m_TextPrefix = i_TextPrefix;

            Text = m_TextPrefix + m_Player.Score;
        }

        private void    player_PlayerScoreChangedEvent()
        {
            Text = m_TextPrefix + m_Player.Score;
        }
    }
}
