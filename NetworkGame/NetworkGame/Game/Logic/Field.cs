using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using NetworkGame.Drawing;
using NetworkGame.Engine;
using NetworkGame.Engine.UI;
using NetworkGame.UI;
using System;
using System.Collections.Generic;

namespace NetworkGame.Logic
{
    public struct CompleteBundle
    {
        public LevelDescriptor Descriptor { get; set; }
        public int RotatesCount { get; set; }
        public TimeSpan Time { get; set; }
    }

    public class Field : CanvasScreen, IUpdatable
    {
        public static AnimationManager FieldManager { get; private set; } = new AnimationManager();

        int m_sideSize;
        int m_scaleSideSize;
        int m_scaleStep;
        float m_pitchDelta;
        int m_maxScale;
        int m_minScale;
        Area m_area;

        FieldData m_fieldData;
        bool m_isUpdateFieldData;
        Cell[,] m_cells;
        int m_cellOffsetX;
        int m_cellOffsetY;

        Cell m_sourceCell;
        int m_displaysCount;

        HashSet<Cell> m_prevPathCellsSet;
        HashSet<Cell> m_pathCellsSet;
        Queue<Cell> m_pathCellsQueue;

        DateTime m_startTime;
        int m_rotationsCount;

        FieldStatus m_fieldStatus;

        public Action<CompleteBundle> Complete;
        public int ConnectedDisplaysCount;

        public LevelDescriptor Descriptor { get; protected set; }
        public SpriteButton MenuButton { get; protected set; }

        public Field(GraphicsDevice device)
             : base(device)
        {
            this.m_sideSize = Game.Content.GetSizeInDpi(64);
            this.m_scaleStep = Game.Content.GetSizeInDpi(4);
            this.m_maxScale = this.m_sideSize;
            this.m_minScale = m_sideSize / 2;
            this.m_scaleSideSize = this.m_sideSize;

            this.m_area = new Area();
            this.m_area.HorizontalAligin = ContentAligin.Center;
            this.m_area.VerticalAligin = ContentAligin.Center;
            this.m_area.Padding = Game.Content.Common.FieldPadding;
            this.m_area.View = Canvas.View;

            this.m_isUpdateFieldData = true;

            this.m_prevPathCellsSet = new HashSet<Cell>();
            this.m_pathCellsSet = new HashSet<Cell>();
            this.m_pathCellsQueue = new Queue<Cell>();

            this.m_fieldStatus = new FieldStatus();

            int menuButtonMargin = Game.Content.GetSizeInDpi(20);
            this.MenuButton = new SpriteButton(ButtonType.FieldButton, Game.Content.Animations.FieldPress10) { Texture = Game.Content.Textures.MenuIcon, Fixed = true };
            this.MenuButton.X = Canvas.View.Width - menuButtonMargin - this.MenuButton.Width / 2;
            this.MenuButton.Y = Canvas.View.Height - menuButtonMargin - this.MenuButton.Height / 2;
        }

        public override void CreateCanvas(GraphicsDevice device, int width, int height)
        {
            Canvas = new FieldCanvas(device, width, height);
        }

        public override void Draw(GameTime gameTime)
        {
            if (m_isUpdateFieldData)
                return;

            base.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (m_isUpdateFieldData)
                return;

            if (!Visible)
                return;

            FieldManager.Update(gameTime);

            m_area.Update(gameTime);
        }

        void Clear()
        {
            foreach (var animations in FieldManager.Animations())
                animations.Clear();

            m_cells = null;
            m_fieldData = null;
            m_sourceCell = null;
            m_prevPathCellsSet.Clear();
            m_pathCellsSet.Clear();
            m_pathCellsQueue.Clear();
            ((FieldCanvas)Canvas).Clear();
            Descriptor = null;
            m_displaysCount = 0;
            ConnectedDisplaysCount = 0;

            GC.Collect(2, GCCollectionMode.Forced);
        }

        public void BeginUpdateFieldData()
        {
            m_isUpdateFieldData = true;
        }

        public void EndUpdateFieldData()
        {
            m_isUpdateFieldData = false;
        }

