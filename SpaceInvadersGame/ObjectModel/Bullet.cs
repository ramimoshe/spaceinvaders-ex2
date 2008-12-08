using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.ObjectModel
{    
    /// <summary>
    /// A parent class for the bullet components in the game
    /// </summary>
    public abstract class Bullet : CollidableSprite
    {
        private const string k_AssetName = @"Content\Sprites\Bullet";
        private Rectangle m_ViewPortBounds;

        #region CTOR's
        public Bullet(Game i_Game) 
            : this(k_AssetName, i_Game, Int32.MaxValue, Int32.MaxValue)
        {
        }

        public Bullet(
            string i_AssetName, 
            Game i_Game, 
            int i_UpdateOrder, 
            int i_DrawOrder) 
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }
        #endregion

        /// <summary>
        /// Initialize the bullet data by saving the game screen bounds
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_ViewPortBounds = new Rectangle(
                0, 
                0,
                Game.GraphicsDevice.Viewport.Width,
                Game.GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// Updates the bullet state in the game
        /// </summary>
        /// <param name="i_GameTime">The game time passed from the previous 
        /// update call</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);            

            // If the bullet is out of the screen, or collided with another 
            // component (not visible), we need to dispose it
            if (!Bounds.Intersects(m_ViewPortBounds) || !Visible)            
            {                
                Dispose();             
            }
        }

        /// <summary>
        /// Check if the bullet colides with another component
        /// </summary>
        /// <param name="i_OtherComponent">The component we want to check the collision
        /// against</param>
        /// <returns>True if the components collides or false if the component is another bullet
        /// or that there is no collision between the components</returns>
        public override bool  CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is Bullet) &&
                    base.CheckForCollision(i_OtherComponent);
        }                
    }
}
