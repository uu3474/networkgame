using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine
{
    public class AnimationContext
    {
        public BaseSprite Sprite { get; set; }
        public bool Reverse { get; set; }
        public double Time { get; set; }
        public bool IsEnd { get; set; }
        public bool IsBegin { get; set; }

        public Action Begin { get; set; }
        public Action Complete { get; set; }
    }

    public class ValueAnimationContext : AnimationContext
    {
        public float StartValue { get; set; }
        public float FinalValue { get; set; }
    }

    public class VectorAnimationContext : AnimationContext
    {
        public float StartX { get; set; }
        public float FinalX { get; set; }

        public float StartY { get; set; }
        public float FinalY { get; set; }
    }

    public class ColorAnimationContext : AnimationContext
    {
        public float StartR { get; set; }
        public float FinalR { get; set; }

        public float StartG { get; set; }
        public float FinalG { get; set; }

        public float StartB { get; set; }
        public float FinalB { get; set; }
    }

    public class RelativeValueAnimationContext : ValueAnimationContext
    {
        public float PrevValue { get; set; }
    }

    public class RelativeVectorAnimationContext : VectorAnimationContext
    {
        public float PrevX { get; set; }
        public float PrevY { get; set; }
    }
}
