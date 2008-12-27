using System;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.Services;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// Class implements DrawableGameComponent to enable a general Component to 
    /// be used by Game.Componenets
    /// </summary>
    public abstract class DrawableLoadableComponent : DrawableGameComponent
    {
        /// <summary>
        /// Defines asset name (.png file in case of a sprite)
        /// </summary>
        protected string m_AssetName;

        /// <summary>
        /// Defines the game's contetnt manager
        /// </summary>
        protected ContentManager m_ContentManager;

        /// <summary>
        /// Marks if we want to load a fresh copy of the asset
        /// texture each time we call the LoadContent
        /// </summary>
        protected bool m_LoadFreshTextureCopy = false;

        /// <summary>
        /// The constructor intiates the main constructor, with DrawOrder &
        /// UpdateOrder which default to zero.
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        public DrawableLoadableComponent(string i_AssetName, Game i_Game)
            : this(i_AssetName, i_Game, 0, 0)
        {
        }

        /// <summary>
        /// Main constructor (called by other constructor). Calls 
        /// DrawableGameComponent constructor, and initializes assetName, 
        /// and draw&update order id's.
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        /// <param name="i_UpdateOrder">Number defining the order in which update 
        /// of all game components is called</param>
        /// <param name="i_DrawOrder">Number defining the order in which draw
        /// of all game components is called</param>
        public DrawableLoadableComponent(   
               string i_AssetName, 
               Game i_Game,
               int i_UpdateOrder, 
               int i_DrawOrder) 
            : base(i_Game)
        {
            this.m_AssetName = i_AssetName;
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;
        }

        /// <summary>
        /// Called when all game components are initialized, to enable
        /// initialization of new game elements including Content Manager.
        /// </summary>
        public override void    Initialize()
        {
            // In case we want a fresh copy of the texture, will change the content
            // manager to provide it
            if (m_LoadFreshTextureCopy)
            {
                m_ContentManager = new MyContentManager(
                    Game.Content.ServiceProvider);
            }
            else
            {
                m_ContentManager = Game.Content;
            }

            base.Initialize();

            // Component's position on graphics device is initialized
            InitBounds();
        }

        /// <summary>
        /// Initialized initial component position on screen.
        /// </summary>
        protected abstract void     InitBounds();
    }
}
