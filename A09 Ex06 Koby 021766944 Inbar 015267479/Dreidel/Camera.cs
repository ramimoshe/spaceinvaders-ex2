using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreidelGame
{
    public class Camera
    {
        private bool m_ShouldUpdateViewMatrix = true;

        // TODO: Remove all the form references

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

        public Camera()
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

        // we are standing 20 units in front of our target:
        protected Vector3 m_Position = new Vector3(1, 1, 20);

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

        static readonly float sr_OneRadian = MathHelper.ToRadians(1);

        public void UpdateByInput()
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
        }        
    }
}
