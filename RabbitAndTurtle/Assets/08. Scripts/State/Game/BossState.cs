using Manager;

namespace State.GameState
{
    public class BossState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState SceneType => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public BossState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            _currentGameState = GameState.Boss;
        }

        public void Enter()
        {
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
