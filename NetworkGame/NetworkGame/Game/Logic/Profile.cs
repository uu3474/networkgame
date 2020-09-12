using NetworkGame.Logic.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;

namespace NetworkGame.Logic
{
    public class Profile
    {
        const string profileShortFileName = "profile.json";

        static BaseProfileMigration[] Migrations = new BaseProfileMigration[]
        {
            new ProfileMigrationFrom1to2(),
        };

        DataContractJsonSerializer m_profileDataSerializer;
        BinaryFormatter m_fieldDataSerializer;
        LevelDescriptorGenerator m_descriptorGenerator;
        string m_profileDir;
        string m_profileFileName;
        ProfileData m_profileData;
        object m_profileLock;

        public bool IsFirstPlay { get; protected set; }
        public List<LevelDescriptor> Levels { get; protected set; }

        public int MaxLevel
        {
            get { return m_profileData.MaxLevel; }
            protected set { m_profileData.MaxLevel = value; }
        }

        public Profile(string profileDir)
        {
            this.m_profileDataSerializer = new DataContractJsonSerializer(typeof(ProfileData));
            this.m_fieldDataSerializer = new BinaryFormatter();

            this.m_descriptorGenerator = new LevelDescriptorGenerator();
            this.IsFirstPlay = false;
            this.Levels = new List<LevelDescriptor>();

            this.m_profileDir = profileDir;
            if (!Directory.Exists(this.m_profileDir))
                Directory.CreateDirectory(this.m_profileDir);

            this.m_profileFileName = Path.Combine(this.m_profileDir, profileShortFileName);
            this.m_profileData = null;
            this.m_profileLock = new object();
        }

        public void SaveProfileData()
        {
            lock (m_profileLock)
            {
                using (var file = File.Create(m_profileFileName))
                    m_profileDataSerializer.WriteObject(file, m_profileData);

                IsFirstPlay = false;
            }
        }

        public void LoadProfileData()
        {
            lock (m_profileLock)
            {
                if (File.Exists(m_profileFileName))
                {
                    try
                    {
                        using (var file = File.OpenRead(m_profileFileName))
                            m_profileData = (ProfileData)m_profileDataSerializer.ReadObject(file);

                        MigrateProfileData();

                        IsFirstPlay = false;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Cant load profile from save file!", exception);
                    }
                }
                else
                {
                    m_profileData = new ProfileData();
                    IsFirstPlay = true;
                }
            }

            GetLevels();
        }

        void MigrateProfileData()
        {
            bool needSave = false;

            foreach (var migration in Migrations)
            {
                if (m_profileData.Version == migration.FromVersion)
                {
                    migration.Migrate(m_profileData, m_profileDir, m_profileFileName);
                    needSave = true;
                }
            }

            if (needSave)
                SaveProfileData();
        }

        void GetLevels()
        {
            Levels.Clear();
            for (int i = 0; i <= MaxLevel; i++)
                Levels.Add(m_descriptorGenerator.GetNextLevel());
        }

        public void AddLevel()
        {
            var nextDescriptor = m_descriptorGenerator.GetNextLevel();
            MaxLevel = nextDescriptor.Index;
            Levels.Add(nextDescriptor);
        }

        string GetFiedDataFileName(LevelDescriptor descriptor)
        {
            return Path.Combine(this.m_profileDir, "level_" + (descriptor.Index + 1).ToString() + ".bin");
        }

        public FieldData GetFieldData(LevelDescriptor descriptor)
        {
            lock (descriptor)
            {
                var fileName = GetFiedDataFileName(descriptor);
                if (File.Exists(fileName))
                {
                    try
                    {
                        using (var file = File.OpenRead(fileName))
                            return (FieldData)m_fieldDataSerializer.Deserialize(file);
                    }
                    catch //(Exception exception)
                    {
                        File.Delete(fileName);
                    }
                }
                return FieldGenerator.GenerateFieldData(descriptor);
            }
        }

        public void SaveFieldData(FieldData fieldData, LevelDescriptor descriptor)
        {
            lock (descriptor)
            {
                using (var file = File.Create(GetFiedDataFileName(descriptor)))
                    m_fieldDataSerializer.Serialize(file, fieldData);
            }
        }

        public void DeleteFieldData(LevelDescriptor descriptor)
        {
            lock (descriptor)
            {
                var fileName = GetFiedDataFileName(descriptor);
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

    }

}
