using Manager;

namespace State.GameState
{
    public class StageClearState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState CurrentGameState => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public StageClearState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void Enter()
        {
            _currentGameState = GameState.StageClear;
            _gameStateManager.StageClearInit();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
