using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine
{
    public class DictionaryContainer : BaseContainer
    {
        Dictionary<BaseSprite, AnimationContext> m_dictionary;

        public DictionaryContainer()
        {
            m_dictionary = new Dictionary<BaseSprite, AnimationContext>();
        }

        public override void Add(AnimationContext context)
        {
            m_dictionary.Add(context.Sprite, context);
        }

        public override IEnumerable<AnimationContext> All()
        {
            return m_dictionary.Values;
        }

        public override IEnumerable<AnimationContext> Get(BaseSprite sprite)
        {
            AnimationContext context = null;
            if (m_dictionary.TryGetValue(sprite, out context))
                yield return context;
        }

        public override void Remove(Predicate<AnimationContext> match, IEnumerable<AnimationContext> contexts)
        {
            foreach (var context in contexts)
                m_dictionary.Remove(context.Sprite);
        }

        public override void Clear()
        {
            m_dictionary.Clear();
        }
    }
}
