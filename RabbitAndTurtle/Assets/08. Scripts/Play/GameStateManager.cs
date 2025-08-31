using DG.Tweening;
using State.GameState;
using StateMachine.SceneStateMachine;
using UnityEngine;

namespace Manager
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instace { get; set; }

        [SerializeField] private StageDataSO _stageData;
        [SerializeField] private GameObject _stageClearUI;
        [SerializeField] private WaveManager _waveManager;

        private StageData _currentStageData;

        private int _currentBossStage;
        private int _maxBossStage;

        private string _currentScenePath;
        private int _currentStage;
        private int _maxStage;

        private IGameState _nextGameState;

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

            _maxStage = SceneDataManager.Instance.GetWaveSubSceneCount();
            _maxBossStage = SceneDataManager.Instance.GetBossSubSceneCount();

            _currentStageData = _stageData.DefaultStageDataList[_currentStage];
            _gameStateMachine = new GameStateMachine(this);
        }

        private void Start()
        {
            _gameStateMachine.SetState(_gameStateMachine._readyState);
        }

        private void OnEnable()
        {
            GameEventHandler.GameOverExcuted += UIEventHandler.ToggleGameOverUI_Invoke;

            GameEventHandler.ReadyExcuted += GameEvent_ToReady;
            GameEventHandler.StageExcuted += GameEvent_ToStage;
            GameEventHandler.WaveExcuted += GameEvent_ToWave;
            GameEventHandler.WaveClearExcuted += GameEvent_ToWaveClear;
            GameEventHandler.StageClearExcuted += GameEvent_ToStageClear;
            GameEventHandler.BossExcuted += GameEvent_ToBoss;
            GameEventHandler.BossClearExcuted += GameEvent_ToBossClear;
        }

        private void OnDisable()
        {
            GameEventHandler.GameOverExcuted -= UIEventHandler.ToggleGameOverUI_Invoke;

            GameEventHandler.ReadyExcuted -= GameEvent_ToReady;
            GameEventHandler.StageExcuted -= GameEvent_ToStage;
            GameEventHandler.WaveExcuted -= GameEvent_ToWave;
            GameEventHandler.WaveClearExcuted -= GameEvent_ToWaveClear;
            GameEventHandler.StageClearExcuted -= GameEvent_ToStageClear;
            GameEventHandler.BossExcuted -= GameEvent_ToBoss;
            GameEventHandler.BossClearExcuted -= GameEvent_ToBossClear;
        }

        private void GameEvent_ToReady()
        {
            _nextGameState = _gameStateMachine._readyState;

            GameEvent_TransitionScene(_nextGameState);
        }

        private void GameEvent_ToStage()
        {
            _nextGameState = _gameStateMachine._stageState;

            GameEvent_TransitionScene(_nextGameState);
        }

        private void GameEvent_ToWave()
        {
            _nextGameState = _gameStateMachine._waveState;

            GameEvent_TransitionScene(_nextGameState);
        }

        public void GameEvent_ToWaveClear()
        {
            _nextGameState = _gameStateMachine._waveClearState;

            GameEvent_TransitionScene(_nextGameState);
        }

        private void GameEvent_ToStageClear()
        {
            _nextGameState = _gameStateMachine._stageClearState;

            GameEvent_TransitionScene(_nextGameState);
        }

        private void GameEvent_ToBoss()
        {
            _nextGameState = _gameStateMachine._bossState;

            GameEvent_TransitionScene(_nextGameState);
        }

        private void GameEvent_ToBossClear()
        {
            _nextGameState = _gameStateMachine._bossClearState;

            GameEvent_TransitionScene(_nextGameState);
        }

        private void GameEvent_TransitionScene(IGameState state)
        {
            StateManager.Instance._stateMachine = _gameStateMachine;
            StateManager.Instance._nextState = state;

            Debug.Log(state);
            switch (state.SceneType)
            {
                case GameState.Ready:
                    InputManager.Instance.DisableInput();
                    SceneEventHandler.SceneLoadedByPath_Invoke(_currentScenePath);
                    break;
                default:
                    _gameStateMachine.TransitionTo(state);
                    break;
            }
        }

        /// <summary>
        /// Ready 상태에서 변수 초기화
        /// </summary>
        public void ReadyInit()
        {
            SceneEventHandler.SceneLoadedByPath_Invoke(_currentScenePath);
        }

        /// <summary>
        /// Stage 상태에서 변수 초기화
        /// </summary>
        public void StageInit()
        {
            _waveManager.MaxWave = _currentStageData.MaxWaveCount;
            _waveManager.ResetWaveCount();

            _currentStage++;

            GameEventHandler.WaveExcuted_Invoke();
        }

        /// <summary>
        /// Wave 상태에서 변수 초기화
        /// </summary>
        public void WaveInit()
        {
            Debug.Log("실행");

            _waveManager.WaveTime = _currentStageData.MaxWaveTime[_waveManager.CurrentWave];
            InputManager.Instance.EnableInput();
            _waveManager.NextWaveCount();
        }

        /// <summary>
        /// Wave 클리어 상태
        /// </summary>
        public void WaveClearInit()
        {
            if (_waveManager.IsStageClear())
            {
                GameEventHandler.StageClearExcuted_Invoke();
            }
            else
            {
                //다음 웨이브 진행
                GameEventHandler.WaveExcuted_Invoke();
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

                //GameEventHandler.GameClearExcuted_Invoke();

                //보스 스테이지로
                //_currentStageData = _stageData.BossStageDataList[_currentBossStage];
                //_currentScenePath = SceneDataManager.Instance.GetBossSubScene(_currentBossStage);

            }
            else
            {
                Debug.Log("다음 스테이지");

                //다음 스테이지로
                _currentStageData = _stageData.DefaultStageDataList[_currentStage];
                _currentScenePath = SceneDataManager.Instance.GetWaveSubScene(_currentStage);
            }

            Sequence stageClearSequence = DOTween.Sequence();

            stageClearSequence.AppendCallback(() =>
                {
                    _stageClearUI.SetActive(true);
                }    
            );

            stageClearSequence.AppendInterval(3f);

            stageClearSequence.AppendCallback(() => {
                Debug.Log("실행");
                _stageClearUI.SetActive(false);
                if(IsAllStageClear())
                {
                    GameEventHandler.GameClearExcuted_Invoke();
                }
                else
                {
                    GameEventHandler.ReadyExcuted_Invoke();
                }
            });

            stageClearSequence.Play();
        }

        /// <summary>
        /// Boss 상태에서 변수 초기화
        /// </summary>
        public void BossInit()
        {
            //_waveManager.MaxWave = _stageData.BossStageDataList[_currentBossStage].MaxWaveCount;
            InputManager.Instance.EnableInput();
            _currentBossStage++;
        }

        /// <summary>
        /// Boss 클리어 상태
        /// </summary>
        public void BossClearInit()
        {
            if (IsGameClear())
            {
                GameEventHandler.GameClearExcuted_Invoke();
            }
            else
            {
                _currentScenePath = SceneDataManager.Instance.GetWaveSubScene(_currentStage);

                //원래는 다음 스테이지의 MaxStage를 다시 받아와야하지만
                //우리 게임은 단일 스테이지라 해당 부분은 생략
                GameEventHandler.ReadyExcuted_Invoke();
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
    }
}
