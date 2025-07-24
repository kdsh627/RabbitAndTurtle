using Manager;
using State.SceneState;
using Utilities;

public class GamePlayState : ISceneState
{
    private SceneManager _sceneManager;

    private SceneState _currentSceneState;
    public SceneState CurrentSceneState 
    { 
        get => _currentSceneState; 
        set => _currentSceneState = value; 
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="sceneManager"></param>
    public GamePlayState(SceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }

    public void Enter()
    {
        _currentSceneState = SceneState.GamePlay;
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
