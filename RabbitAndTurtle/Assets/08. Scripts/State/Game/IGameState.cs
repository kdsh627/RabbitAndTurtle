namespace State.GameState
{
    public enum GameState
    {
        Ready,
        Stage,
        Wave,
        Boss,
        WaveClear,
        StageClear,
        BossClear,
    }

    public interface IGameState : IState
    {
        public GameState CurrentGameState { get; }
    }
}
