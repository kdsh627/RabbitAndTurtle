using Manager;

namespace State.GameState
{
    public class WaveState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState SceneType => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public WaveState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            _currentGameState = GameState.Wave;
        }

        public void Enter()
        {
            _gameStateManager.WaveInit();
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}