        public void UpdateFieldData(FieldData fieldData, LevelDescriptor descriptor)
        {
            if (!m_isUpdateFieldData)
                throw new Exception("Call BeginUpdateFieldData() first");

            Clear();
            Canvas.View.Scale = new Vector2(1f, 1f);
            this.m_scaleSideSize = this.m_sideSize;

            m_fieldData = fieldData;
            Descriptor = descriptor;

            PrepareCells();
            FillCanvas();

            m_fieldStatus.SetInitialStatus(Descriptor, ConnectedDisplaysCount, m_displaysCount);

            float borderX = Descriptor.Width * m_sideSize / 2;
            float borderY = Descriptor.Height * m_sideSize / 2;
            m_area.SetBorder(-borderX, -borderY, borderX, borderY);
            m_area.Stop();
            m_area.Aligin();

            m_startTime = DateTime.Now;
            m_rotationsCount = 0;
        }

        public FieldData GetFieldData()
        {
            m_fieldData.RotatesCount = m_rotationsCount;
            m_fieldData.PlayTime = (DateTime.Now - m_startTime);
            return m_fieldData;
        }

        void CheckComplete()
        {
            if (ConnectedDisplaysCount >= m_displaysCount)
            {
                Complete?.Invoke(new CompleteBundle()
                {
                    Descriptor = Descriptor,
                    RotatesCount = m_fieldData.RotatesCount + m_rotationsCount,
                    Time = (m_fieldData.PlayTime + (DateTime.Now - m_startTime))
                });
            }
        }

        void PrepareCells()
        {
            m_prevPathCellsSet.Clear();
            m_pathCellsSet.Clear();
            m_pathCellsQueue.Clear();
            m_displaysCount = 0;
            ConnectedDisplaysCount = 0;

            m_cellOffsetX = m_sideSize / 2 + this.Descriptor.Width * m_sideSize / 2;
            m_cellOffsetY = m_sideSize / 2 + this.Descriptor.Height * m_sideSize / 2;
            m_cells = new Cell[m_fieldData.CellsData.GetLength(0), m_fieldData.CellsData.GetLength(1)];
            for (int x = 0; x < m_cells.GetLength(0); x++)
            {
                for (int y = 0; y < m_cells.GetLength(1); y++)
                {
                    Cell cell = new Cell(m_fieldData.CellsData[x, y], this, m_sideSize);

                    cell.X = (x + 1) * m_sideSize - m_cellOffsetX;
                    cell.Y = (y + 1) * m_sideSize - m_cellOffsetY;

                    if (cell.Data.Content == ContentType.Router)
                        m_sourceCell = cell;

                    if (cell.Data.Content == ContentType.Display)
                        ++m_displaysCount;

                    m_cells[x, y] = cell;
                }
            }

            m_prevPathCellsSet.Add(m_sourceCell);

            Canvas.View.X = -Canvas.View.Width / 2;
            Canvas.View.Y = -Canvas.View.Height / 2;

            for (int x = 0; x < m_cells.GetLength(0); x++)
            {
                for (int y = 0; y < m_cells.GetLength(1); y++)
                {
                    var cell = m_cells[x, y];

                    var rightCell = GetCell(x + 1, y);
                    var bottomCell = GetCell(x, y + 1);
                    var leftCell = GetCell(x - 1, y);
                    var topCell = GetCell(x, y - 1);

                    cell.UpdateRightSide(rightCell);
                    cell.UpdateBottomSide(bottomCell);
                    cell.UpdateLeftSide(leftCell);
                    cell.UpdateTopSide(topCell);
                }
            }

            UpdatePaths(true, false);
        }

        void FillCanvas()
        {
            var fieldCanvas = (FieldCanvas)Canvas;

            fieldCanvas.Clear();

            fieldCanvas.CellsOffsetX = m_cellOffsetX;
            fieldCanvas.CellsOffsetY = m_cellOffsetY;
            fieldCanvas.SideSize = m_sideSize;
            fieldCanvas.Cells = m_cells;

            m_fieldStatus.AddToCanvas(fieldCanvas);
            MenuButton.AddToCanvas(fieldCanvas);
        }

