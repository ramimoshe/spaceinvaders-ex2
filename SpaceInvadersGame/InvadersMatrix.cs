using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.ObjectModel;
using SpaceInvadersGame.ObjectModel.Screens;

namespace SpaceInvadersGame
{
    /// <summary>
    /// The delegate is used by the InvadersMatrix to inform that all
    /// invaders in the matrix are dead
    /// </summary>
    public delegate void NoRemainingInvadersDelegate();

    /// <summary>
    /// The delegate is used by the InvadersMatrix to inform that a certain
    /// invader in the matrix reached the allowed Y position
    /// </summary>
    public delegate void InvaderReachedScreenEndDelegate();

    /// <summary>
    /// Holds all the invaders in the game and control their moves
    /// </summary>
    public class InvadersMatrix : CompositeDrawableComponent<InvaderComposite>
    {        

        // TODO: Remove  the variable
        private TimeSpan m_Sleep = TimeSpan.FromSeconds(1.5f);

        private const int k_NumOfEnemiesLines = 5;

        private const int k_EnemyWidth = 32;
        private const int k_EnemyHeight = 32;
        private const int k_InvadersUpdateOrder = 1;

        private const int k_EnemyMotionYVal = 16;

        // The percent will decrease in the time it takes the enemies 
        // to move. used to increase the enemies speed
        private const float k_IncreaseEnemiesSpeedFactor = 0.5f;

        // The time we want to wait between two enemies shoots
        private readonly TimeSpan r_DefaultTimeBetweenShots = TimeSpan.FromSeconds(.75f);

        public event NoRemainingInvadersDelegate AllInvaderssEliminated;

        public event InvaderReachedScreenEndDelegate InvaderReachedScreenEnd;        

        private bool m_ChangeInvadersDirection = false;
        private int m_EnemiesInLineNum;

        // A time counter that contains a random seconds for the time space between
        // the enemies shoots
        private TimeSpan m_PrevShotTime;

        // A list of invaders types representing the types order 
        // in the invaders matrix
        private readonly eInvadersType[] r_EnemiesLines = 
            new eInvadersType[k_NumOfEnemiesLines]
            {
                eInvadersType.PinkInvader,            
                eInvadersType.BlueInvader,
                eInvadersType.BlueInvader,
                eInvadersType.YellowInvader,
                eInvadersType.YellowInvader
            };

        private Invader[] m_LastInvadersInLine;

        // A list of all the visible invaders in the matrix.
        // Used for choosing an active invader for shooting.
        private List<Invader> m_EnabledInvaders = new List<Invader>();

        private float m_MaxInvadersYPositionYVal;

        private GameLevelData m_GameLevelData;

        public InvadersMatrix(Game i_Game) 
            : base(i_Game)
        {
            m_PrevShotTime = r_DefaultTimeBetweenShots;
            m_LastInvadersInLine = new Invader[k_NumOfEnemiesLines];
        }

        /// <summary>
        /// A property for setting the maximum value an invader is allowed
        /// to reach in the Y axis    
        /// </summary>
        public float    InvaderMaxPositionY
        {
            set
            {
                m_MaxInvadersYPositionYVal = value;

                updateInvadersMaxYValue(); 
            }
        }

        /// <summary>
        /// Sets the component game level data and change the invaders matrix
        /// column num according to the given level data
        /// </summary>
        public GameLevelData    LevelData
        {
            private get { return m_GameLevelData; }

            set
            {
                m_GameLevelData = value;
                onSettingLevelData();                
            }
        }

        /// <summary>
        /// Initialize the component by creating the enemies matrix and initialize
        /// the invaders maximum Y value to be the screen height
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_MaxInvadersYPositionYVal = Game.GraphicsDevice.Viewport.Height;

            initInvadersList();            
        }

