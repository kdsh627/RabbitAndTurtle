using Manager;
using State.SceneState;

public class TitleState : ISceneState
{
    private SceneStateManager _sceneManager;

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
    public TitleState(SceneStateManager sceneManager)
    {
        _sceneManager = sceneManager;
    }

    public void Enter()
    {
        _currentSceneState = SceneState.Title;
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}