        public Cell GetCell(int x, int y)
        {
            if (m_cells == null)
                return null;

            if (x < 0 || x >= m_cells.GetLength(0))
                return null;

            if (y < 0 || y >= m_cells.GetLength(1))
                return null;

            return m_cells[x, y];
        }

        void RotateCell90(int x, int y, bool animate)
        {
            var cell = GetCell(x, y);
            if (cell == null)
                return;

            cell.Rotate90(animate);
            ++m_rotationsCount;

            var rightCell = GetCell(x + 1, y);
            var bottomCell = GetCell(x, y + 1);
            var leftCell = GetCell(x - 1, y);
            var topCell = GetCell(x, y - 1);

            cell.UpdateRightSide(rightCell);
            if (rightCell != null)
                rightCell.UpdateLeftSide(cell);

            cell.UpdateBottomSide(bottomCell);
            if (bottomCell != null)
                bottomCell.UpdateTopSide(cell);

            cell.UpdateLeftSide(leftCell);
            if (leftCell != null)
                leftCell.UpdateRightSide(cell);

            cell.UpdateTopSide(topCell);
            if (topCell != null)
                topCell.UpdateBottomSide(cell);

            UpdatePaths(false, animate);

            m_fieldStatus.SetStatus(ConnectedDisplaysCount, m_displaysCount);
            CheckComplete();
        }

        void UpdatePaths(bool init, bool animate)
        {
            m_pathCellsQueue.Enqueue(m_sourceCell);
            while(m_pathCellsQueue.Count > 0)
            {
                var nextPathCell = m_pathCellsQueue.Dequeue();
                m_pathCellsSet.Add(nextPathCell);

                if(nextPathCell.Data.RightSide == SideState.Connected)
                {
                    var rightCell = GetCell(nextPathCell.Data.XIndex + 1, nextPathCell.Data.YIndex);
                    if (rightCell != null && !m_pathCellsSet.Contains(rightCell))
                        m_pathCellsQueue.Enqueue(rightCell);
                }
                if (nextPathCell.Data.BottomSide == SideState.Connected)
                {
                    var bottomCell = GetCell(nextPathCell.Data.XIndex, nextPathCell.Data.YIndex + 1);
                    if (bottomCell != null && !m_pathCellsSet.Contains(bottomCell))
                        m_pathCellsQueue.Enqueue(bottomCell);
                }
                if (nextPathCell.Data.LeftSide == SideState.Connected)
                {
                    var leftCell = GetCell(nextPathCell.Data.XIndex - 1, nextPathCell.Data.YIndex);
                    if (leftCell != null && !m_pathCellsSet.Contains(leftCell))
                        m_pathCellsQueue.Enqueue(leftCell);
                }
                if (nextPathCell.Data.TopSide == SideState.Connected)
                {
                    var topCell = GetCell(nextPathCell.Data.XIndex, nextPathCell.Data.YIndex - 1);
                    if (topCell != null && !m_pathCellsSet.Contains(topCell))
                        m_pathCellsQueue.Enqueue(topCell);
                }
            }

            foreach (var cell in m_pathCellsSet)
            {
                if (!m_prevPathCellsSet.Contains(cell))
                    cell.UpdateSignal(true, init, animate);
            }

            foreach (var prevCell in m_prevPathCellsSet)
            {
                if (!m_pathCellsSet.Contains(prevCell))
                    prevCell.UpdateSignal(false, init, animate);
            }

            m_prevPathCellsSet.Clear();
            foreach (var cell in m_pathCellsSet)
                m_prevPathCellsSet.Add(cell);

            m_pathCellsSet.Clear();
        }

        protected override void BackCore()
        {
            MenuButton.Click?.Invoke();
        }

        protected override void MouseCore(MouseHandlerParams _params)
        {
            MenuButton.Mouse(_params);

            if (_params.Handled)
                return;

            _params.Handled = true;

            if (_params.IsLeftButtonPressed)
                OnClick(_params.Position);

            if (_params.IsDrag)
                m_area.Drag(_params.DeltaPosition);

            if (_params.DeltaWheelValue != 0)
                OnMouseWheel(_params.Position, _params.DeltaWheelValue);
        }