        /// <summary>
        /// Initialize the enemies matrix
        /// </summary>
        private void    initInvadersList()
        {
            float startingPositionX = 0;

            float startingPositionY = ((float)
                Game.GraphicsDevice.Viewport.Height / 2);

            Vector2 currPosition = new Vector2(startingPositionX, startingPositionY);
            Invader currEnemy = null;
            eInvadersType? prevRowType = null;
            int currInvaderRow = 1;
            InvadersBuilder invadersBuilder = InvadersBuilder.GetInstance();

            // Creates all the enemies according to the enemies two dimentional 
            // array            
            for (int i = k_NumOfEnemiesLines - 1; i >= 0; i--)
            {                
                List<Invader> currList = new List<Invader>();
                currInvaderRow = 1;

                for (int j = 0; j < m_EnemiesInLineNum; j++)
                {
                    if (prevRowType != null)
                    {
                        // If it's the first invader in the list, we'll check
                        // if the invader equals the previous one so that will
                        // change the starting texture                        
                        if (j == 0 &&
                            prevRowType.Equals(r_EnemiesLines[i]))
                        {
                            currInvaderRow = 2;
                        }
                    }

                    currEnemy = invadersBuilder.CreateInvader(
                        r_EnemiesLines[i],
                        Game,
                        k_InvadersUpdateOrder,
                        currInvaderRow);

                    currEnemy.Score = 
                        m_GameLevelData.GetInvaderScore(currEnemy.InvaderType);
                    currEnemy.PositionForDraw = currPosition;
                    currEnemy.DefaultPosition = currPosition;
                    currEnemy.InvaderMaxPositionY = m_MaxInvadersYPositionYVal;
                    currEnemy.ReachedScreenBounds += new InvaderReachedScreenBoundsDelegate(invader_ReachedScreenBounds);
                    currEnemy.InvaderWasHit += new InvaderWasHitDelegate(invader_InvaderWasHit);

                    InvaderComposite invaderHolder = new InvaderComposite(Game, currEnemy);
                    invaderHolder.Disposed += invader_Disposed;

                    this.Add(invaderHolder);
                    m_EnabledInvaders.Add(currEnemy);

                    currPosition.X += k_EnemyWidth * 2;
                    prevRowType = r_EnemiesLines[i];
                }

                // Save the last invader of the current line
                // (used for ivaders matrix column add later on)
                m_LastInvadersInLine[i] = currEnemy;

                currPosition.Y -= k_EnemyHeight;
                currPosition.Y -= (k_EnemyHeight / 2);
                currPosition.X = startingPositionX;                
            }
        }

        /// <summary>
        /// Change the invaders matrix by changing their Y position, increase
        /// their moving speed, and change their moving direction on the X axis
        /// </summary>
        /// <param name="i_YMotionFactor">The factor that we want to move the 
        /// enemies in the Y axis by</param>
        /// <param name="i_ChangePosition">Mark if we want to change the
        /// invaders position or only the invaders moving speed</param>
        private void    changeInvadersMatrixPositions(
            float i_YMotionFactor,
            bool i_ChangePosition)
        {
            IEnumerator<InvaderComposite> invadersEnumeration = this.GetEnumerator();

            // Move on the entire enemies matrix and change the enemy position
            // by the given factor
            while (invadersEnumeration.MoveNext())
            {
                Invader enemy = invadersEnumeration.Current.Invader;

                // Increase the number of times the enemy moves in a second
                // (by that we increase the invaders speed)
                TimeSpan moveTime = TimeSpan.FromSeconds(enemy.TimeBetweenMoves.TotalSeconds * 
                                                         k_IncreaseEnemiesSpeedFactor);
                enemy.TimeBetweenMoves = moveTime;

                if (i_ChangePosition)
                {
                    // Change the Y position so that the enemy will go down                                                            
                    Vector2 position = enemy.PositionForDraw;
                    position.Y += i_YMotionFactor;
                    enemy.PositionForDraw = position;

                    // Change the enemy direction
                    enemy.SwitchPosition();
                }
            }  
        }

        /// <summary>
        /// Updates the invaders state in the game by randomly releasing a shoot
        /// from one of the invaders every a couple of seconds
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            // TODO: Remove the sleep decrease and if. for debug only.
            /*m_Sleep -= i_GameTime.ElapsedGameTime;

            if (m_Sleep.TotalSeconds <= 0)
            {
                m_Sleep = TimeSpan.FromSeconds(10.5f);
                onAllEnemiesEliminated();
            }
            else
            {*/

