using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame
{
    /// <summary>
    /// Manages the drawing of a player score
    /// </summary>
    public class PlayerScoreDrawer : SpriteFontComponent
    {
        private const string k_FontAssetName = @"Fonts\David";

        private readonly Color[] r_PlayersTintColor = new Color[] {
            Color.Blue, Color.Green };

        private IPlayer m_Player;
        private string m_TextPrefix;

        public PlayerScoreDrawer(
            Game i_Game, 
            IPlayer i_Player, 
            string i_TextPrefix)
            : base(i_Game, k_FontAssetName, i_TextPrefix)
        {
            m_Player = i_Player;
            m_Player.PlayerScoreChangedEvent += new PlayerScoreChangedDelegate(player_PlayerScoreChangedEvent);
            m_TextPrefix = i_TextPrefix;

            TintColor = r_PlayersTintColor[m_Player.PlayerNum - 1];
            Text = m_TextPrefix + m_Player.Score;
        }

        /// <summary>
        /// Catch a score changed event raised by the player, and updates
        /// the players score
        /// </summary>
        private void    player_PlayerScoreChangedEvent()
        {
            Text = m_TextPrefix + m_Player.Score;
        }
    }
}
