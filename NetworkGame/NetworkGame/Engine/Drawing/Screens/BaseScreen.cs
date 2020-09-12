using System;
using Microsoft.Xna.Framework;

namespace NetworkGame.Engine
{
    public abstract class BaseScreen : IDrawable, IMouseHandler, ITouchHandler
    {
        bool m_visible;

        public bool IsProcessInput { get; set; }
        public Action<bool> VisibleChanged;

        public virtual float X
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }
        public virtual float Y
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }
        public bool Visible
        {
            get { return m_visible; }
            set
            {
                m_visible = value;
                OnVisibleChange();
                VisibleChanged?.Invoke(m_visible);
            }
        }

        public BaseScreen()
        {
            this.m_visible = true;
            this.IsProcessInput = false;
        }

        public virtual void OnVisibleChange()
        {
        }

        public virtual void AddToCanvas(DefaultCanvas canvas)
        {
        }

        public virtual void ApplyAnimation(BaseAnimation animation, bool reverse = false, Action onComplete = null, Action onBegin = null)
        {
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Mouse(MouseHandlerParams _params)
        {
        }

        public virtual void Touch(ref TouchHandlerParams _params)
        {
        }

    }

}
