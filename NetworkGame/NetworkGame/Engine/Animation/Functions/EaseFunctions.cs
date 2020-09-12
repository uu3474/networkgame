namespace NetworkGame.Engine
{
    public class EaseQuadFunction : BaseFunction
    {
        public override float GetValue(double currentTime, float startValue, float finalValue, float duration)
        {
            currentTime /= duration / 2;
            finalValue -= startValue;

            if (currentTime < 1)
                return (float)((finalValue) / 2 * currentTime * currentTime + startValue);

            return (float)(-(finalValue) / 2 * ((--currentTime) * (currentTime - 2) - 1) + startValue);
        }
    }
}
