using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using XnaGamesInfrastructure.ObjectModel.Screens;
using SpaceInvadersGame.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvadersGame.Interfaces;
using SpaceInvadersGame;
using Microsoft.Xna.Framework.Input;
using SpaceInvadersGame.Service;

namespace SpaceInvadersGame.ObjectModel.Screens
{   
    // TODO: Move the delegate to another class
    
    /// <summary>
    /// A delegate for catching a new component created event for
    /// adding it to the current game screen
    /// </summary>
    public delegate void AddGameComponentDelegate(IGameComponent i_NewComponent);

    /// <summary>
    /// The main screen of the space invaders game
    /// </summary>
    public class SpaceInvadersGameScreen : SpaceInvadersScreenAbstract
    {
        public event GameOverDelegate ExitGame;

        // TODO: Remove the code
        //protected SoundManager m_SoundManager;

        private IGameLevelDataManager m_GameLevelDataManager;
        private int m_CurrLevelNum = 1;     
        private SpaceShipComposite[] m_Players;
        private bool[] m_PlayersAliveMark;

        private const int k_SpaceBetweenLivesDraw = 30;
        private const string k_Player1ScorePrefix = "P1 Score: ";
        private const string k_Player2ScorePrefix = "P2 Score: ";

        private readonly Vector2 r_Player1ScorePosition = new Vector2(5, 10);
        private readonly Vector2 r_Player2ScorePosition = new Vector2(5, 30);
        
        private InvadersMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private PlayerLivesDrawer[] m_PlayersLiveDrawer;
        private PlayerScoreDrawer[] m_PlayersScoreDrawer;

        private GameScreen m_LevelTransitionGameScreen;
        private GameScreen m_PauseScreen;
        private GameOverScreen m_GameOverScreen;

        // TODO: Remove the code
        /*private PlayerLivesDrawer m_Player2LivesDrawer;
        private PlayerScoreDrawer m_Player1ScoreDrawer;
        private PlayerScoreDrawer m_Player2ScoreDrawer;*/

        private BarriersHolder m_BarrierHolder;

        private bool m_GameOver = false;

        // TODO: Remove code

/*        private bool m_Player1IsDead = false;
        private bool m_Player2IsDead = false;*/

        // TODO: Change the transition screen so that it won't be 
        // a parameter

        public SpaceInvadersGameScreen(
            Game i_Game, 
            int i_PlayersNum
            /*,
            GameScreen i_LevelTransitionScreen*/)
            : base(i_Game)
        {
            this.IsModal = true;
            createGameComponents();
            createPlayers(i_PlayersNum);
            m_PlayersNum = i_PlayersNum;

            m_LevelTransitionGameScreen = new LevelTransitionScreen(Game);
            m_PauseScreen = new PauseScreen(Game);
            m_GameOverScreen = new GameOverScreen(Game, Players);
            m_GameOverScreen.ExitGame += new GameOverDelegate(gameOverScreen_ExitGame);
        }

        private int m_PlayersNum;

        public int PlayersNum
        {
            get
            {
                return m_PlayersNum;
            }

            set
            {
                m_PlayersNum = value;
            }
        }

        /// <summary>
        /// Property that gets/sets indication whether the game ended
        /// </summary>
        private bool    GameEnded
        {
            get
            {
                bool playersDead = !m_PlayersAliveMark[0];
                
                playersDead = (m_PlayersAliveMark.Length == 2) ?
                     playersDead && !m_PlayersAliveMark[1] :
                     playersDead;
                return m_GameOver || playersDead;
            }

            set
            {
                m_GameOver = value;
            }
        }

        /// <summary>
        /// Gets an array of the game players
        /// </summary>
        private IPlayer[]   Players
        {
            get
            {
                IPlayer[] retVal = null;
                if (m_Players != null)
                {
                    retVal = new SpaceShip[m_Players.Length];                    
                    int i = 0;

                    foreach (SpaceShipComposite spaceShip in m_Players)
                    {
                        retVal[i] = spaceShip.SpaceShip;
                        i++;
                    }
                }

                return retVal;
            }
        }

