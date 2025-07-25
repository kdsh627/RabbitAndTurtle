using System;
using UnityEngine;

public static class SceneEventHandler
{
    /// <summary>
    /// 사전 로딩 완료 시
    /// </summary>
    public static Action PreloadCompleted;

    /// <summary>
    /// 로딩 진행률 변화 시 마다 실행할 이벤트
    /// </summary>
    public static Action<float> LoadProgressUpdated;

    /// <summary>
    /// 현재 씬 다시 로드 시 호출
    /// </summary>
    public static Action SceneReloaded;

    /// <summary>
    /// 가장 최근 씬 언로드
    /// </summary>
    public static Action LastSceneUnloaded;

    /// <summary>
    /// 씬 경로로 로드 (겹쳐서)
    /// </summary>
    public static Action<string> SceneLoadedAdditivelyByPath;

    /// <summary>
    /// 씬 경로로 로드 (최근 씬 닫음)
    /// </summary>
    public static Action<string> SceneLoadedByPath;

    /// <summary>
    /// 씬의 상태가 변경되는 로드
    /// </summary>
    public static Action<string, string> SceneStateChanged;

    /// <summary>
    /// 씬 경로로 언로드
    /// </summary>
    public static Action<string> SceneUnloadedByPath;

    /// <summary>
    /// 씬 리스트에 겹쳐서 활성화된 모든 씬 언로드
    /// </summary>
    public static Action AllSceneUnloaded;

    /// <summary>
    /// 빌드 세팅에 있는 씬 인덱스로 씬 로드(겹쳐서)
    /// </summary>
    public static Action<int> SceneLoadedByIndex;

}