using System;
using XnaGamesInfrastructure.ObjectInterfaces;
using XnaGamesInfrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations;
using XnaGamesInfrastructure.Services;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// This class implements a 2Dimensional sprite, which generalize the use of
    /// 2D texture and implements the required methods for collision detection.
    /// </summary>
    public abstract class Sprite : SpriteBase
    {          
        /// <summary>
        /// The constructor intiates the base constructor 
        /// (DrawableLoadableComponent)
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        public  Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game)
        {
        }

        /// <summary>
        /// Calls Base constructor
        /// </summary>
        /// <param name="i_AssetName">Name of file to load component from</param>
        /// <param name="i_Game">Game holding the component</param>
        /// <param name="i_UpdateOrder">Number defining the order in which update 
        /// of all game components is called</param>
        /// <param name="i_DrawOrder">Number defining the order in which draw
        /// of all game components is called</param>
        public  Sprite(
            string i_AssetName,
            Game i_Game,
            int i_UpdateOrder,
            int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        {
        }

        /// <summary>
        /// Read only property to the component texture color array
        /// </summary>
        public Color[] ColorData
        {
            get
            {
                Color[] colorData = new Color[m_Texture.Width * m_Texture.Height];
                Texture.GetData(colorData, 0, Texture.Width * Texture.Height);
                return colorData;
            }
        }
        
        protected override int TextureWidth
        {
            get
            {
                return Texture.Width;
            }
        }

        protected override int TextureHeight
        {
            get
            {
                return Texture.Height;
            }
        }

        /// <summary>
        /// Defines the 2D texture
        /// </summary>
        protected Texture2D   m_Texture;

        /// <summary>
        /// Gets/sets the 2D texture
        /// </summary>
        public Texture2D    Texture
        {
            get
            {
                return m_Texture;
            }

            set
            {
                m_Texture = value;
            }
        }

        protected override void DoLoadContent()
        {
            m_Texture = m_ContentManager.Load<Texture2D>(m_AssetName);

            Game.GraphicsDevice.Textures[0] = null;
        }

        public override void  DoDraw()
        {
 	        SpriteBatch.Draw(
                m_Texture,
                this.PositionForDraw,
                this.SourceRectangle,
                this.TintColor,
                this.Rotation,
                this.RotationOrigin,
                this.Scale,
                SpriteEffects.None,
                this.LayerDepth);
        }        
    }
}
