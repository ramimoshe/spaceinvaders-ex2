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

        public SpriteFontComponent(
            Game i_Game,
            string i_FontAssetName, 
            string i_Text)
            : base(i_FontAssetName, i_Game)
        {
            m_Text = i_Text;
        }

        protected override int TextureHeight
        {
            get
            {
                return (int)m_Font.MeasureString(Text).Y;
            }
        }

        protected override int TextureWidth
        {
            get
            {
                return (int)m_Font.MeasureString(Text).X;
            }
        }

        public string Text
        {
            get
            {
                return m_Text;
            }

            set
            {
                m_Text = value;
            }
        }

        protected override void DoLoadContent()
        {
            m_Font = m_ContentManager.Load<SpriteFont>(m_AssetName);
        }

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
    }
}
