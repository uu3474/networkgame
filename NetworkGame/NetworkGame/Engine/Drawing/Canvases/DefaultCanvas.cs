using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NetworkGame.Engine
{
    public class DefaultCanvas : BaseCanvas
    {
        List<BaseSprite> m_sprites;

        public DefaultCanvas(GraphicsDevice device, bool createTexture = true)
            : base(device, createTexture)
        {
            m_sprites = new List<BaseSprite>();
        }

        public DefaultCanvas(GraphicsDevice device, int width, int height)
            : base(device, width, height)
        {
            m_sprites = new List<BaseSprite>();
        }

        public virtual void Add(BaseSprite sprite)
        {
            m_sprites.Add(sprite);
        }

        public virtual void AddRange(IEnumerable<BaseSprite> sprites)
        {
            m_sprites.AddRange(sprites);
        }

        public virtual void Remove(BaseSprite sprite)
        {
            m_sprites.Remove(sprite);
        }

        public virtual void Clear()
        {
            m_sprites.Clear();
        }

        public override void Draw(GameTime gameTime)
        {
            BeginDraw();
            DrawSprites();
            EndDraw();
        }

        protected void DrawSprites()
        {
            foreach (var sprite in m_sprites)
            {
                if (sprite.GetCompleteVisible() && View.IsInViewPortByBoundingRadius(sprite))
                    sprite.Draw(m_spriteBatch, (sprite.Fixed ? null : View));
            }
        }
    }
}
