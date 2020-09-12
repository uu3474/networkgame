using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Drawing;
using NetworkGame.Engine;

namespace NetworkGame.Logic
{
    public class Cell
    {
        public CellData Data { get; protected set; }

        public AtlasSprite WireSprite { get; protected set; }
        public BaseCellHeader Header { get; protected set; }

        public bool HasSignal { get; protected set; }

        public float X
        {
            get { return WireSprite.X; }
            set
            {
                WireSprite.X = value;
                if (Header != null)
                    Header.X = value;
            }
        }
        public float Y
        {
            get { return WireSprite.Y; }
            set
            {
                WireSprite.Y = value;
                if(Header != null)
                    Header.Y = value;
            }
        }

        public Cell(CellData data, Field field, int sideSize)
        {
            this.Data = data;

            this.WireSprite = new AtlasSprite();

            switch (this.Data.Wire)
            {
                case WireType.End:
                    this.WireSprite.Frame = Game.Content.Textures.WireEnd;
                    break;
                case WireType.Line:
                    this.WireSprite.Frame = Game.Content.Textures.WireLine;
                    break;
                case WireType.Angle2:
                    this.WireSprite.Frame = Game.Content.Textures.WireAngle2;
                    break;
                case WireType.Angle3:
                    this.WireSprite.Frame = Game.Content.Textures.WireAngle3;
                    break;
                case WireType.Angle4:
                    this.WireSprite.Frame = Game.Content.Textures.WireAngle4;
                    break;
                default:
                    this.WireSprite.Visible = false;
                    break;
            }

            float rotation = (int)this.Data.Angle * MathHelper.PiOver2;
            this.WireSprite.Rotation = rotation;

            this.UpdateSignalCore(this.Data.Content == ContentType.Router);

            switch (this.Data.Content)
            {
                case ContentType.Wire:
                    break;
                case ContentType.Router:
                    this.Header = new RouterHeader();
                    break;
                case ContentType.Display:
                    this.Header = new DisplayHeader(field);
                    break;
            }
            if(this.Header != null)
                this.Header.InitRotate(rotation);
        }

        void UpdateSignalCore(bool signal)
        {
            HasSignal = signal;
            WireSprite.SetColor(HasSignal ? Game.Content.Colors.WireConnected : Game.Content.Colors.WireDisconnected);
        }

        public void UpdateSignal(bool signal, bool init, bool animate)
        {
            if (Data.Content == ContentType.Router)
                return;

            if (HasSignal == signal)
                return;

            UpdateSignalCore(signal);
            if (Header != null)
                Header.UpdateSignal(signal, init, animate);

            if(animate)
            {
                if (!Game.Content.Animations.FieldWireToConnected.Revert(WireSprite))
                    Game.Content.Animations.FieldWireToConnected.Apply(WireSprite, !signal);
            }
        }

        public void UpdateRightSide(Cell rightCell)
        {
            if (Data.RightSide != SideState.Empty)
            {
                Data.RightSide = (rightCell == null || rightCell.Data.LeftSide == SideState.Empty)
                    ? SideState.Disconnected : SideState.Connected;
            }
        }

        public void UpdateBottomSide(Cell bottomCell)
        {
            if (Data.BottomSide != SideState.Empty)
            {
                Data.BottomSide = (bottomCell == null || bottomCell.Data.TopSide == SideState.Empty)
                    ? SideState.Disconnected : SideState.Connected;
            }
        }

        public void UpdateLeftSide(Cell leftCell)
        {
            if (Data.LeftSide != SideState.Empty)
            {
                Data.LeftSide = (leftCell == null || leftCell.Data.RightSide == SideState.Empty)
                    ? SideState.Disconnected : SideState.Connected;
            }
        }

        public void UpdateTopSide(Cell topCell)
        {
            if (Data.TopSide != SideState.Empty)
            {
                Data.TopSide = (topCell == null || topCell.Data.BottomSide == SideState.Empty)
                    ? SideState.Disconnected : SideState.Connected;
            }
        }

        public void Rotate90(bool animate)
        {
            Data.Rotate90();

            if (Header != null)
                Header.Rotate90(animate);

            if (animate)
            {
                Game.Content.Animations.FieldRotate90.Apply(WireSprite);
                Game.Content.Animations.FieldPress10.Apply(WireSprite);
            }
        }
    }
}
