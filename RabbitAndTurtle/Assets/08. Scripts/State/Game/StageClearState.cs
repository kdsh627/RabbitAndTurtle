using Manager;

namespace State.GameState
{
    public class StageClearState : IGameState
    {
        private GameStateManager _gameStateManager;

        private GameState _currentGameState;

        public GameState SceneType => _currentGameState;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="gameStateManager"></param>
        public StageClearState(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            _currentGameState = GameState.StageClear;
        }

        public void Enter()
        {
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
