using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NetworkGame.Content
{
    public class GameCommon
    {
        public readonly OpeningGameContent OpeningContent;
        public readonly ContentManager Manager;
        public readonly ContentSizeChooser SizeChooser;

        public readonly bool ShowExitButton;
        public readonly int FieldPadding;
        public readonly bool UseAdvancedEffects;
        public readonly int FieldShadowLength;

        public GameCommon(GraphicsDevice device, OpeningGameContent openingContent, GameParams _params)
        {
            this.OpeningContent = openingContent;
            this.Manager = openingContent.Manager;
            this.SizeChooser = new ContentSizeChooser(_params.DPI);

            this.ShowExitButton = (_params.Exit != null);
            this.FieldPadding = GetFieldPadding(device.Viewport.Width, device.Viewport.Height, _params.DPI);
            this.UseAdvancedEffects = _params.UseAdvancedEffects;
            this.FieldShadowLength = GetSizeInDpi(16); 
        }

        public int GetSizeInDpi(int size)
        {
            return SizeChooser.GetSizeInDpi(size);
        }

        public int GetFieldPadding(int width, int height, int dpi)
        {
            const int minPadding = 100;
            const int maxPadding = 200;
            const int maxInchs = 8;

            int minSide = Math.Min(width, height);
            float inchs = minSide / dpi;
            if (inchs > maxInchs)
                inchs = maxInchs;

            int padding = (int)(maxPadding * (inchs / maxInchs));
            if (padding < minPadding)
                padding = minPadding;
            if (padding > maxPadding)
                padding = maxPadding;

            return GetSizeInDpi(padding);
        }

    }

}
