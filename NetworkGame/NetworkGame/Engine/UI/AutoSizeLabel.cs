using Microsoft.Xna.Framework;
using NetworkGame.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine.UI
{
    class AutoSizeLabel
    {
        float m_paddingX;
        float m_paddingY;
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
        public Vector2 Padding
        {
            get { return new Vector2(m_paddingX, m_paddingY); }
            set
            {
                m_paddingX = value.X;
                m_paddingY = value.Y;
                SetBackSize();
            }
        }
        public float Scale
        {
            get { return m_caption.ScaleX; }
            set
            {
                m_caption.ScaleX = value;
                m_caption.ScaleY = value;
                SetBackSize();
            }
        }
        public int Width
        {
            get { return m_back.Width; }
        }
        public int Height
        {
            get { return m_back.Height; }
        }
        public string Text
        {
            get { return m_caption.Text; }
            set
            {
                m_caption.Text = value;
                SetBackSize();
            }
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

        public AutoSizeLabel(GameFont font)
        {
            this.m_back = new FilledRect() { Alpha = Game.Content.Colors.LabelBackAlpha };
            this.m_back.SetColor(Game.Content.Colors.LabelBack);

            this.m_caption = new TextSprite(font);
            this.m_caption.SetColor(Game.Content.Colors.ButtonText);
        }

        void SetBackSize()
        {
            m_back.Width = (int)(m_caption.Width * m_caption.ScaleX + m_paddingX * m_caption.ScaleX * 2);
            m_back.Height = (int)(m_caption.Height * m_caption.ScaleY + m_paddingY * m_caption.ScaleY * 2);
        }

        public void SetParams(string text, float scale, Vector2 padding)
        {
            m_caption.Text = text;
            m_caption.ScaleX = scale;
            m_caption.ScaleY = scale;
            m_paddingX = padding.X;
            m_paddingY = padding.Y;
            SetBackSize();
        }

        public void AddToCanvas(DefaultCanvas spriteCanvas)
        {
            spriteCanvas.Add(m_back);
            spriteCanvas.Add(m_caption);
        }
    }
}
