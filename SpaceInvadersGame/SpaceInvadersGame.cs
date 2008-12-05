using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        // TODO: Move the dll to the project reference

        // Add an asembly reference to the MessageBox so that we can use it in
        // our game
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, string text, string caption, uint type);

        private const int k_SpaceBetweenLivesDraw = 30;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpaceShip m_Player1;
        private SpaceShip m_Player2;
        private BackGround m_BackGround;
        private InvadersMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private PlayerLivesDrawer m_Player2LivesDrawer;

        // TODO: Check if i need to change the false to a constant

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

            // TODO: Move the keys to the constants class

            PlayerControls player2Controls = new PlayerControls(
                Keys.Space,
                Keys.A,
                Keys.D,
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
            
            m_BackGround = new BackGround(this);
            m_EnemiesMatrix = new InvadersMatrix(this);
            m_EnemiesMatrix.InvaderReachedScreenEnd += new InvaderReachedScreenEndDelegate(invadersMatrix_InvaderReachedScreenEnd);
            m_EnemiesMatrix.AllInvaderssEliminated += new NoRemainingInvadersDelegate(invadersMatrix_AllInvadersEliminated);

            m_MotherShip = new MotherShip(this);            

            this.Components.Add(m_EnemiesMatrix);

            m_Player2LivesDrawer = new PlayerLivesDrawer(this, m_Player2);
            this.Components.Add(new PlayerLivesDrawer(this, m_Player1));
            this.Components.Add(m_Player2LivesDrawer);
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

            // TODO: Check if i need to put the init in here
            
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
                MessageBox(
                    new IntPtr(0), 
                        "Game Over. \nPlayer1 Score Is: " + m_Player1.Score + 
                    "\nPlayer2 Score Is: " + m_Player2.Score, 
                    "Game Over", 
                    0);

                this.Exit();
            }

            base.Update(i_GameTime);
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
