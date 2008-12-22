using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ServiceInterfaces;
using XnaGamesInfrastructure.Services;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaGamesInfrastructure.ObjectInterfaces;
using SpaceInvadersGame.Interfaces;
using SpaceInvadersGame.ObjectModel.Screens;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;
using XnaGamesInfrastructure.ObjectModel.Animations;

namespace SpaceInvadersGame.ObjectModel
{
    // TODO: Change the class to inherit from composite component

    /// <summary>
    /// The class represents the player's component in the game (the 
    /// SpaceShip)
    /// </summary>
    public class SpaceShip : CollidableSprite, IShoot, IPlayer
    {                
        private const int k_AllowedBulletsNum = 3;
        private const int k_Motion = 200;
        private const int k_BulletVelocity = 250;
        private const int k_LostLifeScoreDecrease = 2000;
        private const int k_LivesNum = 3;

        // The initialized position. needed so that we'll know where to position
        // the ship in case of an hit
        private Vector2 m_DefaultPosition;

        private int m_RemainingLivesLeft;

        private List<Bullet> m_Bullets;
        private IInputManager m_InputManager;

        private int m_PlayerNum;
        private int m_PlayerScore = 0;
        private IPlayerControls m_PlayerKeys;
        private bool m_WasDefaultPositionSet = false;

        // Raised when the player collides with a bullet and there is no more
        // lives left, or in case the ship collides with an invader
        public event PlayerIsDeadDelegate PlayerIsDead;

        public event PlayerScoreChangedDelegate PlayerScoreChangedEvent;

        public event PlayerWasHitDelegate PlayerWasHitEvent;

        public event AddGameComponentDelegate ReleasedShot;

        #region CTOR's     

        // TODO: Change the draw and update to constants

        public SpaceShip(
            Game i_Game, 
            string i_AssetName,
            IPlayerControls i_PlayerControls,
            int i_PlayerNum)
            : this(
            i_AssetName,
            i_Game,
            -1,
            0,
            i_PlayerControls,
            i_PlayerNum)
        {
        }

        public SpaceShip(
            string k_AssetName, 
            Game i_Game, 
            int i_UpdateOrder,
            int i_DrawOrder,
            IPlayerControls i_PlayerControls,
            int i_PlayerNum)
            : base(k_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
            m_Bullets = new List<Bullet>();
            m_RemainingLivesLeft = k_LivesNum;
            m_PlayerKeys = i_PlayerControls;
            PlayerNum = i_PlayerNum;
        }

        #endregion

        public int PlayerNum
        {
            get { return m_PlayerNum; }

            private set { m_PlayerNum = value;  }
        }

        /// <summary>
        /// The property holds the space ship starting position.
        /// </summary>
        public Vector2   DefaultPosition
        {
            get
            {
                return m_DefaultPosition;
            }

            set
            {
                m_DefaultPosition = value;
            }
        }

        /// <summary>
        /// A property to the player's current score
        /// </summary>
        public int  Score
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                if (value < 0)
                {
                    m_PlayerScore = 0;
                }
                else
                {
                    m_PlayerScore = value;
                }

