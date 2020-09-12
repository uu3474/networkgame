using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NetworkGame.Engine
{
    public class BaseSprite
    {
        public BaseSprite Pivot;
        public float X;
        public float Y;
        public float ColorR;
        public float ColorG;
        public float ColorB;
        public float Alpha;
        public float Rotation;
        public float OriginX;
        public float OriginY;
        public float ScaleX;
        public float ScaleY;
        public SpriteEffects Effects;
        public float Depth;
        public bool Visible;
        public bool Fixed;
        public bool IsPositionFloat;

        public virtual int Width
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }
        public virtual int Height
        {
            get { return 0; }
            set { throw new NotImplementedException(); }
        }

        public Color Color
        {
            get { return new Color(ColorR, ColorG, ColorB); }
            set { SetColor(value); }
        }
        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }
        public Vector2 Origin
        {
            get { return new Vector2(OriginX, OriginY); }
            set
            {
                OriginX = value.X;
                OriginY = value.Y;
            }
        }
        public Vector2 Scale
        {
            get { return new Vector2(ScaleX, ScaleY); }
            set
            {
                ScaleX = value.X;
                ScaleY = value.Y;
            }
        }

        public BaseSprite()
        {
            this.ColorR = 1f;
            this.ColorG = 1f;
            this.ColorB = 1f;
            this.Alpha = 1f;
            this.ScaleX = 1f;
            this.ScaleY = 1f;
            this.Visible = true;
        }

        public void SetColor(float r, float g, float b, float a)
        {
            SetColor(r, g, b);
            Alpha = a;
        }

        public void SetColor(float r, float g, float b)
        {
            ColorR = r;
            ColorG = g;
            ColorB = b;
        }

        public void SetColor(float color)
        {
            SetColor(color, color, color);
        }

        public void SetColor(Color color)
        {
            SetColor(color.R / 255f, color.G / 255f, color.B / 255f);
        }

        public Color GetCompleteColor()
        {
            return new Color(ColorR, ColorG, ColorB) * Alpha;
        }

        public virtual float GetBoundingRadius()
        {
            var width = Math.Max(OriginX, Width - OriginX) * ScaleX;
            var height = Math.Max(OriginY, Height - OriginY) * ScaleY;
            return (float)Math.Sqrt(width * width + height * height);
        }

        public virtual Circle GetBoundingCircle()
        {
            return new Circle(GetCompletePosition(), (int)GetBoundingRadius());
        }

        public virtual Rectangle GetBoundingBoxNoRotation()
        {
            Vector2 position = GetCompletePosition();
            return new Rectangle((int)(position.X - OriginX * ScaleX), (int)(position.Y - OriginY * ScaleY),
                (int)(Width * ScaleX), (int)(Height * ScaleY));
        }

        public virtual Rectangle GetBoundingBox()
        {
            var completeOrigin = GetCompleteOrigin();
            Matrix toWorldSpace =
               Matrix.CreateTranslation(new Vector3(-completeOrigin.X, -completeOrigin.Y, 0.0f)) *
               Matrix.CreateScale(ScaleX, ScaleY, 0) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateTranslation(new Vector3(GetCompletePosition(), 0.0f));

            Vector2 leftTop = new Vector2(0, 0);
            Vector2 rightTop = new Vector2(Width, 0);
            Vector2 leftBottom = new Vector2(0, Height);
            Vector2 rightBottom = new Vector2(Width, Height);

            Vector2.Transform(ref leftTop, ref toWorldSpace, out leftTop);
            Vector2.Transform(ref rightTop, ref toWorldSpace, out rightTop);
            Vector2.Transform(ref leftBottom, ref toWorldSpace, out leftBottom);
            Vector2.Transform(ref rightBottom, ref toWorldSpace, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public virtual Vector2 GetCompletePosition(BaseSprite viewport = null)
        {
            var position = new Vector2(X, Y);

            if (Pivot != null)
            {
                position.X += Pivot.X;
                position.Y += Pivot.Y;
            }

            if (viewport != null)
            {
                var viewportPosition = viewport.GetCompletePosition();
                position.X = position.X * viewport.ScaleX - viewportPosition.X * viewport.ScaleX;
                position.Y = position.Y * viewport.ScaleY - viewportPosition.Y * viewport.ScaleY;
            }

            if (IsPositionFloat)
                return position;

            position.X = (int)position.X;
            position.Y = (int)position.Y;

            return position;
        }

        public virtual Vector2 GetCompleteOrigin()
        {
            return new Vector2((int)Math.Floor(OriginX), (int)Math.Floor(OriginY));
        }

        public virtual Vector2 GetCompleteScale(BaseSprite viewport = null)
        {
            var scale = new Vector2(ScaleX, ScaleY);

            if (viewport != null)
            {
                scale.X = scale.X * viewport.ScaleX;
                scale.Y = scale.Y * viewport.ScaleY;
            }

            return scale;
        }

        public virtual bool GetCompleteVisible()
        {
            return Visible && (Pivot == null || Pivot.Visible) && Alpha > 0.001f;
        }

        public virtual void Draw(SpriteBatch spriteBatch, BaseSprite viewport = null)
        {
        }

    }
}
