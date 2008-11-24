using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace A09_Ex02_Koby_021766944_Inbar_015267479
{    
    public abstract class Bullet : Sprite
    {
        public event SpriteReachedScreenBoundsDelegate ReachedScreenBounds;

        private const string k_AssetName = @"Sprites\Bullet";

        public Bullet(Game i_Game) : this(k_AssetName, i_Game,
                                          Int32.MaxValue, Int32.MaxValue)
        {
        }

        public Bullet(string i_AssetName, Game i_Game, int i_UpdateOrder,
                      int i_DrawOrder) : base(i_AssetName, i_Game, 
                                              i_UpdateOrder, i_DrawOrder)
        {
        }

        public override void    Update(GameTime gameTime)
        {
            base.Update(gameTime);            

            // TODO Check if i can implement with the viewport

            if ((Bounds.Top < 0) || (Visible == false))
            {
                Dispose();
                // TODO Remove the remark

                // Notify the listeners that the bullet reached the screen top
                //OnReachedScreenBounds();

                // TODO remove the method

                //Game.Components.Remove(this);
            }
        }

        public override bool  CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return !(i_OtherComponent is Bullet) && 
                    base.CheckForCollision(i_OtherComponent);
        } 

        

        /// <summary>
        /// Raise the ReachedScreenBounds event
        /// </summary>
        protected void  OnReachedScreenBounds()
        {
            if (ReachedScreenBounds != null)
            {
                ReachedScreenBounds(this);
            }
        }
    }
}
