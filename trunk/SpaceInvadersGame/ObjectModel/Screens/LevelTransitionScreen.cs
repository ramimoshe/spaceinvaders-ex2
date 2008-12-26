using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using XnaGamesInfrastructure.ObjectModel.Screens;
using SpaceInvadersGame.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.Services;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    /// <summary>
    /// The class is a transition screen between the game levels
    /// </summary>
    public class LevelTransitionScreen : SpaceInvadersScreenAbstract
    {
        private const int k_TransitionScreenTime = 3;
        private const int k_StartingLevelNum = 1;

        private readonly string r_EndingSecondStr = "1";
        private readonly string r_LevelMessageText = "Level {0}";
        private readonly string r_SecondsMessageText = "Level will start in {0} seconds";

        private SpriteFontComponent m_LevelMessage;
        private SpriteFontComponent m_SecondsMessage;

        private readonly TimeSpan r_TimeBetweenUpdate = TimeSpan.FromSeconds(1f);        

        private int m_TimeRemainingForScreen = k_TransitionScreenTime;

        private TimeSpan m_PrevTime;

        private int m_CurrLevelNum;

        public LevelTransitionScreen(Game i_Game)
            : base(i_Game)
        {
            m_CurrLevelNum = k_StartingLevelNum;

            m_LevelMessage = new SpriteFontComponent(
                i_Game, 
                @"Fonts\David28", 
                getMessageText(r_LevelMessageText, m_CurrLevelNum));            
            m_SecondsMessage = new  SpriteFontComponent(
                                    i_Game, 
                                    @"Fonts\David",
                                    getMessageText(r_SecondsMessageText, k_TransitionScreenTime));

            m_LevelMessage.TintColor = Color.White;
            m_SecondsMessage.TintColor = Color.White;
            // TODO: Change the parameter to a constant

            this.Add(m_LevelMessage);
            this.Add(m_SecondsMessage);
            this.HasFocus = true;
            
            m_PrevTime = r_TimeBetweenUpdate;
        }

        /// <summary>
        /// Format the given string by adding the current level number
        /// </summary>
        /// <param name="r_Message">The message we want to format</param>
        /// <param name="i_NumInMsg">The number we want to put in the message</param>
        /// <returns>The formated message containing the current level num</returns>
        private string getMessageText(string r_Message, int i_NumInMsg)
        {
            return String.Format(r_Message, i_NumInMsg);
        }

        /// <summary>
        /// Initialize the screen messages according to the current screen
        /// data
        /// </summary>
        private void    initMessagesText()
        {
            m_LevelMessage.Text = m_LevelMessage.Text.Replace(
                Convert.ToString(m_CurrLevelNum - 1),
                Convert.ToString(m_CurrLevelNum));

            m_SecondsMessage.Text = m_SecondsMessage.Text.Replace(
                r_EndingSecondStr,
                Convert.ToString(m_TimeRemainingForScreen));
        }        

        /// <summary>
        /// Initialize the screen by setting the messages positions
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            Vector2 center = m_LevelMessage.ViewPortCenter;
            
            m_LevelMessage.PositionOfOrigin = center;
            m_LevelMessage.PositionOrigin = m_LevelMessage.SpriteCenter;

            m_SecondsMessage.Text = m_SecondsMessage.Text;
            m_SecondsMessage.PositionOfOrigin = new Vector2(center.X, center.Y + m_LevelMessage.HeightAfterScale);
            m_SecondsMessage.PositionOrigin = m_SecondsMessage.SpriteCenter;
        }

        /// <summary>
        /// Update the screen
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            m_PrevTime -= i_GameTime.ElapsedGameTime;

            if (m_PrevTime.TotalSeconds <= 0)
            {
                m_TimeRemainingForScreen--;
                m_PrevTime = r_TimeBetweenUpdate;

                // If the number of seconds for current screen presentation
                // had passed, we'll exit the screen otherwise will keep counting
                if (m_TimeRemainingForScreen > 0)
                {
                    // Change the remaining seconds in the message
                    m_SecondsMessage.Text = 
                        m_SecondsMessage.Text.Replace(
                            Convert.ToString(m_TimeRemainingForScreen + 1),
                            Convert.ToString(m_TimeRemainingForScreen));
                }
                else
                {
                    beforeExitScreen();
                    ExitScreen();
                }
            }
        }

        /// <summary>
        /// Update and initialize the screen data for the next screen 
        /// appearance
        /// </summary>
        private void    beforeExitScreen()
        {
            m_TimeRemainingForScreen = k_TransitionScreenTime;
            m_CurrLevelNum++;
            initMessagesText();
        }
    }
}
