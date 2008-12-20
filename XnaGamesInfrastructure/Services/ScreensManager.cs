using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.Services
{
    public class ScreensMananger : CompositeDrawableComponent<GameScreen>, IScreensMananger
    {
        public ScreensMananger(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
        }

        private Stack<GameScreen> m_ScreensStack = new Stack<GameScreen>();

        public GameScreen ActiveScreen
        {
            get
            {
                return m_ScreensStack.Count > 0 ? m_ScreensStack.Peek() : null;
            }
        }

        public void SetCurrentScreen(GameScreen i_GameScreen)
        {
            Push(i_GameScreen);

            i_GameScreen.Activate();
        }

        public void Push(GameScreen i_GameScreen)
        {
            i_GameScreen.ScreensManager = this;

            if (!this.Contains(i_GameScreen))
            {
                this.Add(i_GameScreen);

                i_GameScreen.StateChanged += Screen_StateChanged;
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

        void Screen_StateChanged(object sender, StateChangedEventArgs e)
        {
            switch (e.CurrentState)
            {
                case eScreenState.Activating:
                    break;
                case eScreenState.Active:
                    break;
                case eScreenState.Deactivating:
                    break;
                case eScreenState.Closing:
                    Pop(sender as GameScreen);
                    break;
                case eScreenState.Deactive:
                    break;
                case eScreenState.Closed:
                    Remove(sender as GameScreen);
                    break;
                default:
                    break;
            }

            OnScreenStateChanged(sender, e);
        }

        private void Pop(GameScreen i_GameScreen)
        {
            m_ScreensStack.Pop();

            if (m_ScreensStack.Count > 0)
            {
                ActiveScreen.Activate();
            }
        }

        private new bool Remove(GameScreen i_Screen)
        {
            return base.Remove(i_Screen);
        }

        private new void Add(GameScreen i_Component)
        {
            base.Add(i_Component);
        }

        public event EventHandler<StateChangedEventArgs> ScreenStateChanged;
        protected virtual void OnScreenStateChanged(object sender, StateChangedEventArgs e)
        {
            if (ScreenStateChanged != null)
            {
                ScreenStateChanged(sender, e);
            }
        }

        protected override void OnComponentRemoved(GameComponentEventArgs<GameScreen> e)
        {
            base.OnComponentRemoved(e);

            e.GameComponent.StateChanged -= Screen_StateChanged;

            if (m_ScreensStack.Count == 0)
            {
                Game.Exit();
            }
        }

        public override void Initialize()
        {
            Game.Services.AddService(typeof(IScreensMananger), this);

            base.Initialize();
        }
    }
}
