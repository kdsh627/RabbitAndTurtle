using System;
using UnityEngine;


namespace Manager
{
    public class SceneManager : MonoBehaviour
    {
        [Header("--- 씬 관련 정보 ---")]
        [SerializeField] private string[] _gameScenePaths;
        [SerializeField] private string _clearScenePaths;

        private void OnEnable()
        {
            GameEventHandler.Clear += LoadClearScene;
            GameEventHandler.LoadTitle += LoadTitleScene;
        }

        private void OnDisable()
        {
            GameEventHandler.Clear -= LoadClearScene;
            GameEventHandler.LoadTitle -= LoadTitleScene;
        }

        private void Start()
        {
            LoadAdditivelyScene(_gameScenePaths[0]);
        }

        public void LoadGameScene(int index)
        {
            LoadScene(_gameScenePaths[index]);
        }

        public void LoadClearScene()
        {
            LoadScene(_clearScenePaths);
        }

        public void LoadTitleScene()
        {
            SceneEventHandler.LastSceneUnloaded.Invoke();
            SceneEventHandler.TitleSceneLoaded.Invoke();
        }

        private void LoadScene(string scenePath)
        {
            SceneEventHandler.SceneLoadedByPath(scenePath);
        }

        private void LoadAdditivelyScene(string scenePath)
        {
            SceneEventHandler.SceneLoadedAdditivelyByPath(scenePath);
        }
    }
}

