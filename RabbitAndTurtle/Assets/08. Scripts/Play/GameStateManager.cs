using State.GameState;
using StateMachine.SceneStateMachine;
using UnityEngine;

namespace Manager
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instace { get; set; }

        [SerializeField] private StageDataSO _stageData;
        [SerializeField] private WaveManager _waveManager;

        private int _currentBossStage;
        private int _maxBossStage;

        private string _scenePath;
        private int _currentStage;
        private int _maxStage;

        private GameStateMachine _gameStateMachine;

        public GameStateMachine SceneStateMachine => _gameStateMachine;
        public void SetWaveManager(WaveManager waveManager)
        {
            _waveManager = waveManager;
        }

        private void Awake()
        {
            Instace = this;

            _currentStage = 0;
            _currentBossStage = 0;

            _maxStage = _stageData.DefaultStageDataList.Count;
            _maxBossStage = _stageData.BossStageDataList.Count;

            _scenePath = _stageData.DefaultStageDataList[_currentStage].ScenePath;
            _gameStateMachine = new GameStateMachine(this);
        }

        private void Start()
        {
            _gameStateMachine.Initialize(_gameStateMachine._readyState);
        }

        private void OnEnable()
        {
            GameEventHandler.ReadyExcuted += GameEvent_ReadyExcuted;
            GameEventHandler.WaveExcuted += GameEvent_WaveExcuted;
            GameEventHandler.BossExcuted += GameEvent_BossExcuted;
            GameEventHandler.StageClearExcuted +=  GameEvent_StageClearExcuted;
        }

        private void OnDisable()
        {
            GameEventHandler.ReadyExcuted -= GameEvent_ReadyExcuted;
            GameEventHandler.WaveExcuted -= GameEvent_WaveExcuted;
            GameEventHandler.BossExcuted -= GameEvent_BossExcuted;
            GameEventHandler.StageClearExcuted -= GameEvent_StageClearExcuted;
        }

        private void GameEvent_ReadyExcuted() => GameEvent_TransitionState(GameState.Ready);
        private void GameEvent_WaveExcuted() => GameEvent_TransitionState(GameState.Wave);
        private void GameEvent_BossExcuted() => GameEvent_TransitionState(GameState.Boss);
        private void GameEvent_StageClearExcuted() => GameEvent_TransitionState(GameState.WaveClear);


        /// <summary>
        /// Ready 상태에서 변수 초기화
        /// </summary>
        public void ReadyInit()
        {
            SceneEventHandler.SceneLoadedByPath(_scenePath);
        }

        /// <summary>
        /// Wave 상태에서 변수 초기화
        /// </summary>
        public void WaveInit()
        {
            _waveManager.WaveTime = _stageData.DefaultStageDataList[_waveManager.CurrentWave].MaxWaveTime;
            _waveManager.NextWaveCount();
        }

        /// <summary>
        /// Boss 상태에서 변수 초기화
        /// </summary>
        public void BossInit()
        {

        }

        /// <summary>
        /// Boss 클리어 상태
        /// </summary>
        public void BossClearInit()
        {
            if(IsGameClear())
            {
                GameEventHandler.GameClearExcuted?.Invoke();
            }
            else
            {
                _currentStage++;
                _waveManager.MaxWave = _stageData.DefaultStageDataList[_currentStage].MaxWaveCount;
                _scenePath = _stageData.DefaultStageDataList[_currentStage].ScenePath;
            }
        }

        /// <summary>
        /// Wave 클리어 상태
        /// </summary>
        public void WaveClearInit()
        {
            if(_waveManager.IsStageClear())
            {
                if (IsAllStageClear())
                {
                    //보스 스테이지로
                    _currentBossStage++;
                    _waveManager.MaxWave = _stageData.BossStageDataList[_currentBossStage].MaxWaveCount;
                    _scenePath = _stageData.BossStageDataList[_currentBossStage].ScenePath;
                }
                else
                {
                    //다음 스테이지로
                    _currentStage++;
                    _waveManager.MaxWave = _stageData.DefaultStageDataList[_currentStage].MaxWaveCount;
                    _scenePath = _stageData.DefaultStageDataList[_currentStage].ScenePath;
                }
            }
            else
            {
                //다음 웨이브 진행
                GameEventHandler.WaveExcuted?.Invoke();
            }
        }

        private bool IsAllStageClear()
        {
            return _currentStage == _maxStage;
        }

        private bool IsGameClear()
        {
            return _currentBossStage == _maxBossStage;
        }


        private void GameEvent_TransitionState(GameState state)
        {
            _gameStateMachine.TransitionState(state);
        }
    }
}
