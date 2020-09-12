namespace NetworkGame.Engine
{
    public class InfinitySpinnerAnimation : BaseAnimation
    {
        float m_diffRadians;
        float m_diffTicks;

        public InfinitySpinnerAnimation(float diffRadians, float diffTicks,
            ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            : base(diffTicks + 1, null, containerType, manager)
        {
            this.m_diffRadians = diffRadians;
            this.m_diffTicks = diffTicks;
        }

        protected override void UpdateCore(AnimationContext context)
        {
            if(context.Time / m_diffTicks >= 1)
            {
                context.Time = 0;
                context.Sprite.Rotation += m_diffRadians;
            }
        }
    }

    public class InfinityFlipAnimation : BaseAnimation
    {
        float m_flipTicks;
        float m_waitTicks;

        public InfinityFlipAnimation(float flipTicks, float waitTicks,
            ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            : base(flipTicks + waitTicks + 1, Functions.RoundtripQuad, containerType, manager)
        {
            this.m_flipTicks = flipTicks;
            this.m_waitTicks = waitTicks;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new ValueAnimationContext()
            {
                StartValue = sprite.ScaleX,
                FinalValue = 0,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var relativeContext = (ValueAnimationContext)context;

            if (relativeContext.Time < m_flipTicks)
                relativeContext.Sprite.ScaleX = Function.GetValue(relativeContext.Time, relativeContext.StartValue, relativeContext.FinalValue, m_flipTicks);
            else
                relativeContext.Sprite.ScaleX = 1;

            if (relativeContext.Time / (m_flipTicks + m_waitTicks) >= 1)
                relativeContext.Time = 0;
        }
    }
}
