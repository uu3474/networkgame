namespace NetworkGame.Engine
{
    public static class Functions
    {
        public static EaseQuadFunction Quad { get; private set; } = new EaseQuadFunction();
        public static RoundtripFunction RoundtripQuad { get; private set; } = new RoundtripFunction(Functions.Quad);

    }
}
