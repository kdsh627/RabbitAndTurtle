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
        public string ScenePath { get; }
        public SceneState StateType { get; }
    }
}