                m_PrevShotTime -= i_GameTime.ElapsedGameTime;

                if (m_PrevShotTime.TotalSeconds < 0)
                {
                    // TODO: Remove the remark
                    //shootThePlayer();
                    m_PrevShotTime = r_DefaultTimeBetweenShots;
                }

                // In case an enemy reached the end of the screen width, we
                // need to change the invaders Y position, change their
                // X motion, and increase their moving speed
                if (m_ChangeInvadersDirection)
                {
                    changeInvadersMatrixPositions(k_EnemyMotionYVal, true);
                    m_ChangeInvadersDirection = false;
                }
          //  }
        }             

        /// <summary>
        /// Raise an AllEnemiesEliminated event, stating that there are no more
        /// enemies in the screen
        /// </summary>
        private void    onAllEnemiesEliminated()
        {
            if (AllInvaderssEliminated != null)
            {
                // TODO: Check if i should put it in here

                AllInvaderssEliminated();
            }
        }

        /// <summary>
        /// Catch the ReachedScreenBounds event raised by an invader.
        /// The method changes the enemies movement direction in the X axis, 
        /// change the enemies position in the Y axis by moving them down in 
        /// the screen and increases the invaders moving speed
        /// </summary>
        /// <param name="i_Invader">The invader that raised the event</param>
        public void     invader_ReachedScreenBounds(Invader i_Invader)
        {            
            // If the invader reached the maximum allowed Y position, than
            // we need to raise an InvaderReachedScreenEnd event
            if (!(i_Invader.Bounds.Bottom >= m_MaxInvadersYPositionYVal))
            {
                m_ChangeInvadersDirection = true;
            }
            else
            { 
                onInvaderReachedScreenEnd();
            }
        }

        /// <summary>
        /// Catch the InvaderWasHit event, removes the invader from the 
        /// enabled invaders list and increase the other invaders moving
        /// speed
        /// </summary>
        /// <param name="i_Invader">The invader that was hit</param>
        public void     invader_InvaderWasHit(Invader i_Invader)
        {
            removeInvaderFromEnabledList(i_Invader);

            // Increase the invaders moving speed
            changeInvadersMatrixPositions(0, false);
        }

        /// <summary>
        /// Raise an EnemyReachedScreenEnd event when a certain enemy in the 
        /// enemies matrix reaches the maximum allowed Y value
        /// </summary>
        private void    onInvaderReachedScreenEnd()
        {                        
            if (InvaderReachedScreenEnd != null)
            {
                InvaderReachedScreenEnd();
            }
        }

        /// <summary>
        /// Randomly choose an enemy to shoot
        /// </summary>
        private void    shootThePlayer()
        {
            Random rand = new Random();

            // In case there are visible invaders we'll shoot the player from
            // a random invader
            if (m_EnabledInvaders.Count > 0)
            {
                // Randomly choose an invader from the visible invaders list
                int invaderIndex = rand.Next(0, m_EnabledInvaders.Count - 1);
                m_EnabledInvaders[invaderIndex].Shoot();
            }
        }
       
        /// <summary>
        /// Catch an invader disposed event, remove it from the matrix and in
        /// case there are no enemies left raise an event
        /// </summary>
        /// <param name="i_Sender">The disposed enemy</param>
        /// <param name="i_EventArgs">The event arguments</param>
        private void    invader_Disposed(object i_Sender, EventArgs i_EventArgs)
        {
            InvaderComposite enemy = i_Sender as InvaderComposite;

            removeInvaderFromMatrix(enemy);

            removeInvaderFromEnabledList(enemy.Invader);
        }

        /// <summary>
        /// Removes an invader from the invaders matrix
        /// </summary>
        /// <param name="i_Enemy">The enemy that we want to remove from the matrix</param>
        private void    removeInvaderFromMatrix(InvaderComposite i_Enemy)
        {
            this.Remove(i_Enemy);                        
        }

        /// <summary>
        /// Removes an invader from the enabled invaders list
        /// </summary>
        /// <param name="i_Invader">The invader we want to remove from the list</param>
        private void    removeInvaderFromEnabledList(Invader i_Invader)
        {
            m_EnabledInvaders.Remove(i_Invader);

            if (m_EnabledInvaders.Count <= 0)
            {
                onAllEnemiesEliminated();
            }
        }

        /// <summary>
        /// Update the invaders max Y position for all the invaders in the 
        /// matrix
        /// </summary>
        private void    updateInvadersMaxYValue()
        {
            IEnumerator<InvaderComposite> invadersEnumeration = this.GetEnumerator();

            while (invadersEnumeration.MoveNext())
            {
                invadersEnumeration.Current.Invader.InvaderMaxPositionY = 
                    m_MaxInvadersYPositionYVal;
            }     
        }

        /// <summary>
        /// Change the mother ship score according to the level data
        /// </summary>
        private void    onSettingLevelData()
        {
            if (m_EnemiesInLineNum != 0)
            {
                addColumnsToMatrix(LevelData.InvadersColumnNum - m_EnemiesInLineNum);
            }

            m_EnemiesInLineNum = LevelData.InvadersColumnNum;

            // TODO: Delete the remarks

            // If all the invaders are eliminated (end of the level), we need
            // to make them all visible
            /*if (m_EnabledInvaders.Count == 0)
            {*/
                enableAndUpdateInvadersScore();
            //}            
        }

        /// <summary>
        /// Move on all the invaders in the matrix and update their 
        /// score according to the game level data
        /// </summary>
        private void    enableAndUpdateInvadersScore()
        {
            IEnumerator<InvaderComposite> invadersEnumeration = this.GetEnumerator();

            while (invadersEnumeration.MoveNext())
            {                
                Invader currInvader = invadersEnumeration.Current.Invader;
                currInvader.Score =
                    m_GameLevelData.GetInvaderScore(currInvader.InvaderType);
                currInvader.Visible = true;
                currInvader.ResetInvader();

                // TODO: check if i should put it in here
                if (!m_EnabledInvaders.Contains(currInvader))
                {
                    m_EnabledInvaders.Add(currInvader);
                }
            }
        }

        /// <summary>
        /// Adds columns to the invaders matrix
        /// </summary>
        /// <param name="i_ColumnsNum">The number of columns that we want to 
        /// add to the matrix</param>
        private void    addColumnsToMatrix(int i_ColumnsNum)
        {
            if (i_ColumnsNum > 0)
            {
                Invader currEnemy = null;
                InvadersBuilder invadersBuilder = InvadersBuilder.GetInstance();
                Vector2 currPosition = Vector2.Zero;

                // Move on the invaders lines and adds new invaders at the end
                // according to the given column number
                for (int i = 0; i < m_LastInvadersInLine.Length; i++)
                {
                    currPosition = m_LastInvadersInLine[i].DefaultPosition +
                        new Vector2(k_EnemyWidth * 2, 0);

                    for (int j = i_ColumnsNum; j > 0; j--)
                    {
                        // TODO: Move the creation code to a method and replace the initialize
                        // also

                        // TODO: Add invader row in the builder call and check 
                        // what to put in the update order

                        currEnemy = invadersBuilder.CreateInvader(
                            m_LastInvadersInLine[i].InvaderType,
                            Game,
                            k_InvadersUpdateOrder,
                            1);

                        currEnemy.Score =
                            m_GameLevelData.GetInvaderScore(currEnemy.InvaderType);
                        currEnemy.PositionForDraw = currPosition;
                        currEnemy.DefaultPosition = currPosition;
                        currEnemy.InvaderMaxPositionY = m_MaxInvadersYPositionYVal;
                        currEnemy.ReachedScreenBounds += new InvaderReachedScreenBoundsDelegate(invader_ReachedScreenBounds);
                        currEnemy.InvaderWasHit += new InvaderWasHitDelegate(invader_InvaderWasHit);

                        InvaderComposite invaderHolder = new InvaderComposite(Game, currEnemy);
                        invaderHolder.Disposed += invader_Disposed;

                        this.Add(invaderHolder);
                        m_EnabledInvaders.Add(currEnemy);

                        currPosition.X += k_EnemyWidth * 2;
                    }

                    m_LastInvadersInLine[i] = currEnemy;
                }
            }
        }
    }
}
