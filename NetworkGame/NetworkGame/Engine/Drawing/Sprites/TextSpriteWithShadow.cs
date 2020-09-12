using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Content;

namespace NetworkGame.Engine
{
    public class TextSpriteWithShadow : TextSprite
    {
        public TextSpriteWithShadow(GameFont font)
            :base(font)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, BaseSprite viewport = null)
        {
            if (!GetCompleteVisible())
                return;

            spriteBatch.DrawString(Font.Font, Text,
                GetCompletePosition(viewport) + Vector2.One,
                Color.Black,
                Rotation,
                GetCompleteOrigin(),
                GetCompleteScale(viewport),
                Effects,
                Depth);

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
