using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;
using NetworkGame.Logic;

namespace NetworkGame.Drawing
{
    public class FieldCanvas : DefaultCanvas
    {
        Cell[,] m_cells;
        CellsCanvas m_cellsCanvas;
        Sprite m_cellsSprite;

        public float CellsOffsetX;
        public float CellsOffsetY;
        public int SideSize;
        public Cell[,] Cells
        {

            get { return m_cells; }
            set
            {
                m_cells = value;
                m_cellsCanvas.Cells = m_cells;
            }
        }

        public FieldCanvas(GraphicsDevice device, bool createTexture = true)
            : base(device, createTexture)
        {
            this.m_cellsCanvas = new CellsCanvas(device, createTexture);
            this.Init();
        }

        public FieldCanvas(GraphicsDevice device, int width, int height)
            : base(device, width, height)
        {
            this.m_cellsCanvas = new CellsCanvas(device, width, height);
            this.Init();
        }

        void Init()
        {
            m_cellsSprite = new Sprite()
            {
                Texture = m_cellsCanvas.RenderTarget,
                X = m_device.Viewport.Width / 2,
                Y = m_device.Viewport.Height / 2,
                Fixed = true,
            };
        }

        public override void Clear()
        {
            Cells = null;
            SideSize = 0;

            base.Clear();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Cells == null && SideSize <= 0)
                return;

            int xBeginIndex = (int)((View.X + CellsOffsetX) / SideSize) - 1;
            int yBeginIndex = (int)((View.Y + CellsOffsetY) / SideSize) - 1;
            if (xBeginIndex >= Cells.GetLength(0) || yBeginIndex >= Cells.GetLength(1))
                return;

            int xEndIndex = xBeginIndex + (int)(View.Width * (1f / View.ScaleX)) / SideSize + 2;
            int yEndIndex = yBeginIndex + (int)(View.Height * (1f / View.ScaleX)) / SideSize + 2;
            if (xEndIndex < 0 || yEndIndex < 0)
                return;

            if (xBeginIndex < 0)
                xBeginIndex = 0;

            if (yBeginIndex < 0)
                yBeginIndex = 0;

            if (xEndIndex >= Cells.GetLength(0))
                xEndIndex = Cells.GetLength(0) - 1;

            if (yEndIndex >= Cells.GetLength(1))
                yEndIndex = Cells.GetLength(1) - 1;

            m_cellsCanvas.CustomDraw(xBeginIndex, xEndIndex, yBeginIndex, yEndIndex, View);

            bool useAdvancedEffects = Game.Content.Common.UseAdvancedEffects;
            if (useAdvancedEffects)
            {
                Effect = Game.Content.Effects.LongShadow;
                Effect.Parameters["ShadowLength"].SetValue(Game.Content.Common.FieldShadowLength * View.ScaleX);
                Effect.Parameters["PixelWidth"].SetValue(1f / m_cellsSprite.Width);
                Effect.Parameters["PixelHeight"].SetValue(1f / m_cellsSprite.Height);
                BeginDraw();
                m_cellsSprite.Draw(m_spriteBatch, (m_cellsSprite.Fixed ? null : View));
                EndDraw(true);
                Effect = null;
            }

            BeginDraw(useAdvancedEffects);
            m_cellsSprite.Draw(m_spriteBatch, (m_cellsSprite.Fixed ? null : View));
            DrawSprites();
            EndDraw();
        }

    }

}
