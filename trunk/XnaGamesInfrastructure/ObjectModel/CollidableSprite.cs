using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// Class implements a sprite capable of participating in collision managemnt
    /// </summary>
    public class CollidableSprite : Sprite, ICollidable
    {
        /// <summary>
        /// The constructor intiates the base constructor 
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        public  CollidableSprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game)
        {
        }

        /// <summary>
        /// Calls Base constructor
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        /// <param name="i_UpdateOrder">Number defining the order in which update 
        /// of all game components is called</param>
        /// <param name="i_DrawOrder">Number defining the order in which draw
        /// of all game components is called</param>
        public  CollidableSprite(
                string i_AssetName,
                Game i_Game,
                int i_UpdateOrder,
                int i_DrawOrder)
                : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
            Dying = false;
        }

        /// <summary>
        /// Collision manager (handling collision)
        /// </summary>
        protected ICollisionManager m_CollisionManager;

        /// <summary>
        /// State the type of collision check we need to perform on the 
        /// </summary>
        protected eCollidableCheckType m_CollisionCheckType;

        /// <summary>
        /// Marks if the current component was hit
        /// </summary>
        protected bool m_WasComponentHit;

        /// <summary>
        /// Property that states the collision check that the component 
        /// needs. 
        /// By default the check is RectangleCollision.
        /// </summary>
        public eCollidableCheckType     CollisionCheckType
        {
            get { return m_CollisionCheckType; }
        }

        /// <summary>
        /// Read only property that marks if the object is in a dying state
        /// </summary>
        public bool     Dying
        {
            get
            {
                return m_WasComponentHit;
            }

            protected set
            {
                m_WasComponentHit = value;
            }
        }

        /// <summary>
        /// Initialize collision manager, and registers itself as an ICollidable
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_CollisionManager = Game.Services.GetService(typeof(ICollisionManager)) as ICollisionManager;

            if (m_CollisionManager != null)
            {
                m_CollisionManager.AddObjectToMonitor(this);
            }
        }

        /// <summary>
        /// Raises position-changed event when position was changed
        /// </summary>
        /// <param name="gameTime">Elapsed time since last call</param>
        public override void    Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_MotionVector != Vector2.Zero)
            {
                OnPositionChanged();
            }
        }

        /// <summary>
        /// Check if the current components pixels collides with another
        /// component pixels
        /// </summary>
        /// <param name="i_OtherComponent">The other component we want to 
        /// check for collision against</param>        
        /// <param name="io_CollidablePixels">A list that will be filled with
        /// the collided pixels. in case the list is null we simply perform
        /// a check, otherwise we'll clear the list and return all the 
        /// colliding pixels</param>
        /// <returns>indication whether the components pixels collides</returns>
        protected bool  CheckForPixelCollision(
                ICollidable i_OtherComponent, 
                ref List<int> io_CollidablePixels)
        {
            int top = Math.Max(Bounds.Top, i_OtherComponent.Bounds.Top);
            int bottom = Math.Min(Bounds.Bottom, i_OtherComponent.Bounds.Bottom);
            int left = Math.Max(Bounds.Left, i_OtherComponent.Bounds.Left);
            int right = Math.Min(Bounds.Right, i_OtherComponent.Bounds.Right);

            bool retVal = false;

            Color[] dataA = ColorData;
            Color[] dataB = i_OtherComponent.ColorData;

            if (io_CollidablePixels != null)
            {
                io_CollidablePixels.Clear();
            }

            // Check every point within the intersection bounds
            for (int y = top; 
                 y < bottom && (!retVal || io_CollidablePixels != null); 
                 y++)
            {
                for (int x = left;
                     x < right && (!retVal || io_CollidablePixels != null); 
                     x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - Bounds.Left) +
                                         ((y - Bounds.Top) * Bounds.Width)];
                    Color colorB = dataB[(x - i_OtherComponent.Bounds.Left) +
                                         ((y - i_OtherComponent.Bounds.Top) * 
                                           i_OtherComponent.Bounds.Width)];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        if (io_CollidablePixels != null)
                        {
                            io_CollidablePixels.Add((x - Bounds.Left) +
                                         ((y - Bounds.Top) * Bounds.Width));
                        }

                        // An intersection has been found
                        retVal = true;
                    }
                }
            }

            return retVal;
        }

        #region ICollidable Members

        /// <summary>
        /// Check if a given component colides with the current one
        /// </summary>
        /// <param name="i_OtherComponent">The component that we want to check 
        /// collision against</param>
        /// <returns>true if the given component colides the current one or false 
        /// otherwise</returns>
        public virtual bool     CheckForCollision(ICollidable i_OtherComponent)
        {
            bool retVal = this.Bounds.Intersects(i_OtherComponent.Bounds);

            // In case the components bounds collides and one of the 
            // components need a pixel collision check, we'll perform a check
            if (retVal && 
                (CollisionCheckType == eCollidableCheckType.PixelCollision ||
                 i_OtherComponent.CollisionCheckType == eCollidableCheckType.PixelCollision))
            {
                // Make sure that we simply perform a check and won't search
                // for the colliding pixels
                List<int> pixels = null;
                retVal = CheckForPixelCollision(i_OtherComponent, ref pixels);
            }

            return retVal;
        }

        /// <summary>
        /// Default behaviour in case of collision - hides the sprite
        /// </summary>
        /// <param name="i_OtherComponent"></param>
        public virtual void     Collided(ICollidable i_OtherComponent)
        {
            if (CollisionCheckType == eCollidableCheckType.PixelCollision)
            {
                List<int> pixels = new List<int>();
                CheckForPixelCollision(i_OtherComponent, ref pixels);

                PerformPixelCollision(i_OtherComponent, pixels);
                Texture.SetData<Color>(ColorData);
            }
            else if (CollisionCheckType == 
                eCollidableCheckType.RectangleCollision)
            {
                this.Visible = false;
            }
        }

        /// <summary>
        /// Implement the pixel collision between the current component and 
        /// a given component
        /// </summary>
        /// <param name="i_OtherComponent">The component we collided with</param>
        /// <param name="i_CollidingPixels">The current component pixels
        /// that collides with the given component</param>
        /// <returns>An array of the new component texture colors</returns>
        protected virtual Color[]      PerformPixelCollision(
            ICollidable i_OtherComponent,
            List<int> i_CollidingPixels)
        {
            Color[] colors = ColorData;

            if (i_CollidingPixels != null)
            {
                foreach (int pixel in i_CollidingPixels)
                {
                    Vector4 color = colors[pixel].ToVector4();
                    color.W = 0;
                    colors[pixel] = new Color(color);
                }
            }

            return colors;            
        }

        /// <summary>
        /// Raised when sprite's position is modified in current update
        /// </summary>
        public event PositionChangedDelegate PositionChanged;
        
        /// <summary>
        /// Raises PositionChanged event if there are registered observers.
        /// </summary>
        protected virtual void  OnPositionChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this);
            }
        }        

        #endregion       
    }
}
