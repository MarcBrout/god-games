using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

namespace GodsGame
{
    public static class SaveLoad
    {
        private static readonly string m_ProfilePath = Application.persistentDataPath + "/"; // + "/Profiles/";
        private static readonly string m_FileExtension = ".pgg";

        public static void SaveHighscore(Highscore highscore) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(FormatPath("highscore" + highscore.Level));
            bf.Serialize(file, highscore);
            file.Close();
        }

        public static Highscore LoadHighscore(int level) {
            Highscore highscore = null;
            string path = FormatPath("highscore" + level);
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                highscore = (Highscore)bf.Deserialize(file);
                file.Close();
            }
            return highscore;
        }

        public static void SaveProfile(PlayerProfile profile)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Debug.Log("Saving to: " + FormatPath(profile.Name));
            FileStream file = File.Create(FormatPath(profile.Name));
            bf.Serialize(file, profile);
            file.Close();
        }

        public static string[] GetProfilesName()
        {
            DirectoryInfo dir = new DirectoryInfo(m_ProfilePath);
            FileInfo[] info = dir.GetFiles("*" + m_FileExtension);
            var fullNames = info.Select(f => f.FullName).ToArray();
            return fullNames;
        }

        public static PlayerProfile LoadProfile(string profileName)
        {
            //Debug.Log(m_ProfilePath);
            PlayerProfile profile = null;
            string path = FormatPath(profileName);
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                profile = (PlayerProfile)bf.Deserialize(file);
                file.Close();
            }
            return profile;
        }

        public static bool DoesProfileExist(string profileName)
        {
            return File.Exists(FormatPath(profileName));
        }

        private static string FormatPath(string fileName)
        {
            return m_ProfilePath + fileName + m_FileExtension;
        }
    }
}