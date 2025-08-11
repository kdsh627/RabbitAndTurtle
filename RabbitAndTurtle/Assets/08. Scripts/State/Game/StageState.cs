using Manager;

namespace State.GameState
{
    public class StageState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState CurrentGameState => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public StageState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void Enter()
        {
            _currentGameState = GameState.Stage;
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
