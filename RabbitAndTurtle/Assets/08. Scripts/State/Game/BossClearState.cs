using Manager;

namespace State.GameState
{
    public class BossClearState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState CurrentGameState => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public BossClearState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void Enter()
        {
            _currentGameState = GameState.BossClear;

            _gameStateManager.BossClearInit();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
