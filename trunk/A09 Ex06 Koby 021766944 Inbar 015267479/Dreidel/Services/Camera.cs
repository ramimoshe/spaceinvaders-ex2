using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreidelGame.Services
{
    public class Camera : GameComponent
    {
        private bool m_ShouldUpdateViewMatrix = true;
        FormCameraProperties m_FormCameraProperties = new FormCameraProperties();

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

        public Camera(Game i_Game)
            : base(i_Game)
        {
            m_FormCameraProperties.BoundObject = this;
            m_FormCameraProperties.Show();
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

        public Vector3 Up
        {
            get
            {
                return Vector3.Transform(Vector3.UnitY, RotationQuaternion);
            }
        }

        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(Vector3.Forward, m_RotationQuaternion);
            }
        }

        public Vector3 Right
        {
            get
            {
                return Vector3.Transform(Vector3.Right, m_RotationQuaternion);
            }
        }

        static readonly float sr_RotationSpeed = MathHelper.ToRadians(0.5f);

        public void UpdateByInput()
        {
            InputManager input = (InputManager) Game.Services.GetService(typeof(InputManager));

            if (input.KeyHeld(Keys.S))
            {
                if (input.ButtonHeld(eInputButtons.Left))
                {
                    Vector3 positionDelta = new Vector3(
                        input.MousePositionDelta.X,
                        -input.MousePositionDelta.Y,
                        0);
                    m_Position += Vector3.Transform(positionDelta, RotationQuaternion);
                    ShouldUpdateViewMatrix = true;
                }
                else if (input.ButtonHeld(eInputButtons.Middle))
                {
                    Vector3 positionDelta = new Vector3(
                        0,
                        0,
                        input.MousePositionDelta.Y);
                    m_Position += Vector3.Transform(positionDelta, RotationQuaternion);
                    ShouldUpdateViewMatrix = true;
                }
                else if (input.ButtonHeld(eInputButtons.Right))
                {
                    m_Rotations.X += input.MousePositionDelta.X * (float) Math.Sin(MathHelper.TwoPi);
                    ShouldUpdateViewMatrix = true;
                }
            }

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
                m_Rotations.Y = sr_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }
            // rotate right:
            else if (keyboardState.IsKeyDown(Keys.NumPad6))
            {
                m_Rotations.Y = -sr_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }

            // rotate Up:
            if (keyboardState.IsKeyDown(Keys.NumPad8))
            {
                m_Rotations.X = sr_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }
            // rotate Down:
            else if (keyboardState.IsKeyDown(Keys.NumPad2))
            {
                m_Rotations.X = -sr_RotationSpeed;
                ShouldUpdateViewMatrix = true;
            }

            if (keyboardState.IsKeyDown(Keys.R))
            {
                Position = new Vector3(0, 0, 20);
                RotationQuaternion = Quaternion.Identity;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateByInput();
        }
    }
}
