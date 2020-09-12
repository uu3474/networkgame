using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Content
{
    public class GameContent
    {
        public GameCommon Common { get;  }
        public GameColors Colors { get;  }
        public GameTextures Textures { get;  }
        public GameEffects Effects { get; }
        public GameFonts Fonts { get;  }
        public GameAnimations Animations { get; }

        public GameContent(GraphicsDevice device, OpeningGameContent openingContent, GameParams _params)
        {
            this.Common = new GameCommon(device, openingContent, _params);

            this.Colors = new GameColors(this.Common);
            this.Textures = new GameTextures(this.Common);
            this.Effects = new GameEffects(this.Common);
            this.Fonts = new GameFonts(this.Common);
            this.Animations = new GameAnimations(this.Common, this.Colors);
        }

        public int GetSizeInDpi(int size)
        {
            return Common.GetSizeInDpi(size);
        }

    }

}
