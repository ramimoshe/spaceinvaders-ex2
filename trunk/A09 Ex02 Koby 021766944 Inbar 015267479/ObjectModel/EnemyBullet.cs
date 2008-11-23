using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace A09_Ex02_Koby_021766944_Inbar_015267479.ObjectModel
{
    public class EnemyBullet : Bullet
    {
        public EnemyBullet(Game i_Game)
            : base(i_Game)
        {
            TintColor = Color.Blue;
        }


        public override bool CheckForCollision(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            return ((!(i_OtherComponent is Enemy)) &&
                    (base.CheckForCollision(i_OtherComponent)));
        }

/*        public override void Collided(XnaGamesInfrastructure.ObjectInterfaces.ICollidable i_OtherComponent)
        {
            if (!(i_OtherComponent is Enemy))
            {
                base.Collided(i_OtherComponent);
            }
        }*/
    }
}
