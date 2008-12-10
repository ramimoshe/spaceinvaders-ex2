using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XnaGamesInfrastructure.Services;
using SpaceInvadersGame.ObjectModel;

namespace SpaceInvadersGame
{
    /// <summary>
    /// A delegate that is used by the game components to notify the 
    /// SpaceInvadersGame class that the game ended
    /// </summary>
    public delegate void GameOverDelegate();

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceInvadersGame : Microsoft.Xna.Framework.Game
    {       
        private const int k_SpaceBetweenLivesDraw = 30;
        private const string k_Player1ScorePrefix = "P1 Score: ";
        private const string k_Player2ScorePrefix = "P2 Score: ";

        private readonly Vector2 r_Player1ScorePosition = new Vector2(5, 10);
        private readonly Vector2 r_Player2ScorePosition = new Vector2(5, 30);

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpaceShip m_Player1;
        private SpaceShip m_Player2;
        private BackGround m_BackGround;
        private InvadersMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private PlayerLivesDrawer m_Player2LivesDrawer;
        private PlayerScoreDrawer m_Player1ScoreDrawer;
        private PlayerScoreDrawer m_Player2ScoreDrawer;
        private BarriersHolder m_BarrierHolder;

        private bool m_GameOver = false;
        private bool m_Player1IsDead = false;
        private bool m_Player2IsDead = false;

        public  SpaceInvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            InputManager inputManager = new InputManager(this, 1);
            CollisionManager collisionManager = new CollisionManager(this, 10000);

            createGameComponents();            
        }

        /// <summary>
        /// Creates the main game components
        /// </summary>
        private void    createGameComponents()
        {
            PlayerControls player2Controls = new PlayerControls(
                Microsoft.Xna.Framework.Input.Keys.Space,
                Microsoft.Xna.Framework.Input.Keys.A,
                Microsoft.Xna.Framework.Input.Keys.D,
                false);

            m_Player2 = new SpaceShip(
                this,
                Constants.k_Player2AssetName,
                player2Controls);
            m_Player2.PlayerIsDead += new GameOverDelegate(spaceShip_Player2IsDead);

            m_Player1 = new SpaceShip(
                this,
                Constants.k_Player1AssetName,
                new PlayerControls());
            m_Player1.PlayerIsDead += new GameOverDelegate(spaceShip_Player1IsDead);

            m_BackGround = new BackGround(this, Constants.k_StarsNum);
            m_EnemiesMatrix = new InvadersMatrix(this);
            m_EnemiesMatrix.InvaderReachedScreenEnd += new InvaderReachedScreenEndDelegate(invadersMatrix_InvaderReachedScreenEnd);
            m_EnemiesMatrix.AllInvaderssEliminated += new NoRemainingInvadersDelegate(invadersMatrix_AllInvadersEliminated);

            m_MotherShip = new MotherShip(this);

            m_Player2LivesDrawer = new PlayerLivesDrawer(this, m_Player2);
            this.Components.Add(new PlayerLivesDrawer(this, m_Player1));
            this.Components.Add(m_Player2LivesDrawer);

            m_Player1ScoreDrawer = new PlayerScoreDrawer(
                this,
                m_Player1,
                k_Player1ScorePrefix);
            m_Player1ScoreDrawer.TintColor = Color.Blue;

            m_Player2ScoreDrawer = new PlayerScoreDrawer(
                this,
                m_Player2,
                k_Player2ScorePrefix);
            m_Player2ScoreDrawer.TintColor = Color.Green;

            m_BarrierHolder = new BarriersHolder(this);
        }

        /// <summary>
        /// Property that gets/sets indication whether the game ended
        /// </summary>
        private bool    GameOver
        {
            get
            {
                return m_GameOver || (m_Player1IsDead && m_Player2IsDead);
            }

            set
            {
                m_GameOver = value;
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void     Initialize()
        {            
            base.Initialize();

            initComponentsPosition();                        
        }

        /// <summary>
        /// Change the main components position in the screen according
        /// to the desire game requierments
        /// </summary>
        private void    initComponentsPosition()
        {
            // Change the players position
            Vector2 player1Position = new Vector2(
                m_Player1.Texture.Width * 2,
                this.GraphicsDevice.Viewport.Height - m_Player1.Texture.Height);
            Vector2 player2Position = new Vector2(
                m_Player1.Texture.Width,
                this.GraphicsDevice.Viewport.Height - m_Player1.Texture.Height);

            m_Player1.PositionForDraw = player1Position;
            m_Player1.DefaultPosition = player1Position;
            m_Player2.PositionForDraw = player2Position;
            m_Player2.DefaultPosition = player2Position;

            m_EnemiesMatrix.InvaderMaxPositionY = m_Player1.Bounds.Top;

            Vector2 pos = m_Player2LivesDrawer.DrawPosition;
            pos.Y += k_SpaceBetweenLivesDraw;
            m_Player2LivesDrawer.DrawPosition = pos;

            m_Player1ScoreDrawer.Position = r_Player1ScorePosition;
            m_Player2ScoreDrawer.Position = r_Player2ScorePosition;

            m_BarrierHolder.UpdateBarriersPossition(
                m_Player1.Bounds.Top,
                m_Player1.Texture.Height);          
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void     LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void     UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        protected override void     Update(GameTime i_GameTime)
        {
            if (GameOver)
            {
                string winningMsg = getWinningPlayerMsg();

                MessageBox.Show(
                    "Game Over. \nPlayer1 Score Is: " + m_Player1.Score + 
                    "\nPlayer2 Score Is: " + m_Player2.Score + winningMsg, 
                    "Game Over", 
                    MessageBoxButtons.OK, 
                    System.Windows.Forms.MessageBoxIcon.Information);                

                this.Exit();
            }

            base.Update(i_GameTime);
        }

        /// <summary>
        /// Return a string that states the wining player
        /// </summary>
        /// <returns>A string that states the winning player number</returns>
        private string  getWinningPlayerMsg()
        {
            string retVal = "\nPlayer {0} Won";

            if (m_Player1.Score > m_Player2.Score)
            {
                retVal = String.Format(retVal, 1);
            }
            else if (m_Player1.Score > m_Player2.Score)
            {
                retVal = String.Format(retVal, 2);
            }
            else
            {
                retVal = "\nTie. Players scores are equal";
            }

            return retVal;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        protected override void     Draw(GameTime i_GameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }

        /// <summary>
        /// Catch a PlayerIsDead event raised by player1 and mark that the
        /// player is dead
        /// </summary>
        private void    spaceShip_Player1IsDead()
        {
            m_Player1IsDead = true;
        }

        /// <summary>
        /// Catch a PlayerIsDead event raised by player2 and mark that the
        /// player is dead
        /// </summary>
        private void    spaceShip_Player2IsDead()
        {
            m_Player2IsDead = true;
        }

        /// <summary>
        /// Catch an AllEnemiesEliminated event raised by the EnemiesMatrix
        /// and mark the game for exit in the next call to update
        /// </summary>
        private void     invadersMatrix_AllInvadersEliminated()
        {
            GameOver = true;
        }

        /// <summary>
        /// Catch an EnemyReachedScreenEnd event and mark the game for exit 
        /// in the next call to update
        /// </summary>
        private void     invadersMatrix_InvaderReachedScreenEnd()
        {
            GameOver = true;
        }
    }
}
