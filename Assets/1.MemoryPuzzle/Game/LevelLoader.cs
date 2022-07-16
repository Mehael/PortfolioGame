using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MemoryPuzzle
{
    public class LevelLoader : MonoBehaviour
    {
        public static int CurrentLevel => SceneManager.GetActiveScene().buildIndex;
        
        public static void LoadLevel(int index)
        {
            SceneManager.LoadScene(index);
        }

        public static void LoadNextLevel()
        {
            SceneManager.LoadScene(Math.Min(SceneManager.sceneCount - 1, CurrentLevel + 1));
        }

        public static void RestartLevel()
        {
            SceneManager.LoadScene(CurrentLevel);
        }

#if UNITY_EDITOR
        private readonly Dictionary<KeyCode, int> levelLoadingCheatCodes = new Dictionary<KeyCode, int>()
        {
            {KeyCode.Keypad1, 1},
            {KeyCode.Keypad2, 2},
            {KeyCode.Keypad3, 3},
            {KeyCode.Keypad4, 4},
            {KeyCode.Keypad5, 5},
            {KeyCode.Keypad6, 6},
            {KeyCode.Keypad7, 7},
            {KeyCode.Keypad8, 8},
            {KeyCode.Keypad9, 9},
            {KeyCode.Keypad0, 10},
        };

        private void Update()
        {
            foreach (var cheatCode in levelLoadingCheatCodes)
                if (Input.GetKeyDown(cheatCode.Key))
                    LoadLevel(cheatCode.Value);
        }
#endif
    }
}