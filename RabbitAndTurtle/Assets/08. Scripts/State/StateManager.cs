using State;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    public StateMachine.StateMachine _stateMachine;
    public IState _nextState;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        SceneEventHandler.SceneExited += HandleSceneExited;
    }

    private void OnDisable()
    {
        SceneEventHandler.SceneExited -= HandleSceneExited;
    }

    public void HandleSceneExited()
    {
        _stateMachine.TransitionTo(_nextState);
    }
}
