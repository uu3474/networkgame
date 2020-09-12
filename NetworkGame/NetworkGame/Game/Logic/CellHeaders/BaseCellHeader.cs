using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;

namespace NetworkGame.Logic
{
    public abstract class BaseCellHeader
    {
        public bool HasSignal { get; protected set; }

        public virtual float X { get; set; }
        public virtual float Y { get; set; }

        public virtual void Draw(SpriteBatch spriteBatch, BaseSprite viewport)
        {
        }

        public virtual void InitRotate(float rotation)
        {
        }

        public virtual void Rotate90(bool animate)
        {
        }

        public virtual void UpdateSignalCore(bool signal, bool init, bool animate)
        {
        }

        public void UpdateSignal(bool signal, bool init, bool animate)
        {
            if (HasSignal == signal)
                return;

            UpdateSignalCore(signal, init, animate);

            HasSignal = signal;
        }
    }
}
