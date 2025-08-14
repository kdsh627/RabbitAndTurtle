using System.Collections.Generic;
using NUnit.Framework;
using State;
using State.SceneState;
using StateMachine.SceneStateMachine;
using UnityEngine;


namespace Manager
{
    public class SceneStateManager : MonoBehaviour
    {
        private SceneStateMachine _sceneStateMachine;
        private string _currentScenePath;

        public SceneStateMachine SceneStateMachine => _sceneStateMachine;
        public string TitleScenePath => SceneDataManager.Instance.TitleScene;
        public string GameScenePath => SceneDataManager.Instance.WaveCoreScene;
        public string ClearScenePath => SceneDataManager.Instance.ClearScene;


        private void Awake()
        {
            _currentScenePath = "";
            _sceneStateMachine = new SceneStateMachine(this);
        }

        private void Start()
        {
            _sceneStateMachine.Initialize(_sceneStateMachine._titleState);
        }

        private void OnEnable()
        {
            _sceneStateMachine.stateChanged += ChangeScene;

            GameEventHandler.TitleExcuted += () => GameEvent_TransitionState(SceneState.Title);
            GameEventHandler.GamePlayExcuted += () => GameEvent_TransitionState(SceneState.GamePlay);
            GameEventHandler.GameClearExcuted += () => GameEvent_TransitionState(SceneState.Clear);
            GameEventHandler.ExitExcuted += GameEvent_Exit;
        }

        private void OnDisable()
        {
            _sceneStateMachine.stateChanged -= ChangeScene;
            GameEventHandler.TitleExcuted -= () => GameEvent_TransitionState(SceneState.Title);
            GameEventHandler.GamePlayExcuted -= () => GameEvent_TransitionState(SceneState.GamePlay);
            GameEventHandler.GameClearExcuted -= () => GameEvent_TransitionState(SceneState.Clear);
            GameEventHandler.ExitExcuted -= GameEvent_Exit;
        }

        private void GameEvent_TransitionState(SceneState state)
        {
            _sceneStateMachine.TransitionState(state);
        }

        private void GameEvent_Exit()
        {
            Application.Quit();
        }

        private void ChangeScene(IState state)
        {
            string scenePath = (state as ISceneState).ScenePath;

            List<string> subScenes = new List<string>();
            subScenes.Add(SceneDataManager.Instance.GetWaveSubScene(0));

            switch ((state as ISceneState).CurrentSceneState)
            {
                case SceneState.GamePlay:
                    SceneEventHandler.SceneStateChangedAndLoadScenes_Invoke(scenePath, _currentScenePath, subScenes);
                    break;
                default:
                    SceneEventHandler.SceneStateChanged_Invoke(scenePath, _currentScenePath);
                    break;
            }

            _currentScenePath = scenePath;
        }
    }
}

