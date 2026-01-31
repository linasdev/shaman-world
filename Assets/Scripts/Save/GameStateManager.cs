using System;
using System.IO;
using Exceptions;
using UnityEngine;

namespace Save
{
    public static class GameStateManager
    {
        private static readonly string SaveFileLocation = Application.persistentDataPath + "/save.json";
        private static GameState _currentState;

        public static GameState GetCurrentState()
        {
            return _currentState ?? throw new SavedGameStateNotLoadedException();
        }

        public static void LoadFromDisk()
        {
            try
            {
                var saveContents = File.ReadAllText(SaveFileLocation);
                _currentState = JsonUtility.FromJson<GameState>(saveContents);
            }
            catch (Exception ex)
            {
                if (ex is not (DirectoryNotFoundException or FileNotFoundException)) throw;

                _currentState = new GameState();
                SaveToDisk();
            }
        }

        public static void SaveToDisk()
        {
            var saveContents = JsonUtility.ToJson(GetCurrentState());
            File.WriteAllText(SaveFileLocation, saveContents);
        }
    }
}
