using Microsoft.Xna.Framework;

namespace NetworkGame.Engine
{
    public class FixedColorAnimation : BaseAnimation
    {
        Color m_startColor;
        Color m_finalColor;

        public FixedColorAnimation(Color startColor, Color finalColor, float duration,
            BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            : base(duration, function, containerType, manager)
        {
            this.m_startColor = startColor;
            this.m_finalColor = finalColor;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new ColorAnimationContext()
            {
                StartR = m_startColor.R / 255f,
                FinalR = m_finalColor.R / 255f,

                StartG = m_startColor.G / 255f,
                FinalG = m_finalColor.G / 255f,

                StartB = m_startColor.B / 255f,
                FinalB = m_finalColor.B / 255f,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var colorContext = (ColorAnimationContext)context;

            colorContext.Sprite.ColorR = Function.GetValue(colorContext.Time, colorContext.StartR, colorContext.FinalR, Duration);
            colorContext.Sprite.ColorG = Function.GetValue(colorContext.Time, colorContext.StartG, colorContext.FinalG, Duration);
            colorContext.Sprite.ColorB = Function.GetValue(colorContext.Time, colorContext.StartB, colorContext.FinalB, Duration);
        }
    }

    public class FixedFadeAnimation : BaseAnimation
    {
        float m_startA;
        float m_finishA;

        public FixedFadeAnimation(float startA, float finishA, float duration,
            BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            : base(duration, function, containerType, manager)
        {
            this.m_startA = startA;
            this.m_finishA = finishA;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new ValueAnimationContext()
            {
                StartValue = m_startA,
                FinalValue = m_finishA,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var colorContext = (ValueAnimationContext)context;
            colorContext.Sprite.Alpha = Function.GetValue(colorContext.Time, colorContext.StartValue, colorContext.FinalValue, Duration);
        }
    }

    public class FixedFadeToAnimation : BaseAnimation
    {
        float m_fadeTo;

        public FixedFadeToAnimation(float fadeTo, float duration,
            BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            : base(duration, function, containerType, manager)
        {
            this.m_fadeTo = fadeTo;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new ValueAnimationContext()
            {
                StartValue = sprite.Alpha,
                FinalValue = m_fadeTo,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var colorContext = (ValueAnimationContext)context;
            colorContext.Sprite.Alpha = Function.GetValue(colorContext.Time, colorContext.StartValue, colorContext.FinalValue, Duration);
        }
    }

    public class FixedScaleAnimation : BaseAnimation
    {
        float m_startScaleX;
        float m_finalScaleX;
        float m_startScaleY;
        float m_finalScaleY;

        public FixedScaleAnimation(float startScaleX, float finalScaleX, float startScaleY, float finalScaleY, float duration,
            BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
            : base(duration, function, containerType, manager)
        {
            this.m_startScaleX = startScaleX;
            this.m_finalScaleX = finalScaleX;
            this.m_startScaleY = startScaleY;
            this.m_finalScaleY = finalScaleY;
        }

        protected override AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new VectorAnimationContext()
            {
                StartX = m_startScaleX,
                FinalX = m_finalScaleX,
                StartY = m_startScaleY,
                FinalY = m_finalScaleY,
            };
        }

        protected override void UpdateCore(AnimationContext context)
        {
            var vectorContext = (VectorAnimationContext)context;

            vectorContext.Sprite.ScaleX = Function.GetValue(vectorContext.Time, vectorContext.StartX, vectorContext.FinalX, Duration);
            vectorContext.Sprite.ScaleY = Function.GetValue(vectorContext.Time, vectorContext.StartY, vectorContext.FinalY, Duration);
        }

    }

}
