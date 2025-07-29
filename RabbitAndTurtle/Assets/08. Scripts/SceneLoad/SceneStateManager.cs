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
        public string TitleScenePath => _titleScenePath;
        public string GameScenePath => _gameScenePath;
        public string ClearScenePath => _clearScenePath;


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
        }

        private void OnDisable()
        {
            _sceneStateMachine.stateChanged -= ChangeScene;
            GameEventHandler.TitleExcuted -= () => GameEvent_TransitionState(SceneState.Title);
            GameEventHandler.GamePlayExcuted -= () => GameEvent_TransitionState(SceneState.GamePlay);
            GameEventHandler.GameClearExcuted -= () => GameEvent_TransitionState(SceneState.Clear);
        }

        private void GameEvent_TransitionState(SceneState state)
        {
            _sceneStateMachine.TransitionState(state);
        }

        private void ChangeScene(IState state)
        {
            string scenePath = (state as ISceneState).ScenePath;

            SceneEventHandler.SceneStateChanged(scenePath, _currentScenePath);

            _currentScenePath = scenePath;
        }
    }
}

