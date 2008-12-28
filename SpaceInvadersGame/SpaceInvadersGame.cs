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
using SpaceInvadersGame.ObjectModel.Screens;
using SpaceInvadersGame.ObjectModel.Screens.Menus;
using SpaceInvadersGame.Interfaces;
using XnaGamesInfrastructure.ObjectModel.Screens;
using SpaceInvadersGame.Service;

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

        private readonly Vector2 r_Player1ScorePosition = new Vector2(5, 10);
        private readonly Vector2 r_Player2ScorePosition = new Vector2(5, 30);

        private GraphicsDeviceManager graphics;
        private SpriteBatch m_SpriteBatch;

        private SoundManager m_SoundManager;

        public SpaceInvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Services.AddService(typeof(GraphicsDeviceManager), graphics);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            InputManager inputManager = new InputManager(this, 1);            
            GameLevelDataManager gameLevelDataManager = new GameLevelDataManager(this);

            PauseScreen pauseScreen = new PauseScreen(this);
            ScreensMananger screensMananger = new ScreensMananger(this);
            BackgroundScreen backgroundScreen = new BackgroundScreen(this, 100);
            screensMananger.Push(backgroundScreen);
            
            MainMenuScreen mainMenu = new MainMenuScreen(this);
            mainMenu.IsModal = true;
            screensMananger.Push(mainMenu);
            
            WelcomeScreen welcomeScreen = new WelcomeScreen(this);
            screensMananger.SetCurrentScreen(welcomeScreen);
            backgroundScreen.State = eScreenState.Active;

            m_SoundManager = new SoundManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), m_SpriteBatch);

            base.Initialize();

            m_SoundManager.Play(Constants.k_MusicCueName, true);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }    

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime i_GameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(i_GameTime);
        }  
    }
}
