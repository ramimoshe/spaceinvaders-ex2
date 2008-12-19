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
        protected int m_EnemyScore;

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
            Score = 0;         
        }
#endregion

        /// <summary>
        /// Initialize the component animations
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();
            ScaleAnimation scaleAnimation =
                new ScaleAnimation(k_ScaleAnimationName, Vector2.Zero, TimeSpan.FromSeconds(.2f), false);
            scaleAnimation.Finished += new AnimationFinishedEventHandler(ScaleAnimation_Finished);
            Animations.Add(scaleAnimation);
            Animations.Enabled = false;
            Animations.ResetAfterFinish = false;
        }

        /// <summary>
        /// Catch the end event raised by the component animation
        /// </summary>
        /// <param name="i_Animation">the animation that ended</param>
        protected virtual void  ScaleAnimation_Finished(SpriteAnimation i_Animation)
        {
            Visible = false;
            Dying = false;
        }

        /// <summary>
        /// Implement the component collision logic by starting the Enemy
        /// death animation
        /// </summary>
        /// <param name="i_OtherComponent">The colliding component</param>
        public override void    Collided(ICollidable i_OtherComponent)
        {
            Dying = true;

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
        /// A property that gets/sets the enemy score
        /// </summary>
        public int     Score
        {
            get
            {
                return m_EnemyScore;
            }

            set
            {
                m_EnemyScore = value;
            }
        }       
    }
}
