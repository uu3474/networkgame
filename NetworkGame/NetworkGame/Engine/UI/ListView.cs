using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkGame.Engine.UI
{
    public class ListViewItem
    {
        FilledRect m_back;
        int m_width;
        int m_height;

        public virtual float X
        {
            get { return m_back.X; }
            set { m_back.X = value; }
        }
        public virtual float Y
        {
            get { return m_back.Y; }
            set { m_back.Y = value; }
        }
        public virtual int Width
        {
            get { return m_width; }
            set
            {
                m_width = value;
                m_back.Width = m_width;
            }
        }
        public virtual int Height
        {
            get { return m_height; }
            protected set
            {
                m_height = value;
                m_back.Height = m_height;
            }
        }

        public ListViewItem()
        {
            this.m_back = new FilledRect() { Alpha = Game.Content.Colors.ButtonBackAlpha };
            this.m_back.SetColor(Game.Content.Colors.ButtonBack);
        }

        public virtual void Draw(SpriteBatch spriteBatch, BaseSprite viewport)
        {
            m_back.Draw(spriteBatch, viewport);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)(X - Width / 2), (int)(Y - Height / 2), (int)(Width), (int)(Height));
        }

        public virtual void Tap()
        {
            Game.Content.Animations.Press10.Apply(m_back);
        }

    }

    public class ListView<ItemType> : CanvasScreen
        where ItemType : ListViewItem
    {
        int m_wheelScrollStep;
        int m_wheelStepMultiplier;
        List<ItemType> m_items;
        Area m_area;

        public int Spacing { get; protected set; }
        public Action<ItemType> ItemClick { get; set; }

        public IEnumerable<ItemType> Items { get { return m_items; }  }

        public ListView(GraphicsDevice device, int width, int height, int spacing = 10)
            :base(device, width, height)
        {
            this.m_wheelScrollStep = Game.Content.GetSizeInDpi(5);
            this.m_wheelStepMultiplier = 0;

            this.m_items = new List<ItemType>();

            this.m_area = new Area();
            this.m_area.HorizontalAligin = ContentAligin.Center;
            this.m_area.VerticalAligin = ContentAligin.LeftOrTop;
            this.m_area.View = Canvas.View;
            this.m_area.SetBorder(0, 0, width, height);

            this.Spacing = spacing;

            var listViewCanvas = (ListViewCanvas<ItemType>)this.Canvas;
            listViewCanvas.Items = this.m_items;
            listViewCanvas.Spacing = this.Spacing;
            listViewCanvas.View.SetColor(Game.Content.Colors.ButtonBack);
            listViewCanvas.View.Alpha = Game.Content.Colors.ButtonBackAlpha;
        }

        public override void CreateCanvas(GraphicsDevice device, int width, int height)
        {
            Canvas = new ListViewCanvas<ItemType>(device, width, height);
        }

        public void AddItem(ItemType item)
        {
            item.Width = Canvas.View.Width - Spacing * 2;
            item.X = Spacing + item.Width / 2;
            item.Y = item.Height / 2 + Spacing;

            var lastItem = m_items.LastOrDefault();
            if (lastItem != null)
                item.Y += lastItem.Y + lastItem.Height / 2;

            m_items.Add(item);

            m_area.BorderBottom = item.Y + item.Height / 2 + Spacing;
        }

        public void AddItems(IEnumerable<ItemType> items)
        {
            foreach (var item in items)
                AddItem(item);
        }

        public void Update(GameTime gameTime)
        {
            m_area.Update(gameTime);
        }

        protected override void MouseCore(MouseHandlerParams _params)
        {
            if (_params.Handled)
                return;

            _params.Handled = true;

            if (_params.IsLeftButtonPressed)
                OnClick(_params.Position);

            if (_params.IsDrag)
                m_area.Drag(new Vector2(0, _params.DeltaPosition.Y));

            if (_params.DeltaWheelValue != 0)
                OnMouseWheel(_params.DeltaWheelValue);
        }

        protected override void TouchCore(ref TouchHandlerParams _params)
        {
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
                    m_area.Drag(new Vector2(0, _params.Gesture.Delta.Y));
                    break;
                case GestureType.DragComplete:
                case GestureType.Pinch:
                case GestureType.PinchComplete:
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

            if (!m_items.Any())
                return;

            var scrolledPosition = new Vector2(position.X, position.Y + Canvas.View.Y);
            var index = (int)(scrolledPosition.Y / (Spacing + m_items.First().Height));
            if (index < 0 || index >= m_items.Count)
                return;

            var item = m_items[index];
            if (!item.GetBoundingBox().Contains(scrolledPosition))
                return;

            ItemClick?.Invoke(item);
            item.Tap();
        }

        void OnMouseWheel(int deltaWheelValue)
        {
            if (deltaWheelValue == 0)
                return;

            const int maxMultiplier = 10;
            if (m_area.IsScroll)
            {
                if (m_wheelStepMultiplier <= maxMultiplier)
                    m_wheelStepMultiplier++;
            }
            else
            {
                m_wheelStepMultiplier = 1;
            }

            if (deltaWheelValue > 0)
                m_area.Drag(new Vector2(0, m_wheelStepMultiplier * m_wheelScrollStep));

            if (deltaWheelValue < 0)
                m_area.Drag(new Vector2(0, -m_wheelStepMultiplier * m_wheelScrollStep));
        }
    }
}
