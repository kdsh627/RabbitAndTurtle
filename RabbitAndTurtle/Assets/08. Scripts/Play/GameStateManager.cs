using State;
using State.GameState;
using StateMachine.SceneStateMachine;
using UnityEngine;

namespace Manager
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private StageDataSO _stageData;

        private int _currentBossStage;
        private int _maxBossStage;

        private string _scenePath;
        private int _currentStage;
        private int _maxStage;

        private float _waveTime;
        private int _currentWave;
        private int _maxWave;

        private GameStateMachine _gameStateMachine;
        private GameState _gameState;

        public GameStateMachine SceneStateMachine => _gameStateMachine;

        private void Awake()
        {
            _gameState = GameState.Wave;
            _currentStage = 0;
            _currentBossStage = 0;

            _maxStage = _stageData.DefaultStageDataList.Count;
            _maxBossStage = _stageData.BossStageDataList.Count;

            _gameStateMachine = new GameStateMachine(this);
        }

        private void Start()
        {
            _gameStateMachine.Initialize(_gameStateMachine._readyState);
        }

        private void OnEnable()
        {
            _gameStateMachine.stateChanged += ChangeState;

            GameEventHandler.ReadyExcuted += () => GameEvent_ExcuteState(GameState.Ready);
            GameEventHandler.WaveExcuted += () => GameEvent_ExcuteState(GameState.Wave);
            GameEventHandler.BossExcuted += () => GameEvent_ExcuteState(GameState.Boss);
            GameEventHandler.StageClearExcuted += () => GameEvent_ExcuteState(GameState.Clear);
        }

        private void OnDisable()
        {
            GameEventHandler.ReadyExcuted -= () => GameEvent_ExcuteState(GameState.Ready);
            GameEventHandler.WaveExcuted -= () => GameEvent_ExcuteState(GameState.Wave);
            GameEventHandler.BossExcuted -= () => GameEvent_ExcuteState(GameState.Boss);
            GameEventHandler.StageClearExcuted -= () => GameEvent_ExcuteState(GameState.Clear);
        }

        /// <summary>
        /// Ready 상태에서 변수 초기화
        /// </summary>
        public void ReadyInit()
        {
            _currentWave = 0;
            switch(_gameState)
            {
                case GameState.Wave:
                    _currentStage++;
                    _maxWave = _stageData.DefaultStageDataList[_currentStage].MaxWaveCount;
                    _scenePath = _stageData.DefaultStageDataList[_currentStage].ScenePath;
                    break;
                case GameState.Boss:
                    _currentBossStage++;
                    _maxWave = _stageData.BossStageDataList[_currentBossStage].MaxWaveCount;
                    _scenePath = _stageData.DefaultStageDataList[_currentBossStage].ScenePath;
                    break;
            }
        }

        /// <summary>
        /// Wave 상태에서 변수 초기화
        /// </summary>
        public void WaveInit()
        {
            _currentWave++;
            _waveTime = _stageData.DefaultStageDataList[_currentWave].MaxWaveTime;
        }

        /// <summary>
        /// Boss 상태에서 변수 초기화
        /// </summary>
        public void BossInit()
        {

        }

        /// <summary>
        /// Clear 상태에서 변수 초기화
        /// </summary>
        public void ClearInit()
        {

        }

        /// <summary>
        /// 클리어 상태에서 다음 단계 판별
        /// </summary>
        private void ProcessStageTransition()
        {
            //보스 클리어
            if(_gameState == GameState.Boss)
            {
                //모든 보스 스테이지 클리어
                if (_currentBossStage == _maxBossStage)
                {
                    //게임 클리어
                    GameEventHandler.GameClearExcuted.Invoke();
                }
                else
                {
                    _gameState = GameState.Wave;
                }
            }
            //현재 스테이지 클리어
            else if (_currentWave == _maxWave)
            {
                //모든 일반 스테이지 클리어
                if (_currentStage == _maxStage)
                {
                    //보스 스테이지로 진입
                    _gameState = GameState.Boss;
                }
            }
        }

        private void GameEvent_ExcuteState(GameState state)
        {
            switch (state)
            {
                case GameState.Ready:
                    _gameStateMachine.TransitionTo(_gameStateMachine._readyState);
                    break;
                case GameState.Wave:
                    _gameStateMachine.TransitionTo(_gameStateMachine._waveState);
                    break;
                case GameState.Boss:
                    _gameStateMachine.TransitionTo(_gameStateMachine._bossState);
                    break;
                case GameState.Clear:
                    _gameStateMachine.TransitionTo(_gameStateMachine._clearState);
                    break;
            }
        }

        private void ChangeState(IState state)
        {
            GameState sceneState = (state as IGameState).CurrentGameState;

            switch (sceneState)
            {
                case GameState.Ready:
                    SceneEventHandler.SceneLoadedByPath(_scenePath);
                    break;
                case GameState.Wave:

                    break;
                case GameState.Boss:

                    break;
                case GameState.Clear:
                    ProcessStageTransition();
                    break;
            }
        }
    }
}
