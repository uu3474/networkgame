using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NetworkGame.Engine
{
    public class AnimationManager : IUpdatable
    {
        public static AnimationManager DefaultManager { get; private set; } = new AnimationManager();

        List<BaseAnimation> m_animations;

        public AnimationManager()
        {
            this.m_animations = new List<BaseAnimation>();
        }

        public void Add(BaseAnimation animation)
        {
            m_animations.Add(animation);
        }

        public IEnumerable<BaseAnimation> Animations()
        {
            foreach (var animation in m_animations)
                yield return animation;
        }

        public void Update(GameTime gameTime)
        {
            m_animations.ForEach(x => x.Update(gameTime));
        }

    }

}
