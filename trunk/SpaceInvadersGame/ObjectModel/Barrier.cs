using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;
using SpaceInvadersGame.Interfaces;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// Represents the barrier game component
    /// </summary>
    public class Barrier : CollidableSprite, IDefend, ISoundableGameComponent
    {
        private const string k_AssetName = @"Content\Sprites\Barrier_44x32";
        private const int k_DefaultUpdateOrder = 1;
        private const int k_DefaultDrawOrder = 1;

        private const int k_XMotionSpeed = 100;
        private const float k_TransparentPercent = .75f;        

        private float m_MaxXValue = 0;
        private float m_MinXValue = 0;        

        private Color[] m_DefaultColorData;

        public event PlayActionSoundDelegate PlayActionSoundEvent;

        public Barrier(Game i_Game)
            : base( 
                k_AssetName, 
                i_Game, 
                k_DefaultUpdateOrder,
                k_DefaultDrawOrder)
        {            
            MotionVector = new Vector2(k_XMotionSpeed, 0);
            m_CollisionCheckType = eCollidableCheckType.PixelCollision;
            m_LoadFreshTextureCopy = true;
        }

        private Vector2 m_DefaultPosition;

        public Vector2 DefaultPosition
        {
            get { return m_DefaultPosition; }
            set { m_DefaultPosition = value; }
        }


        /// <summary>
        /// Move the barrier in the screen 
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            // If the barrier reached one of the allowed bounds, we'll switch
            // the movment direction
            if (Bounds.Left <= m_MinXValue || Bounds.Right >= m_MaxXValue)
            {
                Vector2 position = PositionOfOrigin;
                position.X = MathHelper.Clamp(position.X, m_MinXValue, m_MaxXValue - WidthAfterScale);
                PositionOfOrigin = position;
                MotionVector *= -1;
            }
        }

        /// <summary>
        /// Load the component data and saves the loaded texture pixel data
        /// </summary>
        protected override void     LoadContent()
        {
            base.LoadContent();

            Color[] colorData = this.ColorData;

            m_DefaultColorData = new Color[colorData.Length];
            Array.Copy(colorData, m_DefaultColorData, colorData.Length);            
        }

        /// <summary>
        /// Initialize the component by calculating the bounds it 
        /// can move between
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            CalcBarrierBounds();
        }

        /// <summary>
        /// Calculate the bounds that the barrier can move between
        /// </summary>
        public void    CalcBarrierBounds()
        {
            m_MinXValue = Bounds.Left - (Texture.Width / 2);
            m_MaxXValue = Bounds.Right + (Texture.Width / 2);
        }

        /// <summary>
        /// Perform the collision logic between the barrier and a given 
        /// component and than raise a PlayActionSound event
        /// </summary>
        /// <param name="i_OtherComponent">The coponent the barrier colided with</param>
        public override void    Collided(ICollidable i_OtherComponent)
        {
            base.Collided(i_OtherComponent);

            onPlayActionSound(eSoundActions.BarrierHit);
        }

        /// <summary>
        /// Implement the pixel collision between the current component and 
        /// a given component.
        /// in case it's an Enemy we'll call the base behaviour, otherwise 
        /// we'll transparent pixels in the colliding position according
        /// to the sent component texture size.
        /// </summary>
        /// <param name="i_OtherComponent">The component we collided with</param>
        /// <param name="i_CollidingPixels">The current component pixels
        /// that collides with the given component</param>
        /// <returns>An array of the new component texture colors</returns>
        protected override Color[]      PerformPixelCollision(
            ICollidable i_OtherComponent, 
            List<int> i_CollidingPixels)
        {
            Color[] retVal = null;
       
            if (i_OtherComponent is Enemy)
            {
                retVal = base.PerformPixelCollision(
                            i_OtherComponent,
                            i_CollidingPixels);
            }
            else if (i_CollidingPixels != null)
            {
                retVal = ColorData;

                int pixelToTransparentNum = (int)
                    (k_TransparentPercent * (i_OtherComponent.Texture.Width *
                     i_OtherComponent.Texture.Height));

                // Calculate the direction which we need to transparent the 
                // pixels accroding to the colliding component movement
                // direction
                int transparentDirection = (int)
                    (i_OtherComponent.MotionVector.Y / 
                     Math.Abs(i_OtherComponent.MotionVector.Y));

                i_CollidingPixels.Sort();
                int currPixel = i_CollidingPixels[0];

                bool finish = false;

                while (pixelToTransparentNum > 0 && !finish)
                {
                    int widthPixel = currPixel;
                    bool finishWidth = false;

                    // Calculate the last pixel we need to transpaernt in the
                    // current line
                    int finishPixel = currPixel + 
                        (i_OtherComponent.Texture.Width * transparentDirection);

                    // Calculate the last pixel that is in the current pixel 
                    // texture line
                    int boundPixel = currPixel + 
                            (transparentDirection * (currPixel % Texture.Width));

                    // If we're close to the bounds (left or right), we
                    // need to make sure that when will tansperent a barrier
                    // pixel we won't Accidentally reach the other side 
                    // (for example if the component collided with a pixel 
                    // in the left side and decreasing/increasing the pixel 
                    // to transparent can reach a pixel in the right side)
                    if (transparentDirection < 0)
                    {
                        finishPixel = Math.Max(finishPixel, boundPixel);
                    }
                    else
                    {
                        finishPixel = Math.Min(finishPixel, boundPixel);
                    }

                    int trasparentPixels = 0;

                    while (pixelToTransparentNum > 0 &&
                           widthPixel != finishPixel && 
                           !finishWidth)
                    {
                        Vector4 color = retVal[widthPixel].ToVector4();
                        color.W = 0;
                        retVal[widthPixel] = new Color(color);

                        widthPixel += transparentDirection;
                                                
                        pixelToTransparentNum--;

                        finishWidth = widthPixel > retVal.Length - 1 || 
                                      widthPixel < 0;
                    }

                    // Even if we didn't transparent enough pixels in the 
                    // Texture width, we'll decrease the num of pixel
                    // needed to transparent so that we won't Transparent
                    // more pixels than needed from the texture height
                    if (trasparentPixels < i_OtherComponent.Texture.Width)
                    {
                        pixelToTransparentNum -=
                            i_OtherComponent.Texture.Width -
                            trasparentPixels;
                    }

                    currPixel += Texture.Width * transparentDirection;

                    finish = currPixel > retVal.Length - 1 || 
                             currPixel < 0;
                }                
            }

            return retVal;
        }

        /// <summary>
        /// Return the barrier to the starting state
        /// </summary>
        public void     ResetBarrier()
        {
            if (m_DefaultColorData != null)
            {
                this.Texture.SetData<Color>(m_DefaultColorData);
                this.PositionForDraw = DefaultPosition;               
            }
        }

        /// <summary>
        /// Raise a PlayActionSoundEvent
        /// </summary>
        /// <param name="i_Action">The action that cause the event</param>
        private void onPlayActionSound(eSoundActions i_Action)
        {
            if (PlayActionSoundEvent != null)
            {
                PlayActionSoundEvent(i_Action);
            }
        }
    }
}
