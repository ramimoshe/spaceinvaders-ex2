using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XnaGamesInfrastructure.Services
{
    /// <summary>
    /// Manages the content loading of all the game components.
    /// The class is used when we want to load a fresh copy of an asset
    /// each time we load it.
    /// </summary>
    public class MyContentManager : ContentManager
    {
        public MyContentManager(IServiceProvider i_ServiceProvider)
            : base(i_ServiceProvider)
        { 
        }

        /// <summary>
        /// Loads an asset that has been processed by the Content Pipeline. 
        /// Each call to the procedure will return a fresh copy of the asset 
        /// texture.
        /// </summary>
        /// <typeparam name="T">The type of asset to load.</typeparam>
        /// <param name="assetName">The name of the asset we want to load</param>
        /// <returns>A fresh copy of the asset texture</returns>
        public override T   Load<T>(string i_AssetName)
        {
            return ReadAsset<T>(i_AssetName, null);
        }
    }
}
