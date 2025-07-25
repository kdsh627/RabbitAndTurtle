using Manager;

namespace State.SceneState
{
    public class ClearState : ISceneState
    {
        private SceneStateManager _sceneStateManager;

        private SceneState _currentSceneState;
        public SceneState CurrentSceneState => _currentSceneState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="sceneStateManager"></param>
        public ClearState(SceneStateManager sceneStateManager)
        {
            _sceneStateManager = sceneStateManager;
        }

        public void Enter()
        {
            _currentSceneState = SceneState.Clear;
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}