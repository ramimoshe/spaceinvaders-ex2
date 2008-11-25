using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.ObjectModel;

namespace SpaceInvadersGame
{
    // A delegate for notifying when all enemies are dead
    public delegate void NoRemainingEnemiesDelegate();

    public delegate void EnemyReachedScreenEndDelegate();

    /// <summary>
    /// Holds all the enemies in the game and control their moves
    /// </summary>
    public class EnemiesMatrix : GameComponent
    {
        private const int k_EnemiesInLineNum = 9;
        private const int k_NumOfEnemiesLines = 5;

        public event NoRemainingEnemiesDelegate AllEnemiesEliminated;

        public event EnemyReachedScreenEndDelegate EnemyReachedScreenEnd;

        private const float k_IncreaseEnemiesSpeedFactor = 0.7f;

        private readonly TimeSpan r_DefaultTimeBetweenShots = TimeSpan.FromSeconds(1.5f);

        private bool m_ChangeEnemiesDirection = false;

        // A time counter that contains a random seconds for the time space between
        // the enemies shoots
        private TimeSpan m_PrevShotTime;       

        private const int k_EnemyWidth = 32;
        private const int k_EnemyHeight = 32;

        private const int k_EnemyMotionYVal = 16;

        private int m_RemainigEnemiesNum;

