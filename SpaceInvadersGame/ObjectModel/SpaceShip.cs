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

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// The class represents the player's component in the game (the game's
    /// SpaceShip)
    /// </summary>
    public class SpaceShip : CollidableSprite, IShootable
    {
        private const string k_AssetName = @"Sprites\Ship01_32x32";

        private const int k_AllowedBulletsNum = 3;
        private const int k_Velocity = 200;
        private const int k_BulletVelocity = 200;
        private const int k_LostLifeScoreDecrease = 2000;
        private const int k_LivesNum = 3;

        private Vector2 m_DefaultPosition;

        private int m_RemainingLivesLeft;

        private readonly TimeSpan r_ShootingCoolingOff = TimeSpan.FromSeconds(0.5f);
        private TimeSpan m_PreviousShootingTime;

        private List<Bullet> m_Bullets;
        private IInputManager m_InputManager;

        private int m_PlayerScore = 0;

        public event GameOverDelegate PlayerIsDead;

        #region CTOR's

        public SpaceShip(Game i_Game)
            : this(k_AssetName, i_Game,
                                             Int32.MaxValue, Int32.MaxValue)
        {
        }

        public SpaceShip(string k_AssetName, Game i_Game, int i_UpdateOrder,
                         int i_DrawOrder)
            : base(k_AssetName, i_Game,
                                                 i_UpdateOrder, i_DrawOrder)
        {
            m_Bullets = new List<Bullet>();
            m_PreviousShootingTime = r_ShootingCoolingOff;
            m_RemainingLivesLeft = k_LivesNum;
        }

        #endregion

        /// <summary>
        /// The property holds the starting position.
        /// used in case the space ship is hit be an enemy
        /// </summary>
        protected Vector2 DefaultPosition
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
        /// A property to the player's score
        /// </summary>
        public int Score
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
            }
        }

        #region IShootable Members

        /// <summary>
        /// Release a SpaceShip shoot
        /// </summary>
        public void Shoot()
        {
            // If we didn't reach the maximum bullets allowed, we'll create 
            // a new one and add it to the game components
            if (m_Bullets.Count < k_AllowedBulletsNum)
            {
                SpaceShipBullet bullet = new SpaceShipBullet(this.Game);
                bullet.Initialize();
                bullet.TintColor = Color.Red;
                bullet.Position = new Vector2(m_Position.X + Bounds.Width / 2,
                m_Position.Y - bullet.Bounds.Height / 2);
                bullet.MotionVector = new Vector2(0, -k_BulletVelocity);
                bullet.BulletCollision += new BulletCollisionDelegate(spaceShipBullet_BulletCollision);
                bullet.Disposed += new EventHandler(spaceShipBullet_Disposed);

                m_Bullets.Add(bullet);
            }
        }

        #endregion

        /// <summary>
        /// Initialize the SpaceShip position
        /// </summary>
        protected override void     InitPosition()
        {
            base.InitPosition();

            float x = (float)GraphicsDevice.Viewport.Width / 2;
            float y = ((float)GraphicsDevice.Viewport.Height) - 40;

            Position = new Vector2(x - (this.Texture.Width / 2), y);
            DefaultPosition = Position;
        }

        /// <summary>
        /// Updates the SpaceShip according to the player choise (move the ship
        /// or release a shoot)
        /// </summary>
        /// <param name="i_GameTime">The time passed from the previous update call</param>
        public override void    Update(GameTime i_GameTime)
        {
            Vector2 newMotion = Vector2.Zero;

            m_PreviousShootingTime -= i_GameTime.ElapsedGameTime;

            if (m_InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                newMotion.X = k_Velocity * -1;
            }
            else if (m_InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                newMotion.X = k_Velocity;
            }

            MotionVector = newMotion;


            // Shoot only if the player press the space key and it passed enough time from the previous shoot
            if ((m_InputManager.KeyPressed(Keys.Space) ||
                 m_InputManager.ButtonPressed(eInputButtons.Left)) &&
                m_PreviousShootingTime.TotalSeconds < 0)
            {
                m_PreviousShootingTime = r_ShootingCoolingOff;

                Shoot();
            }

            base.Update(i_GameTime);

            this.m_Position.X += m_InputManager.MousePositionDelta.X;

            m_Position.X = MathHelper.Clamp(m_Position.X, 0,
                                this.GraphicsDevice.Viewport.Width -
                                this.Texture.Width);
        }

        /// <summary>
        /// Initialize the space ship by getting the game's input manager
        /// </summary>
        public override void Initialize()
        {
            m_InputManager = Game.Services.GetService(typeof(InputManager)) as IInputManager;
            base.Initialize();
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
        /// <returns>True if the components collides or false otherwise</returns>
        public override bool    CheckForCollision(ICollidable i_OtherComponent)
        {
            return ((!(i_OtherComponent is SpaceShipBullet)) &&
                    (base.CheckForCollision(i_OtherComponent)));
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
            Score -= k_LostLifeScoreDecrease;
            m_RemainingLivesLeft--;

            if ((m_RemainingLivesLeft <= 0) || (i_OtherComponent is Enemy))
            {
                onPlayerIsDead();
            }
            else
            {
                Position = DefaultPosition;
            }
        }

        /// <summary>
        /// Raise the PlayerIsDead event
        /// </summary>
        private void    onPlayerIsDead()
        {
            if (PlayerIsDead != null)
            {
                PlayerIsDead();
            }
        }

        /// <summary>
        /// Catch the collision event between the space bullet and a game component.
        /// incase it's a scorable component, we'll add the component score to the
        /// space ship score
        /// </summary>
        /// <param name="i_OtherComponent">The component the bullet colided with</param>
        /// <param name="i_Bullet">The space ship bullet that colided with the coponent</param>
        private void    spaceShipBullet_BulletCollision(ICollidable i_OtherComponent, 
                                                        SpaceShipBullet i_Bullet)
        {
            if (i_OtherComponent is IScorable)
            {
                IScorable enemy = i_OtherComponent as IScorable;
                Score += enemy.Score;
            } 
        }

    }
}
