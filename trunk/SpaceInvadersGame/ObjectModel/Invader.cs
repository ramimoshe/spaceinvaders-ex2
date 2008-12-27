using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvadersGame.Interfaces;
using SpaceInvadersGame.ObjectModel.Screens;
using XnaGamesInfrastructure.ObjectModel.Animations;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// An enumeration of all the invader types in the game
    /// </summary>
    public enum eInvadersType
    {
        YellowInvader,
        BlueInvader,
        PinkInvader
    }

    /// <summary>
    /// Used by Invader in order to inform that the he reached the 
    /// invaders allowed screen bounds
    /// </summary>
    /// <param name="i_Invader">The invader that reached the allowed screen bounds</param>
    public delegate void InvaderReachedScreenBoundsDelegate(Invader i_Invader);

    /// <summary>
    /// Used by Invader in order to inform that the he reached the 
    /// invaders allowed screen bounds
    /// </summary>
    /// <param name="i_Invader">The invader that reached the allowed screen bounds</param>
    public delegate void InvaderWasHitDelegate(Invader i_Invader);    

    // TODO: Change the class to inherit from composite component

    /// <summary>
    /// An abstract class that all the small invaders in the invaders matrix 
    /// inherits from
    /// </summary>
    public abstract class Invader : Enemy, IShoot, ISoundableGameComponent
    {
        private readonly TimeSpan r_DefaultTimeBetweenMoves = 
            TimeSpan.FromSeconds(0.5f);

        private const string k_AssetName = @"Sprites\allInvaders";
        private const int k_AllowedBulletsNum = 3;
        private readonly Vector2 r_DefaultMotionVector = new Vector2(500, 0);

        public event PlayActionSoundDelegate PlayActionSoundEvent;

        // Raised when an invader reaches one of the allowed screen bounderies
        public event InvaderReachedScreenBoundsDelegate ReachedScreenBounds;

        // Raised when an invader was hit by a player bullet
        public event InvaderWasHitDelegate InvaderWasHit;

        public event AddGameComponentDelegate ReleasedShot;

        private List<Bullet> m_Bullets = new List<Bullet>();
        private const int k_BulletVelocity = 200;
        private const int k_InvaderSizeWidth = 32;
        private const int k_InvaderSizeHeight = 32;
        private const int k_DefaultInvadersListNum = 1;
        private const int k_NumOfFrames = 2;

        private TimeSpan m_TimeBetweenMove;
        protected TimeSpan m_TimeLeftToNextMove;
        
        protected Vector2 m_CurrMotion = new Vector2(500, 0);

        private float m_EnemyMaxPositionYVal;
        private Vector2 m_DefaultPosition;

        // The current frame from the invader texture
        protected int m_StartingCel;

        public Invader(Game i_Game)
            : this(i_Game, 0, 0)
        {
        }

        public Invader(
            Game i_Game,
            int i_UpdateOrder)
            : this(i_Game, i_UpdateOrder, k_DefaultInvadersListNum)
        {
        }

        public Invader(
            Game i_Game, 
            int i_UpdateOrder,
            int i_InvaderStartFrame)
            : this(i_Game, i_UpdateOrder, 0, i_InvaderStartFrame)
        {
        }        

        public Invader(
            Game i_Game, 
            int i_UpdateOrder, 
            int i_DrawOrder,
            int i_InvaderStartFrame)
            : base(k_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
            m_TimeBetweenMove = r_DefaultTimeBetweenMoves;
            m_TimeLeftToNextMove = m_TimeBetweenMove;
            m_StartingCel = i_InvaderStartFrame % k_NumOfFrames;

            // TODO: Remove the code
            Game.Components.Remove(this);
        }

        /// <summary>
        /// Gets the invader starting position
        /// </summary>
        public Vector2      DefaultPosition
        {
            get { return m_DefaultPosition; }

            set { m_DefaultPosition = value; }
        }

        /// <summary>
        /// A property for the maximum value the enemy is allowed to reach
        /// on the Y axis
        /// </summary>
        public float    InvaderMaxPositionY
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

        /// <summary>
        /// A property for the time the enemy waits between two moves
        /// </summary>
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

        /// <summary>
        /// Read only property that returns the invader type
        /// </summary>
        public abstract eInvadersType   InvaderType
        {
            get;
        }

        /// <summary>
        /// Read only property that gets the invader hit action enum value
        /// </summary>
        protected abstract eSoundActions   HitAction
        {
            get;
        }        

        #region ICollidable Members

        /// <summary>
        /// Check for collision with a given component.        
        /// </summary>
        /// <param name="i_OtherComponent">the component we want to check for collision 
        /// against</param>        
        /// <returns>true in case the invader collides with the given component 
        /// or false in case the given component is an EnemyBullet or there is no collision
        /// between the components </returns>
        public override bool    CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is EnemyBullet) &&
                      base.CheckForCollision(i_OtherComponent);
        }

        /// <summary>
        /// Implement the component collision logic. 
        /// In case we collided with a defend component we do nothing,
        /// otherwise we call the base logic.
        /// </summary>
        /// <param name="i_OtherComponent">The colliding component</param>
        public override void    Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            if (!(i_OtherComponent is IDefend))
            {
                base.Collided(i_OtherComponent);

                onPlayActionSound(this.HitAction);
            }
        }

        #endregion

        #region IShootable Members

        /// <summary>
        /// Realse a shoot in the game
        /// </summary>
        public void     Shoot()
        {
            bool shot = false;

            if (m_Bullets.Count < k_AllowedBulletsNum)
            {
                Bullet bullet = new EnemyBullet(Game);
                onReleasedShot(bullet);
                
                bullet.TintColor = Color.Blue;
                bullet.PositionForDraw = new Vector2(
                                        PositionForDraw.X + (Bounds.Width / 2),
                                        PositionForDraw.Y - (bullet.Bounds.Height / 2));
                bullet.MotionVector = new Vector2(0, k_BulletVelocity);
                bullet.Disposed += new EventHandler(bullet_Disposed);

                m_Bullets.Add(bullet);

                shot = true;
            }
            else
            {
                // Search for an existing bullet that isn't active
                foreach (EnemyBullet bullet in m_Bullets)
                {
                    if (!bullet.Visible)
                    {
                        bullet.PositionForDraw = new Vector2(
                                    PositionForDraw.X + (Bounds.Width / 2),
                                    PositionForDraw.Y - (bullet.Bounds.Height / 2));
                        bullet.Visible = true;
                        shot = true;
                        break;                        
                    }
                }
            }

            // If the invader shot we'll raise a sound event
            if (shot)
            {
                onPlayActionSound(eSoundActions.EnemyShoot);
            }
        }

        #endregion

        /// <summary>
        /// Initialize the invader by creating the invader animation, and 
        /// setting he's bounds on the screen
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            CelAnimation cellAnimation = new CelAnimation(
                                    m_TimeBetweenMove,
                                    k_NumOfFrames,
                                    TimeSpan.Zero,
                                    m_StartingCel);

            Animations.Add(cellAnimation);
            Animations.Enabled = true;
            Animations[k_ScaleAnimationName].Enabled = false;
        }

        /// <summary>
        /// Catch a bullet disposed event and remove the bullet from the 
        /// bullets list.
        /// </summary>
        /// <param name="i_Sender">The bullet object that got disposed</param>
        /// <param name="i_Args">The event arguments</param>
        private void    bullet_Disposed(object i_Sender, EventArgs i_Args)
        {
            Bullet bullet = i_Sender as Bullet;

            if (bullet != null)
            {
                m_Bullets.Remove(bullet);
            }
        }        

        /// <summary>
        /// Move the invader in the screen in case enough time had passed 
        /// since last move
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        public override void    Update(GameTime i_GameTime)
        {
            bool moveEnemy = false;

            // TODO: Remove the dispose code

            // If the enemy was hit (changed to unvisible), we need to 
            // dispose it
        /*    if (Visible == false)
            {
                Dispose();
            }
            else
            {*/
                m_TimeLeftToNextMove -= i_GameTime.ElapsedGameTime;

                // Check if enough time had passed since previous move
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

                // If we moved the enemy this time we'll also check if the 
                // enemy is close to the screen bounds and raise an event
                if (moveEnemy)
                {
                    moveEnemy = false;

                    if ((Bounds.Left <= 0 || Bounds.Right >= Game.GraphicsDevice.Viewport.Width || 
                         Bounds.Bottom >= m_EnemyMaxPositionYVal) && Visible)
                    {
                        OnReachedScreenBounds();
                    }
                }
            // TODO: Remove the remark

            //}
        }

        /// <summary>
        /// Catch a AnimationFinished event and make the component invisible
        /// </summary>
        /// <param name="i_Animation">the animation that ended</param>
        protected override void    ScaleAnimation_Finished(SpriteAnimation i_Animation)
        {
            base.ScaleAnimation_Finished(i_Animation);
            OnInvaderWasHit();           
        }

        /// <summary>
        /// Initialize the invader default width and height, and also 
        /// initialize the invader frame position in the texture        
        /// </summary>
        protected override void     InitBounds()
        {
            m_WidthBeforeScale = k_InvaderSizeWidth;
            m_HeightBeforeScale = k_InvaderSizeHeight;

            SourcePosition = new Vector2(
                m_StartingCel * k_InvaderSizeWidth,
                SourcePosition.Y);

            InitSourceRectangle();
        }

        /// <summary>
        /// Initialize the component source rectangle in the components
        /// texture
        /// </summary>
        protected override void     InitSourceRectangle()
        {
            this.SourceRectangle = new Rectangle(
                (int)this.SourcePosition.X,
                (int)this.SourcePosition.Y,
                k_InvaderSizeWidth,
                k_InvaderSizeHeight);
        }

        /// <summary>
        /// Change the enemy moving direction, so that in the next move the 
        /// enemy will move to the other screen side on the X axis
        /// </summary>
        public void     SwitchPosition()
        {
            m_CurrMotion *= -1;         
        }

        /// <summary>
        /// Raising the ReachedScreenBounds event that marks that the enemy
        /// reached the allowed bounds in the screen
        /// </summary>
        protected void     OnReachedScreenBounds()
        {
            if (ReachedScreenBounds != null)
            {
                ReachedScreenBounds(this);
            }
        }

        /// <summary>
        /// Raising the InvaderWsasHit event that marks the invader was hit 
        /// by a players bullet
        /// </summary>
        protected void      OnInvaderWasHit()
        {
            if (InvaderWasHit != null)
            {
                InvaderWasHit(this);
            }
        }

        private void    onReleasedShot(Bullet i_Bullet)
        {
            if (ReleasedShot != null)
            {
                ReleasedShot(i_Bullet);
            }
        }

        /// <summary>
        /// Return the invader to the starting state
        /// </summary>
        public void     ResetInvader()
        {
            this.PositionForDraw = DefaultPosition;
            m_CurrMotion = r_DefaultMotionVector;
            m_TimeBetweenMove = r_DefaultTimeBetweenMoves;
            m_TimeLeftToNextMove = m_TimeBetweenMove;
            Animations[k_ScaleAnimationName].Reset();
            Animations[k_ScaleAnimationName].Pause();

            if (m_Bullets != null)
            {
                foreach (EnemyBullet bullet in m_Bullets)
                {
                    bullet.Visible = false;
                }
            }
        }

        /// <summary>
        /// Raise a PlayActionSoundEvent
        /// </summary>
        /// <param name="i_Action">The action we want to put in the raised
        /// event</param>
        private void    onPlayActionSound(eSoundActions i_Action)
        {
            if (PlayActionSoundEvent != null)
            {
                PlayActionSoundEvent(i_Action);
            }
        }
    }
}
