using System;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel
{
    public abstract class DrawableLoadableComponent : DrawableGameComponent
    {
        protected string m_AssetName;
        protected ContentManager m_ContentManager;
        protected GraphicsDevice m_GraphicsDevice;

        public DrawableLoadableComponent(string i_AssetName, Game i_Game) 
            : base (i_Game)
        {
            this.m_AssetName = i_AssetName;
        }

        public DrawableLoadableComponent(string i_AssetName, Game i_Game,
                                         int i_UpdateOrder, 
                                         int i_DrawOrder) 
            : this(i_AssetName, i_Game)
        {
            this.m_AssetName = i_AssetName;
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;
            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            m_ContentManager = Game.Content;
            m_GraphicsDevice = Game.GraphicsDevice;

            base.Initialize();

            InitBounds();
            InitPosition();
        }

        protected abstract void InitBounds();

        protected abstract void InitPosition();
    }
}
