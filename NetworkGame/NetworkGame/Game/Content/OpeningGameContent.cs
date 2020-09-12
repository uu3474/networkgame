using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NetworkGame.Content
{
    public class OpeningGameContent
    {
        public readonly ContentManager Manager;
        public readonly ContentSizeChooser SizeChooser;

        public readonly Color BackgroundColor;
        //public readonly Texture2D MainLogoTexture;

        public OpeningGameContent(ContentManager manager, GameParams _params)
        {
            this.Manager = manager;
            this.SizeChooser = new ContentSizeChooser(_params.DPI);

            this.BackgroundColor = new Color(71, 85, 86);

            var prefix = SizeChooser.SizeMultiplierPrefix;
            //this.MainLogoTexture = this.Manager.Load<Texture2D>(prefix + "/main_logo");
        }

    }

}
