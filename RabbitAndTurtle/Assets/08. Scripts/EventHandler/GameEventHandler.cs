using System;

public static class GameEventHandler
{
    #region 씬 상태 관련 이벤트
    public static Action GameClearExcuted;

    public static Action TitleExcuted;

    public static Action GamePlayExcuted;

    public static Action ExitExcuted;
    #endregion

    #region 게임 플레이 상태 관련 이벤트
    public static Action WaveExcuted;

    public static Action ReadyExcuted;

    public static Action BossExcuted;

    public static Action StageClearExcuted;
    #endregion
}