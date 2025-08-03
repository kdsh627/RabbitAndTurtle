using System.Collections.Generic;
using NUnit.Framework;
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

        private StageData _currentStageData;

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

            _currentStageData = _stageData.DefaultStageDataList[_currentStage];
            _gameStateMachine = new GameStateMachine(this);
        }

        private void Start()
        {
            _gameStateMachine.Initialize(_gameStateMachine._readyState);
        }

        private void OnEnable()
        {
            GameEventHandler.ReadyExcuted += GameEvent_ReadyExcuted;
            GameEventHandler.StageExcuted += GameEvent_StageExcuted;
            GameEventHandler.WaveExcuted += GameEvent_WaveExcuted;
            GameEventHandler.WaveClearExcuted += GameEvent_WaveClearExcuted;
            GameEventHandler.StageClearExcuted += GameEvent_StageClearExcuted;
            GameEventHandler.BossExcuted += GameEvent_BossExcuted;
            GameEventHandler.BossClearExcuted += GameEvent_BossClearExcuted;
        }

        private void OnDisable()
        {
            GameEventHandler.ReadyExcuted -= GameEvent_ReadyExcuted;
            GameEventHandler.StageExcuted -= GameEvent_StageExcuted;
            GameEventHandler.WaveExcuted -= GameEvent_WaveExcuted;
            GameEventHandler.WaveClearExcuted -= GameEvent_WaveClearExcuted;
            GameEventHandler.StageClearExcuted -= GameEvent_StageClearExcuted;
            GameEventHandler.BossExcuted -= GameEvent_BossExcuted;
            GameEventHandler.BossClearExcuted -= GameEvent_BossClearExcuted;
        }

        private void GameEvent_ReadyExcuted() => GameEvent_TransitionState(GameState.Ready);

        private void GameEvent_StageExcuted() => GameEvent_TransitionState(GameState.Stage);
        private void GameEvent_WaveExcuted() => GameEvent_TransitionState(GameState.Wave);
        private void GameEvent_WaveClearExcuted() => GameEvent_TransitionState(GameState.WaveClear);
        private void GameEvent_StageClearExcuted() => GameEvent_TransitionState(GameState.StageClear);

        private void GameEvent_BossExcuted() => GameEvent_TransitionState(GameState.Boss);

        private void GameEvent_BossClearExcuted() => GameEvent_TransitionState(GameState.BossClear);


        /// <summary>
        /// Ready 상태에서 변수 초기화
        /// </summary>
        public void ReadyInit()
        {
            _scenePath = _currentStageData.ScenePath;
            SceneEventHandler.SceneLoadedByPath(_scenePath);
        }

        /// <summary>
        /// Stage 상태에서 변수 초기화
        /// </summary>
        public void StageInit()
        {
            _waveManager.MaxWave = _currentStageData.MaxWaveCount;
            _waveManager.ResetWaveCount();

            _currentStage++;
            GameEventHandler.WaveExcuted?.Invoke();
        }

        /// <summary>
        /// Wave 상태에서 변수 초기화
        /// </summary>
        public void WaveInit()
        {
            _waveManager.WaveTime = _currentStageData.MaxWaveTime[_waveManager.CurrentWave];
            _waveManager.NextWaveCount();
        }

        /// <summary>
        /// Wave 클리어 상태
        /// </summary>
        public void WaveClearInit()
        {
            if (_waveManager.IsStageClear())
            {
                GameEventHandler.StageClearExcuted?.Invoke();
            }
            else
            {
                //다음 웨이브 진행
                GameEventHandler.WaveExcuted?.Invoke();
            }
        }

        /// <summary>
        /// StageClear 상태에서 변수 초기화
        /// </summary>
        public void StageClearInit()
        {
            if (IsAllStageClear())
            {
                Debug.Log("스테이지 클리어");

                //보스 스테이지로
                _currentStageData = _stageData.BossStageDataList[_currentBossStage];

            }
            else
            {
                Debug.Log("다음 스테이지");

                //다음 스테이지로
                _currentStageData = _stageData.BossStageDataList[_currentBossStage];
            }

            GameEventHandler.ReadyExcuted?.Invoke();
        }

        /// <summary>
        /// Boss 상태에서 변수 초기화
        /// </summary>
        public void BossInit()
        {
            //_waveManager.MaxWave = _stageData.BossStageDataList[_currentBossStage].MaxWaveCount;
            _currentBossStage++;
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
                _scenePath = _stageData.DefaultStageDataList[_currentStage].ScenePath;
                //원래는 다음 스테이지의 MaxStage를 다시 받아와야하지만
                //우리 게임은 단일 스테이지라 해당 부분은 생략
                GameEventHandler.ReadyExcuted?.Invoke();
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
