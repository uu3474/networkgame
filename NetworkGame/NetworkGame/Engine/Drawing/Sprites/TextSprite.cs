using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Content;

namespace NetworkGame.Engine
{
    public class TextSprite : BaseSprite
    {
        int m_width;
        int m_height;
        string m_text;

        public GameFont Font { get; protected set; }

        public override int Width { get { return m_width; } }
        public override int Height { get { return m_height; } }
        public string Text
        {
            get { return m_text; }
            set
            {
                m_text = value;
                var size = (string.IsNullOrEmpty(m_text) ? Vector2.Zero : Font.Font.MeasureString(m_text));
                m_width = (int)size.X;
                m_height = (int)size.Y;
                OriginX = m_width / 2;
                OriginY = m_height / 2 + Font.BaselineOffset;
            }
        }

        public TextSprite(GameFont font)
        {
            this.Font = font;
        }

        public override bool GetCompleteVisible()
        {
            return base.GetCompleteVisible() && !string.IsNullOrEmpty(Text);
        }

        public override void Draw(SpriteBatch spriteBatch, BaseSprite viewport = null)
        {
            if (!GetCompleteVisible())
                return;

            spriteBatch.DrawString(Font.Font, Text,
                GetCompletePosition(viewport),
                GetCompleteColor(),
                Rotation,
                GetCompleteOrigin(),
                GetCompleteScale(viewport),
                Effects,
                Depth);
        }
    }
}
