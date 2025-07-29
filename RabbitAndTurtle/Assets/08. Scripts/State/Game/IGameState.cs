namespace State.GameState
{
    public enum GameState
    {
        Ready,
        Wave,
        Boss,
        WaveClear,
        BossClear
    }

    public interface IGameState : IState
    {
        public GameState CurrentGameState { get; }
    }
}
