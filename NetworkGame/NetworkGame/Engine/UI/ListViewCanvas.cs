using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine.UI
{
    class ListViewCanvas<ItemType> : BaseCanvas
        where ItemType : ListViewItem
    {
        public List<ItemType> Items { get; set; }
        public int Spacing { get; set; }

        public ListViewCanvas(GraphicsDevice device, bool createTexture = true)
            : base(device, createTexture)
        {
        }

        public ListViewCanvas(GraphicsDevice device, int width, int height)
            : base(device, width, height)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            BeginDraw();

            if (Items == null || !Items.Any())
                return;

            var itemHeight = Items.First().Height + Spacing;

            var startIndex = (int)(View.Y / itemHeight) - 1;
            if (startIndex >= Items.Count)
                return;

            var endIndex = startIndex + View.Height / itemHeight + 2;
            if (endIndex < 0)
                return;

            if (startIndex < 0)
                startIndex = 0;

            if (endIndex >= Items.Count)
                endIndex = Items.Count - 1;

            for (int i = startIndex; i < endIndex + 1; i++)
                Items[i].Draw(m_spriteBatch, View);

            EndDraw();
        }

    }
}
