using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// The delegate is used by the Bullet to notify that he colided
    /// with another component
    /// </summary>
    /// <param name="i_OtherComponent">The component the bullet colided with</param>
    /// <param name="i_Bullet">The current bullet that coliided with the component</param>
    public delegate void BulletCollisionDelegate(ICollidable i_OtherComponent, Bullet i_Bullet);

    /// <summary>
    /// A parent class for the bullet components in the game
    /// </summary>
    public abstract class Bullet : CollidableSprite
    {
        private const string k_AssetName = @"Sprites\Bullet";

        private Rectangle m_ViewPortBounds;

        public event BulletCollisionDelegate BulletCollision; 

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

            // TODO: Remove the proc (we should't dispose bullets)

            // If the bullet is out of the screen, or collided with another 
            // component (not visible), we need to dispose it
            if (!Bounds.Intersects(m_ViewPortBounds) || (!Visible && !(this is SpaceShipBullet)))            
            {                
                Dispose();             
            }
        }

        /// <summary>
        /// Raise a collision with a component event
        /// </summary>
        /// <param name="i_Enemy">The component that the bullet colided with</param>
        protected void  onBulletCollision(ICollidable i_OtherComponent)
        {
            if (BulletCollision != null)
            {
                BulletCollision(i_OtherComponent, this);
            }
        }    
    }
}
