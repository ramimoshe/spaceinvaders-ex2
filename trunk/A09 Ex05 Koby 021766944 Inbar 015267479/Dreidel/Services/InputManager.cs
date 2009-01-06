using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Text;

namespace DreidelGame.Services
{
    // TODO: Check the inherited class

    public class InputManager : GameComponent
    {
        // Data Members
        #region DataMembers

        private KeyboardState m_CurrKeyboardState;
        private KeyboardState m_PrevKeyboardState;
        private MouseState m_CurrMouseState;
        private MouseState m_PrevMouseState;
        
        #endregion

        /// <summary>
        /// Sets a manager with default maximum update order
        /// </summary>
        /// <param name="i_Game">Game holding the service</param>
        public  InputManager(Game i_Game)
            : this(i_Game, Int32.MinValue)
        {
        }

        /// <summary>
        /// Sets a new manager
        /// </summary>
        /// <param name="i_Game">Game holding the service</param>
        public  InputManager(Game i_Game, int i_UpdateOrder)
            : base(i_Game)
        {
            this.UpdateOrder = i_UpdateOrder;

            // TODO: Check if it's ok
            Game.Components.Add(this);
        }

        #region  internalMethods

        /// <summary>
        /// Method gets a mouse button state
        /// </summary>
        /// <param name="i_Button">The specified button</param>
        /// <param name="i_CurrentState">Indicates if button state is from current
        /// or previous state</param>
        /// <returns></returns>
        private ButtonState     getMouseButtonState(eInputButtons i_Button, bool i_CurrentState)
        {
            MouseState requestedMouseState;
            
            // Checking if requested state is current or previous
            if (i_CurrentState)
            {
                requestedMouseState = m_CurrMouseState;
            }
            else
            {
                requestedMouseState = m_PrevMouseState;
            }
            
            // Determining which button is specified
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

        /// <summary>
        /// Gets the current mouse state
        /// </summary>
        public MouseState   MouseState
        {
            get
            {
                return m_CurrMouseState;
            }
        }

        /// <summary>
        /// Gets the current keyboard state
        /// </summary>
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

        /// <summary>
        /// Gets if button's state was changed from not-pressed to pressed since
        /// last call based on current&previous state
        /// </summary>
        /// <param name="i_Button">the specified button</param>
        /// <returns>true if button was pressed since last call, else false</returns>
        public bool ButtonPressed(eInputButtons i_Button)
        {
            return
                getMouseButtonState(i_Button, true) == ButtonState.Pressed &&
                getMouseButtonState(i_Button, false) == ButtonState.Released;
        }

        /// <summary>
        /// Gets if button's state was changed from pressed to not-pressed since
        /// last call based on current&previous state
        /// </summary>
        /// <param name="i_Button">the specified button</param>
        /// <returns>true if button was released since last call, else false</returns>
        public bool ButtonReleased(eInputButtons i_Button)
        {
            return
                getMouseButtonState(i_Button, true) == ButtonState.Released &&
                getMouseButtonState(i_Button, false) == ButtonState.Pressed;
        }

        #endregion

        // Gets the current state-changes in keyboard keys
        #region keyboardKeyChanges

        /// <summary>
        /// Inidicates if key was pressed since last update
        /// </summary>
        /// <param name="i_Key">the specified key</param>
        /// <returns>true if key was pressed, else false</returns>
        public bool KeyPressed(Keys i_Key)
        {
            return 
                m_CurrKeyboardState.IsKeyDown(i_Key) && 
                m_PrevKeyboardState.IsKeyUp(i_Key);
        }

        /// <summary>
        /// Inidicates if key was released since last update
        /// </summary>
        /// <param name="i_Key">the specified key</param>
        /// <returns>true if key was released, else false</returns>
        public bool KeyReleased(Keys i_Key)
        {
            return
                m_CurrKeyboardState.IsKeyUp(i_Key) &&
                m_PrevKeyboardState.IsKeyDown(i_Key);
        }

        /// <summary>
        /// Inidicates if key is held since last update
        /// </summary>
        /// <param name="i_Key">the specified key</param>
        /// <returns>true if key is held, else false</returns>
        public bool     KeyHeld(Keys i_Key)
        {
            return
                m_CurrKeyboardState.IsKeyDown(i_Key) &&
                m_PrevKeyboardState.IsKeyDown(i_Key);
        }

        #endregion

        // Gets changes in various input device states
        #region Deltas

        /// <summary>
        /// Gets the change in mouse position
        /// </summary>
        public Vector2  MousePositionDelta
        {
            get 
            {
                return new Vector2(
                    m_CurrMouseState.X - m_PrevMouseState.X,
                    m_CurrMouseState.Y - m_PrevMouseState.Y);
            }
        }

        /// <summary>
        /// Gets the change in mouse wheel
        /// </summary>
        public int ScrollWheelDelta
        {
            get
            {
                return m_CurrMouseState.ScrollWheelValue - m_PrevMouseState.ScrollWheelValue;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the input device states
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_CurrMouseState = Mouse.GetState();
            m_PrevMouseState = m_CurrMouseState;
            m_CurrKeyboardState = Keyboard.GetState();
            m_PrevKeyboardState = m_CurrKeyboardState;
        }

        /// <summary>
        /// Stores and updates input device states
        /// </summary>
        /// <param name="gameTime">Elapsed time since last call</param>
        public override void    Update(GameTime gameTime)
        {
            m_PrevMouseState = m_CurrMouseState;
            m_CurrMouseState = Mouse.GetState();
            m_PrevKeyboardState = m_CurrKeyboardState; 
            m_CurrKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }
        public string PressedKeys
        {
            get
            {
                Keys[] pressedKeys = KeyboardState.GetPressedKeys();
                string keys = string.Empty;

                if (pressedKeys.Length > 0)
                {
                    StringBuilder keysMsgBuilder = new StringBuilder(pressedKeys.Length * 3);
                    int keysCount = 0;
                    foreach (Keys key in pressedKeys)
                    {
                        keysCount++;
                        keysMsgBuilder.Append(key.ToString());
                        if (keysCount < pressedKeys.Length)
                        {
                            keysMsgBuilder.Append(", ");
                        }
                    }

                    keys = keysMsgBuilder.ToString();
                }

                return keys;
            }
        }

                public override string ToString()
        {
            string status = string.Format(@"
Keyboard.PressedKeys:       {10}

Mouse.X:            {0}
Mouse.Y:            {1}
Mouse.DeltaXY:      {2}
Mouse.Left:         {3}
Mouse.Middle:       {4}
Mouse.Right:        {5}
Mouse.XButton1:     {6}
Mouse.XButton2:     {7}
ScrollWheelValue:   {8}
ScrollWheelDelta:   {9}
",

 MouseState.X,
 MouseState.Y,
 MousePositionDelta,
 MouseState.LeftButton,
 MouseState.MiddleButton,
 MouseState.RightButton,
 MouseState.XButton1,
 MouseState.XButton2,
 MouseState.ScrollWheelValue,
 ScrollWheelDelta,
 PressedKeys
 );
			return status;
        }
    }

    // TODO: Check where to move the enum

    /// <summary>
    /// Declares all buttons to be supported by input manager.
    /// </summary>
    /// <remarks>GamePad butons are for future use</remarks>
    [Flags]
    public enum eInputButtons
    {
        // GamePad Butons
        DPadUp = 1,
        DPadDown = 2,
        DPadLeft = 4,
        DPadRight = 8,
        Start = 16,
        Back = 32,
        LeftStick = 64,
        RightStick = 128,
        LeftShoulder = 256,
        RightShoulder = 512,
        A = 4096,
        B = 8192,
        X = 16384,
        Y = 32768,
        LeftThumbstickLeft = 2097152,
        RightTrigger = 4194304,
        LeftTrigger = 8388608,
        RightThumbstickUp = 16777216,
        RightThumbstickDown = 33554432,
        RightThumbstickRight = 67108864,
        RightThumbstickLeft = 134217728,
        LeftThumbstickUp = 268435456,
        LeftThumbstickDown = 536870912,
        LeftThumbstickRight = 1073741824,

        // Mouse Buttons
        Left = 65536,
        Middle = 131072,
        Right = 262144,
        XButton1 = 524288,
        XButton2 = 1048576
    }
}
