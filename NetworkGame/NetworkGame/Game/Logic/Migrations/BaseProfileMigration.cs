using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Logic.Migrations
{
    public abstract class BaseProfileMigration
    {
        public abstract int FromVersion { get; }
        public virtual int ToVersion { get { return FromVersion + 1; } }

        public void Migrate(ProfileData profileData, string profileDir, string profileFileName)
        {
            MigrateCore(profileData, profileDir, profileFileName);
            profileData.Version = ToVersion;
        }

        protected abstract void MigrateCore(ProfileData profileData, string profileDir, string profileFileName);
    }
}
