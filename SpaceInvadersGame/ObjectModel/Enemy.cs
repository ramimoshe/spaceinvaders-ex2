using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// An abstract class that all the enemies type in the game 
    /// inherits from
    /// </summary>
    public abstract class Enemy : CollidableSprite
    {
        #region CTORs

        public Enemy(string i_AssetName, Game i_Game)
            : this(i_AssetName, i_Game, 0, 0)
        {
        }

        public Enemy(string i_AssetName, Game i_Game, int i_UpdateOrder)
            : this(i_AssetName, i_Game, i_UpdateOrder, 0)
        {
        }

        public Enemy(
            string i_AssetName, 
            Game i_Game, 
            int i_UpdateOrder,
            int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {                        
        }
#endregion

        public override void Initialize()
        {
            base.Initialize();
            ScaleAnimation scaleAnimation =
                new ScaleAnimation("Scale_enemyDeath", TimeSpan.FromSeconds(0.1), new Vector2(0f, 0f), TimeSpan.Zero, false);
            Animations.Add(scaleAnimation);
            Animations.Enabled = false;
        }
        /// <summary>
        /// A property that return the enemy score
        /// </summary>
        public abstract int     Score
        {
            get;
        }       
    }
}
