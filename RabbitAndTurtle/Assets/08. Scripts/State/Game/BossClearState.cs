using Manager;

namespace State.GameState
{
    public class BossClearState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState SceneType => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public BossClearState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            _currentGameState = GameState.BossClear;
        }

        public void Enter()
        {
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
