using Manager;
using State.SceneState;

namespace State.SceneState
{
    public class TitleState : ISceneState
    {
        private SceneStateManager _sceneStateManager;

        private SceneState _currentSceneState;
        public SceneState CurrentSceneState => _currentSceneState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="sceneStateManager"></param>
        public TitleState(SceneStateManager sceneStateManager)
        {
            _sceneStateManager = sceneStateManager;
        }

        public void Enter()
        {
            _currentSceneState = SceneState.Title;
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}