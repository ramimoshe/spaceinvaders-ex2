using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;
using XnaGamesInfrastructure.ObjectModel.Animations;
using XnaGamesInfrastructure.ObjectInterfaces;
using SpaceInvadersGame.Interfaces;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// An abstract class that all the enemies type in the game 
    /// inherits from
    /// </summary>
    public abstract class Enemy : CollidableSprite, IEnemy
    {
        protected const string k_ScaleAnimationName = "Scale_enemyDeath";
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
                new ScaleAnimation(k_ScaleAnimationName, Vector2.Zero, TimeSpan.FromSeconds(.2f), false);
            scaleAnimation.Finished += new AnimationFinishedEventHandler(EnemyScale_Finished);
            Animations.Add(scaleAnimation);
            Animations.Enabled = false;
            Animations.ResetAfterFinish = false;
        }

        protected virtual void EnemyScale_Finished(SpriteAnimation i_Animation)
        {
        }

        public override void Collided(ICollidable i_OtherComponent)
        {
            if (i_OtherComponent is Bullet)
            {
                Animations.Restart();
            }
            else
            {
                base.Collided(i_OtherComponent);
            }
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
