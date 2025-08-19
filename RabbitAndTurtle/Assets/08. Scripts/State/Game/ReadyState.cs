using Manager;

namespace State.GameState
{
    public class ReadyState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState SceneType => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public ReadyState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            _currentGameState = GameState.Ready;
        }

        public void Enter()
        {
            _gameStateManager.ReadyInit();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