        /// <summary>
        /// Create the game players 
        /// </summary>
        /// <param name="i_PlayersNum">The number of players we want in the 
        /// game</param>
        private void    createPlayers(int i_PlayersNum)
        {            
            m_Players = new SpaceShipComposite[i_PlayersNum];
            m_PlayersAliveMark = new bool[i_PlayersNum];
            m_PlayersLiveDrawer = new PlayerLivesDrawer[i_PlayersNum];
            m_PlayersScoreDrawer = new PlayerScoreDrawer[i_PlayersNum];

            createPlayer(
                new PlayerControls(), 
                1,
                Constants.k_Player1AssetName,
                k_Player1ScorePrefix);            

            // In case we have 2 players in the game, we'll create the second
            // player
            if (i_PlayersNum == 2)
            {
                PlayerControls player2Controls = new PlayerControls(
                    Microsoft.Xna.Framework.Input.Keys.Space,
                    Microsoft.Xna.Framework.Input.Keys.A,
                    Microsoft.Xna.Framework.Input.Keys.D,
                    false);

                createPlayer(
                    player2Controls, 
                    2,
                    Constants.k_Player2AssetName,
                    k_Player2ScorePrefix);                
            }
        }

        /// <summary>
        /// Creates a new player in the game and adds it to the class 
        /// game components
        /// </summary>
        /// <param name="i_PlayerControls">The controls that we want the new player
        /// will have</param>
        /// <param name="i_PlayerNum">The number of the new player</param>        
        private void   createPlayer(
            PlayerControls i_PlayerControls,
            int i_PlayerNum,
            string i_PlayerAsset,
            string i_ScorePrefix)
        {
            SpaceShip newPlayer =  new SpaceShip(
                    Game,
                    i_PlayerAsset,
                    i_PlayerControls,
                    i_PlayerNum);

            newPlayer.PlayerIsDead += new PlayerIsDeadDelegate(spaceShip_PlayerIsDead);
            newPlayer.PlayActionSoundEvent += new PlayActionSoundDelegate(Component_PlayActionSoundEvent);
            SpaceShipComposite spaceShipHolder = new SpaceShipComposite(
                Game,
                newPlayer);

            PlayerLivesDrawer lDrawer = new PlayerLivesDrawer(
                    Game,
                    newPlayer);
            PlayerScoreDrawer sDrawer = new PlayerScoreDrawer(
                Game,
                newPlayer,
                i_ScorePrefix);

            this.Add(spaceShipHolder);
            this.Add(lDrawer);
            this.Add(sDrawer);
            
            m_Players[i_PlayerNum - 1] = spaceShipHolder;
            m_PlayersScoreDrawer[i_PlayerNum - 1] = sDrawer;
            m_PlayersLiveDrawer[i_PlayerNum - 1] = lDrawer;
            m_PlayersAliveMark[i_PlayerNum - 1] = true;
        }

        /// <summary>
        /// Creates the main game components
        /// </summary>
        private void    createGameComponents()
        {           
            m_EnemiesMatrix = new InvadersMatrix(Game);
            m_EnemiesMatrix.InvaderReachedScreenEnd += new InvaderReachedScreenEndDelegate(invadersMatrix_InvaderReachedScreenEnd);
            m_EnemiesMatrix.AllInvaderssEliminated += new NoRemainingInvadersDelegate(invadersMatrix_AllInvadersEliminated);
            m_EnemiesMatrix.PlayActionSoundEvent += new PlayActionSoundDelegate(Component_PlayActionSoundEvent);
            
            this.Add(m_EnemiesMatrix);

            m_MotherShip = new MotherShip(Game);
            this.Add(m_MotherShip);            

            m_BarrierHolder = new BarriersHolder(Game);
            m_BarrierHolder.PlayActionSoundEvent += new PlayActionSoundDelegate(Component_PlayActionSoundEvent);

            this.Add(m_BarrierHolder);
        }

