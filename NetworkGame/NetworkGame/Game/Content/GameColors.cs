using Microsoft.Xna.Framework;

namespace NetworkGame.Content
{
    public class GameColors
    {
        public readonly Color ButtonText;
        public readonly Color ButtonBack;
        public readonly float ButtonBackAlpha;

        public readonly Color LabelBack;
        public readonly float LabelBackAlpha;

        public readonly Color Background;

        public readonly Color SignalBackground;
        public readonly float SignalBackgroundAlpha;

        public readonly Color Substrate;
        public readonly float SubstrateAlpha;
        public readonly float SubstrateAlphaMouseOver;

        public readonly Color WireDisconnected;
        public readonly Color WireConnected;
        public readonly Color DisplayDisconnected;
        public readonly Color DisplayConnected;

        public readonly Color Router;
        public readonly Color RouterIcon;

        public readonly Color[] DifficultColors;

        public GameColors(GameCommon common)
        {
            this.ButtonText = Color.White;
            this.ButtonBack = Color.Black;
            this.ButtonBackAlpha = 0.2f;

            this.LabelBack = Color.Black;
            this.LabelBackAlpha = 0.1f;

            this.Background = common.OpeningContent.BackgroundColor;

            this.SignalBackground = new Color(this.Background.R - 4, this.Background.G - 4, this.Background.B - 4);
            this.SignalBackgroundAlpha = 1f;

            this.Substrate = Color.White;
            this.SubstrateAlpha = 0.03f;
            this.SubstrateAlphaMouseOver = 0.1f;

            this.WireDisconnected = new Color(140, 176, 161);
            this.WireConnected = new Color(245, 238, 210);
            this.DisplayDisconnected = Color.Gray;
            this.DisplayConnected = Color.CornflowerBlue;

            this.Router = Color.White;
            this.RouterIcon = Color.Gray;

            this.DifficultColors = new[]
            {
                Color.ForestGreen,
                Color.Yellow,
                Color.DarkOrange,
                Color.DarkRed,
            };
        }

    }

}
