using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine
{
    public abstract class BaseContainer
    {
        public abstract void Add(AnimationContext context);
        public abstract IEnumerable<AnimationContext> Get(BaseSprite sprite);
        public abstract IEnumerable<AnimationContext> All();
        public abstract void Remove(Predicate<AnimationContext> match, IEnumerable<AnimationContext> contexts);
        public abstract void Clear();
    }
}
