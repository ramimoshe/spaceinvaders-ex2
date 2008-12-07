using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace XnaGamesInfrastructure.Services
{
    public class MyContentManager : ContentManager
    {
        public MyContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override T Load<T>(string assetName)
        {
            return ReadAsset<T>(assetName, null);
        }
    }
}
