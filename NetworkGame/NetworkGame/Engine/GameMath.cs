using Microsoft.Xna.Framework;

namespace NetworkGame.Engine
{
    public struct Circle
    {
        public float Radius;
        public float X;
        public float Y;

        public Circle(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public Circle(Vector2 position, float radius)
            : this(position.X, position.Y, radius)
        {
        }
    }

    public static class GameMath
    {
        public static bool Intersects(Circle circle, Rectangle rectangle)
        {
            // the first thing we want to know is if any of the corners intersect
            if (ContainsPoint(circle, new Point(rectangle.Top, rectangle.Left)))
                return true;
            if (ContainsPoint(circle, new Point(rectangle.Top, rectangle.Right)))
                return true;
            if (ContainsPoint(circle, new Point(rectangle.Bottom, rectangle.Right)))
                return true;
            if (ContainsPoint(circle, new Point(rectangle.Bottom, rectangle.Left)))
                return true;

            // next we want to know if the left, top, right or bottom edges overlap
            if (circle.X - circle.Radius > rectangle.Right || circle.X + circle.Radius < rectangle.Left)
                return false;

            if (circle.Y - circle.Radius > rectangle.Bottom || circle.Y + circle.Radius < rectangle.Top)
                return false;

            return true;
        }

        public static bool Intersects(Circle circle1, Circle circle2)
        {
            var center1 = new Vector2(circle1.X, circle1.Y);
            var center2 = new Vector2(circle2.X, circle2.Y);
            return Vector2.Distance(center1, center1) < circle2.Radius + circle1.Radius;
        }

        public static bool ContainsPoint(Circle circle, Point point)
        {
            var vector = new Vector2(point.X - circle.X, point.Y - circle.Y);
            return (vector.Length() <= circle.Radius);
        }
    }
}
