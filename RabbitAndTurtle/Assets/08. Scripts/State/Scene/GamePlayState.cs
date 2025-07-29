using Manager;
using State.SceneState;
using Utilities;

namespace State.SceneState
{
    public class GamePlayState : ISceneState
    {
        private SceneStateManager _sceneStateManager;

        private SceneState _currentSceneState;
        private string _scenePath;
        public SceneState CurrentSceneState => _currentSceneState;
        public string ScenePath => _scenePath;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="sceneStateManager"></param>
        public GamePlayState(SceneStateManager sceneStateManager)
        {
            _sceneStateManager = sceneStateManager;
            _scenePath = sceneStateManager.GameScenePath;
        }

        public void Enter()
        {
            _currentSceneState = SceneState.GamePlay;
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}