using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;
using NetworkGame.Logic;
using System;

namespace NetworkGame.Drawing
{
    class CellsCanvas : BaseCanvas
    {
        public Cell[,] Cells;

        public CellsCanvas(GraphicsDevice device, bool createTexture = true)
            : base(device, createTexture)
        {
        }

        public CellsCanvas(GraphicsDevice device, int width, int height)
            : base(device, width, height)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void CustomDraw(int xBeginIndex, int xEndIndex, int yBeginIndex, int yEndIndex, Engine.Viewport view)
        {
            if (Cells == null)
                return;

            BeginDraw();

            for (int x = xBeginIndex; x < xEndIndex + 1; x++)
            {
                for (int y = yBeginIndex; y < yEndIndex + 1; y++)
                {
                    var cell = Cells[x, y];
                    cell.WireSprite.Draw(m_spriteBatch, view);
                    if (cell.Header != null)
                        cell.Header.Draw(m_spriteBatch, view);
                }
            }

            EndDraw();
        }

    }

}