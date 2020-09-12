using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetworkGame.Engine
{
    public class Atlas
    {
        Dictionary<string, Frame> m_frames;

        public Texture2D Texture { get; protected set; }

        public Atlas(ContentManager manager, string atlasName, string atlasXmlDescName)
        {
            this.Texture = manager.Load<Texture2D>(atlasName);
            var rawFrames = manager.Load<Dictionary<string, Rectangle>>(atlasXmlDescName);
            this.m_frames = rawFrames.ToDictionary(x => x.Key, x => new Frame(this, x.Value));
        }

        public Frame this[string fileName]
        {
            get
            {
                Frame frame;
                if (m_frames.TryGetValue(fileName, out frame))
                    return frame;

                return null;
            }
        }

    }
}

