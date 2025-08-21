using Manager;
using State.GameState;

namespace StateMachine.SceneStateMachine
{
    public class GameStateMachine : StateMachine
    {
        //각 상태들
        public ReadyState _readyState;
        public WaveState _waveState;
        public StageState _stageState;
        public StageClearState _stageClearState;
        public BossState _bossState;
        public WaveClearState _waveClearState;
        public BossClearState _bossClearState;

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
            _stageState = new StageState(gameStateManager);
            _stageClearState = new StageClearState(gameStateManager);
            _waveClearState = new WaveClearState(gameStateManager);
            _bossState = new BossState(gameStateManager);
            _bossClearState = new BossClearState(gameStateManager);
        }

        public override void Excute()
        {
            AnyState();

            base.Excute();
        }

        private void AnyState()
        {

        }
    }
}
