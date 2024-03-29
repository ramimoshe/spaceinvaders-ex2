using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.ObjectModel;
using SpaceInvadersGame.ObjectModel.Screens;
using SpaceInvadersGame.Interfaces;

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
        private const int k_NumOfEnemiesLines = 5;
        private const int k_DefaultNumOfEnemiesInLine = 9;

        private const int k_EnemyWidth = 32;
        private const int k_EnemyHeight = 32;
        private const int k_InvadersUpdateOrder = 1;

        private const int k_EnemyMotionYVal = 500;

        // The percent will decrease in the time it takes the enemies 
        // to move. used to increase the enemies speed
        private const float k_IncreaseEnemiesSpeedFactor = 0.95f;

        public event NoRemainingInvadersDelegate AllInvaderssEliminated;

        public event InvaderReachedScreenEndDelegate InvaderReachedScreenEnd;

        public event PlayActionSoundDelegate PlayActionSoundEvent;

        private int m_EnemiesInLineNum;

        private TimeSpan m_TimeBetweenInvadersShots;

        private readonly TimeSpan r_TimeBetweenMoves = TimeSpan.FromSeconds(0.5f);
        private TimeSpan m_TimeBetweenMoves;
        private TimeSpan m_TimeLeftForNextMove;
        private Vector2 m_MotionVectorForInvaders = new Vector2(500, 0);

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
            m_TimeBetweenMoves = r_TimeBetweenMoves;
            m_TimeLeftForNextMove = m_TimeBetweenMoves;
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

                    currEnemy = createInvader(
                            r_EnemiesLines[i],
                            currInvaderRow,
                            currPosition);                    

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
        /// Decrease the time between the invaders move
        /// </summary>
        private void    speedUpInvaders()
        {
            m_TimeBetweenMoves = 
                TimeSpan.FromSeconds(
                m_TimeBetweenMoves.TotalSeconds * 
                k_IncreaseEnemiesSpeedFactor);
        }

        /// <summary>
        /// Updates the invaders state in the game by randomly releasing a shoot
        /// from one of the invaders every a couple of seconds
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        public override void    Update(GameTime i_GameTime)
        {
            m_TimeLeftForNextMove -= i_GameTime.ElapsedGameTime;
            Vector2 enemiesMotionVector = Vector2.Zero;

            if (m_TimeLeftForNextMove <= TimeSpan.Zero)
            {
                enemiesMotionVector = m_MotionVectorForInvaders;
                m_TimeLeftForNextMove = m_TimeBetweenMoves;

                int minX = GraphicsDevice.Viewport.Width;
                int maxX = 0;
                int maxY = 0;

                foreach (Invader invader in m_EnabledInvaders)
                {
                    if (!invader.Dying)
                    {
                        Rectangle bounds = invader.ScreenBoundsAfterScale;
                        minX = Math.Min(minX, bounds.Left);
                        maxX = Math.Max(maxX, bounds.Right);
                        maxY = Math.Max(maxY, bounds.Bottom);
                    }
                }

                int xDiff = (int)((double)m_MotionVectorForInvaders.X * i_GameTime.ElapsedGameTime.TotalSeconds);

                if (minX +  xDiff < 0 ||
                    maxX + xDiff > GraphicsDevice.Viewport.Width)
                {
                    enemiesMotionVector *= -1;
                    enemiesMotionVector.Y = k_EnemyMotionYVal;
                    m_MotionVectorForInvaders *= -1;
                    speedUpInvaders();
                }

                if (maxY >= m_MaxInvadersYPositionYVal)
                {
                    onInvaderReachedScreenEnd();
                }
            }

            foreach (Invader invader in m_EnabledInvaders)
            {
                invader.MotionVector = enemiesMotionVector;
                invader.TimeBetweenMoves = m_TimeBetweenMoves;
            }

            base.Update(i_GameTime);

            m_PrevShotTime -= i_GameTime.ElapsedGameTime;

            if (m_PrevShotTime.TotalSeconds < 0)
            {     
                shootThePlayer();
                m_PrevShotTime = m_TimeBetweenInvadersShots;
            }
        }             

        /// <summary>
        /// Raise an AllEnemiesEliminated event, stating that there are no more
        /// enemies in the screen
        /// </summary>
        private void    onAllEnemiesEliminated()
        {
            if (AllInvaderssEliminated != null)
            {
                AllInvaderssEliminated();
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
            speedUpInvaders();
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
        /// Change the mother ship score according to the level data
        /// </summary>
        private void    onSettingLevelData()
        {
            m_TimeBetweenMoves = r_TimeBetweenMoves;

            if (m_EnemiesInLineNum != 0)
            {
                if (LevelData.InvadersColumnNum > m_EnemiesInLineNum)
                {
                    addColumnsToMatrix(
                        LevelData.InvadersColumnNum - m_EnemiesInLineNum);
                }
                else
                {
                    removeColumnsFromMatrix(
                        m_EnemiesInLineNum - LevelData.InvadersColumnNum);
                }
            }

            m_EnemiesInLineNum = LevelData.InvadersColumnNum;
            m_TimeBetweenInvadersShots = LevelData.TimeBetweenEnemiesShoots;
            m_PrevShotTime = m_TimeBetweenInvadersShots;            
            m_TimeLeftForNextMove = m_TimeBetweenMoves;
  
            enableAndUpdateInvadersScore();
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
                currInvader.AllowedBulletsNum =
                    m_GameLevelData.AllowedEnemiesShootsNum;
                currInvader.Visible = true;
                currInvader.ResetInvader();

                if (!m_EnabledInvaders.Contains(currInvader))
                {
                    m_EnabledInvaders.Add(currInvader);
                }
            }
        }

        /// <summary>
        /// Removes columns from the invaders matrix
        /// </summary>
        /// <param name="i_ColumnsNum">The number of columns that we want to 
        /// remove from the matrix</param>
        private void    removeColumnsFromMatrix(int i_ColumnsNum)
        {
            if (i_ColumnsNum > 0)
            {
                int lastIndex = (this.Count - 1) - 
                    (i_ColumnsNum * k_NumOfEnemiesLines);

                // Remove invaders from the end of the list
                for (int i = this.Count - 1; i > lastIndex; i--)
                {
                    InvaderComposite invader = this[i];

                    if (invader != null)
                    {                        
                        invader.Dispose();
                    }
                }

                m_EnemiesInLineNum -= i_ColumnsNum;
                onRemovingInvadersFromMatrix();
            }
        }

        /// <summary>
        /// Called after we removed invaders from the matrix
        /// </summary>
        private void    onRemovingInvadersFromMatrix()
        {
            updateLastInvadersInLine();
        }

        /// <summary>
        /// Updates the last invaders in line list according to the current 
        /// invaders matrix
        /// </summary>
        private void    updateLastInvadersInLine()
        {
            int startingIndex = 0;
            int changeIndexFactor = 0;
            int lastInvadersIndex = 0;
            int lastInvadersChangeFactor = 0;

            // When adding invaders to the matrix we add an entire
            // column to the matrix end, but before we add columns the invaders
            // are held according to the lines order so that the invaders in 
            // the last line are from position 0 to k_DefaultNumOfEnemiesInLine - 1, 
            // the invaders in the line before thr last line are from position 
            // k_DefaultNumOfEnemiesInLine - 1 to (2 * k_DefaultNumOfEnemiesInLine) - 1
            // and so on
            if (m_EnemiesInLineNum <= k_DefaultNumOfEnemiesInLine)
            {
                startingIndex = m_EnemiesInLineNum - 1;
                changeIndexFactor = m_EnemiesInLineNum;                
                lastInvadersIndex = m_LastInvadersInLine.Length - 1;
                lastInvadersChangeFactor = -1;
            }
            else
            {
                startingIndex = this.Count - 1 - m_EnemiesInLineNum;
                changeIndexFactor = 1;
                lastInvadersIndex = 0;
                lastInvadersChangeFactor = 1;
            }

            // Move on the matrix according to the calculated indexes
            // and put the invaders in the last invader array
            for (int i = startingIndex; i < this.Count;
                     i += changeIndexFactor)
            {
                InvaderComposite invader = this[i];

                if (invader != null)
                {
                    m_LastInvadersInLine[lastInvadersIndex] = 
                        invader.Invader;

                    lastInvadersIndex += lastInvadersChangeFactor;
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
                Vector2 currPosition = Vector2.Zero;

                // Move on the invaders lines and adds new invaders at the end
                // according to the given column number
                for (int i = 0; i < m_LastInvadersInLine.Length; i++)
                {
                    currPosition = m_LastInvadersInLine[i].DefaultPosition +
                        new Vector2(k_EnemyWidth * 2, 0);

                    for (int j = i_ColumnsNum; j > 0; j--)
                    {
                        currEnemy = createInvader(
                            m_LastInvadersInLine[i].InvaderType,
                            m_LastInvadersInLine[i].InvaderRow,
                            currPosition);

                        currPosition.X += k_EnemyWidth * 2;
                    }

                    m_LastInvadersInLine[i] = currEnemy;
                }
            }
        }

        /// <summary>
        /// Creates a new invader and adds it to the matrix
        /// </summary>
        /// <param name="i_InvaderType">The new invader type</param>
        /// <param name="i_InvaderRowLine">The new invader row num</param>
        /// <param name="i_CurrPosition">The new invader position</param>
        /// <returns>The newely created invader</returns>
        private Invader    createInvader(
            eInvadersType i_InvaderType,
            int i_InvaderRowLine, 
            Vector2 i_CurrPosition)
        {
            Invader currEnemy = InvadersBuilder.GetInstance().CreateInvader(
                i_InvaderType,
                Game,
                k_InvadersUpdateOrder,
                i_InvaderRowLine);

            currEnemy.TimeBetweenMoves = m_TimeBetweenMoves;
            currEnemy.Score =
                m_GameLevelData.GetInvaderScore(currEnemy.InvaderType);
            currEnemy.PositionForDraw = i_CurrPosition;
            currEnemy.DefaultPosition = i_CurrPosition;
            currEnemy.InvaderWasHit += new InvaderWasHitDelegate(invader_InvaderWasHit);
            currEnemy.PlayActionSoundEvent += new PlayActionSoundDelegate(invader_PlayActionSoundEvent);

            InvaderComposite invaderHolder = new InvaderComposite(Game, currEnemy);
            invaderHolder.Disposed += invader_Disposed;

            this.Add(invaderHolder);
            m_EnabledInvaders.Add(currEnemy);

            return currEnemy;
        }

        /// <summary>
        /// Catch a PlayActionSound event raised by an invader and raised
        /// it to the listeners
        /// </summary>
        /// <param name="i_Action">The action that cause the event</param>
        private void    invader_PlayActionSoundEvent(eSoundActions i_Action)
        {
            onPlayActionSound(i_Action);
        }

        /// <summary>
        /// Raise a PlayActionSoundEvent that was raised by an invader
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
