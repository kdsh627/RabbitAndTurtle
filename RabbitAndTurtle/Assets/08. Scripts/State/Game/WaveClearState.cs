using Manager;

namespace State.GameState
{
    public class WaveClearState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState CurrentGameState => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public WaveClearState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void Enter()
        {
            _currentGameState = GameState.WaveClear;

            _gameStateManager.WaveClearInit();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
