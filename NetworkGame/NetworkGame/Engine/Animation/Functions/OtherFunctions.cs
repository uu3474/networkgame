namespace NetworkGame.Engine
{
    public class RoundtripFunction : BaseFunction
    {
        protected BaseFunction _Function { get; private set; }

        public RoundtripFunction(BaseFunction function = null)
        {
            this._Function = (function == null ? Functions.Quad : function);
        }

        public override float GetValue(double currentTime, float startValue, float finalValue, float duration)
        {
            return _Function.GetValue(
                (currentTime < duration / 2) ? (currentTime * 2) : ((duration - currentTime) * 2),
                startValue, finalValue, duration);
        }
    }
}
