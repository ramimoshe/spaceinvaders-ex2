using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DreidelGame.Interfaces;

namespace DreidelGame.ObjectModel
{
    public abstract class CompositeGameComponent : BaseDrawableComponent
    {
        private List<BaseDrawableComponent> m_Drawables = new List<BaseDrawableComponent>();

        public override Vector3 Rotations
        {
            set
            {
                base.Rotations = value;

                foreach (BaseDrawableComponent comp in m_Drawables)
                {
                    comp.Rotations = value;
                }
            }
        }

        public override bool SpinComponent
        {
            set
            {
                base.SpinComponent = value;

                foreach (BaseDrawableComponent comp in m_Drawables)
                {
                    comp.SpinComponent = value;
                }
            }
        }

        public override float RotationsPerSecond
        {
            set
            {
                base.RotationsPerSecond = value;

                foreach (BaseDrawableComponent comp in m_Drawables)
                {
                    comp.RotationsPerSecond = value;
                }
            }
        }

        public override Vector3 Position
        {
            set
            {
                base.Position = value;

                foreach (BaseDrawableComponent comp in m_Drawables)
                {
                    comp.Position = value;
                }
            }
        }

        public override Vector3 Scales
        {
            set
            {
                base.Scales = value;

                foreach (BaseDrawableComponent comp in m_Drawables)
                {
                    comp.Scales = value;
                }
            }
        }

        public override Texture2D Texture
        {
            set
            {
                base.Texture = value;

                foreach (BaseDrawableComponent comp in m_Drawables)
                {
                    comp.Texture = value;
                }
            }
        }

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
                i_Drawable.Rotations = Rotations;
                i_Drawable.Scales = Scales;
                i_Drawable.Position = Position;
                i_Drawable.SpinComponent = SpinComponent;

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

        public override void Draw(GameTime gameTime)
        {
            foreach (BaseDrawableComponent drawable in m_Drawables)
            {
                drawable.Draw(gameTime);
            }
        }

        public override void DoDraw(GameTime i_GameTime)
        {
        }
    }
}