        public override void    Initialize()
        {
            // TODO: Check if we can change it to typeof(IGameLevelDataManager)

            m_GameLevelDataManager = Game.Services.GetService(typeof(GameLevelDataManager)) as IGameLevelDataManager;
            m_SoundManager = Game.Services.GetService(typeof(SoundManager)) as SoundManager;

            updateComponentsWithLevelData();

            // TODO: Add a new list for all the level data components

            base.Initialize();

            initComponentsPosition();
        }

        // TODO: Check if i can remove it and the interface also

        /// <summary>
        /// Adds a component to the screen when in case its a SoundableComponent
        /// will add a listener to the components event
        /// </summary>
        /// <param name="i_Component">The component we want to add to the screen</param>
        /*public override void    Add(IGameComponent i_Component)
        {
            base.Add(i_Component);

            ISoundableGameComponent component = i_Component as ISoundableGameComponent;

            if (component != null)
            {
                component.PlayActionSoundEvent += new PlayActionSoundDelegate(component_PlayActionSoundEvent);
            }
        }*/

        /// <summary>
        /// Updates all the relevant components with the current game level 
        /// data
        /// </summary>
        private void    updateComponentsWithLevelData()
        {
            GameLevelData newGameLevelData = 
                m_GameLevelDataManager[m_CurrLevelNum];

            m_MotherShip.LevelData = newGameLevelData ;
            m_EnemiesMatrix.LevelData = newGameLevelData;
            m_BarrierHolder.LevelData = newGameLevelData;
        }

        private void    onLevelEnd()
        {
            m_CurrLevelNum++;
            updateComponentsWithLevelData();

            // Return the players to their default position
            foreach (SpaceShipComposite spaceShipComp in m_Players)
            {
                spaceShipComp.SpaceShip.ResetSpaceShip();
            }
            
            m_SoundManager.StopCue(Constants.k_MusicCueName);

            PlayActionCue(eSoundActions.KillAllEnemies);
        }

        // TODO: Remove the proc

        /*private void    updateComponentsWithLevelData()
        {
            

            m_MotherShip.UpdateComponentLevelData(m_GameLevelDataManager[m_CurrLevelNum]);
            m_EnemiesMatrix.UpdateComponentLevelData(m_GameLevelDataManager[m_CurrLevelNum]);
            m_BarrierHolder.UpdateComponentLevelData(m_GameLevelDataManager[m_CurrLevelNum]);
        }*/

        // TODO: Move the players to a dedicated manager class

        /// <summary>
        /// Change the main components position in the screen according
        /// to the desire game requierments
        /// </summary>
        private void initComponentsPosition()
        {
            initSpaceShipsPosition();

            m_EnemiesMatrix.InvaderMaxPositionY = 
                m_Players[0].SpaceShip.Bounds.Top;

            initScoreDrawerPosition();
            initLivesDrawersPosition();

            m_BarrierHolder.UpdateBarriersPossition(
                m_Players[0].SpaceShip.Bounds.Top,
                m_Players[0].SpaceShip.Texture.Height);
        }

        /// <summary>
        /// Initialize the positions of all the SpaceShip components
        /// </summary>
        private void    initSpaceShipsPosition()
        {
            Vector2 player1Position = new Vector2(
                m_Players[0].SpaceShip.Texture.Width * 2,
                this.GraphicsDevice.Viewport.Height -
                m_Players[0].SpaceShip.Texture.Height);

            m_Players[0].SpaceShip.PositionForDraw = player1Position;
            m_Players[0].SpaceShip.DefaultPosition = player1Position;

            if (m_Players.Length == 2)
            {
                Vector2 player2Position = new Vector2(
                    m_Players[0].SpaceShip.Texture.Width,
                    this.GraphicsDevice.Viewport.Height -
                    m_Players[0].SpaceShip.Texture.Height);

                m_Players[1].SpaceShip.PositionForDraw = player2Position;
                m_Players[1].SpaceShip.DefaultPosition = player2Position;
            }
        }

