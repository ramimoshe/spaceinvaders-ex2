using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.ObjectModel;

namespace XnaGamesInfrastructure.Services
{
    public class InputManager : GameService, IInputManager
    {
        // Data Members
        #region DataMembers

        private KeyboardState m_CurrKeyboardState;
        private KeyboardState m_PrevKeyboardState;
        private MouseState m_CurrMouseState;
        private MouseState m_PrevMouseState;
        
        #endregion

        public InputManager(Game i_Game)
            : this(i_Game, Int32.MinValue)
        {
        }

        public InputManager(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
        }

        #region  internalMethods

        private ButtonState     getMouseButtonState(eInputButtons i_Button, bool i_CurrentState)
        {
            MouseState requestedMouseState;
            
            if (i_CurrentState)
            {
                requestedMouseState = m_CurrMouseState;
            }
            else
            {
                requestedMouseState = m_PrevMouseState;
            }
            
            switch(i_Button)
            {
                case eInputButtons.Left:
                    return requestedMouseState.LeftButton;
                case eInputButtons.Middle:
                    return requestedMouseState.MiddleButton;
                case eInputButtons.Right:
                    return requestedMouseState.RightButton;
                case eInputButtons.XButton1:
                    return requestedMouseState.XButton1;
                case eInputButtons.XButton2:
                    return requestedMouseState.XButton2;
                default:
                    return ButtonState.Released;
            }
        }

        #endregion
        // Getters for current input device states
        #region inputDeviceStates

        public MouseState   MouseState
        {
            get
            {
                return m_CurrMouseState;
            }
        }

        public KeyboardState    KeyboardState
        {
            get
            {
                return m_CurrKeyboardState;
            }
        }

        #endregion

        // Gets the current stante-changes of buttons (in gamepad and mouse)
        #region buttonStateChanges

        public bool     ButtonPressed(eInputButtons i_Button)
        {
            return
                getMouseButtonState(i_Button, true) == ButtonState.Pressed &&
                getMouseButtonState(i_Button, false) == ButtonState.Released;
        }

        public bool     ButtonReleased(eInputButtons i_Button)
        {
            return
                getMouseButtonState(i_Button, true) == ButtonState.Released &&
                getMouseButtonState(i_Button, false) == ButtonState.Pressed;
        }

        #endregion

        // Gets the current stante-changes of buttons (in gamepad and mouse)
        #region keyboardKeyChanges

        public bool     KeyPressed(Keys i_Key)
        {
            return 
                m_CurrKeyboardState.IsKeyDown(i_Key) && 
                m_PrevKeyboardState.IsKeyUp(i_Key);
        }

        public bool     KeyReleased(Keys i_Key)
        {
            return
                m_CurrKeyboardState.IsKeyUp(i_Key) &&
                m_PrevKeyboardState.IsKeyDown(i_Key);
        }

        public bool     KeyHeld(Keys i_Key)
        {
            return
                m_CurrKeyboardState.IsKeyDown(i_Key) &&
                m_PrevKeyboardState.IsKeyDown(i_Key);
        }

        #endregion

        // Gets changes in various input device states
        #region Deltas

        public Vector2  MousePositionDelta
        {
            get 
            {
                return new Vector2(
                    m_CurrMouseState.X - m_PrevMouseState.X,
                    m_CurrMouseState.Y - m_PrevMouseState.Y);
            }
        }

        public int  ScroolWeelDelta
        {
            get
            {
                return m_CurrMouseState.ScrollWheelValue - m_PrevMouseState.ScrollWheelValue;
            }
        }

        #endregion

        public override void    Initialize()
        {
            base.Initialize();

            m_CurrMouseState = Mouse.GetState();
            m_PrevMouseState = m_CurrMouseState;
            m_CurrKeyboardState = Keyboard.GetState();
            m_PrevKeyboardState = m_CurrKeyboardState;
        }

        public override void    Update(GameTime gameTime)
        {
            m_PrevMouseState = m_CurrMouseState;
            m_CurrMouseState = Mouse.GetState();
            m_PrevKeyboardState = m_CurrKeyboardState; 
            m_CurrKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }
    }
}
