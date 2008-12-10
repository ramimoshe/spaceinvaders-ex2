using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.ObjectModel
{
    /// <summary>
    /// A star in the background
    /// </summary>
    public class Star : Sprite
    {
        private const string k_AssetName = @"Sprites\Star";
        private const int k_WidthBitsNum = 2;
        private const int k_DefaultLayerDepth = 1;
        private const int k_DefaultStarSize = 1;

        public Star(Game i_Game, Vector2 i_Position, int i_DrawOrder)
            : this(i_Game, i_Position, k_DefaultStarSize, i_DrawOrder)
        {
        }

        public Star(
            Game i_Game, 
            Vector2 i_Position, 
            int i_StarSize, 
            int i_DrawOrder)
            : base(k_AssetName, i_Game, Int32.MaxValue, i_DrawOrder)
        {
            // We divide the size so that the star will have a maximum
            // bits according to the configured value
            Scale = new Vector2(k_WidthBitsNum, i_StarSize / k_WidthBitsNum);

            PositionForDraw = i_Position;
            LayerDepth = k_DefaultLayerDepth;
        }

        /// <summary>
        /// Initialize the star component by setting the desire animation and 
        /// the component color
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();
            
            Random rand = new Random();

            Vector4 tint = this.TintColor.ToVector4();
            tint.W = (float)rand.NextDouble();
            TintColor = new Color(tint);

            TimeSpan fadeTime = 
                TimeSpan.FromSeconds(1 + (rand.Next() % 4) + rand.NextDouble());

            bool fadeOut = rand.Next(1) == 0 ? true : false;
            TimeSpan animationTime = TimeSpan.Zero;
            FadeAnimation fadeAnimation = new FadeAnimation("star_Fade", true, 0, 1, fadeOut, fadeTime, animationTime, false);
            fadeAnimation.Enabled = true;
            
            Animations.Add(fadeAnimation);
            Animations.Enabled = true;
        }

        /// <summary>
        /// Initialize the star bounds
        /// </summary>
        protected override void     InitBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
        }       
    }
}
