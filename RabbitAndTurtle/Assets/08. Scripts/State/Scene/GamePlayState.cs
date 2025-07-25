using Manager;
using State.SceneState;
using Utilities;

namespace State.SceneState
{
    public class GamePlayState : ISceneState
    {
        private SceneStateManager _sceneStateManager;

        private SceneState _currentSceneState;
        public SceneState CurrentSceneState => _currentSceneState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="sceneStateManager"></param>
        public GamePlayState(SceneStateManager sceneStateManager)
        {
            _sceneStateManager = sceneStateManager;
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