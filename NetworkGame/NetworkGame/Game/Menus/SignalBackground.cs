using Microsoft.Xna.Framework;
using NetworkGame.Engine;
using System.Collections.Generic;

namespace NetworkGame.Menus
{
    public class SignalBackground : IUpdatable
    {
        const string Signal = "010011100100010101010110010001010101001000100000010001010100111001000100";
        const float SpeedInMillisecond = 0.05f;

        int m_lineLenght;
        int m_lineWidth;
        List<Sprite> m_sprites;
        BaseSprite m_linePivot;
        DefaultCanvas m_canvas;

        public float X
        {
            get { return m_linePivot.X; }
            set { m_linePivot.X = value; }
        }
        public float Y
        {
            get { return m_linePivot.Y; }
            set { m_linePivot.Y = value; }
        }
        public bool Visible
        {
            get { return m_linePivot.Visible; }
            set { m_linePivot.Visible = value; }
        }

        public int Width { get; protected set; }
        public int Height { get { return m_lineLenght; } }        

        public SignalBackground()
        {
            this.m_lineLenght = Game.Content.GetSizeInDpi(200);
            this.m_lineWidth = Game.Content.GetSizeInDpi(4);

            this.m_sprites = new List<Sprite>();
            this.m_linePivot = new BaseSprite();

            this.CreateLine(Signal);
        }

        void CreateLine(string signal)
        {
            m_sprites.Clear();

            int offsetX = 0;
            for (int i = 0; i < Signal.Length; i++)
            {
                var bit = Signal[i];
                
                if(i > 0 && bit != Signal[i - 1])
                {
                    var transitionLine = new Sprite()
                    {
                        X = offsetX + m_lineWidth / 2 - 1,
                        Y = 0,
                        Rotation = MathHelper.PiOver2,
                        Texture = Game.Content.Textures.BackgroundSignalPart,
                        Color = Game.Content.Colors.SignalBackground,
                        Alpha = Game.Content.Colors.SignalBackgroundAlpha,
                        IsPositionFloat = true,
                        Pivot = m_linePivot
                    };

                    offsetX += m_lineWidth - 1;
                    m_sprites.Add(transitionLine);
                }

                var signalLine = new Sprite()
                {
                    Y = (bit == '0' ? (m_lineWidth - m_lineLenght) / 2 : (m_lineLenght - m_lineWidth) / 2),
                    Texture = Game.Content.Textures.BackgroundSignalPart,
                    Color = Game.Content.Colors.SignalBackground,
                    Alpha = Game.Content.Colors.SignalBackgroundAlpha,
                    IsPositionFloat = true,
                    Pivot = m_linePivot
                };

                if (i > 0)
                {
                    signalLine.X = offsetX + m_lineLenght / 2 - 1;
                    offsetX += m_lineLenght - 1;
                }
                else
                {
                    signalLine.X = offsetX + m_lineLenght / 2;
                    offsetX += m_lineLenght;
                }

                m_sprites.Add(signalLine);
            }

            Width = offsetX;
        }

        public void AddToCanvas(DefaultCanvas spriteCanvas)
        {
            m_canvas = spriteCanvas;
            m_canvas.AddRange(m_sprites);
            InitAnimation();
        }

        void InitAnimation()
        {
            X = m_canvas.View.Width;

            int yMin = Height / 2;
            int yMax = m_canvas.View.Height - Height / 2;
            Y = (yMin > yMax ? m_canvas.View.Height / 2 : StaticRandom.Next(yMin, yMax));
        }

        public void Update(GameTime gameTime)
        {
            if (!Visible)
                return;

            X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * SpeedInMillisecond;
            if (X < -Width)
                InitAnimation();
        }

    }

}
