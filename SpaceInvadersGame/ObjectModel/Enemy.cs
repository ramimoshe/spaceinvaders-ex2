using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;
using XnaGamesInfrastructure.ObjectModel.Animations;

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
                new ScaleAnimation("Scale_enemyDeath", TimeSpan.FromSeconds(.2f), new Vector2(0f, 0f), TimeSpan.Zero, false);
            scaleAnimation.Finished += ScaleFinished;
            Animations.Add(scaleAnimation);
            Animations.Enabled = false;
        }

        public void ScaleFinished(SpriteAnimation i_Animation)
        {
            this.Visible = false;
        }

        public override void Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            if (i_OtherComponent is Bullet)
            {
                Animations.Enabled = true;
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
