using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreidelGame.Services
{
    public class Camera : GameComponent
    {
        private readonly float r_RotationSpeed = MathHelper.ToRadians(0.5f);

        private bool m_ShouldUpdateViewMatrix = true;
        private MouseState m_PrevMouseState;
        private InputManager m_Input;

        /// <summary>
        /// Sets the default mouse state that the camera will use
        /// </summary>
        public MouseState DefaultMouseState
        {
            set { m_PrevMouseState = value; }
        }

        /// <summary>
        /// Gets/sets an indication whether we need to update the matrix view
        /// </summary>
        public bool ShouldUpdateViewMatrix
        {
            get { return m_ShouldUpdateViewMatrix; }
            set
            {
                m_ShouldUpdateViewMatrix = value;
            }
        }

        protected Matrix m_ViewMatrix;

        /// <summary>
        /// Gets the current camera matrix view according to the camera position, target position
        /// and the up vector
        /// </summary>
        public Matrix ViewMatrix
        {
            get
            {
                if (ShouldUpdateViewMatrix)
                {
                    m_ViewMatrix = Matrix.CreateLookAt(Position, TargetPosition, Up);
                    ShouldUpdateViewMatrix = false;
                }

                return m_ViewMatrix;

            }
        }

        protected Vector3 m_TargetPosition = Vector3.Zero;

        /// <summary>
        /// Gets/sets the target that the camera is looking at
        /// </summary>
        public Vector3 TargetPosition
        {
            get
            {
                if (m_ShouldUpdateViewMatrix)
                {
                    m_TargetPosition = Vector3.Transform(Vector3.Forward, RotationQuaternion);
                    m_TargetPosition = m_Position + m_TargetPosition;
                }

                return m_TargetPosition;

            }
            set
            {
                if (m_TargetPosition != value)
                {
                    m_TargetPosition = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }

        protected Vector3 m_Rotations = Vector3.Zero;

        /// <summary>
        /// Gets/sets the matrix rotation vector
        /// </summary>
        public Vector3 Rotations
        {
            get
            {
                return m_Rotations;
            }
            set
            {
                if (m_Rotations != value)
                {
                    m_Rotations = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }

        protected Quaternion m_RotationQuaternion = Quaternion.Identity;

        /// <summary>
        /// Gets/sets the camera's rotation Quaternion
        /// </summary>
        public Quaternion RotationQuaternion
        {
            get
            {
                if (m_ShouldUpdateViewMatrix)
                {
                    m_RotationQuaternion *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, m_Rotations.X);
                    m_RotationQuaternion *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, m_Rotations.Y);
                }

                return m_RotationQuaternion;
            }
            set
            {
                if (m_RotationQuaternion != value)
                {
                    m_RotationQuaternion = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }

        protected Vector3 m_Position = new Vector3(0, 0, 500);

        /// <summary>
        /// Gets/sets the camera position
        /// </summary>
        public Vector3 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets/sets the camera yaw (rotation on the Y axis)
        /// </summary>
        public float Yaw
        {
            get { return m_Rotations.Y; }
            set
            {
                if (m_Rotations.Y != value)
                {
                    m_Rotations.Y = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets/sets the camera pitch (rotation on the X axis)
        /// </summary>
        public float Pitch
        {
            get { return m_Rotations.X; }
            set
            {
                if (m_Rotations.X != value)
                {
                    m_Rotations.X = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets/sets the camera roll (rotation on the Z axis)
        /// </summary>
        public float Roll
        {
            get { return m_Rotations.Z; }
            set
            {
                if (m_Rotations.Z != value)
                {
                    m_Rotations.Z = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }

        /// <summary>
        /// Gets/sets the cameras up vector
        /// </summary>
        public Vector3 Up
        {
            get
            {
                return Vector3.Transform(Vector3.UnitY, RotationQuaternion);
            }
        }

        /// <summary>
        /// Gets/sets the cameras forward vector
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(Vector3.Forward, m_RotationQuaternion);
            }
        }

        /// <summary>
        /// Gets/sets the cameras right vector
        /// </summary>
        public Vector3 Right
        {
            get
            {
                return Vector3.Transform(Vector3.Right, m_RotationQuaternion);
            }
        }

        public Camera(Game i_Game)
            : base(i_Game)
        {
            m_PrevMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Initialize the input manager
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_Input = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }

        /// <summary>
        /// Updates the camera current position/rotation according to the current mouse position
        /// change from the last update
        /// </summary>
        private void     updateByInput()
        {           
            if (m_Input.KeyHeld(Keys.S))
            {
                float xDifference = Mouse.GetState().X - m_PrevMouseState.X;
                float yDifference = Mouse.GetState().Y - m_PrevMouseState.Y;

                Vector3 positionDelta = Vector3.Zero;

                if (m_Input.ButtonHeld(eInputButtons.Left))
                {
                    positionDelta = new Vector3(
                        xDifference,
                        -yDifference,
                        0);                    
                }
                else if (m_Input.ButtonHeld(eInputButtons.Middle))
                {
                    // Movement on the x axis or on the y axis should update the move value
                    if (xDifference != 0)
                    {
                        positionDelta = new Vector3(
                        0,
                        0,
                        xDifference);
                    }
                    else
                    {
                        positionDelta = new Vector3(
                            0,
                            0,
                            yDifference);
                    }
                }
                else if (m_Input.ButtonHeld(eInputButtons.Right))
                {
                    m_Rotations.X += xDifference * MathHelper.ToRadians((float)Math.Sin(MathHelper.TwoPi));
                    ShouldUpdateViewMatrix = true;
                }

                if (positionDelta != Vector3.Zero)
                {
                    m_Position += Vector3.Transform(positionDelta, RotationQuaternion);
                    ShouldUpdateViewMatrix = true;
                }
            }            

            // TODO: Remove

            KeyboardState keyboardState = Keyboard.GetState();

            // forward:
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                m_Position += Vector3.Transform(Vector3.UnitZ / 2, RotationQuaternion);
                ShouldUpdateViewMatrix = true;
            }
            // backwords:
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                m_Position -= Vector3.Transform(Vector3.UnitZ / 2, RotationQuaternion);
                ShouldUpdateViewMatrix = true;
            }

            // left:
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                m_Position -= Vector3.Transform(Vector3.UnitX / 2, RotationQuaternion);
                ShouldUpdateViewMatrix = true;
            }
            // right:
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                m_Position += Vector3.Transform(Vector3.UnitX / 2, RotationQuaternion);
                ShouldUpdateViewMatrix = true;
            }

            // up:
            if (keyboardState.IsKeyDown(Keys.PageUp))
            {
                m_Position += Vector3.Transform(Vector3.UnitY / 2, RotationQuaternion);
                ShouldUpdateViewMatrix = true;
            }
            // down:
            else if (keyboardState.IsKeyDown(Keys.PageDown))
            {
                m_Position -= Vector3.Transform(Vector3.UnitY / 2, RotationQuaternion);
                ShouldUpdateViewMatrix = true;
            }

            m_Rotations = Vector3.Zero;

            // rotate left:
            if (keyboardState.IsKeyDown(Keys.NumPad4))
            {
                m_Rotations.Y = r_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }
            // rotate right:
            else if (keyboardState.IsKeyDown(Keys.NumPad6))
            {
                m_Rotations.Y = -r_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }

            // rotate Up:
            if (keyboardState.IsKeyDown(Keys.NumPad8))
            {
                m_Rotations.X = r_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }
            // rotate Down:
            else if (keyboardState.IsKeyDown(Keys.NumPad2))
            {
                m_Rotations.X = -r_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }

            if (keyboardState.IsKeyDown(Keys.R))
            {
                Position = new Vector3(0, 0, 20);
                RotationQuaternion = Quaternion.Identity;
            }
        }

        /// <summary>
        /// Updates the camera position and view
        /// </summary>
        /// <param name="gameTime">A snapshot of the current game time</param>
        public override void    Update(GameTime gameTime)
        {
            base.Update(gameTime);

            updateByInput();

            Mouse.SetPosition(
                Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height / 2);
        }
    }
}
