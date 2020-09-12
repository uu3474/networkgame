using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Engine
{
    public class Frame
    {
        Atlas m_parentAtlas;

        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public Texture2D AtlasTexture { get { return m_parentAtlas.Texture; } }

        public Frame(Atlas parentAtlas, Rectangle frame)
        {
            this.m_parentAtlas = parentAtlas;
            this.X = frame.X;
            this.Y = frame.Y;
            this.Width = frame.Width;
            this.Height = frame.Height;
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }

    }

}
