namespace NetworkGame.Engine.UI
{
    class Label
    {
        int m_width;
        int m_height;
        FilledRect m_back;
        TextSprite m_caption;

        public float X
        {
            get { return m_back.X; }
            set
            {
                m_back.X = value;
                m_caption.X = value;
            }
        }
        public float Y
        {
            get { return m_back.Y; }
            set
            {
                m_back.Y = value;
                m_caption.Y = value;
            }
        }
        public int Width
        {
            get { return m_width; }
            set
            {
                m_width = value;
                m_back.Width = m_width;
            }
        }
        public int Height
        {
            get { return m_height; }
            set
            {
                m_height = value;
                m_back.Height = m_height;
            }
        }
        public string Text
        {
            get { return m_caption.Text; }
            set { m_caption.Text = value; }
        }
        public bool Fixed
        {
            get { return m_back.Fixed; }
            set
            {
                m_back.Fixed = value;
                m_caption.Fixed = value;
            }
        }
        public float Depth
        {
            get { return m_caption.Depth; }
            set
            {
                m_caption.Depth = value;
                m_back.Depth = value - DefaultCanvas.DefaultDepthStep;
            }
        }

        public Label()
        {
            this.m_back = new FilledRect() { Alpha = Game.Content.Colors.LabelBackAlpha };
            this.m_back.SetColor(Game.Content.Colors.LabelBack);

            this.m_caption = new TextSprite(Game.Content.Fonts.ButtonFont);
            this.m_caption.SetColor(Game.Content.Colors.ButtonText);
        }

        public void AddToCanvas(DefaultCanvas spriteCanvas)
        {
            spriteCanvas.Add(m_back);
            spriteCanvas.Add(m_caption);
        }
    }
}
