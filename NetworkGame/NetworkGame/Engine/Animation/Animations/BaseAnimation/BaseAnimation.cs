using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace NetworkGame.Engine
{
    public enum ContainerType
    {
        List = 0,
        Dictionary = 1,
    }

    public abstract class BaseAnimation : IUpdatable
    {
        AnimationContext m_context;
        ContainerType m_containerType;
        BaseContainer m_container;
        List<AnimationContext> m_contextsToRemove;

        protected float Duration { get; private set; }
        protected BaseFunction Function { get; private set; }

        public AnimationManager Manager { get; protected set; }

        public BaseAnimation(float duration, BaseFunction function = null, ContainerType containerType = ContainerType.List, AnimationManager manager = null)
        {
            this.Duration = duration;
            this.Function = (function == null ? Functions.Quad : function);
            this.Manager = (manager == null ? AnimationManager.DefaultManager : manager);
            this.Manager.Add(this);

            this.m_containerType = containerType;
        }

        public void Apply(BaseSprite sprite, bool reverse = false, Action onComplete = null, Action onBegin = null)
        {
            var context = ApplyCore(sprite);
            context.Sprite = sprite;
            context.Reverse = reverse;
            context.Begin = onBegin;
            context.Complete = onComplete;

            context.Time = (context.Reverse ? Duration : 0);

            if (m_container != null)
            {
                m_container.Add(context);
            }
            else if (m_context == null)
            {
                m_context = context;
            }
            else
            {
                m_contextsToRemove = new List<AnimationContext>();
                switch (m_containerType)
                {
                    case ContainerType.List:
                        m_container = new ListContainer();
                        break;
                    case ContainerType.Dictionary:
                        m_container = new DictionaryContainer();
                        break;
                    default:
                        break;
                }
                m_container.Add(m_context);
                m_context = null;
                m_container.Add(context);
            }
        }

        protected virtual AnimationContext ApplyCore(BaseSprite sprite)
        {
            return new AnimationContext();
        }

        public bool Revert(BaseSprite sprite)
        {
            if (m_container != null)
            {
                bool result = false;
                foreach (var context in m_container.Get(sprite))
                {
                    context.Reverse = !context.Reverse;
                    result = true;
                }
                return result;
            }
            else if (m_context != null)
            {
                if (m_context.Sprite == sprite)
                {
                    m_context.Reverse = !m_context.Reverse;
                    return true;
                }
                return false;
            }
            return false;
        }

        public void Clear()
        {
            if (m_container != null)
                m_container.Clear();
            else if (m_context != null)
                m_context = null;
        }

        bool IsContextEnd(AnimationContext context)
        {
            return context.IsEnd;
        }

        void UpdateContext(AnimationContext context, GameTime gameTime)
        {
            UpdateCore(context);

            if (!context.IsBegin)
                context.IsBegin = true;

            if (context.Reverse)
            {
                if(context.Time <= 0)
                {
                    context.IsEnd = true;
                    return;
                }

                context.Time -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (context.Time < 0)
                    context.Time = 0;
            }
            else
            {
                if (context.Time >= Duration)
                {
                    context.IsEnd = true;
                    return;
                }

                context.Time += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (context.Time > Duration)
                    context.Time = Duration;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (m_container != null)
            {
                foreach (var context in m_container.All())
                {
                    if (!context.IsBegin)
                        OnBegin(context);

                    UpdateContext(context, gameTime);

                    if (IsContextEnd(context))
                        m_contextsToRemove.Add(context);
                }

                m_container.Remove(IsContextEnd, m_contextsToRemove);

                foreach (var contextToRemove in m_contextsToRemove)
                    OnComplete(contextToRemove);
                m_contextsToRemove.Clear();
            }
            else if (m_context != null)
            {
                if (!m_context.IsBegin)
                    OnBegin(m_context);

                UpdateContext(m_context, gameTime);

                if (IsContextEnd(m_context))
                {
                    var context = m_context;
                    m_context = null;
                    OnComplete(context);
                }
            }
        }

        void OnBegin(AnimationContext context)
        {
            context.Begin?.Invoke();
            OnBeginCore(context);
        }

        void OnComplete(AnimationContext context)
        {
            context.Complete?.Invoke();
            OnCompleteCore(context);
        }

        protected virtual void UpdateCore(AnimationContext context)
        {
        }

        protected virtual void OnBeginCore(AnimationContext context)
        {
        }

        protected virtual void OnCompleteCore(AnimationContext context)
        {
        }
    }
}
