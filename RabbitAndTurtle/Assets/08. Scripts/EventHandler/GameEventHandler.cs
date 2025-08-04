using System;

public static class GameEventHandler
{
    public static Action ExitExcuted;

    #region 씬 상태 관련 이벤트
    public static Action GameClearExcuted;

    public static Action TitleExcuted;

    public static Action GamePlayExcuted;
    #endregion

    #region 게임 플레이 상태 관련 이벤트
    public static Action StageExcuted;

    public static Action StageClearExcuted;

    public static Action WaveExcuted;

    public static Action ReadyExcuted;

    public static Action BossExcuted;

    public static Action WaveClearExcuted;

    public static Action BossClearExcuted;
    #endregion
}