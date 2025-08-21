using Manager;

namespace State.GameState
{
    public class StageState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState SceneType => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public StageState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            _currentGameState = GameState.Stage;
        }

        public void Enter()
        {
            _gameStateManager.StageInit();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