        // A two dimentional array that represents the enemies matrix.
        // each cell in the matrix contains the type of the enemy that will
        // be dinamically created later on
        private Type[,] m_EnemiesMatrix = new Type[k_NumOfEnemiesLines, k_EnemiesInLineNum] 
                                            { 
                                              { typeof(PinkEnemy), typeof(PinkEnemy), 
                                                typeof(PinkEnemy), typeof(PinkEnemy), 
                                                typeof(PinkEnemy), typeof(PinkEnemy), 
                                                typeof(PinkEnemy), typeof(PinkEnemy), 
                                                typeof(PinkEnemy) },                         
                                              { typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy) },                         
                                              { typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy) },               
                                              { typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy) },                         
                                              { typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy)}};

        private List<List<Enemy>> m_Enemies;

        private float m_MaxEnemiesYPositionYVal;

        public EnemiesMatrix(Game i_Game) : base(i_Game)
        {
            m_Enemies = new List<List<Enemy>>();
            m_PrevShotTime = r_DefaultTimeBetweenShots;

            m_RemainigEnemiesNum = k_EnemiesInLineNum * k_NumOfEnemiesLines;            
        }

        public float EnemyMaxPositionY
        {
            set
            {
                m_MaxEnemiesYPositionYVal = value;
            }
        }

        /// <summary>
        /// Initialize the component
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            initEnemiesList();            
        }

        /// <summary>
        /// Initialize the enemies matrix
        /// </summary>
        private void    initEnemiesList()
        {
            // Calculate the positions according to the number of enemies before the 
            // middle enemy position
            float startingPositionX = ((float)Game.GraphicsDevice.Viewport.Width / 2);
            startingPositionX -= ((k_EnemiesInLineNum / 2) * (k_EnemyWidth * 2));
            startingPositionX -= k_EnemyWidth / 2;

            float startingPositionY = ((float)Game.GraphicsDevice.Viewport.Height / 2);

            Vector2 currPosition = new Vector2(startingPositionX, startingPositionY);
            Enemy currEnemy;

            // Creates all the enemies according to the enemies matrix
            for (int i = k_NumOfEnemiesLines - 1; i >= 0; i--)
            {                
                List<Enemy> currList = new List<Enemy>();

                for (int j = k_EnemiesInLineNum - 1; j >= 0; j--)
                {     
                    // Dynamically creates the enemy according to the type in the
                    // enemies member
                    currEnemy = (Enemy)Activator.CreateInstance(
                                                         m_EnemiesMatrix[i, j], 
                                                         Game, 
                                                         currPosition,
                                                         UpdateOrder - 1);

                    currEnemy.EnemyMaxPositionY = m_MaxEnemiesYPositionYVal;
                    currEnemy.ReachedScreenBounds += new SpriteReachedScreenBoundsDelegate(enemy_ReachedScreenBounds);
                    currEnemy.Disposed += enemy_Disposed;

                    currList.Add(currEnemy);
                    currPosition.X += k_EnemyWidth * 2;
                }

                currPosition.Y -= k_EnemyHeight;
                currPosition.Y -= (k_EnemyHeight / 2);
                currPosition.X = startingPositionX;
                m_Enemies.Add(currList);                
            }
        }

        /// <summary>
        /// Change the enemies direction
        /// </summary>
        /// 
        /// <param name="i_YMotionFactor">The factor we want to move the enemies in
        /// the Y axis</param>
        private void    changeEnemiesDirection(float i_YMotionFactor)
        {
            // Move on the entire enemies matrix and change the enemy position
            // by the given factor
            foreach (List<Enemy> enemies in m_Enemies)
            {
                foreach (Enemy enemy in enemies)
                {
                    // Increase the number of times the enemy moves in a second
                    TimeSpan moveTime = TimeSpan.FromSeconds(enemy.TimeBetweenMoves.TotalSeconds * 
                                                             k_IncreaseEnemiesSpeedFactor);
                    enemy.TimeBetweenMoves = moveTime;

                    // Change the Y position so that the enemy will go down                                                            
                    Vector2 position = enemy.Position; 
                    position.Y += i_YMotionFactor;
                    enemy.Position = position;

                    // Change the enemy direction
                    //enemy.MotionVector = new Vector2(enemy.MotionVector.X * -1, 0);
                    enemy.SwitchPosition();
                }
            }
        }

        /// <summary>
        /// Updates the enemies state in the game by randomly releasing a shoot
        /// from on of the enemies every a couple of seconds
        /// </summary>
        /// <param name="i_GameTime">The time passed from the previous call
        /// to the method</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            m_PrevShotTime -= i_GameTime.ElapsedGameTime;

            if (m_PrevShotTime.TotalSeconds < 0)
            {
                shootThePlayer();
                m_PrevShotTime = r_DefaultTimeBetweenShots;
            }

            // In case we changed the enemies position earlier we need
            // to change their Y position so that they won't keep going down
            if (m_ChangeEnemiesDirection)
            {                
                changeEnemiesDirection(k_EnemyMotionYVal);
                m_ChangeEnemiesDirection = false;
            }            
        }             

        /// <summary>
        /// Raise an AllEnemiesEliminated event, stating that there are no more
        /// enemies in the screen
        /// </summary>
        private void    onAllEnemiesEliminated()
        {
            if (AllEnemiesEliminated != null)
            {
                AllEnemiesEliminated();
            }
        }

        /// <summary>
        /// Catch the ReachedScreenBounds event raised by an enemy and change 
        /// the enemies movement direction in the X axis, and change the enemies
        /// position in the Y axis by moving them down in the screen
        /// </summary>
        /// <param name="i_Sprite">The enemy that raised the event</param>
        public void     enemy_ReachedScreenBounds(Sprite i_Sprite)
        {
            if (i_Sprite is Enemy)
            {
                if (!(i_Sprite.Bounds.Bottom >= m_MaxEnemiesYPositionYVal))
                {
                    m_ChangeEnemiesDirection = true;
                }
                else
                { 
                    onEnemyReachedScreenEnd();
                }
            }
        }

        /// <summary>
        /// Raise an EnemyReachedScreenEnd event when a certain enemy in the 
        /// enemies matrix reaches the maximum allowed Y value
        /// </summary>
        private void onEnemyReachedScreenEnd()
        {
            if (EnemyReachedScreenEnd != null)
            {
                EnemyReachedScreenEnd();
            }
        }

        /// <summary>
        /// Randomly choose an enemy to shoot
        /// </summary>
        private void    shootThePlayer()
        {
            Random rand = new Random();

            // In case there are enemies we'll shoot the player from
            // a random enemy
            if (m_Enemies.Count > 0)
            {
                // Randomly choose an enemy from the enemies matrix                
                int enemyMatrixLine = rand.Next(0, m_Enemies.Count - 1);
                int enemyMatrixColumn = rand.Next(0, m_Enemies[enemyMatrixLine].Count - 1);                                
                (m_Enemies[enemyMatrixLine])[enemyMatrixColumn].Shoot();
            }
        }
       
        /// <summary>
        /// Catch an enemy disposed event, remove it from the matrix and in
        /// case there are no enemies left raise an event
        /// </summary>
        /// <param name="i_Sender">The disposed enemy</param>
        /// <param name="i_EventArgs">The event arguments</param>
        private void    enemy_Disposed(object i_Sender, EventArgs i_EventArgs)
        {
            Enemy enemy = i_Sender as Enemy;

            removeEnemyFromMatrix(enemy);

            m_RemainigEnemiesNum--;

            if (m_RemainigEnemiesNum <= 0)
            {
                onAllEnemiesEliminated();
            }
        }

        /// <summary>
        /// Removes an enemy from the enemies matrix
        /// </summary>
        /// <param name="i_Enemy">The enemy that we want to remove from the matrix</param>
        private void    removeEnemyFromMatrix(Enemy i_Enemy)
        {
            foreach (List<Enemy> enemiesLine in m_Enemies)
            {
                if (enemiesLine.Contains(i_Enemy))
                {
                    enemiesLine.Remove(i_Enemy);

                    // In case it was the last enemy in the line we'll remove the entire
                    // enemies line from the matrix
                    if (enemiesLine.Count == 0)
                    {
                        m_Enemies.Remove(enemiesLine);
                    }

                    break;
                }
            }
        }
    }
}
