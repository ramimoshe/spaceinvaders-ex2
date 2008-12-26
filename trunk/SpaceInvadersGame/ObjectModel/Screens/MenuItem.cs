using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations;

namespace SpaceInvadersGame.ObjectModel.Screens
{
    public delegate void MenuItemEventHandler();

    public class MenuItem : SpriteFontComponent
    {
        private bool m_Selected = false;
        readonly private Color r_TintWhenSelected = Color.OrangeRed;
        readonly private Color r_TintWhenDeSelected = Color.Silver;
        public event MenuItemEventHandler Executed = null;
        const string k_DefaultAssetName = @"Fonts\Tahoma28";

        public MenuItem(Game i_Game, string i_Text)
            : base(i_Game, k_DefaultAssetName, i_Text)
        {
            TintColor = r_TintWhenDeSelected;
        }

        public override void Initialize()
        {
            base.Initialize();
            /*
            Animations.Add(new PulseAnimation(
                    "MenuItem_selected",
                    Vector2.One * 0.9f,
                    Vector2.One * 1.1f,
                    TimeSpan.Zero,
                    true,
                    TimeSpan.FromSeconds(0.5f)));
             */
        }

        public bool IsSelected
        {
            get
            {
                return m_Selected;
            }

            set
            {
                if (value != m_Selected)
                {
                    m_Selected = value;
                    if (m_Selected)
                    {
                        OnMenuItemSelected();
                    }
                    else
                    {
                        OnMenuItemDeSelected();
                    }
                }
            }
        }

        public void OnMenuItemSelected()
        {
            TintColor = r_TintWhenSelected;
            
            if (Animations != null)
            {
                Animations.Restart();
            }
        }

        public void OnMenuItemDeSelected()
        {
            TintColor = r_TintWhenDeSelected;

            if (Animations != null)
            {
                Animations.Reset();
                Animations.Pause();
            }
        }

        public void Execute()
        {
            if (Executed != null)
            {
                Executed();
            }
        }
    }
}
