namespace NetworkGame.Engine
{
    public class FilledRect : AtlasSprite
    {
        public override int Width
        {
            get { return (int)(ScaleX * 2); }
            set { ScaleX = (float)value / 2; }
        }
        public override int Height
        {
            get { return (int)(ScaleY * 2); }
            set { ScaleY = (float)value / 2; }
        }

        public FilledRect()
        {
            this.Frame = Game.Content.Textures.Pixel2;
            this.Width = 40;
            this.Height = 40;
        }
    }
}
