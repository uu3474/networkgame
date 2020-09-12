using System;

namespace NetworkGame.Logic
{
    [Serializable]
    public enum WireType : byte
    {
        Empty = 0,

        End = 1,
        Line = 2,

        Angle2 = 3,
        Angle3 = 4,
        Angle4 = 5,
    }

    [Serializable]
    public enum ContentType : byte
    {
        Empty = 0,

        InGeneratorQueue = 10,
        Generated = 11,

        Wire = 100,
        Router = 101,
        Display = 102,
    }

    [Serializable]
    public enum SideState : byte
    {
        Empty = 0,

        Disconnected = 1,
        Connected = 2,
    }

    [Serializable]
    public enum CellAngle : byte
    {
        Angle0 = 0,
        Angle90 = 1,
        Angle180 = 2,
        Angle270 = 3,
    }

    [Serializable]
    public class CellData
    {
        public readonly int XIndex;
        public readonly int YIndex;

        public WireType Wire;
        public ContentType Content;
        public CellAngle InitialAngle;
        public CellAngle Angle;

        public SideState RightSide;
        public SideState BottomSide;
        public SideState LeftSide;
        public SideState TopSide;

        public CellData(int xIndex, int yIndex)
        {
            this.XIndex = xIndex;
            this.YIndex = yIndex;
        }
    }

    public static class CellDataExtensions
    {
        public static void Rotate90(this CellData cellData)
        {
            var tempSide = cellData.TopSide;
            cellData.TopSide = cellData.LeftSide;
            cellData.LeftSide = cellData.BottomSide;
            cellData.BottomSide = cellData.RightSide;
            cellData.RightSide = tempSide;

            cellData.Angle = (CellAngle)(((int)cellData.Angle + 1) % 4);
        }

        public static void Rotate180(this CellData cellData)
        {
            var tempSide = cellData.LeftSide;
            cellData.LeftSide = cellData.RightSide;
            cellData.RightSide = tempSide;
            tempSide = cellData.TopSide;
            cellData.TopSide = cellData.BottomSide;
            cellData.BottomSide = tempSide;

            cellData.Angle = (CellAngle)(((int)cellData.Angle + 2) % 4);
        }

        public static void Rotate270(this CellData cellData)
        {
            var tempSide = cellData.RightSide;
            cellData.RightSide = cellData.BottomSide;
            cellData.BottomSide = cellData.LeftSide;
            cellData.LeftSide = cellData.TopSide;
            cellData.TopSide = tempSide;

            cellData.Angle = (CellAngle)(((int)cellData.Angle + 3) % 4);
        }
    }
}
