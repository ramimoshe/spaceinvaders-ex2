using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ServiceInterfaces
{
    /// <summary>
    /// This interface declares the methods required by and input manager.
    /// It is added in Game.Services as the type for InputManager.
    /// </summary>
    public interface IInputManager
    {
        // Getters for current input device states
        #region inputDeviceStates

        /// <summary>
        /// Gets the current mouse state
        /// </summary>
        MouseState MouseState 
        { 
            get; 
        }

        /// <summary>
        /// Gets the current keyboard state
        /// </summary>
        KeyboardState KeyboardState 
        { 
            get;
        }

        #endregion

        // The following methods get the current state-changes 
        // of buttons (in gamepad and mouse)
        #region buttonsChanges

        /// <summary>
        /// Get's whether specified button was pressed since last update
        /// </summary>
        /// <param name="i_Button">The specified button</param>
        /// <returns>true if button was pressed, else false</returns>
        bool ButtonPressed(eInputButtons i_Button);

        /// <summary>
        /// Get's whether specified button was released since last update
        /// </summary>
        /// <param name="i_Button">The specified button</param>
        /// <returns>true if button was released, else false</returns>
        bool ButtonReleased(eInputButtons i_Button);

        #endregion

        // The following methods get the current state-changes 
        // of keyboard keys
        #region keyboardKeyChanges

        /// <summary>
        /// Get's whether specified key was pressed since last update
        /// </summary>
        /// <param name="i_Button">The specified key</param>
        /// <returns>true if key was pressed, else false</returns>
        bool KeyPressed(Keys i_Key);

        /// <summary>
        /// Get's whether specified key was released since last update
        /// </summary>
        /// <param name="i_Button">The specified key</param>
        /// <returns>true if key was released, else false</returns>
        bool KeyReleased(Keys i_Key);

        /// <summary>
        /// Get's whether specified key was held since last update
        /// </summary>
        /// <param name="i_Button">The specified key</param>
        /// <returns>true if key was held, else false</returns>
        bool KeyHeld(Keys i_Key);

        #endregion

        // The following methods get changes in various input device states
        #region Deltas

        /// <summary>
        /// Gets the change in mouse position
        /// </summary>
        Vector2 MousePositionDelta
        {
            get;
        }

        /// <summary>
        /// Gets the change in mouse wheel
        /// </summary>
        int ScrollWheelDelta
        {
            get;
        }

        #endregion
    }

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
