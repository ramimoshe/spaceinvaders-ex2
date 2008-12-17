using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.Services
{
    /// <summary>
    /// Implements a screen manager
    /// </summary>
    public class ScreensMananger : CompositeDrawableComponent<GameScreen>, IScreensMananger
    {
        /// <summary>
        /// Constructor. Registers itself as a game component
        /// </summary>
        /// <param name="i_Game">Hosting game</param>
        public  ScreensMananger(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
        }

        private Stack<GameScreen> m_ScreensStack = new Stack<GameScreen>();

        /// <summary>
        /// Gets the current active screen
        /// </summary>
        public GameScreen   ActiveScreen
        {
            get
            {
                return m_ScreensStack.Count > 0 ? m_ScreensStack.Peek() : null;
            }
        }

        /// <summary>
        /// Sets the curent Screen
        /// </summary>
        /// <param name="i_GameScreen">The specified screen</param>
        public void SetCurrentScreen(GameScreen i_GameScreen)
        {
            Push(i_GameScreen);

            i_GameScreen.Activate();
        }

        /// <summary>
        /// Adds and activates a new GameScreen at the top of the stack (if not already in 
        /// stack)
        /// </summary>
        /// <param name="i_GameScreen">Specified screen</param>
        public void Push(GameScreen i_GameScreen)
        {
            i_GameScreen.ScreensManager = this;

            if (!this.Contains(i_GameScreen))
            {
                this.Add(i_GameScreen);

                i_GameScreen.Closed += Screen_Closed;
            }

            if (ActiveScreen != i_GameScreen)
            {
                if (ActiveScreen != null)
                {
                    i_GameScreen.PreviousScreen = ActiveScreen;

                    ActiveScreen.Deactivate();
                }
            }

            if (ActiveScreen != i_GameScreen)
            {
                m_ScreensStack.Push(i_GameScreen);
            }

            i_GameScreen.DrawOrder = m_ScreensStack.Count;
        }

        /// <summary>
        /// Removes the notifying screen from Stack
        /// </summary>
        /// <param name="sender">The game screen which closed</param>
        /// <param name="e">Arguments</param>
        private void    Screen_Closed(object sender, EventArgs e)
        {
            Pop(sender as GameScreen);
            Remove(sender as GameScreen);
        }

        /// <summary>
        /// Pops the specified screen from stack, and activates the previous screen
        /// </summary>
        /// <param name="i_GameScreen">Specified game Screen</param>
        private void    Pop(GameScreen i_GameScreen)
        {
            m_ScreensStack.Pop();

            if (m_ScreensStack.Count > 0)
            {
                ActiveScreen.Activate();
            }
        }

        /// <summary>
        /// Removes the specified screen
        /// </summary>
        /// <param name="i_Screen">Specified Screen</param>
        /// <returns>true, if component exists in composite component</returns>
        private new bool    Remove(GameScreen i_Screen)
        {
            return base.Remove(i_Screen);
        }

        /// <summary>
        /// Adds a new screen
        /// </summary>
        /// <param name="i_Component">The new component</param>
        private new void    Add(GameScreen i_Component)
        {
            base.Add(i_Component);
        }

        /// <summary>
        /// Called when a new component is removed from m_Components, to handle 
        /// removal of component from additional collections
        /// </summary>
        /// <param name="e">Argument including the removed screen</param>
        protected override void OnComponentRemoved(GameComponentEventArgs<GameScreen> e)
        {
            base.OnComponentRemoved(e);

            e.GameComponent.Closed -= Screen_Closed;

            if (m_ScreensStack.Count == 0)
            {
                Game.Exit();
            }
        }

        /// <summary>
        /// Initialize screen manager as service
        /// </summary>
        public override void    Initialize()
        {
            Game.Services.AddService(typeof(IScreensMananger), this);

            base.Initialize();
        }
    }
}
