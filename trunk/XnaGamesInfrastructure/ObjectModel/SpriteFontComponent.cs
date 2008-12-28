using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGamesInfrastructure.ObjectModel
{
    /// <summary>
    /// Implements a SpriteFont component that can be used to write on the
    /// screen with
    /// </summary>
    public class SpriteFontComponent : SpriteBase
    {
        private string m_Text = String.Empty;

        private SpriteFont m_Font;

        /// <summary>
        /// Initializes the SpriteFont
        /// </summary>
        /// <param name="i_Game">ths hosing game</param>
        /// <param name="i_FontAssetName">Asset name</param>
        /// <param name="i_Text"></param>
        public  SpriteFontComponent(
            Game i_Game,
            string i_FontAssetName, 
            string i_Text)
            : base(i_FontAssetName, i_Game)
        {
            m_Text = i_Text;
        }

        /// <summary>
        /// Invokes calculation of Font sizes 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            OnTextureModified();
        }

        /// <summary>
        /// Returns the height
        /// </summary>
        protected override int  TextureHeight
        {
            get
            {
                if (m_Font != null)
                {
                    return (int)m_Font.MeasureString(Text).Y;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Returns texture's width
        /// </summary>
        protected override int  TextureWidth
        {
            get
            {
                if (m_Font != null)
                {
                    return (int)m_Font.MeasureString(Text).X;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets / Sets text
        /// </summary>
        public string   Text
        {
            get
            {
                return m_Text;
            }

            set
            {
                if (m_Text != value)
                {
                    m_Text = value;
                    OnTextureModified();
                }
            }
        }

        /// <summary>
        /// Loads the font asset
        /// </summary>
        protected override void DoLoadContent()
        {
            m_Font = m_ContentManager.Load<SpriteFont>(m_AssetName);
        }

        /// <summary>
        /// Calculates sizes when text is modified
        /// </summary>
        private void    OnTextureModified()
        {
            m_HeightBeforeScale = TextureHeight;
            m_WidthBeforeScale = TextureWidth;
        }

        /// <summary>
        /// Performs the actual draw
        /// </summary>
        public override void  DoDraw()
        {
            SpriteBatch.DrawString(
                m_Font,
                Text,
                this.PositionForDraw,
                this.TintColor,
                this.Rotation,
                this.RotationOrigin,
                this.Scale,
                SpriteEffects.None,
                this.LayerDepth);
        }

        /// <summary>
        /// Creates a memberwise clone of sprite
        /// </summary>
        /// <returns>A copy of this sprite</returns>
        public override SpriteBase ShallowClone()
        {
            return this.MemberwiseClone() as SpriteFontComponent;
        }
    }
}
