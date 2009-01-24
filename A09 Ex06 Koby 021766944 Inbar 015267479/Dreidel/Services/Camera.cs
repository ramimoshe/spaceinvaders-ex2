using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreidelGame.Services
{
    // TODO: Create a Service class that automatically register the service

    // TODO: Check if it should be a DrawableGameComponent

    public class Camera : DrawableGameComponent
    {
        private const int k_DefaultPointOfViewZFactor = 250;

        private readonly Keys r_CameraMovementActivationKey = Keys.S;
        private readonly Vector3 r_DefaultPosition = new Vector3(0, 0, k_DefaultPointOfViewZFactor);
        private const float k_ZMoveSpeed = 150f;
        private const float k_XYMoveSpeed = 3f;

        private MouseState m_PrevMouseState;

        private bool m_ShouldUpdateViewMatrix = true;

        // TODO: Remove all the form references

        FormCameraProperties m_FormCameraProperties = new FormCameraProperties();
        InputManager m_InputManager;

        protected bool m_Field;

        public bool ShouldUpdateViewMatrix
        {
            get { return m_ShouldUpdateViewMatrix; }
            set
            {
                m_ShouldUpdateViewMatrix = value;
                m_FormCameraProperties.RefreshGrid();
            }
        }

        public Camera(Game i_Game) : base(i_Game)
        {
            m_FormCameraProperties.BoundObject = Mouse.GetState();
            m_FormCameraProperties.Show();            

            m_Position = r_DefaultPosition;
        }

        protected Matrix m_ViewMatrix;

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

        // TODO: Change the parameters to constants

        protected Vector3 m_Position = new Vector3(0, 0, k_DefaultPointOfViewZFactor);

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

        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(Vector3.Forward, m_RotationMatrix);
            }
        }

        public Vector3 Right
        {
            get
            {
                return Vector3.Transform(Vector3.Right, m_RotationMatrix);
            }
        }
        public Vector3 Up
        {
            get
            {
                return Vector3.Transform(Vector3.Up, m_RotationMatrix);
            }
        }

        public override void  Initialize()
        {
            base.Initialize();
            
            m_InputManager = Game.Services.GetService(typeof(InputManager)) as InputManager;
        }
        
        static readonly float sr_OneRadian = MathHelper.ToRadians(1);

        protected override void     LoadContent()
        {
            base.LoadContent();

            Mouse.SetPosition(
                GraphicsDevice.Viewport.Width / 2, 
                GraphicsDevice.Viewport.Height / 2);

            m_PrevMouseState = Mouse.GetState();
        }

        // TODO: Remove the method

        /// <summary>
        /// Updates the Camera according to the mouse position
        /// </summary>
        /// <param name="i_GameTime">a snapshot of the game time</param>
        public void UpdateNoSet(GameTime i_GameTime)
        {
            base.Update(i_GameTime);           

            if (m_InputManager.KeyboardState.IsKeyDown(r_CameraMovementActivationKey))
            {
                float deltaTime = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

                if (m_InputManager.MouseState.MiddleButton == ButtonState.Pressed)
                {
                    Vector3 moveVector = Vector3.Zero;

                    if (m_InputManager.MousePositionDelta.X < 0 ||
                        m_InputManager.MousePositionDelta.Y < 0)
                    {
                        /*m_Position -=                             
                            Vector3.Transform(
                                Vector3.UnitZ * k_MoveSpeed * deltaTime,                                 
                                RotationMatrix);
                        ShouldUpdateViewMatrix = true;*/

                        moveVector.Z -= k_ZMoveSpeed * deltaTime;

                    }
                    else if (m_InputManager.MousePositionDelta.X > 0 ||
                             m_InputManager.MousePositionDelta.Y > 0)
                    {
                        /*m_Position += 
                            k_MoveSpeed * 
                            Vector3.Transform(Vector3.UnitZ, RotationMatrix);

                        m_Position += 
                            Vector3.Transform(
                                Vector3.UnitZ * k_MoveSpeed * deltaTime,
                                RotationMatrix);

                        ShouldUpdateViewMatrix = true;*/

                        moveVector.Z += k_ZMoveSpeed * deltaTime;
                    }

                    if (moveVector != Vector3.Zero)
                    {
                        m_Position += Vector3.Transform(moveVector, RotationMatrix);
                        ShouldUpdateViewMatrix = true;
                    }
                }
                else if (m_InputManager.MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (m_InputManager.MousePositionDelta != Vector2.Zero)
                    {
                        if (m_InputManager.MousePositionDelta.X < 0)
                        {
                            m_Position -= 
                                k_XYMoveSpeed * 
                                Vector3.Transform(Vector3.UnitX, RotationMatrix);

                            ShouldUpdateViewMatrix = true;
                        }
                        else if (m_InputManager.MousePositionDelta.X > 0)
                        {
                            m_Position +=
                                k_XYMoveSpeed * 
                                Vector3.Transform(Vector3.UnitX, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }

                        if (m_InputManager.MousePositionDelta.Y > 0)
                        {
                            m_Position -=
                                k_XYMoveSpeed *                                 
                                Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }
                        else if (m_InputManager.MousePositionDelta.Y < 0)
                        {
                            m_Position +=
                                k_XYMoveSpeed * 
                                Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }
                    }
                }
                else if (m_InputManager.MouseState.RightButton == ButtonState.Pressed)
                {
                    if (m_InputManager.MousePositionDelta != Vector2.Zero)
                    {
                        if (m_InputManager.MousePositionDelta.X < 0)
                        {
                            m_Position += 
                                Vector3.Transform(
                                    Vector3.UnitX * deltaTime * k_XYMoveSpeed, 
                                    RotationMatrix);

                            Yaw += sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }
                        else if (m_InputManager.MousePositionDelta.X > 0)
                        {
                            //m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                            Yaw += sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }

                        if (m_InputManager.MousePositionDelta.Y < 0)
                        {
                            m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            Pitch += sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }
                        else if (m_InputManager.MousePositionDelta.Y > 0)
                        {
                            m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            Pitch -= sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }
                    }
                }
            }            

            // forward:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Down))
            {
                m_Position += Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // backwords:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Up))
            {
                m_Position -= Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // left:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                m_Position -= Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // right:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // up:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.PageUp))
            {
                m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // down:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.PageDown))
            {
                m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // rotate left:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad4))
            {
                Yaw += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate right:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad6))
            {
                Yaw -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }

            // rotate Up:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad8))
            {
                Pitch += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate Down:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad2))
            {
                Pitch -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }

            if (m_InputManager.KeyboardState.IsKeyDown(Keys.R))
            {
                Position = r_DefaultPosition;
                Rotations = Vector3.Zero;
            }
        }

        /// <summary>
        /// Updates the Camera according to the mouse position
        /// </summary>
        /// <param name="i_GameTime">a snapshot of the game time</param>
        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (m_InputManager.KeyboardState.IsKeyDown(r_CameraMovementActivationKey))
            {
                float deltaTime = (float)i_GameTime.ElapsedGameTime.TotalSeconds;

                float xDifference = Mouse.GetState().X - m_PrevMouseState.X;
                float yDifference = Mouse.GetState().Y - m_PrevMouseState.Y;

                Console.WriteLine(xDifference + " " + yDifference);

                if (m_InputManager.MouseState.MiddleButton == ButtonState.Pressed)
                {
                    Vector3 moveVector = Vector3.Zero;

                    if (xDifference < 0 ||
                        yDifference < 0)
                    {
                        /*m_Position -=                             
                            Vector3.Transform(
                                Vector3.UnitZ * k_MoveSpeed * deltaTime,                                 
                                RotationMatrix);
                        ShouldUpdateViewMatrix = true;*/

                        moveVector.Z -= k_ZMoveSpeed * deltaTime;

                    }
                    else if (xDifference > 0 ||
                             yDifference > 0)
                    {
                        /*m_Position += 
                            k_MoveSpeed * 
                            Vector3.Transform(Vector3.UnitZ, RotationMatrix);

                        m_Position += 
                            Vector3.Transform(
                                Vector3.UnitZ * k_MoveSpeed * deltaTime,
                                RotationMatrix);

                        ShouldUpdateViewMatrix = true;*/

                        moveVector.Z += k_ZMoveSpeed * deltaTime;
                    }

                    if (moveVector != Vector3.Zero)
                    {
                        m_Position += Vector3.Transform(moveVector, RotationMatrix);
                        ShouldUpdateViewMatrix = true;
                    }
                }
                else if (m_InputManager.MouseState.LeftButton == ButtonState.Pressed)
                {
                    if (m_InputManager.MousePositionDelta != Vector2.Zero)
                    {
                        if (xDifference < 0)
                        {
                            m_Position -=
                                k_XYMoveSpeed *
                                Vector3.Transform(Vector3.UnitX, RotationMatrix);

                            ShouldUpdateViewMatrix = true;
                        }
                        else if (xDifference > 0)
                        {
                            m_Position +=
                                k_XYMoveSpeed *
                                Vector3.Transform(Vector3.UnitX, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }

                        if (yDifference > 0)
                        {
                            m_Position -=
                                k_XYMoveSpeed *
                                Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }
                        else if (yDifference < 0)
                        {
                            m_Position +=
                                k_XYMoveSpeed *
                                Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }
                    }
                }
                else if (m_InputManager.MouseState.RightButton == ButtonState.Pressed)
                {
                    if (m_InputManager.MousePositionDelta != Vector2.Zero)
                    {
                        if (xDifference < 0)
                        {
                            m_Position +=
                                Vector3.Transform(
                                    Vector3.UnitX * deltaTime * k_XYMoveSpeed,
                                    RotationMatrix);

                            Yaw += sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }
                        else if (xDifference > 0)
                        {
                            //m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                            Yaw += sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }

                        if (yDifference < 0)
                        {
                            m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            Pitch += sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }
                        else if (yDifference > 0)
                        {
                            m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                            Pitch -= sr_OneRadian;
                            ShouldUpdateViewMatrix = true;
                        }
                    }
                }

                Mouse.SetPosition(
                    GraphicsDevice.Viewport.Width / 2,
                    GraphicsDevice.Viewport.Height / 2);
            }

            // forward:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Down))
            {
                m_Position += Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // backwords:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Up))
            {
                m_Position -= Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // left:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                m_Position -= Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // right:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // up:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.PageUp))
            {
                m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // down:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.PageDown))
            {
                m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // rotate left:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad4))
            {
                Yaw += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate right:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad6))
            {
                Yaw -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }

            // rotate Up:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad8))
            {
                Pitch += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate Down:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad2))
            {
                Pitch -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }

            if (m_InputManager.KeyboardState.IsKeyDown(Keys.R))
            {
                Position = r_DefaultPosition;
                Rotations = Vector3.Zero;
            }            
        }

       /* public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            MouseState currState = Mouse.GetState();

            if (currState.X != m_PrevMouseState.X || currState.Y != m_PrevMouseState.Y)
            {
                float xDifference = currState.X - m_PrevMouseState.X;
                float yDifference = currState.Y - m_PrevMouseState.Y;

                if (m_InputManager.KeyboardState.IsKeyDown(r_CameraMovementActivationKey) &&
                    (m_InputManager.MouseState.MiddleButton == ButtonState.Pressed ||
                     m_InputManager.MouseState.LeftButton == ButtonState.Pressed ||
                     m_InputManager.MouseState.RightButton == ButtonState.Pressed))
                {                    
                    if (m_InputManager.MouseState.MiddleButton == ButtonState.Pressed)
                    {
                        if (xDifference < 0 ||
                            yDifference < 0)
                        {
                            m_Position -= Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }
                        else if (xDifference > 0 ||
                                 yDifference > 0)
                        {
                            m_Position += Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                            ShouldUpdateViewMatrix = true;
                        }
                    }
                    else if (m_InputManager.MouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (m_InputManager.MousePositionDelta != Vector2.Zero)
                        {
                            if (xDifference < 0)
                            {
                                m_Position -= Vector3.Transform(Vector3.UnitX, RotationMatrix);

                                ShouldUpdateViewMatrix = true;
                            }
                            else if (xDifference > 0)
                            {
                                m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                                ShouldUpdateViewMatrix = true;
                            }

                            if (yDifference < 0)
                            {
                                m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                                ShouldUpdateViewMatrix = true;
                            }
                            else if (yDifference > 0)
                            {
                                m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                                ShouldUpdateViewMatrix = true;
                            }
                        }
                    }
                    else if (m_InputManager.MouseState.RightButton == ButtonState.Pressed)
                    {
                        if (m_InputManager.MousePositionDelta != Vector2.Zero)
                        {
                            if (xDifference < 0)
                            {
                                //m_Position -= Vector3.Transform(Vector3.UnitX, RotationMatrix);
                                Yaw -= sr_OneRadian;
                                ShouldUpdateViewMatrix = true;
                            }
                            else if (xDifference > 0)
                            {
                                //m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                                Yaw += sr_OneRadian;
                                ShouldUpdateViewMatrix = true;
                            }

                            if (yDifference < 0)
                            {
                                m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                                Pitch += sr_OneRadian;
                                ShouldUpdateViewMatrix = true;
                            }
                            else if (yDifference > 0)
                            {
                                m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                                Pitch -= sr_OneRadian;
                                ShouldUpdateViewMatrix = true;
                            }
                        }                                              
                    }
                }

                Mouse.SetPosition(
                            this.GraphicsDevice.Viewport.X / 2,
                            this.GraphicsDevice.Viewport.Y / 2);
            }

            // forward:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Down))
            {
                m_Position += Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // backwords:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Up))
            {
                m_Position -= Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // left:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                m_Position -= Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // right:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }         

            // up:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.PageUp))
            {
                m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // down:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.PageDown))
            {
                m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // rotate left:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad4))
            {
                Yaw += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate right:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad6))
            {
                Yaw -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }

            // rotate Up:
            if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad8))
            {
                Pitch += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate Down:
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.NumPad2))
            {
                Pitch -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }

            if (m_InputManager.KeyboardState.IsKeyDown(Keys.R))
            {
                Position = r_DefaultPosition;
                Rotations = Vector3.Zero;
            }
        }   */  

        // TODO: Remove

        /*public void UpdateByInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // forward:
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                m_Position += Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // backwords:
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                m_Position -= Vector3.Transform(Vector3.UnitZ, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            
            // left:
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                m_Position -= Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // right:
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                m_Position += Vector3.Transform(Vector3.UnitX, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // up:
            if (keyboardState.IsKeyDown(Keys.PageUp))
            {
                m_Position += Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }
            // down:
            else if (keyboardState.IsKeyDown(Keys.PageDown))
            {
                m_Position -= Vector3.Transform(Vector3.UnitY, RotationMatrix);
                ShouldUpdateViewMatrix = true;
            }

            // rotate left:
            if (keyboardState.IsKeyDown(Keys.NumPad4))
            {
                Yaw += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate right:
            else if (keyboardState.IsKeyDown(Keys.NumPad6))
            {
                Yaw -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            
            // rotate Up:
            if (keyboardState.IsKeyDown(Keys.NumPad8))
            {
                Pitch += sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }
            // rotate Down:
            else if (keyboardState.IsKeyDown(Keys.NumPad2))
            {
                Pitch -= sr_OneRadian;
                ShouldUpdateViewMatrix = true;
            }

            if (keyboardState.IsKeyDown(Keys.R))
            {
                Position = new Vector3(0, 0, 20);
                Rotations = Vector3.Zero;
            }
        }        */
    }
}
