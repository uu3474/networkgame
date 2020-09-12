using Microsoft.Xna.Framework;

namespace NetworkGame.Engine
{
    public class Area : IUpdatable
    {
        const float aaInMillisecond = 0.001f;

        bool m_isDrag;
        float m_brakingAX;
        float m_brakingSpeedX;
        float m_brakingAY;
        float m_brakingSpeedY;

        float m_deltaX;
        float m_deltaY;

        public float Padding;

        public float BorderLeft;
        public float BorderTop;
        public float BorderRight;
        public float BorderBottom;

        public Viewport View;

        public ContentAligin HorizontalAligin;
        public ContentAligin VerticalAligin;

        float ReverseScale { get { return 1f / View.ScaleX; } }

        int WidthWithScale { get { return (int)(View.Width * ReverseScale); } }
        int HeightWithScale { get { return (int)(View.Height * ReverseScale); } }

        float CompleteBorderLeft { get { return BorderLeft - Padding * ReverseScale; } }
        float CompleteBorderTop { get { return BorderTop - Padding * ReverseScale; } }
        float CompleteBorderRight { get { return BorderRight + Padding * ReverseScale; } }
        float CompleteBorderBottom { get { return BorderBottom + Padding * ReverseScale; } }

        public bool IsScrollX { get { return IsXScrollEnabled && (m_deltaX != 0); } }
        public bool IsScrollY { get { return IsYScrollEnabled && (m_deltaY != 0); } }
        public bool IsScroll { get { return IsScrollEnabled && (IsScrollX || IsScrollY); } }
        public bool IsXScrollEnabled { get { return WidthWithScale < CompleteBorderRight - CompleteBorderLeft; } }
        public bool IsYScrollEnabled { get { return HeightWithScale < CompleteBorderBottom - CompleteBorderTop; } }
        public bool IsScrollEnabled { get { return (IsXScrollEnabled || IsYScrollEnabled); } }

        public Area()
        {
            this.HorizontalAligin = ContentAligin.Center;
            this.VerticalAligin = ContentAligin.Center;
        }

        public void SetBorder(float left, float top, float right, float bottom)
        {
            BorderLeft = left;
            BorderTop = top;
            BorderRight = right;
            BorderBottom = bottom;
        }

        void CalcDeltaX(GameTime gameTime)
        {
            if (m_deltaX > 0)
            {
                m_deltaX -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * m_brakingSpeedX;
                m_brakingAX += aaInMillisecond;
                m_brakingSpeedX += m_brakingAX;

                if (m_deltaX <= 0)
                    StopX();
            }
            else if (m_deltaX < 0)
            {
                m_deltaX += (float)gameTime.ElapsedGameTime.TotalMilliseconds * m_brakingSpeedX;
                m_brakingAX += aaInMillisecond;
                m_brakingSpeedX += m_brakingAX;

                if (m_deltaX >= 0)
                    StopX();
            }
        }

        void MoveX()
        {
            View.X -= m_deltaX;

            if (View.X < CompleteBorderLeft)
            {
                View.X = CompleteBorderLeft;
                StopX();
            }

            if (View.X + WidthWithScale > CompleteBorderRight)
            {
                View.X = CompleteBorderRight - WidthWithScale;
                StopX();
            }
        }

        public void StopX()
        {
            m_brakingAX = 0;
            m_brakingSpeedX = 0;

            m_deltaX = 0;
        }

        void AliginX()
        {
            switch (HorizontalAligin)
            {
                case ContentAligin.LeftOrTop:
                    View.X = CompleteBorderLeft;
                    break;
                case ContentAligin.Center:
                    View.X = (CompleteBorderRight + CompleteBorderLeft) / 2 - WidthWithScale / 2;
                    break;
                case ContentAligin.RightOrBottom:
                    View.X = CompleteBorderRight - WidthWithScale;
                    break;
                default:
                    break;
            }
        }

        void CalcDeltaY(GameTime gameTime)
        {
            if (m_deltaY > 0)
            {
                m_deltaY -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * m_brakingSpeedY;
                m_brakingAY += aaInMillisecond;
                m_brakingSpeedY += m_brakingAY;

                if (m_deltaY <= 0)
                    StopY();
            }
            else if (m_deltaY < 0)
            {
                m_deltaY += (float)gameTime.ElapsedGameTime.TotalMilliseconds * m_brakingSpeedY;
                m_brakingAY += aaInMillisecond;
                m_brakingSpeedY += m_brakingAY;

                if (m_deltaY >= 0)
                    StopY();
            }
        }

        void MoveY()
        {
            View.Y -= m_deltaY;

            if (View.Y < CompleteBorderTop)
            {
                View.Y = CompleteBorderTop;
                StopY();
            }

            if (View.Y + HeightWithScale > CompleteBorderBottom)
            {
                View.Y = CompleteBorderBottom - HeightWithScale;
                StopY();
            }
        }

        public void StopY()
        {
            m_brakingAY = 0;
            m_brakingSpeedY = 0;

            m_deltaY = 0;
        }

        void AliginY()
        {
            switch (VerticalAligin)
            {
                case ContentAligin.LeftOrTop:
                    View.Y = CompleteBorderTop;
                    break;
                case ContentAligin.Center:
                    View.Y = (CompleteBorderBottom + CompleteBorderTop) / 2 - HeightWithScale / 2;
                    break;
                case ContentAligin.RightOrBottom:
                    View.Y = CompleteBorderBottom - HeightWithScale;
                    break;
                default:
                    break;
            }
        }

        public void Stop()
        {
            StopX();
            StopY();
        }

        public void Aligin()
        {
            AliginX();
            AliginY();
        }

        public void Update(GameTime gameTime)
        {
            if (m_isDrag)
            {
                m_isDrag = false;
            }
            else
            {
                if (IsXScrollEnabled)
                    CalcDeltaX(gameTime);

                if (IsYScrollEnabled)
                    CalcDeltaY(gameTime);
            }

            if (IsXScrollEnabled)
                MoveX();
            else
                AliginX();

            if (IsYScrollEnabled)
                MoveY();
            else
                AliginY();
        }

        public void Drag(Vector2 delta)
        {
            if (!IsScrollEnabled)
                return;

            m_isDrag = true;

            m_brakingAX = 0;
            m_brakingSpeedX = 0;
            m_brakingAY = 0;
            m_brakingSpeedY = 0;

            m_deltaX = delta.X * ReverseScale;
            m_deltaY = delta.Y * ReverseScale;
        }

        public bool CanScaleTo(float scale)
        {
            return IsScrollEnabled || scale > View.ScaleX;
        }

        public void Scale(Vector2 position, float scale)
        {
            if (!CanScaleTo(scale))
                return;

            View.X -= (position.X / View.Width) * (View.Width * (1f / scale - ReverseScale));
            View.Y -= (position.Y / View.Height) * (View.Height * (1f / scale - ReverseScale));

            View.ScaleX = scale;
            View.ScaleY = scale;
        }

    }
}