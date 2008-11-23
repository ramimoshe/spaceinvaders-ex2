using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using A09_Ex02_Koby_021766944_Inbar_015267479.ObjectModel;


namespace A09_Ex02_Koby_021766944_Inbar_015267479
{
    // A delegate for an event that states that an enemy reached the screen bounds
    public delegate void SpriteReachedScreenBoundsDelegate(Sprite i_Sprite);
    
    // A delegate for an event that states a enemy had been hit 
    public delegate void EnemyHitDelegate(Enemy i_Enemy);

    public enum eMovingDirection
    {
        Left,
        Right
    }

    public abstract class Enemy : Sprite, IShootable
    {
        public event SpriteReachedScreenBoundsDelegate ReachedScreenBounds;

        public event EnemyHitDelegate EnemyWasHit;

        private readonly TimeSpan r_MoveLength = TimeSpan.FromSeconds(0.5f);
        protected TimeSpan m_TimeLeftToNextMove;        
        protected int m_MovingDirection = 1;
        protected const int k_MotionXVal = 500;

        private const int k_BulletVelocity = 200;

        // TODO Remove the position from the CTOR

        public Enemy(string i_AssetName, Game i_Game, Vector2 i_Position)
            : this(i_AssetName, i_Game, 0, 0, i_Position)
        {

        }

        public Enemy(string i_AssetName, Game i_Game, Vector2 i_Position,
                     int i_UpdateOrder)
            : this(i_AssetName, i_Game, i_UpdateOrder, 0, i_Position)
        {

        }        

        public Enemy(string i_AssetName, Game i_Game, int i_UpdateOrder,
                     int i_DrawOrder, Vector2 i_Position)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {            
            Position = i_Position;
            m_TimeLeftToNextMove = r_MoveLength;            
        }

        // TODO remove the property and enums

        /// <summary>
        /// Property that sets the current enemy moving direction
        /// </summary>
        public eMovingDirection MovingDirection
        {
            set 
            { 
                if (value == eMovingDirection.Left)
                {
                    m_MovingDirection = -1;
                }
                else if (value == eMovingDirection.Right)
                {
                    m_MovingDirection = 1;
                }
            }
        }

        /// <summary>
        /// A property that holds the score that is added to the player,
        /// when he eliminates the current enemy
        /// </summary>
        public abstract int     Score
        {
            get;
        }

        public override bool CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return ((!(i_OtherComponent is EnemyBullet)) &&
                      (base.CheckForCollision(i_OtherComponent)));
        }                

        public override void    Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            base.Collided(i_OtherComponent);

            onEnemyWasHit();

            // TODO Check if i can dispose the enemy

            //Dispose();
        }

        #region IShootable Members        

        public void     Shoot()
        {
            Bullet bullet = new EnemyBullet(Game);
            bullet.Initialize();
            bullet.TintColor = Color.Blue;
            bullet.Position = new Vector2(Position.X + Bounds.Width / 2,
                                          Position.Y - bullet.Bounds.Height / 2);
            bullet.MotionVector = new Vector2(0, k_BulletVelocity);
        }

        #endregion


        protected override void     InitPosition()
        {
            // Don't do nothing cause the initialization is within the ctor
        }

        public override void    Update(GameTime i_GameTime)
        {
            bool moveEnemy = false;

            m_TimeLeftToNextMove -= i_GameTime.ElapsedGameTime;

            // Check if it passed enough time to move the enemy
            if (m_TimeLeftToNextMove.TotalSeconds < 0)
            {
                MotionVector = new Vector2(m_MovingDirection * k_MotionXVal, 
                                           MotionVector.Y);
                m_TimeLeftToNextMove = r_MoveLength;

                moveEnemy = true;
            }
            else
            {
                MotionVector = new Vector2(0, MotionVector.Y);
            }

            base.Update(i_GameTime);

            // Because we move the enemy twice un a minute we'll check
            // if we reached the screen bounds only if we moved the enemy
            if (moveEnemy)
            {
                moveEnemy = false;

                if (Bounds.Left <= 0 || Bounds.Right >= Game.GraphicsDevice.Viewport.Width)
                {
                    OnReachedScreenBounds();
                }        
            }
        }

        public void SwitchPosition()
        {
            this.m_MovingDirection *= -1;
        }

        /// <summary>
        /// Raising the ReachedScreenBounds event
        /// </summary>
        protected void     OnReachedScreenBounds()
        {
            if (ReachedScreenBounds != null)
            {
                ReachedScreenBounds(this);
            }
        }        

        /// <summary>
        /// Raise an EnemyWasHit event
        /// </summary>
        private void    onEnemyWasHit()
        {
            if (EnemyWasHit != null)
            {
                EnemyWasHit(this);
            }
        }
    }
}
