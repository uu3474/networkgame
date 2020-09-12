using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Engine
{
    public class AtlasSprite : BaseSprite
    {
        Frame m_frame;

        public override int Width { get { return m_frame.Width; } }
        public override int Height { get { return m_frame.Height; } }
        public Frame Frame
        {
            get { return m_frame; }
            set
            {
                m_frame = value;
                OriginX = m_frame.Width / 2f;
                OriginY = m_frame.Height / 2f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, BaseSprite viewport = null)
        {
            if (!GetCompleteVisible())
                return;

            spriteBatch.Draw(Frame.AtlasTexture,
                GetCompletePosition(viewport),
                Frame.GetSourceRectangle(),
                GetCompleteColor(),
                Rotation,
                GetCompleteOrigin(),
                GetCompleteScale(viewport),
                Effects,
                Depth);
        }

    }

}
