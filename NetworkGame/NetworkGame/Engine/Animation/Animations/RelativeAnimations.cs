namespace NetworkGame.Engine
{
    public class RelativeTransitionAnimation : BaseAnimation
    {
        float m_diffX;
        float m_diffY;

        public RelativeTransitionAnimation(float diffX, float diffY, float duration,
            BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            :base(duration, function, containerType, manager)
        {
            this.m_diffX = diffX;
            this.m_diffY = diffY;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new RelativeVectorAnimationContext()
            {
                StartX = sprite.X,
                PrevX = sprite.X,
                FinalX = sprite.X + m_diffX,
                StartY = sprite.Y,
                PrevY = sprite.Y,
                FinalY = sprite.Y + m_diffY,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var relativeContext = (RelativeVectorAnimationContext)context;

            var newX = Function.GetValue(relativeContext.Time, relativeContext.StartX, relativeContext.FinalX, Duration);
            relativeContext.Sprite.X += newX - relativeContext.PrevX;
            relativeContext.PrevX = newX;

            var newY = Function.GetValue(relativeContext.Time, relativeContext.StartY, relativeContext.FinalY, Duration);
            relativeContext.Sprite.Y += newY - relativeContext.PrevY;
            relativeContext.PrevY = newY;
        }

    }

    public class RelativeRotateAnimation : BaseAnimation
    {
        float m_diffRadians;

        public RelativeRotateAnimation(float diffRadians, float duration,
            BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            : base(duration, function, containerType, manager)
        {
            this.m_diffRadians = diffRadians;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new RelativeValueAnimationContext()
            {
                StartValue = sprite.Rotation,
                PrevValue = sprite.Rotation,
                FinalValue = sprite.Rotation + m_diffRadians,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var relativeContext = (RelativeValueAnimationContext)context;

            var newValue = Function.GetValue(relativeContext.Time, relativeContext.StartValue, relativeContext.FinalValue, Duration);
            relativeContext.Sprite.Rotation += newValue - relativeContext.PrevValue;
            relativeContext.PrevValue = newValue;
        }
    }

    public class RelativePercentScaleAnimation : BaseAnimation
    {
        float m_percentScaleX;
        float m_percentScaleY;

        public RelativePercentScaleAnimation(float percentScaleX, float percentScaleY, float duration,
            BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            :base(duration, function, containerType, manager)
        {
            this.m_percentScaleX = percentScaleX;
            this.m_percentScaleY = percentScaleY;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new RelativeVectorAnimationContext()
            {
                StartX = sprite.ScaleX,
                PrevX = sprite.ScaleX,
                FinalX = sprite.ScaleX + sprite.ScaleX * m_percentScaleX,
                StartY = sprite.ScaleY,
                PrevY = sprite.ScaleY,
                FinalY = sprite.ScaleY + sprite.ScaleY * m_percentScaleY,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var relativeContext = (RelativeVectorAnimationContext)context;

            var newX = Function.GetValue(relativeContext.Time, relativeContext.StartX, relativeContext.FinalX, Duration);
            relativeContext.Sprite.ScaleX += newX - relativeContext.PrevX;
            relativeContext.PrevX = newX;

            var newY = Function.GetValue(relativeContext.Time, relativeContext.StartY, relativeContext.FinalY, Duration);
            relativeContext.Sprite.ScaleY += newY - relativeContext.PrevY;
            relativeContext.PrevY = newY;
        }

    }
}
