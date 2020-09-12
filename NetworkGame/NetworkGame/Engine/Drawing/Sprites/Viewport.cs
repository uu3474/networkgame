using Microsoft.Xna.Framework;

namespace NetworkGame.Engine
{
    public class Viewport : BaseSprite
    {
        public override int Width { get; set; }
        public override int Height { get; set; }

        public Rectangle GetViewPortBox(BaseSprite sprite)
        {
            return new Rectangle((sprite.Fixed ? 0 : (int)X), (sprite.Fixed ? 0 : (int)Y), Width, Height);
        }

        public bool IsInViewPortByBoundingRadius(BaseSprite sprite)
        {
            return GameMath.Intersects(sprite.GetBoundingCircle(), GetViewPortBox(sprite));
        }

        public bool IsInViewPortByBoundingBox(BaseSprite sprite)
        {
            return GetViewPortBox(sprite).Intersects(sprite.GetBoundingBox());
        }
    }
}
