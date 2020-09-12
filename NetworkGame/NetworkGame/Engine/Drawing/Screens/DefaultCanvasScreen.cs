using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine
{
    public class DefaultCanvasScreen : CanvasScreen
    {
        public new DefaultCanvas Canvas
        {
            get { return (DefaultCanvas)base.Canvas; }
            protected set { base.Canvas = value; }
        }

        public DefaultCanvasScreen(GraphicsDevice device, int width, int height)
            :base(device, width, height)
        {
        }

        public DefaultCanvasScreen(GraphicsDevice device)
            : base(device)
        {
        }

        public override void CreateCanvas(GraphicsDevice device, int width, int height)
        {
            Canvas = new DefaultCanvas(device, width, height);
        }
    }
}
