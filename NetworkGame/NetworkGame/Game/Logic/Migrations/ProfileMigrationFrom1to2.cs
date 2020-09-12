using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Logic.Migrations
{
    public class ProfileMigrationFrom1to2 : BaseProfileMigration
    {
        public override int FromVersion { get { return 1; } }

        protected override void MigrateCore(ProfileData profileData, string profileDir, string profileFileName)
        {
            var fieldFiles = Directory.EnumerateFiles(profileDir, "*.bin");
            foreach (var fieldFileName in fieldFiles)
                File.Delete(fieldFileName);
        }
    }
}
