using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class SceneEventHandler
{
    /// <summary>
    /// 사전 로딩 완료 시
    /// </summary>
    public static event Action PreloadCompleted;

    /// <summary>
    /// 로딩 진행률 변화 시 마다 실행할 이벤트
    /// </summary>
    public static event Action<float> LoadProgressUpdated;

    /// <summary>
    /// 현재 씬 다시 로드 시 호출
    /// </summary>
    public static event Action SceneReloaded;

    /// <summary>
    /// 가장 최근 씬 언로드
    /// </summary>
    public static event Action LastSceneUnloaded;

    /// <summary>
    /// 씬 경로로 로드 (겹쳐서)
    /// </summary>
    public static event Action<string> SceneLoadedAdditivelyByPath;
    /// <summary>
    /// 씬 경로로 로드 (최근 씬 닫음)
    /// </summary>
    public static event Action<string> SceneLoadedByPath;

    /// <summary>
    /// 씬의 상태가 변경되는 로드
    /// </summary>
    public static event Action<string, string> SceneStateChanged;

    /// <summary>
    /// 씬 경로로 언로드
    /// </summary>
    public static event Action<string> SceneUnloadedByPath;

    /// <summary>
    /// 씬 리스트에 겹쳐서 활성화된 모든 씬 언로드
    /// </summary>
    public static event Action AllSceneUnloaded;

    /// <summary>
    /// 빌드 세팅에 있는 씬 인덱스로 씬 로드(겹쳐서)
    /// </summary>
    public static event Action<int> SceneLoadedByIndex;

    /// <summary>
    /// 씬 한꺼번에 로드
    /// </summary>
    public static event Action<string, string, List<string>> SceneStateChangedAndLoadScenes;

    public static event Action SceneExited;

    public static event Func<Tween> SceneFadeOut;

    public static event Action SceneStarted;

    public static event Func<Tween> SceneFadeIn;

    #region Invoke 처리
    public static void PreloadCompleted_Invoke() => PreloadCompleted?.Invoke();
    public static void LoadProgressUpdated_Invoke(float progress) => LoadProgressUpdated?.Invoke(progress);
    public static void SceneReloaded_Invoke() => SceneReloaded?.Invoke();
    public static void LastSceneUnloaded_Invoke() => LastSceneUnloaded?.Invoke();
    public static void SceneLoadedAdditivelyByPath_Invoke(string path) => SceneLoadedAdditivelyByPath?.Invoke(path);
    public static void SceneLoadedByPath_Invoke(string path) => SceneLoadedByPath?.Invoke(path);
    public static void SceneStateChanged_Invoke(string path1, string path2) => SceneStateChanged?.Invoke(path1, path2);
    public static void SceneUnloadedByPath_Invoke(string path) => SceneUnloadedByPath?.Invoke(path);
    public static void AllSceneUnloaded_Invoke() => AllSceneUnloaded?.Invoke();
    public static void SceneLoadedByIndex_Invoke(int index) => SceneLoadedByIndex?.Invoke(index);
    public static void SceneStateChangedAndLoadScenes_Invoke(string path1, string path2, List<string> pathList) => SceneStateChangedAndLoadScenes?.Invoke(path1, path2, pathList);
    public static void SceneExited_Invoke() => SceneExited?.Invoke();
    public static Tween SceneFadeOut_Invoke()
    {
        return SceneFadeOut?.Invoke();
    }
    public static void SceneStarted_Invoke() => SceneStarted?.Invoke();
    public static Tween SceneFadeIn_Invoke()
    {
        return SceneFadeIn?.Invoke();
    }
    #endregion

}