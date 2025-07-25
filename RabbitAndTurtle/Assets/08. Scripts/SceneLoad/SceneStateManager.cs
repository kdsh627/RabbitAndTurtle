using State;
using State.SceneState;
using StateMachine.SceneStateMachine;
using UnityEngine;


namespace Manager
{
    public class SceneStateManager : MonoBehaviour
    {
        [Header("--- 핵심 씬 ---")]
        [SerializeField] private string _titleScenePath;
        [SerializeField] private string _gameScenePath;
        [SerializeField] private string _clearScenePath;

        private SceneStateMachine _sceneStateMachine;
        private string _currentScenePath;

        public SceneStateMachine SceneStateMachine => _sceneStateMachine;

        private void Awake()
        {
            _currentScenePath = "";
            _sceneStateMachine = new SceneStateMachine(this);
        }

        private void Start()
        {
            _sceneStateMachine.Initialize(_sceneStateMachine._titleState);
        }
        private void Update()
        {
            _sceneStateMachine.Excute();
        }

        private void OnEnable()
        {
            _sceneStateMachine.stateChanged += ChangeScene;

            GameEventHandler.ExcuteTitle += () => GameEvent_ExcuteState(SceneState.Title);
            GameEventHandler.ExcuteGamePlay += () => GameEvent_ExcuteState(SceneState.GamePlay);
            GameEventHandler.ExcuteClear += () => GameEvent_ExcuteState(SceneState.Clear);
        }

        private void OnDisable()
        {
            _sceneStateMachine.stateChanged -= ChangeScene;
            GameEventHandler.ExcuteTitle -= () => GameEvent_ExcuteState(SceneState.Title);
            GameEventHandler.ExcuteGamePlay -= () => GameEvent_ExcuteState(SceneState.GamePlay);
            GameEventHandler.ExcuteClear -= () => GameEvent_ExcuteState(SceneState.Clear);
        }

        private void GameEvent_ExcuteState(SceneState state)
        {
            switch (state)
            {
                case SceneState.Title:
                    _sceneStateMachine.TransitionTo(_sceneStateMachine._titleState);
                    break;
                case SceneState.GamePlay:
                    _sceneStateMachine.TransitionTo(_sceneStateMachine._gamePlayState);
                    break;
                case SceneState.Clear:
                    _sceneStateMachine.TransitionTo(_sceneStateMachine._clearState);
                    break;
            }
        }

        private void ChangeScene(IState state)
        {
            SceneState sceneState = (state as ISceneState).CurrentSceneState;

            string scenePath = "";
            switch (sceneState)
            {
                case SceneState.Title:
                    scenePath = _titleScenePath;
                    break;
                case SceneState.GamePlay:
                    scenePath = _gameScenePath;
                    break;
                case SceneState.Clear:
                    scenePath = _clearScenePath;
                    break;
            }

            SceneEventHandler.SceneStateChanged(scenePath, _currentScenePath);

            _currentScenePath = scenePath;
        }
    }
}

