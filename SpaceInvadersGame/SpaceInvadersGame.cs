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
    // A delegate for an event that states the game is over
    public delegate void GameOverDelegate();

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SpaceInvadersGame : Microsoft.Xna.Framework.Game
    {
        // Add an asembly reference to the MessageBox so that we can use it in
        // our game
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, string text, string caption, uint type);

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpaceShip m_Player;
        private BackGround m_BackGround;
        private InvadersMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;

        private bool m_GameOver = false;

        public  SpaceInvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            InputManager inputManager = new InputManager(this, 1);
            CollisionManager collisionManager = new CollisionManager(this, 10000);

            m_Player = new SpaceShip(this);
            m_Player.PlayerIsDead += new GameOverDelegate(spaceShip_PlayerIsDead);
            
            m_BackGround = new BackGround(this);
            m_EnemiesMatrix = new InvadersMatrix(this);
            m_EnemiesMatrix.InvaderReachedScreenEnd += new InvaderReachedScreenEndDelegate(enemiesMatrix_EnemyReachedScreenEnd);
            m_EnemiesMatrix.AllInvaderssEliminated += new NoRemainingInvadersDelegate(enemiesMatrix_AllEnemiesEliminated);

            m_MotherShip = new MotherShip(this);

            this.Components.Add(m_EnemiesMatrix);
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

            m_EnemiesMatrix.InvaderMaxPositionY = m_Player.Bounds.Top;
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
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void     Update(GameTime gameTime)
        {
            if (m_GameOver)
            {
                MessageBox(
                    new IntPtr(0), 
                    "Game Over. Player Score Is: " + m_Player.Score, 
                    "Game Over", 
                    0);

                this.Exit();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void     Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Catch a PlayerIsDead event raised by the player and mark the game 
        /// for exit in the next call to update
        /// </summary>
        public void     spaceShip_PlayerIsDead()
        {
            m_GameOver = true;
        }

        /// <summary>
        /// Catch an AllEnemiesEliminated event raised by the EnemiesMatrix
        /// and mark the game for exit in the next call to update
        /// </summary>
        public void     enemiesMatrix_AllEnemiesEliminated()
        {
            m_GameOver = true;
        }

        /// <summary>
        /// Catch an EnemyReachedScreenEnd event and mark the game for exit 
        /// in the next call to update
        /// </summary>
        public void     enemiesMatrix_EnemyReachedScreenEnd()
        {
            m_GameOver = true;
        }
    }
}
