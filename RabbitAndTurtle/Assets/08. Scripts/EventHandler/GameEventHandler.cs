using System;
public static class GameEventHandler
{
    public static event Action ExitExcuted;

    #region 씬 상태 관련 이벤트
    public static event Action GameClearExcuted;

    public static event Action TitleExcuted;

    public static event Action GamePlayExcuted;

    #endregion
    #region 게임 플레이 상태 관련 이벤트
    public static event Action StageExcuted;

    public static event Action StageClearExcuted;

    public static event Action WaveExcuted;

    public static event Action ReadyExcuted;

    public static event Action BossExcuted;

    public static event Action WaveClearExcuted;

    public static event Action BossClearExcuted;

    public static event Action GameOverExcuted;
    #endregion

    #region Invoke 처리
    public static void ExitExcuted_Invoke() => ExitExcuted?.Invoke();
    public static void GameClearExcuted_Invoke() => GameClearExcuted?.Invoke();
    public static void TitleExcuted_Invoke() => TitleExcuted?.Invoke();
    public static void GamePlayExcuted_Invoke() => GamePlayExcuted?.Invoke();
    public static void StageExcuted_Invoke() => StageExcuted?.Invoke();
    public static void StageClearExcuted_Invoke() => StageClearExcuted?.Invoke();
    public static void WaveExcuted_Invoke() => WaveExcuted?.Invoke();
    public static void ReadyExcuted_Invoke() => ReadyExcuted?.Invoke();
    public static void BossExcuted_Invoke() => BossExcuted?.Invoke();
    public static void WaveClearExcuted_Invoke() => WaveClearExcuted?.Invoke();
    public static void BossClearExcuted_Invoke() => BossClearExcuted?.Invoke();
    public static void GameOverExcuted_Invoke() => GameOverExcuted?.Invoke();
    #endregion

}