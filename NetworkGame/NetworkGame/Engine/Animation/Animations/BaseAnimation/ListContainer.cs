using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine
{
    public class ListContainer : BaseContainer
    {
        List<AnimationContext> m_list;

        public ListContainer()
        {
            m_list = new List<AnimationContext>();
        }

        public override void Add(AnimationContext context)
        {
            m_list.Add(context);
        }

        public override IEnumerable<AnimationContext> Get(BaseSprite sprite)
        {
            foreach (var context in m_list)
            {
                if (context.Sprite == sprite)
                    yield return context;
            }
        }

        public override IEnumerable<AnimationContext> All()
        {
            return m_list;
        }

        public override void Remove(Predicate<AnimationContext> match, IEnumerable<AnimationContext> contexts)
        {
            m_list.RemoveAll(match);
        }

        public override void Clear()
        {
            m_list.Clear();
        }
    }
}