        protected override void TouchCore(ref TouchHandlerParams _params)
        {
            MenuButton.Touch(ref _params);

            if (_params.Handled)
                return;

            if (!_params.IsGestureAvailable)
                return;

            _params.Handled = true;

            switch (_params.Gesture.GestureType)
            {
                case GestureType.Tap:
                    OnClick(_params.Gesture.Position);
                    break;
                case GestureType.FreeDrag:
                    m_area.Drag(_params.Gesture.Delta);
                    break;
                case GestureType.Pinch:
                    OnPinch(_params);
                    break;
                case GestureType.PinchComplete:
                    m_pitchDelta = 0;
                    break;
                default:
                    break;
            }
        }

        void OnClick(Vector2 position)
        {
            if (m_area.IsScroll)
            {
                m_area.Stop();
                return;
            }

            var fieldCoords = new Vector2(position.X + Canvas.View.X * Canvas.View.ScaleX,
                position.Y + Canvas.View.Y * Canvas.View.ScaleY);

            var substrateRect = new Rectangle((int)(-Descriptor.Width * m_sideSize * Canvas.View.ScaleX / 2),
                (int)(-Descriptor.Height * m_sideSize * Canvas.View.ScaleY / 2),
                (int)(Descriptor.Width * m_sideSize * Canvas.View.ScaleX),
                (int)(Descriptor.Height * m_sideSize * Canvas.View.ScaleY));

            if (!substrateRect.Contains(fieldCoords))
                return;

            int x = (int)((fieldCoords.X - substrateRect.X) / (m_sideSize * Canvas.View.ScaleX));
            int y = (int)((fieldCoords.Y - substrateRect.Y) / (m_sideSize * Canvas.View.ScaleY));
            var cell = GetCell(x, y);
            if (cell == null)
                return;

            RotateCell90(x, y, true);
        }

        void OnMouseWheel(Vector2 position, int deltaWheelValue)
        {
            int newScaleSideSize = m_scaleSideSize + Math.Sign(deltaWheelValue) * m_scaleStep;
            if (newScaleSideSize > m_maxScale)
                newScaleSideSize = m_maxScale;
            else if (newScaleSideSize < m_minScale)
                newScaleSideSize = m_minScale;

            if (!m_area.CanScaleTo((float)newScaleSideSize / m_sideSize))
                return;

            m_scaleSideSize = newScaleSideSize;
            m_area.Scale(position, (float)m_scaleSideSize / m_sideSize);
        }

        void OnPinch(TouchHandlerParams _params)
        {
            const int pitchScaleStepMultiplier = 5;

            var oldPosition1 = _params.Gesture.Position - _params.Gesture.Delta;
            var oldPosition2 = _params.Gesture.Position2 - _params.Gesture.Delta2;
            var newDistance = Vector2.Distance(_params.Gesture.Position, _params.Gesture.Position2);
            var oldDistance = Vector2.Distance(oldPosition1, oldPosition2);

            m_pitchDelta += newDistance - oldDistance;
            var scaleBorder = m_scaleStep * pitchScaleStepMultiplier;
            if (Math.Abs(m_pitchDelta) >= scaleBorder)
            {
                int newScaleSideSize = m_scaleSideSize + Math.Sign(m_pitchDelta) * m_scaleStep;

                m_pitchDelta = m_pitchDelta % scaleBorder;

                if (newScaleSideSize < m_minScale)
                    newScaleSideSize = m_minScale;
                else if (newScaleSideSize > m_maxScale)
                    newScaleSideSize = m_maxScale;

                if (!m_area.CanScaleTo((float)newScaleSideSize / m_sideSize))
                    return;

                m_scaleSideSize = newScaleSideSize;

                var positionDiff = _params.Gesture.Position2 - _params.Gesture.Position;
                var positionMidpoint = positionDiff * 0.5f;
                var positionFullMidpoint = positionMidpoint + _params.Gesture.Position;

                m_area.Scale(positionFullMidpoint, (float)m_scaleSideSize / m_sideSize);
            }
        }

    }

}