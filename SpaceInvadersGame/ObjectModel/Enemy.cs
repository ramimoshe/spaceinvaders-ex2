using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvadersGame.Interfaces;


namespace SpaceInvadersGame.ObjectModel
{    
    // A delegate for an event that states that an enemy reached the screen bounds
    public delegate void SpriteReachedScreenBoundsDelegate(Sprite i_Sprite);    

    public abstract class Enemy : CollidableSprite, IShootable, IScorable
    {
        public event SpriteReachedScreenBoundsDelegate ReachedScreenBounds;

        private TimeSpan m_TimeBetweenMove = TimeSpan.FromSeconds(0.5f);
        protected TimeSpan m_TimeLeftToNextMove;        

        protected Vector2 m_CurrMotion = new Vector2(500, 0);

        private const int k_BulletVelocity = 200;

        private float m_EnemyMaxPositionYVal;

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
            m_TimeLeftToNextMove = m_TimeBetweenMove;            
        }

        public abstract int     Score
        {
            get;
        }

        public float    EnemyMaxPositionY
        {
            get
            {
                return m_EnemyMaxPositionYVal;
            }

            set
            {
                m_EnemyMaxPositionYVal = value;
            }
        }

        public TimeSpan     TimeBetweenMoves
        {
            get
            {
                return m_TimeBetweenMove;
            }

            set
            {
                m_TimeBetweenMove = value;
            }
        }
     
        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return ((!(i_OtherComponent is EnemyBullet)) &&
                      (base.CheckForCollision(i_OtherComponent)));
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

        // TODO remove the proc

        protected override void     InitPosition()
        {
            // Don't do nothing cause the initialization is within the ctor
        }

        public override void    Update(GameTime i_GameTime)
        {
            bool moveEnemy = false;

            // If the enemy was hit (changed to unvisible), we need to 
            // dispose the enemy
            if (Visible == false)
            {
                Dispose();
            }
            else
            {

                m_TimeLeftToNextMove -= i_GameTime.ElapsedGameTime;

                // Check if it passed enough time to move the enemy
                if (m_TimeLeftToNextMove.TotalSeconds < 0)
                {
                    MotionVector = m_CurrMotion;
                    m_TimeLeftToNextMove = m_TimeBetweenMove;

                    moveEnemy = true;
                }
                else
                {
                    MotionVector = new Vector2(0, MotionVector.Y);
                }

                base.Update(i_GameTime);

                // Because we move the enemy twice in a second we'll check
                // if we reached the screen bounds only if we moved the enemy
                if (moveEnemy)
                {
                    // TODO Remove the remarks

                    //Rectangle nextMoveBounds = Bounds;
                    //nextMoveBounds.X += (int)(MotionVector.X * i_GameTime.ElapsedGameTime.TotalSeconds);

                    moveEnemy = false;

                    if (Bounds.Left <= 0 || Bounds.Right >= Game.GraphicsDevice.Viewport.Width || 
                        Bounds.Bottom >= m_EnemyMaxPositionYVal)
                    {
                        OnReachedScreenBounds();
                    }
                }
            }
        }

        public void     SwitchPosition()
        {
            m_CurrMotion *= -1;         
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
    }
}
