using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Engine
{
    public class Sprite : BaseSprite
    {
        Texture2D m_texture;

        public override int Width { get { return m_texture.Width; } }
        public override int Height { get { return m_texture.Height; } }
        public Texture2D Texture
        {
            get { return m_texture; }
            set
            {
                m_texture = value;
                OriginX = m_texture.Width / 2f;
                OriginY = m_texture.Height / 2f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, BaseSprite viewport = null)
        {
            if (!GetCompleteVisible())
                return;

            spriteBatch.Draw(Texture,
                GetCompletePosition(viewport),
                null,
                GetCompleteColor(),
                Rotation,
                GetCompleteOrigin(),
                GetCompleteScale(viewport),
                Effects,
                Depth);
        }
    }
}
