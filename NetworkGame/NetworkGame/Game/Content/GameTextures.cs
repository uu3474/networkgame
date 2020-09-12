using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;

namespace NetworkGame.Content
{
    public class GameTextures
    {
        Atlas _menuAtlas;
        Atlas _fieldAtlas;
        Atlas _fieldHeadersAtlas;

        public readonly Texture2D MainLogo;
        public readonly Texture2D BackgroundSignalPart;
        public readonly Texture2D LoadingScreenSpinner;
        public readonly Texture2D MenuIcon;

        public readonly Frame Pixel2;
        public readonly Frame ButtonCircle;

        public readonly Frame WireAngle2;
        public readonly Frame WireAngle3;
        public readonly Frame WireAngle4;
        public readonly Frame WireEnd;
        public readonly Frame WireLine;

        public readonly Frame DisplayBack;
        public readonly Frame DisplayFront;
        public readonly Frame DisplayGlobe;
        public readonly Frame DisplaySpinner;

        public GameTextures(GameCommon common)
        {
            var prefix = common.SizeChooser.SizeMultiplierPrefix;

            this.MainLogo = common.Manager.Load<Texture2D>($"{prefix}/main_logo");
            this.BackgroundSignalPart = common.Manager.Load<Texture2D>($"{prefix}/background_signal_part");
            this.LoadingScreenSpinner = common.Manager.Load<Texture2D>($"{prefix}/loading_screen_spinner");
            this.MenuIcon = common.Manager.Load<Texture2D>($"{prefix}/menu_icon");

            this._menuAtlas = new Atlas(common.Manager, $"{prefix}/menuAtlas", $"{prefix}/menuAtlasDesc");
            this.Pixel2 = this._menuAtlas["pixel2"];
            this.ButtonCircle = this._menuAtlas["button_circle"];

            this._fieldAtlas = new Atlas(common.Manager, $"{prefix}/fieldAtlas", $"{prefix}/fieldAtlasDesc");
            this.WireAngle2 = this._fieldAtlas["wire_angle_2"];
            this.WireAngle3 = this._fieldAtlas["wire_angle_3"];
            this.WireAngle4 = this._fieldAtlas["wire_angle_4"];
            this.WireEnd = this._fieldAtlas["wire_end"];
            this.WireLine = this._fieldAtlas["wire_line"];

            this._fieldHeadersAtlas = new Atlas(common.Manager, $"{prefix}/fieldHeadersAtlas", $"{prefix}/fieldHeadersAtlasDesc");
            this.DisplayBack = this._fieldHeadersAtlas["display_back"];
            this.DisplayFront = this._fieldHeadersAtlas["display_front"];
            this.DisplayGlobe = this._fieldHeadersAtlas["display_globe"];
            this.DisplaySpinner = this._fieldHeadersAtlas["display_spinner"];
        }

    }

}
