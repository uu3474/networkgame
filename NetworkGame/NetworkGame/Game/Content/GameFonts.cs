using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Content
{
    public class GameFont
    {
        public int BaselineOffset;
        public SpriteFont Font;
    }

    public class GameFonts
    {
        public readonly GameFont ButtonFont;
        public readonly GameFont FieldStatusFont;

        public GameFonts(GameCommon common)
        {
            var prefix = common.SizeChooser.SizeMultiplierPrefix;

            this.ButtonFont = new GameFont()
            {
                Font = common.Manager.Load<SpriteFont>(prefix + "/button_font"),
                BaselineOffset = 2
            };
            this.FieldStatusFont = new GameFont()
            {
                Font = common.Manager.Load<SpriteFont>(prefix + "/field_status_font"),
                BaselineOffset = 1
            };
        }

    }

}
