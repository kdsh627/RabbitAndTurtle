using System;
using UnityEngine;
using UnityEngine.InputSystem;
public enum ActionMap
{
    Player,
    SettingUI
}


namespace Manager
{
    [RequireComponent(typeof(PlayerAction), typeof(UIAction))]
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public static Action<ActionMap> OnSwitchActionMap; //액션맵 변경 이벤트

        [Header("---- 플레이어 ----")]
        [SerializeField] private GameObject _currentPlayer;
        private PlayerInput _playerInput;
        private PlayerAction _playerAction;
        private UIAction _UIAction;
        public GameObject CurrentPlayer => _currentPlayer;

        public void DisableInput()
        {
            _playerInput.enabled = false;
        }

        public void EnableInput()
        {
            _playerInput.enabled = true;
        }

        private void Awake()
        {
            Instance = this;
            Init();
        }

        private void Start()
        {
            DisableInput();
            OnSwitchActionMap += SwitchActionMap;
        }

        private void OnDestroy()
        {
            OnSwitchActionMap -= SwitchActionMap;
        }

        private void Init()
        {
            //플레이어 인풋 컴포넌트 추가
            _playerInput = GetComponent<PlayerInput>();

            //액션에 의존성 주입
            _playerAction = GetComponent<PlayerAction>();
            if (_playerAction != null)
            {
                _playerAction.Player = _currentPlayer;
            }

            _UIAction = GetComponent<UIAction>();
        }

        //액션 맵 변경
        private void SwitchActionMap(ActionMap action)
        {
            _playerInput.SwitchCurrentActionMap(action.ToString());
        }
    }
}