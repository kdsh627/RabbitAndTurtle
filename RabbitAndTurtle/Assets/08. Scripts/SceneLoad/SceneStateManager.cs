using System.Collections.Generic;
using State.SceneState;
using StateMachine.SceneStateMachine;
using UnityEngine;


namespace Manager
{
    public class SceneStateManager : MonoBehaviour
    {
        public static SceneStateManager Instance { get; private set; }

        private SceneStateMachine _sceneStateMachine;
        public ISceneState _nextSceneState;
        private string _currentScenePath;

        public SceneStateMachine SceneStateMachine => _sceneStateMachine;
        public string TitleScenePath => SceneDataManager.Instance.TitleScene;
        public string GameScenePath => SceneDataManager.Instance.WaveCoreScene;
        public string ClearScenePath => SceneDataManager.Instance.ClearScene;

        public ISceneState NextSceneState => _nextSceneState;


        private void Awake()
        {
            Instance = this;
            _currentScenePath = "";
            _sceneStateMachine = new SceneStateMachine(this);
        }

        private void Start()
        {
            _sceneStateMachine.Initialize(_sceneStateMachine._titleState);
            GameEventHandler.TitleExcuted_Invoke();
        }

        private void OnEnable()
        {
            GameEventHandler.TitleExcuted += GameEvent_ToTitle;
            GameEventHandler.GamePlayExcuted += GameEvent_ToGame;
            GameEventHandler.GameClearExcuted += GameEvent_ToClear;
            GameEventHandler.ExitExcuted += GameEvent_Exit;
        }

        private void OnDisable()
        {
            GameEventHandler.TitleExcuted -= GameEvent_ToTitle;
            GameEventHandler.GamePlayExcuted -= GameEvent_ToGame;
            GameEventHandler.GameClearExcuted -= GameEvent_ToClear;
            GameEventHandler.ExitExcuted -= GameEvent_Exit;
        }

        private void GameEvent_Exit()
        {
            Application.Quit();
        }

        private void GameEvent_ToTitle()
        {
            _nextSceneState = _sceneStateMachine._titleState;

            GameEvent_TransitionScene(_nextSceneState);
        }

        private void GameEvent_ToGame()
        {
            _nextSceneState = _sceneStateMachine._gamePlayState;

            GameEvent_TransitionScene(_nextSceneState);
        }

        private void GameEvent_ToClear()
        {
            _nextSceneState = _sceneStateMachine._clearState;

            GameEvent_TransitionScene(_nextSceneState);
        }

        private void GameEvent_TransitionScene(ISceneState state)
        {
            StateManager.Instance._stateMachine = _sceneStateMachine;
            StateManager.Instance._nextState = _nextSceneState;
            ChangeScene(state);
        }

        private void ChangeScene(ISceneState state)
        {
            string scenePath = state.ScenePath;

            if (state.StateType == SceneState.GamePlay)
            {
                List<string> subScenes = new List<string>() { SceneDataManager.Instance.GetWaveSubScene(0) };

                SceneEventHandler.SceneStateChangedAndLoadScenes_Invoke(scenePath, _currentScenePath, subScenes);
            }
            else
            {
                SceneEventHandler.SceneStateChanged_Invoke(scenePath, _currentScenePath);
            }

            _currentScenePath = scenePath;
        }
    }
}

