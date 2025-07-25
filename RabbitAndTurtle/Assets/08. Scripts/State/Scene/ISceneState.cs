namespace State.SceneState
{
    public enum SceneState
    {
        Title,
        GamePlay,
        Clear
    }

    public interface ISceneState : IState
    {
        public SceneState CurrentSceneState { get; set; }
    }
}

