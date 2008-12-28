using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvadersGame
{
    /// <summary>
    /// An enumeration of all the actions that we should play a cue
    /// when they occur
    /// </summary>
    public enum eSoundActions
    {
        PlayerShoot,
        EnemyShoot,
        PlayerHit,
        MotherShipHit,
        YellowInvaderHit,
        BlueInvaderHit,
        PinkInvaderHit,
        BarrierHit,
        MenuItemChanged,
        GameOver,
        KillAllEnemies
    }

    /// <summary>
    /// A fectory class that is responsible for getting the name of a cue 
    /// that matches a certain action in the game
    /// </summary>
    public sealed class SoundFactory
    {       
        private const string k_PlayerShootCue = "PlayerShoot";
        private const string k_EnemyShootCue = "EnemyShoot";
        private const string k_SpaceShipHitCue = "PlayerHit";
        private const string k_MotherShipHitCue = "MotherShipHit";
        private const string k_YInvaderHitCue = "YInvaderHit";
        private const string k_BInvaderHitCue = "BInvaderHit";
        private const string k_PInvaderHitCue = "PInvaderHit";
        private const string k_BarrierHitCue = "BarrierHit";
        private const string k_MenuItemChangedCue = "MenuItemChanged";
        private const string k_GameOverCue = "GameOver";
        private const string k_KillAllEnemiesCue = "ClearEnemies";

        private static Dictionary<eSoundActions, string> s_ActionCues;

        /// <summary>
        /// Static constructor that creates the dictionary that mapes 
        /// between an action and a cue
        /// </summary>
        static SoundFactory()
        {
            s_ActionCues = new Dictionary<eSoundActions, string>();

            // Creates a dictionary that holds all the cue names for the 
            // game actions
            s_ActionCues.Add(eSoundActions.PlayerShoot, k_PlayerShootCue);
            s_ActionCues.Add(eSoundActions.EnemyShoot, k_EnemyShootCue);
            s_ActionCues.Add(eSoundActions.PlayerHit, k_SpaceShipHitCue);
            s_ActionCues.Add(eSoundActions.MotherShipHit, k_MotherShipHitCue);
            s_ActionCues.Add(eSoundActions.YellowInvaderHit, k_YInvaderHitCue);
            s_ActionCues.Add(eSoundActions.BlueInvaderHit, k_BInvaderHitCue);
            s_ActionCues.Add(eSoundActions.PinkInvaderHit, k_PInvaderHitCue);
            s_ActionCues.Add(eSoundActions.BarrierHit, k_BarrierHitCue);
            s_ActionCues.Add(eSoundActions.MenuItemChanged, k_MenuItemChangedCue);
            s_ActionCues.Add(eSoundActions.GameOver, k_GameOverCue);
            s_ActionCues.Add(eSoundActions.KillAllEnemies, k_KillAllEnemiesCue);
        }

        /// <summary>
        /// Gets a game action cue name
        /// </summary>
        /// <param name="i_Action">The action we want to get a cue for</param>
        /// <returns>The name of the cue that matches the given action or String.Empty
        /// if no such cue exists</returns>
        public static string    GetActionCue(eSoundActions i_Action)
        {
            string retVal = String.Empty;

            s_ActionCues.TryGetValue(i_Action, out retVal);

            return retVal;
        }
    }
}
