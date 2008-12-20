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
    public class SpaceInvadersGameScreen : GameScreen
    {
        public event GameOverDelegate GameOver;        

        private IGameLevelDataManager m_GameLevelDataManager;
        private int m_CurrLevelNum = 1;
        private BackGroundComposite m_BackGround;        
        private SpaceShipComposite[] m_Players;
        private bool[] m_PlayersAliveMark;

        private const int k_SpaceBetweenLivesDraw = 30;
        private const string k_Player1ScorePrefix = "P1 Score: ";
        private const string k_Player2ScorePrefix = "P2 Score: ";

        private readonly Vector2 r_Player1ScorePosition = new Vector2(5, 10);
        private readonly Vector2 r_Player2ScorePosition = new Vector2(5, 30);
        
        private InvadersMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private PlayerLivesDrawer m_Player2LivesDrawer;
        private PlayerScoreDrawer m_Player1ScoreDrawer;
        private PlayerScoreDrawer m_Player2ScoreDrawer;
        private BarriersHolder m_BarrierHolder;

        private bool m_GameOver = false;
        private bool m_Player1IsDead = false;
        private bool m_Player2IsDead = false;

        public SpaceInvadersGameScreen(Game i_Game, int i_PlayersNum)
            : base(i_Game)
        {
            createPlayers(i_PlayersNum);
            createGameComponents();

            /*m_WelcomeMessage = new SpriteFontComponent(i_Game, @"Fonts\David");
            m_WelcomeMessage.Text = "Hello";
            m_WelcomeMessage.TintColor = Color.White ;
            m_WelcomeMessage.Scale = new Vector2(3,3);

            m_Background = new BackGround(i_Game, 100);
            this.Add(m_WelcomeMessage);
            this.Add(m_Background);*/
        }

        /// <summary>
        /// Property that gets/sets indication whether the game ended
        /// </summary>
        private bool GameEnded
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
        /// Create the game players 
        /// </summary>
        /// <param name="i_PlayersNum">The number of players we want in the 
        /// game</param>
        private void    createPlayers(int i_PlayersNum)
        {            
            m_Players = new SpaceShipComposite[i_PlayersNum];
            m_PlayersAliveMark = new bool[i_PlayersNum];

            m_Players[0] = createPlayer(
                new PlayerControls(), 
                1,
                Constants.k_Player1AssetName);
            m_PlayersAliveMark[0] = true;

            // In case we have 2 players in the game, we'll create the second
            // player
            if (i_PlayersNum == 2)
            {
                PlayerControls player2Controls = new PlayerControls(
                    Microsoft.Xna.Framework.Input.Keys.Space,
                    Microsoft.Xna.Framework.Input.Keys.A,
                    Microsoft.Xna.Framework.Input.Keys.D,
                    false);

                m_Players[1] = createPlayer(
                    player2Controls, 
                    2,
                    Constants.k_Player2AssetName);
                m_PlayersAliveMark[1] = true;
            }
        }

        /// <summary>
        /// Creates a new player in the game and adds it to the class 
        /// game components
        /// </summary>
        /// <param name="i_PlayerControls">The controls that we want the new player
        /// will have</param>
        /// <param name="i_PlayerNum">The number of the new player</param>
        /// <returns></returns>
        private SpaceShipComposite   createPlayer(
            PlayerControls i_PlayerControls,
            int i_PlayerNum,
            string i_PlayerAsset)
        {
            SpaceShip newPlayer =  new SpaceShip(
                    Game,
                    i_PlayerAsset,
                    i_PlayerControls,
                    i_PlayerNum);

            newPlayer.PlayerIsDead += new PlayerIsDeadDelegate(spaceShip_PlayerIsDead);
            SpaceShipComposite spaceShipHolder = new SpaceShipComposite(
                Game,
                newPlayer);

            this.Add(spaceShipHolder);

            return spaceShipHolder;
        }

        /// <summary>
        /// Creates the main game components
        /// </summary>
        private void    createGameComponents()
        {           
            m_BackGround = new BackGroundComposite(Game, Constants.k_StarsNum);
            //this.Add(m_BackGround);

            m_EnemiesMatrix = new InvadersMatrix(Game);
            m_EnemiesMatrix.InvaderReachedScreenEnd += new InvaderReachedScreenEndDelegate(invadersMatrix_InvaderReachedScreenEnd);
            m_EnemiesMatrix.AllInvaderssEliminated += new NoRemainingInvadersDelegate(invadersMatrix_AllInvadersEliminated);
            
            this.Add(m_EnemiesMatrix);

            m_MotherShip = new MotherShip(Game);
            this.Add(m_MotherShip);

            // TODO: Remove the code

            /*m_Player2LivesDrawer = new PlayerLivesDrawer(this, m_Player2);
            //this.Components.Add(new PlayerLivesDrawer(this, m_Player1));
            //this.Components.Add(m_Player2LivesDrawer);            

            /*m_Player1ScoreDrawer = new PlayerScoreDrawer(
                this,
                m_Player1,
                k_Player1ScorePrefix);
            m_Player1ScoreDrawer.TintColor = Color.Blue;

            m_Player2ScoreDrawer = new PlayerScoreDrawer(
                this,
                m_Player2,
                k_Player2ScorePrefix);
            m_Player2ScoreDrawer.TintColor = Color.Green;*/

            m_BarrierHolder = new BarriersHolder(Game);

            this.Add(m_BarrierHolder);
        }

        public override void    Initialize()
        {
            // TODO: Check if we can change it to typeof(IGameLevelDataManager)

            m_GameLevelDataManager = Game.Services.GetService(typeof(GameLevelDataManager)) as IGameLevelDataManager;

            initComponentsWithLevelData();

            // TODO: Add a new list for all the level data components

            base.Initialize();

            initComponentsPosition();

        }

        /// <summary>
        /// Updates all the relevant components with the current game level 
        /// data
        /// </summary>
        private void    initComponentsWithLevelData()
        {
            m_MotherShip.LevelData = m_GameLevelDataManager[m_CurrLevelNum];
            m_EnemiesMatrix.LevelData = m_GameLevelDataManager[m_CurrLevelNum];
        }

        // TODO: Move the players to a dedicated manager class

        /// <summary>
        /// Change the main components position in the screen according
        /// to the desire game requierments
        /// </summary>
        private void initComponentsPosition()
        {
            // Change the players position
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

            m_EnemiesMatrix.InvaderMaxPositionY = 
                m_Players[0].SpaceShip.Bounds.Top;

            // TODO: Return the code

            /*Vector2 pos = m_Player2LivesDrawer.DrawPosition;
            pos.Y += k_SpaceBetweenLivesDraw;
            m_Player2LivesDrawer.DrawPosition = pos;

            m_Player1ScoreDrawer.PositionOfOrigin = r_Player1ScorePosition;
            m_Player2ScoreDrawer.PositionOfOrigin = r_Player2ScorePosition;*/

            m_BarrierHolder.UpdateBarriersPossition(
                m_Players[0].SpaceShip.Bounds.Top,
                m_Players[0].SpaceShip.Texture.Height);
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
                onGameOver();
            }
            else
            {
                base.Update(gameTime);
            }
        }

        // TODO: Remove the code

        /*public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }*/     
        
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
            GameEnded = true;
        }

        /// <summary>
        /// Catch an EnemyReachedScreenEnd event and mark the game for exit 
        /// in the next call to update
        /// </summary>
        private void invadersMatrix_InvaderReachedScreenEnd()
        {
            GameEnded = true;
        }

        private void    onGameOver()
        {
            if (GameOver != null)
            {
                IPlayer[] players = new SpaceShip[m_Players.Length];
                int i = 0;

                foreach (SpaceShipComposite spaceShip in m_Players)
                {
                    players[i] = spaceShip.SpaceShip;
                    i++;
                }

                GameOver(players);
            }
        }       
    }
}
