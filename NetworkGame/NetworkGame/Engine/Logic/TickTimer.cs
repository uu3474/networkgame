using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine
{
    public class TickTimerContext
    {
        public float Time { get; set; }
        public float Interval { get; set; }
        public bool Repeat { get; set; }
        public Action Tick { get; set; }
    }

    public class TickTimer : IUpdatable
    {
        List<TickTimerContext> m_context;

        public TickTimer()
        {
            m_context = new List<TickTimerContext>();
        }

        public void Add(float interval, Action tick, bool repeat = false)
        {
            m_context.Add(new TickTimerContext() { Time = interval, Interval = interval, Tick = tick, Repeat = repeat });
        }

        bool IsContextEnd(TickTimerContext context)
        {
            return (context.Time <= 0);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var context in m_context)
            {
                context.Time -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (IsContextEnd(context))
                {
                    context.Tick();
                    
                    if(context.Repeat)
                        context.Time = context.Interval;
                }
            }

            m_context.RemoveAll(IsContextEnd);
        }
    }
}
