using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;

namespace NetworkGame.Engine.UI
{
    public enum ButtonType : byte
    {
        Empty = 0,
        MainMenuButton = 1,
        FieldButton = 2,
    }

    public class BaseButton<ContentSpriteType> : IMouseHandler, ITouchHandler
        where ContentSpriteType : BaseSprite
    {
        FilledRect m_back;
        int m_width;
        int m_height;

        protected ContentSpriteType m_content;

        public float X
        {
            get { return m_back.X; }
            set
            {
                m_back.X = value;
                m_content.X = value;
            }
        }
        public float Y
        {
            get { return m_back.Y; }
            set
            {
                m_back.Y = value;
                m_content.Y = value;
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
        public bool Fixed
        {
            get { return m_back.Fixed; }
            set
            {
                m_back.Fixed = value;
                m_content.Fixed = value;
            }
        }
        public float Depth
        {
            get { return m_content.Depth; }
            set
            {
                m_content.Depth = value;
                m_back.Depth = value - DefaultCanvas.DefaultDepthStep;
            }
        }

        public BaseAnimation ClickAnimation { get; protected set; }
        public Action Click { get; set; }

        public BaseButton(ButtonType buttonType = ButtonType.Empty, BaseAnimation clickAnimation = null)
        {
            this.ClickAnimation = (clickAnimation == null ? Game.Content.Animations.Press10 : clickAnimation);

            this.m_back = new FilledRect() { Alpha = Game.Content.Colors.ButtonBackAlpha };
            this.m_back.SetColor(Game.Content.Colors.ButtonBack);

            switch (buttonType)
            {
                case ButtonType.MainMenuButton:
                    {
                        this.Width = Game.Content.GetSizeInDpi(200);
                        this.Height = Game.Content.GetSizeInDpi(40);
                    }
                    break;
                case ButtonType.FieldButton:
                    {
                        this.Width = Game.Content.GetSizeInDpi(40);
                        this.Height = Game.Content.GetSizeInDpi(40);
                    }
                    break;
                default:
                    break;
            }
        }

        public void AddToCanvas(DefaultCanvas spriteCanvas)
        {
            spriteCanvas.Add(m_back);
            spriteCanvas.Add(m_content);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)(X - Width / 2), (int)(Y - Height / 2), (int)(Width), (int)(Height));
        }

        void OnClick()
        {
            ClickAnimation?.Apply(m_back);
            ClickAnimation?.Apply(m_content);

            Click?.Invoke();
        }

        public void Mouse(MouseHandlerParams _params)
        {
            if (_params.Handled)
                return;

            if (!_params.IsLeftButtonPressed)
                return;

            if (!GetBoundingBox().Contains(_params.Position))
                return;

            _params.Handled = true;

            OnClick();
        }

        public void Touch(ref TouchHandlerParams _params)
        {
            if (_params.Handled)
                return;

            if (!_params.IsGestureAvailable)
                return;

            if (_params.Gesture.GestureType != GestureType.Tap)
                return;

            if (!GetBoundingBox().Contains(_params.Gesture.Position))
                return;

            _params.Handled = true;

            OnClick();
        }
    }
}
