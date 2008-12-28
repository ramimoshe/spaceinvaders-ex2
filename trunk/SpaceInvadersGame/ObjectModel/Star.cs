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
            Scale = Vector2.One * i_StarSize;

            PositionOfOrigin = i_Position;
            LayerDepth = k_DefaultLayerDepth;
        }

        private static Random m_Random = new Random();

        /// <summary>
        /// Initialize the star component by setting the desire animation and 
        /// the component color
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();   

            Vector4 tint = this.TintColor.ToVector4();
            tint.W = (float)m_Random.NextDouble();
            TintColor = new Color(tint);

            TimeSpan fadeTime = TimeSpan.FromSeconds(m_Random.Next(1, 4) + m_Random.NextDouble());

            bool fadeOut = m_Random.Next() % 2 == 0;
            TimeSpan animationTime = TimeSpan.Zero;
            FadeAnimation fadeAnimation = new   FadeAnimation(
                                                "star_Fade", 
                                                true, 
                                                0, 
                                                1, 
                                                fadeOut, 
                                                fadeTime, 
                                                animationTime, 
                                                false);
            fadeAnimation.Enabled = true;
            
            Animations.Add(fadeAnimation);
            Animations.Enabled = true;
        }
    } 
}
