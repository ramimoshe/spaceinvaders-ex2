using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ServiceInterfaces
{
    /// <summary>
    /// This interface declares the methods required by and input manager
    /// </summary>
    public interface IInputManager
    {
        // Getters for current input device states
        #region inputDeviceStates

        MouseState MouseState 
        { 
            get; 
        }

        KeyboardState KeyboardState 
        { 
            get;
        }

        #endregion


        // Gets the current stante-changes of buttons (in gamepad and mouse)
        #region buttonsChanges

        bool ButtonPressed(eInputButtons i_Button);

        bool ButtonReleased(eInputButtons i_Button);

        #endregion

        // Gets the current stante-changes of buttons (in gamepad and mouse)
        #region keyboardKeyChanges

        bool KeyPressed(Keys i_Key);

        bool KeyReleased(Keys i_Key);

        bool KeyHeld(Keys i_Key);

        #endregion

        // Gets changes in various input device states
        #region Deltas

        Vector2 MousePositionDelta
        {
            get;
        }

        int ScroolWeelDelta
        {
            get;
        }

        #endregion
    }

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
