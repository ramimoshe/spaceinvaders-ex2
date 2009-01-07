using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreidelGame.ObjectModel
{
    /// <summary>
    /// A component that holds components and manages their activity
    /// </summary>
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
                    if (comp.NeedTexture)
                    {
                        comp.Texture = value;
                    }
                }
            }
        }

        public CompositeGameComponent(Game i_Game)
            : base(i_Game)
        {
            i_Game.Components.Add(this);
        }

        /// <summary>
        /// Adds a new component to the components list
        /// </summary>
        /// <param name="i_Drawable">The new compoenent we want to add to the list</param>
        public void     Add(BaseDrawableComponent i_Drawable)
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

        /// <summary>
        /// Initialize the component by initializing all the compponents we hold
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            foreach (BaseDrawableComponent drawable in m_Drawables)
            {
                drawable.Initialize();
            }
        }

        /// <summary>
        /// Update the component by updating all the compponents we hold
        /// </summary>
        public override void    Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (BaseDrawableComponent drawable in m_Drawables)
            {
                drawable.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all the components we hold
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (BaseDrawableComponent drawable in m_Drawables)
            {
                drawable.Draw(gameTime);
            }
        }

        /// <summary>
        /// An empty method cause the class doesn't have anything to draw, it simply draws all the
        /// components he holds
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time</param>
        public override void DoDraw(GameTime i_GameTime)
        {
        }
    }
}
