using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;

namespace A09_Ex02_Koby_021766944_Inbar_015267479
{
    public class EnemiesMatrix : GameComponent
    {
        private const int k_EnemiesInLineNum = 9;
        private const int k_NumOfEnemiesLines = 5;

        private readonly TimeSpan r_DefaultTimeBetweenShots = TimeSpan.FromSeconds(1.5f);

        // A time counter that contains a random seconds for the time space between
        // the enemies shoots
        private TimeSpan m_PrevShotTime;

        // TODO Delete the member

        //private const int k_StartPositionY = 100;

        private const int k_EnemyWidth = 32;
        private const int k_EnemyHeight = 32;
        private const int k_EnemyMotionYVal = 350;

        private bool m_ChangeEnemiesPosition = false;
        private bool m_LastChangeEnemiesPosition = false;

        // A two dimentional array that represents the enemies matrix.
        // each cell in the matrix contains the type of the enemy that will
        // be dinamically created later on
        Type[,] m_EnemiesMatrix = new Type[k_NumOfEnemiesLines, k_EnemiesInLineNum] 
                                            { { typeof(PinkEnemy), typeof(PinkEnemy), typeof(PinkEnemy), 
                                                typeof(PinkEnemy), typeof(PinkEnemy), typeof(PinkEnemy), 
                                                typeof(PinkEnemy), typeof(PinkEnemy), typeof(PinkEnemy) },                         
                                              { typeof(BlueEnemy), typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), typeof(BlueEnemy) },                         
                                              { typeof(BlueEnemy), typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), typeof(BlueEnemy), 
                                                typeof(BlueEnemy), typeof(BlueEnemy), typeof(BlueEnemy) },                         
                                              { typeof(YellowEnemy), typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), typeof(YellowEnemy) },                         
                                              { typeof(YellowEnemy), typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), typeof(YellowEnemy), 
                                                typeof(YellowEnemy), typeof(YellowEnemy), typeof(YellowEnemy) }};


        private List<List<Enemy>> m_Enemies;

        public EnemiesMatrix(Game i_Game) : base(i_Game)
        {
            m_Enemies = new List<List<Enemy>>();
            m_PrevShotTime = r_DefaultTimeBetweenShots;
        }

        public override void Initialize()
        {
            initEnemiesList();

            base.Initialize();
        }

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
                    currEnemy = (Enemy)Activator.CreateInstance(m_EnemiesMatrix[i, j], 
                                                         Game, 
                                                         currPosition,
                                                         UpdateOrder - 1);

                    currEnemy.ReachedScreenBounds += new SpriteReachedScreenBoundsDelegate(enemy_ReachedScreenBounds);

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
        /// Change the enemies position by a given factor
        /// </summary>
        /// <param name="i_ChangeFactor">The factor that we want to change the enemies position by</param>
        private void    changeEnemiesMotion(bool i_ChangeXDirection, float i_YMotionFactor)
        {
            // Move on the entire enemies matrix and change the enemy position
            // by the given factor
            foreach (List<Enemy> enemies in m_Enemies)
            {
                foreach (Enemy enemy in enemies)
                {
                    Vector2 motion = enemy.MotionVector;

                    if (i_ChangeXDirection)
                    {
                        enemy.SwitchPosition();

                        // TODO Return the velocity

                        //enemy.Velocity += new Vector2(1, 0);
                    }
                    motion.Y += i_YMotionFactor;
                    enemy.MotionVector = motion;
                }
            }
        }

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
            if (m_ChangeEnemiesPosition)
            {
                m_ChangeEnemiesPosition = false;
                m_LastChangeEnemiesPosition = true;

                changeEnemiesMotion(true, k_EnemyMotionYVal);
            }

            // In case we changed the enemies motion in the previous update
            // we need to change their motion again so that the enemies won't 
            // keep going down
            else if (m_LastChangeEnemiesPosition)
            {
                m_LastChangeEnemiesPosition = false;

                // Return the enemies motion vector to be as before so that they
                // won't keep going down
                changeEnemiesMotion(false, -k_EnemyMotionYVal);
            }
        }

        public void     enemy_ReachedScreenBounds(Sprite i_Sprite)
        {
            // TODO Check if i can implement with "as"
            if (i_Sprite is Enemy)
            {
                m_ChangeEnemiesPosition = true;   
            }
        }

        /// <summary>
        /// Randomly choose an enemy to shoot
        /// </summary>
        private void    shootThePlayer()
        {
            Random rand = new Random();

            // Randomly choose an enemy from the enemies matrix
            int enemyMatrixLine = rand.Next(0, m_Enemies.Count - 1);
            int enemyMatrixColumn = rand.Next(0, m_Enemies[enemyMatrixLine].Count - 1);
            (m_Enemies[enemyMatrixLine])[enemyMatrixColumn].Shoot();
        }
    }
}
