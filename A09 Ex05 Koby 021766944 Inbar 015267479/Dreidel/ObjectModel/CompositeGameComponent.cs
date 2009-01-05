using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    public abstract class CompositeGameComponent : BaseDrawableComponent
    {
        private List<BaseDrawableComponent> m_Drawables = new List<BaseDrawableComponent>();
        private const int k_ZFactor = 8;

        public CompositeGameComponent(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
        }


        // TODO: Check if the proc is ok

        public void Add(BaseDrawableComponent i_Drawable)
        {
            if (m_Drawables.IndexOf(i_Drawable) < 0)
            {
                m_Drawables.Add(i_Drawable);
                i_Drawable.SharedGraphicsDevice = true;

                if (Game.Components.IndexOf(i_Drawable) >= 0)
                {
                    Game.Components.Remove(i_Drawable);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (BaseDrawableComponent drawable in m_Drawables)
            {
                drawable.Initialize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (BaseDrawableComponent drawable in m_Drawables)
            {
                drawable.Update(gameTime);
            }
        }

        public override void DoDraw(GameTime gameTime)
        {
            foreach (BaseDrawableComponent drawable in m_Drawables)
            {
                drawable.Draw(gameTime);
            }
        }
    }
}