        /// <summary>
        /// Initialize the positions of all the score drawer components
        /// </summary>
        private void    initScoreDrawerPosition()
        {
            m_PlayersScoreDrawer[0].PositionOfOrigin = r_Player1ScorePosition;

            if (m_Players.Length == 2)
            {
                m_PlayersScoreDrawer[1].PositionOfOrigin = r_Player2ScorePosition;
            }
        }

        /// <summary>
        /// Initialize the positions of all the life drawer components
        /// </summary>
        private void    initLivesDrawersPosition()
        {
            Vector2 livesDrawPosition = new Vector2(
                Game.GraphicsDevice.Viewport.Width -
                (m_Players[0].SpaceShip.PlayerTexture.Width *
                 Constants.k_LivesDrawScaleValue),
                m_Players[0].SpaceShip.PlayerTexture.Height *
                 Constants.k_LivesDrawScaleValue);

            foreach (PlayerLivesDrawer lifeDraw in m_PlayersLiveDrawer)
            {
                lifeDraw.DrawPosition = livesDrawPosition;
                livesDrawPosition.Y += k_SpaceBetweenLivesDraw;
            }
        }

        /// <summary>
        /// Updates the screen state by checking if the game ended, if so
        /// will raise an event otherwise will continue with the game logic
        /// </summary>
        /// <param name="gameTime">A snapshot of the current game time</param>
        public override void    Update(GameTime gameTime)
        {
            if (GameEnded)
            {
                onGameEnded();
            }
            else
            {
                base.Update(gameTime);

                // Move the P key to constant

                if (InputManager.KeyPressed(Keys.P))
                {
                    ScreensManager.SetCurrentScreen(m_PauseScreen);
                }
            }
        }   
        
        /// <summary>
        /// Catch a PlayerIsDead event raised by the players and mark that 
        /// the player is dead
        /// </summary>
        private void spaceShip_PlayerIsDead(IPlayer i_Player)
        {
            m_PlayersAliveMark[i_Player.PlayerNum - 1] = false;
        }

        /// <summary>
        /// Catch an AllEnemiesEliminated event raised by the EnemiesMatrix
        /// and mark the game for exit in the next call to update
        /// </summary>
        private void invadersMatrix_AllInvadersEliminated()
        {
            onLevelEnd();
            ScreensManager.SetCurrentScreen(m_LevelTransitionGameScreen);
        }

        /// <summary>
        /// Catch an EnemyReachedScreenEnd event and mark the game for exit 
        /// in the next call to update
        /// </summary>
        private void invadersMatrix_InvaderReachedScreenEnd()
        {
            GameEnded = true;
        }

        private void    onExitGame()
        {
            if (ExitGame != null)
            {
                // TODO: Remove the remarked code

                /*IPlayer[] players = new SpaceShip[m_Players.Length];
                int i = 0;

                foreach (SpaceShipComposite spaceShip in m_Players)
                {
                    players[i] = spaceShip.SpaceShip;
                    i++;
                }*/

                ExitGame(/*players*/);
            }
        }

        private void    onGameEnded()
        {
            PlayActionCue(eSoundActions.GameOver);
            ScreensManager.SetCurrentScreen(m_GameOverScreen);
        }

        private void    gameOverScreen_ExitGame()
        {
            onExitGame();
        }

        // TODO: Remove the code

        /// <summary>
        /// Catch the PlayActionSoundEvent raised by a SoundableGameComponent
        /// and plays the action sound
        /// </summary>
        /// <param name="i_Action">The action that happened in the game that 
        /// should result in playing a sound</param>
/*        private void    component_PlayActionSoundEvent(eSoundActions i_Action)
        {
            string cue = SoundFactory.GetActionCue(i_Action);
            if (!cue.Equals(String.Empty))
            {
                m_SoundManager.Play(cue);
            }
        }*/
    }
}