                onPlayerScoreChanged();
            }
        }

        /// <summary>
        /// Property that gets\sets the remaining lives that the player has
        /// </summary>
        public int  RemainingLives
        {
            get
            {
                return m_RemainingLivesLeft;
            }

            set
            {
                m_RemainingLivesLeft = value;
            }
        }

        /// <summary>
        /// Read only property that gets the players texture
        /// </summary>
        public Texture2D PlayerTexture
        {
            get 
            {
                return Texture;
            }
        }

        /// <summary>
        /// Read only property that gets a bullet for shooting. 
        /// in case the players shot bullets number is as imposed by the
        /// maximum value, we'll find an invisible bullet and return it, 
        /// otherwise we'll create a new bullet.
        /// Icase there isn't an invisible bullet and the player already shot
        /// the allowed bullets number, we'll return null.
        /// </summary>
        /// <returns></returns>
        private SpaceShipBullet     Bullet
        {
            get
            {
                SpaceShipBullet retVal = null;

                if (m_Bullets.Count < k_AllowedBulletsNum)
                {
                    retVal = new SpaceShipBullet(this.Game);
                    
                    // TODO: Check if it's ok and remove the remarked code
                    onReleasedShot(retVal);
                    //retVal.Initialize();

                    retVal.TintColor = Color.Red;
                    retVal.PositionForDraw = new Vector2(
                                        PositionForDraw.X + (Bounds.Width / 2),
                                        PositionForDraw.Y - (retVal.Bounds.Height / 2));
                    retVal.MotionVector = new Vector2(0, -k_BulletVelocity);
                    retVal.BulletCollision += new BulletCollisionDelegate(spaceShipBullet_BulletCollision);
                    retVal.Disposed += new EventHandler(spaceShipBullet_Disposed);

                    m_Bullets.Add(retVal);                    
                }
                else
                {
                    // Search for an existing bullet that isn't active
                    foreach (SpaceShipBullet bullet in m_Bullets)
                    {
                        if (!bullet.Visible)
                        {
                            retVal = bullet;
                            retVal.PositionForDraw = new Vector2(
                                        PositionForDraw.X + (Bounds.Width / 2),
                                        PositionForDraw.Y - (bullet.Bounds.Height / 2));
                            retVal.Visible = true;
                            break;
                        }
                    }
                }

                return retVal;
            }
        }

        #region IShootable Members

        /// <summary>
        /// Release a SpaceShip shoot only if there are less than the number
        /// of allowed bullets and enough time had passed since last shoot
        /// </summary>
        public void     Shoot()
        {
            // TODO: Remove the remark

            // If we didn't reach the maximum bullets allowed, we'll create 
            // a new one and add it to the game components
           /* if (m_Bullets.Count < k_AllowedBulletsNum)
            {
                SpaceShipBullet bullet = new SpaceShipBullet(this.Game);
                bullet.Initialize();
                bullet.TintColor = Color.Red;
                bullet.PositionForDraw = new Vector2(
                                    PositionForDraw.X + (Bounds.Width / 2),
                                    PositionForDraw.Y - (bullet.Bounds.Height / 2));
                bullet.MotionVector = new Vector2(0, -k_BulletVelocity);
                bullet.BulletCollision += new BulletCollisionDelegate(spaceShipBullet_BulletCollision);
                bullet.Disposed += new EventHandler(spaceShipBullet_Disposed);

                m_Bullets.Add(bullet);
            }*/

            // TODO: Move the Bullet property code in here

            SpaceShipBullet b = Bullet;
        }

        #endregion

        /// <summary>
        /// Initialize the SpaceShip position
        /// </summary>
        protected override void     InitBounds()
        {
            base.InitBounds();

            if (!m_WasDefaultPositionSet)
            {
                float x = (float)Texture.Width * 2;
                float y = ((float)GraphicsDevice.Viewport.Height) - 40;

                PositionForDraw = new Vector2(x - (this.Texture.Width / 2), y);
                DefaultPosition = PositionForDraw;
            }
            else
            {
                PositionForDraw = DefaultPosition;
            }
        }

        /// <summary>
        /// Updates the SpaceShip according to the player choise (move the ship
        /// or release a shoot)
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        public override void    Update(GameTime i_GameTime)
        {
            Vector2 newMotion = Vector2.Zero;

            if (m_InputManager.KeyboardState.IsKeyDown(m_PlayerKeys.LeftMovmentKey))
            {
                newMotion.X = k_Motion * -1;
            }
            else if (m_InputManager.KeyboardState.IsKeyDown(m_PlayerKeys.RightMovmentKey))
            {
                newMotion.X = k_Motion;
            }

            MotionVector = newMotion;            

            // Shoot only if the player press the action key or the player 
            // can make an actiuon using the mouse and pressed the mouse 
            // button. In addition we also verify that enough time had 
            // passed from the previous shoot.
            if (m_InputManager.KeyPressed(m_PlayerKeys.ActionKey) ||
               (m_PlayerKeys.UseMouse && 
                m_InputManager.ButtonPressed(eInputButtons.Left)))
            {               
                Shoot();
            }

            base.Update(i_GameTime);

            Vector2 position = PositionForDraw;

            if (m_PlayerKeys.UseMouse)
            {
                position.X += m_InputManager.MousePositionDelta.X;
            }

            position.X = MathHelper.Clamp(
                                position.X, 
                                0,
                                this.GraphicsDevice.Viewport.Width - this.Texture.Width);

            PositionForDraw = position;
        }

        /// <summary>
        /// Initialize the space ship by getting the game's input manager
        /// and setting the components fade and rotate animations
        /// </summary>
        public override void    Initialize()
        {
            m_InputManager = Game.Services.GetService(typeof(InputManager)) as IInputManager;
            base.Initialize();

            RotateAnimation rotateAnimation = new RotateAnimation("spaceship_deathRotate", 2 * (float) Math.PI, TimeSpan.FromSeconds(.4f), true);
            FadeAnimation fadeAnimation = new FadeAnimation("spaceship_deathFade", false, 0, 1, true, TimeSpan.FromSeconds(.4f), TimeSpan.FromSeconds(.4f), true);
            Animations.Add(rotateAnimation);
            Animations.Add(fadeAnimation);
            Animations.Enabled = false;
            Animations.Finished += this.spaceShip_DeathAnimationEnded;
            Animations.ResetAfterFinish = false;
        }

        /// <summary>
        /// Catch the animation ended event raised by the animations
        /// </summary>
        /// <param name="i_Animation">The animation that ended</param>
        public void     spaceShip_DeathAnimationEnded(SpriteAnimation i_Animation)
        {
            Score -= k_LostLifeScoreDecrease;
            RemainingLives -= 1;
            onPlayerWasHit();

            if (m_RemainingLivesLeft <= 0)
            {
                onPlayerIsDead();
            }
            else
            {
                Animations.Pause();
                PositionForDraw = DefaultPosition;
            }
        }

        /// <summary>
        /// Catch the space ship bullet disposed event and remove the bullet
        /// from the bullets list
        /// </summary>
        /// <param name="i_Sender">The bullet that had been disposed</param>
        /// <param name="i_Args">The event arguments</param>
        public void     spaceShipBullet_Disposed(object i_Sender, EventArgs i_Args)
        {
            SpaceShipBullet bullet = i_Sender as SpaceShipBullet;
            
            m_Bullets.Remove(bullet);            
        }

        /// <summary>
        /// Check if the space ship colides with another component
        /// </summary>
        /// <param name="i_OtherComponent">The component we want to check the collision
        /// against</param>
        /// <returns>true in case the components collides or false in case the 
        /// given component is a SpaceShipBullet or there is no collision
        /// between the components </returns>
        public override bool    CheckForCollision(ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is SpaceShipBullet) && 
                   !(i_OtherComponent is SpaceShip) &&
                    base.CheckForCollision(i_OtherComponent);
        }

        /// <summary>
        /// Implement the collision between the space ship and a game component.
        /// Decrease a life from the space ship remaining lives, decrease 2000 points
        /// from the players score and return the ship to the stating position. 
        /// In addition, in case there are no remaining lives or the space ship 
        /// colides with an enemy, raise the PlayerIsDead event
        /// </summary>
        /// <param name="i_OtherComponent">The component the ship colided with</param>
        public override void    Collided(ICollidable i_OtherComponent)
        {
            Animations.Restart();
        }

        /// <summary>
        /// Raise a PlayerIsDead event
        /// </summary>
        private void    onPlayerIsDead()
        {
            Visible = false;
            Enabled = false;

            if (PlayerIsDead != null)
            {
                PlayerIsDead(this);
            }
        }

        /// <summary>
        /// Catch the collision event between the space bullet and a game component.
        /// incase it's an enemy component, we'll add the component score to 
        /// the space ship score
        /// </summary>
        /// <param name="i_OtherComponent">The component the bullet colided with</param>
        /// <param name="i_Bullet">The space ship bullet that colided with the coponent</param>
        private void    spaceShipBullet_BulletCollision(
                            ICollidable i_OtherComponent, 
                            Bullet i_Bullet)
        {
            if (i_OtherComponent is Enemy)
            {
                Score += (i_OtherComponent as Enemy).Score;                
            } 
        }

        /// <summary>
        /// Raise a PlayerWasHit event
        /// </summary>
        private void    onPlayerWasHit()
        {
            if (PlayerWasHitEvent != null)
            {
                PlayerWasHitEvent();
            }
        }

        /// <summary>
        /// Raise a PlayerScoreChanged event
        /// </summary>
        private void    onPlayerScoreChanged()
        {
            if (PlayerScoreChangedEvent != null)
            {
                PlayerScoreChangedEvent();
            }
        }

        /// <summary>
        /// Raise an event when the player release a new shot
        /// </summary>
        /// <param name="i_Bullet">The new bullet that the player shot</param>
        private void    onReleasedShot(Bullet i_Bullet)
        {
            if (ReleasedShot != null)
            {
                ReleasedShot(i_Bullet);
            }
        }

        /// <summary>
        /// Reset the space ship to the starting state by removing the 
        /// bullets and moving it to the default position
        /// </summary>
        public void     ResetSpaceShip()
        {
            this.PositionForDraw = DefaultPosition;

            // "Remove" all the spaceship bullets
            foreach (SpaceShipBullet bullet in m_Bullets)
            {
                bullet.Visible = false;
            }
        }


    }
}