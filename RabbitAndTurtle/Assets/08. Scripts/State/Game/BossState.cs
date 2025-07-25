using Manager;

namespace State.GameState
{
    public class BossState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState CurrentGameState => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public BossState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void Enter()
        {
            _currentGameState = GameState.Boss;
            _gameStateManager.BossInit();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
