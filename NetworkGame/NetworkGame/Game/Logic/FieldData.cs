using System;
using System.Runtime.Serialization;

namespace NetworkGame.Logic
{
    [Serializable]
    public class FieldData
    {
        public const int ActualVersion = 2;

        public int Version;
        public int RotatesCount;
        public TimeSpan PlayTime;
        public CellData[,] CellsData;

        public FieldData()
        {
            this.Version = ActualVersion;
            this.RotatesCount = 0;
            this.PlayTime = TimeSpan.Zero;
            this.CellsData = null;
        }

    }

}
