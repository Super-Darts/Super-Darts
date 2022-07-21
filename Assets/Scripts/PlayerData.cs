/*
 *  Author: Lewis Comstive
 *  Usage: Holds data related to player achievements and settings
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts
{
    public static class PlayerData
    {
        #region Save data

        [Serializable]
        public class SaveData : ISerializable
        {
            /// <summary>
            /// Save path, relative to `Application.persistentDataPath`
            /// </summary>
            public string Path = "Save.dat";

            /// <summary>
            /// Highest achieved score for this save
            /// </summary>
            public int Highscore = 0;

            /// <summary>
            /// Score achieved for this run.
            /// </summary>
            public int Score = 0;

            /// <summary>
            /// The highest level unlocked for this save file
            /// </summary>
            /// `byte` because it will always be within range 0-255
            public byte UnlockedLevelIndex = 0;

            /// Volumes ///

            /// <summary>
            /// Master volume of audio mixer. Percentage range from 0.0-1.0
            /// </summary>
            public float MasterVolume = 1.0f;

            /// <summary>
            /// Is the master volume muted?
            /// </summary>
            public bool MasterMuted = false;

            /// <summary>
            /// Music volume of audio mixer. Percentage range from 0.0-1.0
            /// </summary>
            public float MusicVolume = 1.0f;

            /// <summary>
            /// Is the music volume muted?
            /// </summary>
            public bool MusicMuted = false;

            /// <summary>
            /// SFX volume of audio mixer. Percentage range from 0.0-1.0
            /// </summary>
            public float SFXVolume = 1.0f;

            /// <summary>
            /// Is the SFX volume muted?
            /// </summary>
            public bool SFXMuted = false;

            /// <summary>
            /// Names of all prizes that are unlocked
            /// </summary>
            public List<string> UnlockedPrizes = new List<string>();

            public SaveData()
            { }

            public SaveData(SerializationInfo info, StreamingContext context)
            {
                MasterVolume = info.GetSingle("MasterVolume");
                MusicVolume = info.GetSingle("MusicVolume");
                SFXVolume = info.GetSingle("SFXVolume");

                MasterMuted = info.GetBoolean("MasterMuted");
                MusicMuted = info.GetBoolean("MusicMuted");
                SFXMuted = info.GetBoolean("SFXMuted");

                Highscore = info.GetInt32("Highscore");
                UnlockedLevelIndex = info.GetByte("UnlockedLevelIndex");

                UnlockedPrizes = new List<string>();
                int unlockedPrizeCount = info.GetInt32("UnlockedPrizeCount");
                for (int i = 0; i < unlockedPrizeCount; i++)
                    UnlockedPrizes.Add(info.GetString($"UnlockedPrize_{i}"));
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("MasterVolume", MasterVolume);
                info.AddValue("MusicVolume", MusicVolume);
                info.AddValue("SFXVolume", SFXVolume);

                info.AddValue("MasterMuted", MasterMuted);
                info.AddValue("MusicMuted", MusicMuted);
                info.AddValue("SFXMuted", SFXMuted);

                info.AddValue("Highscore", Highscore);
                info.AddValue("UnlockedLevelIndex", UnlockedLevelIndex);

                info.AddValue("UnlockedPrizeCount", UnlockedPrizes.Count);
                for (int i = 0; i < UnlockedPrizes.Count; i++)
                    info.AddValue($"UnlockedPrize_{i}", UnlockedPrizes[i]);
            }
        }

        #endregion Save data

        public static readonly string DefaultSavePath = "Player.dat";

        private static string BaseSavePath => Application.persistentDataPath;

        public static SaveData Data { get; set; } = Load(DefaultSavePath);

        public static void Save(SaveData data)
        {
            string path = $"{BaseSavePath}/{data.Path}";

            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, data);

            stream.Flush();
            stream.Close();

            Debug.Log($"Saved player data to '{path}'");
        }

        public static void Save() => Save(Data);

        public static SaveData Load(string path)
        {
            path = $"{BaseSavePath}/{path}";

            if (!File.Exists(path))
                return new SaveData();

            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter = new BinaryFormatter();

            SaveData data = (SaveData)formatter.Deserialize(stream);
            stream.Close();

            Debug.Log($"Loaded player data from '{path}'");

            return data;
        }

        public static void Load()
        {
            int score = Data.Score;

            Data = Load(Data.Path);

            Data.Score = score;
        }

        public static void UnlockPrize(string name)
        { }
    }
}