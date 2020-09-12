using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NetworkGame.Engine
{
    public abstract class CanvasScreen : BaseScreen
    {
        Sprite m_canvasSprite;
        MouseHandlerParams m_mouseParams;

        public BaseCanvas Canvas { get; protected set; }

        public override float X
        {
            get { return m_canvasSprite.X; }
            set { m_canvasSprite.X = value; }
        }
        public override float Y
        {
            get { return m_canvasSprite.Y; }
            set { m_canvasSprite.Y = value; }
        }

        public CanvasScreen(GraphicsDevice device, int width, int height)
        {
            this.m_mouseParams = new MouseHandlerParams();
            this.IsProcessInput = false;
            CreateCanvas(device, width, height);
            this.m_canvasSprite = new Sprite()
            {
                Texture = this.Canvas.RenderTarget,
                X = device.Viewport.Width / 2,
                Y = device.Viewport.Height / 2,
                Visible = this.Visible,
            };
        }

        public CanvasScreen(GraphicsDevice device)
            :this(device, device.Viewport.Width, device.Viewport.Height)
        {
        }

        public virtual void CreateCanvas(GraphicsDevice device, int width, int height)
        {
        }

        public override void OnVisibleChange()
        {
            m_canvasSprite.Visible = Visible;
        }

        public override void AddToCanvas(DefaultCanvas canvas)
        {
            canvas.Add(m_canvasSprite);
        }

        public override void ApplyAnimation(BaseAnimation animation, bool reverse = false, Action onComplete = null, Action onBegin = null)
        {
            animation.Apply(m_canvasSprite, reverse, onComplete, onBegin);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            DrawCore(gameTime);
            Canvas.Draw(gameTime);
        }

        protected virtual void DrawCore(GameTime gameTime)
        {
        }

        public override void Back()
        {
            if (!Visible)
                return;

            if (!IsProcessInput)
                return;

            BackCore();
        }

        protected virtual void BackCore()
        {
        }

        public override void Mouse(MouseHandlerParams _params)
        {
            if (!Visible)
                return;

            if (!IsProcessInput)
                return;

            if (_params.Handled)
                return;

            if (!m_canvasSprite.GetBoundingBoxNoRotation().Contains(_params.Position))
                return;

            m_mouseParams.CopyFrom(_params, new Vector2(-(m_canvasSprite.X - m_canvasSprite.OriginX), -(m_canvasSprite.Y - m_canvasSprite.OriginY)));

            MouseCore(m_mouseParams);

            _params.Handled = m_mouseParams.Handled;
        }

        protected virtual void MouseCore(MouseHandlerParams _params)
        {
        }

        public override void Touch(ref TouchHandlerParams _params)
        {
            if (!Visible)
                return;

            if (!IsProcessInput)
                return;

            if (_params.Handled)
                return;

            if (!_params.IsGestureAvailable)
                return;

            if (!m_canvasSprite.GetBoundingBoxNoRotation().Contains(_params.Gesture.Position))
                return;

            var screentouchParams = _params.Copy(new Vector2(-(m_canvasSprite.X - m_canvasSprite.OriginX), -(m_canvasSprite.Y - m_canvasSprite.OriginY)));

            TouchCore(ref screentouchParams);

            _params.Handled = screentouchParams.Handled;
        }

        protected virtual void TouchCore(ref TouchHandlerParams _params)
        {
        }

    }

}
