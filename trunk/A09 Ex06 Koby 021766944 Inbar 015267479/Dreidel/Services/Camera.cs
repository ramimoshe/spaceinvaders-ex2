using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreidelGame.Services
{
    /// <summary>
    /// The games camera
    /// </summary>
    public class Camera : DrawableGameComponent
    {
        private const int k_DefaultPointOfViewZFactor = 500;

        private static readonly float sr_OneRadian = MathHelper.ToRadians(1);
        private readonly Keys r_CameraMovementActivationKey = Keys.S;
        private readonly Vector3 r_DefaultPosition = new Vector3(0, 0, k_DefaultPointOfViewZFactor);
        private const float k_ZMoveSpeed = 150f;
        private const float k_XYMoveSpeed = 3f;
        
        // TODO: Remove

        private MouseState m_PrevMouseState;

        private bool m_ShouldUpdateViewMatrix = true;
        private InputManager m_InputManager;

        protected Matrix m_ViewMatrix;
        protected Vector3 m_Position = new Vector3(0, 0, k_DefaultPointOfViewZFactor);

        /// <summary>
        /// Gets/sets an indication wether a change was made to camera rotation/position 
        /// and we should update the cameras' view
        /// </summary>
        public bool ShouldUpdateViewMatrix
        {
            get { return m_ShouldUpdateViewMatrix; }
            set
            {
                m_ShouldUpdateViewMatrix = value;                
            }
        }        

        /// <summary>
        /// Gets/sets the cameras' current view matrix
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
        /// Gets/sets the position of the target that the camera is looking at
        /// </summary>
        public Vector3 TargetPosition
        {
            get
            {
                if (m_ShouldUpdateViewMatrix)
                {
                    m_TargetPosition = Vector3.Transform(Vector3.Forward, RotationMatrix);
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
        /// Gets/sets the camera's rotation values
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

        protected Matrix m_RotationMatrix;

        /// <summary>
        /// Gets/sets the cameras rotation matrix
        /// </summary>
        public Matrix RotationMatrix
        {
            get
            {
                if (m_ShouldUpdateViewMatrix)
                {
                    Matrix.CreateFromYawPitchRoll(m_Rotations.Y, m_Rotations.X, m_Rotations.Z, out m_RotationMatrix);
                }

                return m_RotationMatrix;
            }
            set
            {
                if (m_RotationMatrix != value)
                {
                    m_RotationMatrix = value;
                    ShouldUpdateViewMatrix = true;
                }
            }
        }        

        /// <summary>
        /// Gets/sets the current camera position
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
        /// Gets/sets the camera yaw value (rotation on the Y axis)
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
        /// Gets/sets the camera pitch value (rotation on the X axis)
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
        /// Gets/sets the camera roll value (rotation on the Z axis)
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
        /// Gets the Camera forward vector according to the current rotation matrix
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(Vector3.Forward, m_RotationMatrix);
            }
        }

        /// <summary>
        /// Gets the Camera right vector according to the current rotation matrix
        /// </summary>
        public Vector3 Right
        {
            get
            {
                return Vector3.Transform(Vector3.Right, m_RotationMatrix);
            }
        }

        /// <summary>
        /// Gets the Camera up vector according to the current rotation matrix
        /// </summary>
        public Vector3 Up
        {
            get
            {
                return Vector3.Transform(Vector3.Up, m_RotationMatrix);
            }
        }

        public Camera(Game i_Game) : base(i_Game)
        {
            m_Position = r_DefaultPosition;
        }        

        /// <summary>
        /// Initialize the InputManager variable
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();
            
            m_InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }                

        /// <summary>
        /// Initialize the Mouse position to be in the center of the screen
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            Mouse.SetPosition(
                GraphicsDevice.Viewport.Width / 2, 
                GraphicsDevice.Viewport.Height / 2);

            m_PrevMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Updates the Camera according to the mouse position
        /// </summary>
        /// <param name="i_GameTime">a snapshot of the game time</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (m_InputManager.KeyboardState.IsKeyDown(r_CameraMovementActivationKey))
            {
                float deltaTime = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

                // TODO: Remove remarks

                float xDifference = Mouse.GetState().X - m_PrevMouseState.X;
                float yDifference = Mouse.GetState().Y - m_PrevMouseState.Y;

                if (m_InputManager.MouseState.MiddleButton == ButtonState.Pressed ||
                    m_InputManager.MouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector3 moveVector = Vector3.Zero;

                    if (m_InputManager.MouseState.MiddleButton == ButtonState.Pressed)
                    {
                        moveVector = processScrollButton(deltaTime, xDifference, yDifference, moveVector);
                    }
                    else
                    {
                        moveVector = processLeftButton(xDifference, yDifference, moveVector);                   
                    }

                    if (moveVector != Vector3.Zero)
                    {
                        m_Position += Vector3.Transform(moveVector, RotationMatrix);
                        ShouldUpdateViewMatrix = true;
                    }
                }                                
            }

            Mouse.SetPosition(
                    Game.GraphicsDevice.Viewport.Width / 2,
                    Game.GraphicsDevice.Viewport.Height / 2);  
        }

        /// <summary>
        /// Calculate the camera movment for the left mouse button press
        /// </summary>
        /// <param name="i_XDifference">Units the mouse move on the X axis</param>
        /// <param name="i_YDifference">Units the mouse move on the Y axis</param>
        /// <param name="i_MoveVector">A zero Vector3</param>
        /// <returns>Vector3 representing the units that we need to change the cameras position</returns>
        private Vector3 processLeftButton(
            float i_XDifference, 
            float i_YDifference, 
            Vector3 i_MoveVector)
        {
            if (i_XDifference < 0)
            {
                i_MoveVector.X -= k_XYMoveSpeed;
            }
            else if (i_XDifference > 0)
            {
                i_MoveVector.X += k_XYMoveSpeed;
            }

            if (i_YDifference > 0)
            {
                i_MoveVector.Y -= k_XYMoveSpeed;
            }
            else if (i_YDifference < 0)
            {
                i_MoveVector.Y += k_XYMoveSpeed;
            }
            return i_MoveVector;
        }

        /// <summary>
        /// Calculate the camera movment for the scroll mouse button press
        /// </summary>
        /// <param name="i_DeltaTime">Time passed from game start</param>
        /// <param name="i_XDifference">Units the mouse move on the X axis</param>
        /// <param name="i_YDifference">Units the mouse move on the Y axis</param>
        /// <param name="i_MoveVector">A zero Vector3</param>
        /// <returns>Vector3 representing the units that we need to change the cameras position</returns>
        private Vector3 processScrollButton(
            float i_DeltaTime, 
            float i_XDifference, 
            float i_YDifference, 
            Vector3 i_MoveVector)
        {
            if (i_XDifference < 0 ||
                i_YDifference < 0)
            {
                i_MoveVector.Z -= k_ZMoveSpeed * i_DeltaTime;                
            }
            else if (i_XDifference > 0 ||
                     i_YDifference > 0)
            {
                i_MoveVector.Z += k_ZMoveSpeed * i_DeltaTime;                
            }

            return i_MoveVector;
        }
    }
}
