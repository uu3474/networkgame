using System;

namespace NetworkGame.Logic
{
    [Serializable]
    public class ProfileData
    {
        public const int ActualVersion = 2;

        public int Version;
        public int MaxLevel;

        public ProfileData()
        {
            this.Version = ActualVersion;
            this.MaxLevel = 0;
        }

    }

}
