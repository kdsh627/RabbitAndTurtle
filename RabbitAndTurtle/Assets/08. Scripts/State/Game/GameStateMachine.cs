using Manager;
using State.GameState;

namespace StateMachine.SceneStateMachine
{
    public class GameStateMachine : StateMachine
    {
        //각 상태들
        public ReadyState _readyState;
        public WaveState _waveState;
        public BossState _bossState;
        public ClearState _clearState;

        private GameStateManager _gameStateManager;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="player"></param>
        public GameStateMachine(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;

            _readyState = new ReadyState(gameStateManager);
            _waveState = new WaveState(gameStateManager);
            _clearState = new ClearState(gameStateManager);
            _bossState = new BossState(gameStateManager);
        }

        public override void Excute()
        {
            AnyState();

            base.Excute();
        }

        private void AnyState()
        {
            //if ()
            //{
            //    TransitionTo();
            //}
        }
    }
}
