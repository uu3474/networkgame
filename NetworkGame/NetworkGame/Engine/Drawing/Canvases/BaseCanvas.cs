using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Engine
{
    public abstract class BaseCanvas : IDrawable
    {
        public const float DefaultDepthStep = 0.0001f;

        protected GraphicsDevice m_device;
        protected SpriteBatch m_spriteBatch;

        public bool SortSprites { get; set; }
        public Matrix TransformMatrix { get; set; }
        public Effect Effect { get; set; }
        public RenderTarget2D RenderTarget { get; protected set; }
        public Viewport View { get; protected set; }

        BaseCanvas(GraphicsDevice device, bool createTexture, int width, int height)
        {
            if (width % 2 == 1)
                ++width;
            if (height % 2 == 1)
                ++height;

            this.SortSprites = false;
            this.TransformMatrix = Matrix.Identity;
            this.m_device = device;
            this.m_spriteBatch = new SpriteBatch(this.m_device);
            this.View = new Viewport() { Width = width, Height = height };
            if (createTexture)
            {
                this.RenderTarget = new RenderTarget2D(this.m_device, width, height, false, this.m_device.PresentationParameters.BackBufferFormat, DepthFormat.None);
                this.View.Alpha = 0f;
            }
        }

        public BaseCanvas(GraphicsDevice device, bool createTexture = true)
            : this(device, createTexture, device.Viewport.Width, device.Viewport.Height)
        {
        }

        public BaseCanvas(GraphicsDevice device, int width, int height)
            : this(device, true, width, height)
        {
        }

        protected void BeginDraw(bool continueDraw = false)
        {
            if (!continueDraw)
            {
                if (RenderTarget != null)
                    m_device.SetRenderTarget(RenderTarget);

                m_device.Clear(View.GetCompleteColor());
            }

            m_spriteBatch.Begin(sortMode: (SortSprites ? SpriteSortMode.FrontToBack : SpriteSortMode.Deferred),
                blendState: BlendState.AlphaBlend,
                effect: Effect,
                transformMatrix: TransformMatrix);
        }

        public abstract void Draw(GameTime gameTime);

        protected void EndDraw(bool continueDraw = false)
        {
            m_spriteBatch.End();

            if (!continueDraw)
            {
                if (RenderTarget != null)
                    m_device.SetRenderTarget(null);
            }
        }

    }

}
