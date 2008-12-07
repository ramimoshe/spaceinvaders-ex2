using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace SpaceInvadersGame.ObjectModel
{
    public class Barrier : CollidableSprite
    {
        private const string k_AssetName = @"Content\Sprites\Barrier_44x32";

        private const int k_XMotionSpeed = 100;

        private bool m_FirstUpdate = true;
        private float m_MaxXValue = 0;
        private float m_MinXValue = 0;

        List<int> m_CollidingPixels = new List<int>();
        private int m_CollidingPixel;

        public Barrier(Game i_Game)
            : base(k_AssetName, i_Game)
        {            
            MotionVector = new Vector2(k_XMotionSpeed, 0);
        }

        public override void    Update(GameTime i_GameTime)
        {
            // On the first update, we'll set the barrier maximum and
            // minimum bounds 
            if (m_FirstUpdate)
            {
                m_FirstUpdate = false;
                m_MinXValue = Bounds.Left - Texture.Width / 2;
                m_MaxXValue = Bounds.Right + Texture.Width / 2;
            }

            base.Update(i_GameTime);

            // If the barrier reached one of the allowed bounds, we'll switch
            // the movment direction
            if (Bounds.Left <= m_MinXValue || Bounds.Right >= m_MaxXValue)
            {
                MotionVector *= -1;
            }
        }

        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        // TODO: Move to CollidableSprite

        public bool     CheckPixelCollision(ICollidable i_OtherComponent)
        {
            int top = Math.Max(Bounds.Top, i_OtherComponent.Bounds.Top);
            int bottom = Math.Min(Bounds.Bottom, i_OtherComponent.Bounds.Bottom);
            int left = Math.Max(Bounds.Left, i_OtherComponent.Bounds.Left);
            int right = Math.Min(Bounds.Right, i_OtherComponent.Bounds.Right);           

            bool retVal = false;

            Color[] dataA = ColorData;
            Color[] dataB = i_OtherComponent.ColorData;

            m_CollidingPixels.Clear();
            m_CollidingPixel = -1;

            // TODO: Remove the remark

            /*Texture.GetData<Color>(dataA);
            i_OtherComponent.Texture.GetData<Color>(dataB);            */

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - Bounds.Left) +
                                         (y - Bounds.Top) * Bounds.Width];
                    Color colorB = dataB[(x - i_OtherComponent.Bounds.Left) +
                                         (y - i_OtherComponent.Bounds.Top) * i_OtherComponent.Bounds.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found

                        if (m_CollidingPixel == -1)
                        {
                            m_CollidingPixel = (x - Bounds.Left) +
                                         (y - Bounds.Top) * Bounds.Width;
                        }

                        m_CollidingPixels.Add((x - Bounds.Left) +
                                         (y - Bounds.Top) * Bounds.Width);

                        retVal = true;
                    }
                }
            }

            // No intersection found
            return retVal;
        }

        public override bool    CheckForCollision(ICollidable i_OtherComponent)
        {
            bool retVal = base.CheckForCollision(i_OtherComponent);
 	     
            if (retVal)
            {
                retVal = CheckPixelCollision(i_OtherComponent);
            }

            return retVal;
        }

        public override void Collided(ICollidable i_OtherComponent)
        {
            Color[] colors = ColorData;

            if (i_OtherComponent is Enemy)
            {
                foreach (int pixel in m_CollidingPixels)
                {
                    Vector4 color = colors[pixel].ToVector4();
                    color.W = 0;
                    colors[pixel] = new Color(color);
                }
            }
            else
            {

                bool finish = false;

                int pixelToTransperent = (int)(.75f * (i_OtherComponent.Texture.Width *
                                         i_OtherComponent.Texture.Height));
                int currPixel = m_CollidingPixel;
                int transperentDirection = -1 * (int)(i_OtherComponent.MotionVector.Y / Math.Abs(i_OtherComponent.MotionVector.Y));

                while (pixelToTransperent > 0 && !finish)
                {
                    int widthPixel = currPixel;
                    bool finishWidth = false;

                    while (widthPixel < currPixel + i_OtherComponent.Texture.Width && !finishWidth)
                    {
                        Vector4 color = colors[widthPixel].ToVector4();
                        color.W = 0;
                        colors[widthPixel] = new Color(color);

                        widthPixel += transperentDirection;
                        pixelToTransperent--;

                        if (widthPixel > colors.Length - 1 || widthPixel < 0)
                        {
                            finishWidth = true;
                        }
                    }

                    currPixel += Texture.Width * transperentDirection;

                    if (currPixel > colors.Length - 1 || currPixel < 0)
                    {
                        finish = true;
                    }
                }
            }
           // }

            
            ColorData = colors;
            Texture.SetData<Color>(colors);
        }        
    }
}
