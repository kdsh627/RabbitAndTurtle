namespace State.GameState
{
    public enum GameState
    {
        Ready,
        Wave,
        Boss,
        Clear
    }

    public interface IGameState : IState
    {
        public GameState CurrentGameState { get; }
    }
}
