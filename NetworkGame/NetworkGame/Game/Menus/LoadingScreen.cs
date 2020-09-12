using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;
using System;

namespace NetworkGame.Menus
{
    public class LoadingScreen : BaseScreen
    {
        Sprite m_loadingSprite;

        public override float X
        {
            get { return m_loadingSprite.X; }
            set { m_loadingSprite.X = value; }
        }
        public override float Y
        {
            get { return m_loadingSprite.Y; }
            set { m_loadingSprite.Y = value; }
        }

        public LoadingScreen(GraphicsDevice device)
        {
            this.m_loadingSprite = new Sprite()
            {
                Texture = Game.Content.Textures.LoadingScreenSpinner,
                X = device.Viewport.Width / 2,
                Y = device.Viewport.Height / 2,
                Visible = this.Visible,
            };
            Game.Content.Animations.DisplaySpinner.Apply(m_loadingSprite);
        }

        public override void OnVisibleChange()
        {
            m_loadingSprite.Visible = Visible;
        }

        public override void AddToCanvas(DefaultCanvas canvas)
        {
            canvas.Add(m_loadingSprite);
        }

        public override void ApplyAnimation(BaseAnimation animation, bool reverse = false, Action onComplete = null, Action onBegin = null)
        {
            animation.Apply(m_loadingSprite, reverse, onComplete, onBegin);
        }

    }

}
