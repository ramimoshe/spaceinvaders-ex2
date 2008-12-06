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

        // TODO: Change the 1 to a constant

        public Star(Game i_Game, Vector2 i_Position, int i_DrawOrder) 
            : this(i_Game, i_Position, 1, i_DrawOrder)
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

        public override void Initialize()
        {
            base.Initialize();
            
            Random rand = new Random();

            Vector4 tint = this.TintColor.ToVector4();
            tint.W = (float)rand.NextDouble();
            TintColor = new Color(tint);

            TimeSpan fadeTime = TimeSpan.FromSeconds(0.5 + rand.Next() % 4 + rand.NextDouble());

            bool fadeOut = rand.Next(1) == 0 ? true : false;
            TimeSpan animationTime = TimeSpan.MaxValue;
            FadeAnimation fadeAnimation = new FadeAnimation("star_Fade", fadeTime, true, 0, 1, fadeOut, animationTime);
            fadeAnimation.Enabled = true;
            
            Animations.Add(fadeAnimation);
            Animations.Enabled = true;
        }

        protected override void     InitBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
        }       
    }
}
