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
            GameEventHandler.ReadyExcuted += () => GameEvent_TransitionState(GameState.Ready);
            GameEventHandler.WaveExcuted += () => GameEvent_TransitionState(GameState.Wave);
            GameEventHandler.BossExcuted += () => GameEvent_TransitionState(GameState.Boss);
            GameEventHandler.StageClearExcuted += () => GameEvent_TransitionState(GameState.WaveClear);
        }

        private void OnDisable()
        {
            GameEventHandler.ReadyExcuted -= () => GameEvent_TransitionState(GameState.Ready);
            GameEventHandler.WaveExcuted -= () => GameEvent_TransitionState(GameState.Wave);
            GameEventHandler.BossExcuted -= () => GameEvent_TransitionState(GameState.Boss);
            GameEventHandler.StageClearExcuted -= () => GameEvent_TransitionState(GameState.WaveClear);
        }

        /// <summary>
        /// Ready 상태에서 변수 초기화
        /// </summary>
        public void ReadyInit()
        {
            _currentWave = 0;
            SceneEventHandler.SceneLoadedByPath(_scenePath);
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
                //일반 스테이지로
                _currentStage++;
                _maxWave = _stageData.DefaultStageDataList[_currentStage].MaxWaveCount;
                _scenePath = _stageData.DefaultStageDataList[_currentStage].ScenePath;
            }
        }

        /// <summary>
        /// Wave 클리어 상태
        /// </summary>
        public void WaveClearInit()
        {
            if(IsStageClear())
            {
                if (IsAllStageClear())
                {
                    //보스 스테이지로
                    _currentBossStage++;
                    _maxWave = _stageData.BossStageDataList[_currentBossStage].MaxWaveCount;
                    _scenePath = _stageData.DefaultStageDataList[_currentBossStage].ScenePath;
                }
                else
                {
                    //다음 스테이지로
                    _currentStage++;
                    _maxWave = _stageData.DefaultStageDataList[_currentStage].MaxWaveCount;
                    _scenePath = _stageData.DefaultStageDataList[_currentStage].ScenePath;
                }
            }
            else
            {
                //다음 웨이브 진행
                GameEventHandler.WaveExcuted?.Invoke();
            }
        }

        private bool IsStageClear()
        {
            return _currentWave == _maxWave;
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
