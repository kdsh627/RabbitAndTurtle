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
        public static Action<ActionMap> OnSwitchActionMap; //액션맵 변경 이벤트

        private GameObject[] _players;
        private PlayerInput _playerInput;
        private GameObject _currentPlayer;
        private PlayerAction _playerAction;
        private UIAction _UIAction;
        public GameObject CurrentPlayer => _currentPlayer;

        private void Start()
        {
            Init();
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

            //모든 플레이어 추가
            _players = GameObject.FindGameObjectsWithTag("Player");
            if (_players.Length > 0)
            {
                _currentPlayer = _players[0];
            }

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