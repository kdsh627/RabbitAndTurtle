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
    public static void PreloadCompleted_Invoke() => PreloadCompleted?.Invoke();

    /// <summary>
    /// 로딩 진행률 변화 시 마다 실행할 이벤트
    /// </summary>
    public static event Action<float> LoadProgressUpdated;
    public static void LoadProgressUpdated_Invoke(float progress) => LoadProgressUpdated?.Invoke(progress);

    /// <summary>
    /// 현재 씬 다시 로드 시 호출
    /// </summary>
    public static event Action SceneReloaded;
    public static void SceneReloaded_Invoke() => SceneReloaded?.Invoke();

    /// <summary>
    /// 가장 최근 씬 언로드
    /// </summary>
    public static event Action LastSceneUnloaded;
    public static void LastSceneUnloaded_Invoke() => LastSceneUnloaded?.Invoke();

    /// <summary>
    /// 씬 경로로 로드 (겹쳐서)
    /// </summary>
    public static event Action<string> SceneLoadedAdditivelyByPath;
    public static void SceneLoadedAdditivelyByPath_Invoke(string path) => SceneLoadedAdditivelyByPath?.Invoke(path);
    /// <summary>
    /// 씬 경로로 로드 (최근 씬 닫음)
    /// </summary>
    public static event Action<string> SceneLoadedByPath;
    public static void SceneLoadedByPath_Invoke(string path) => SceneLoadedByPath?.Invoke(path);

    /// <summary>
    /// 씬의 상태가 변경되는 로드
    /// </summary>
    public static event Action<string, string> SceneStateChanged;
    public static void SceneStateChanged_Invoke(string path1, string path2) => SceneStateChanged?.Invoke(path1, path2);

    /// <summary>
    /// 씬 경로로 언로드
    /// </summary>
    public static event Action<string> SceneUnloadedByPath;
    public static void SceneUnloadedByPath_Invoke(string path) => SceneUnloadedByPath?.Invoke(path);

    /// <summary>
    /// 씬 리스트에 겹쳐서 활성화된 모든 씬 언로드
    /// </summary>
    public static event Action AllSceneUnloaded;
    public static void AllSceneUnloaded_Invoke() => AllSceneUnloaded?.Invoke();

    /// <summary>
    /// 빌드 세팅에 있는 씬 인덱스로 씬 로드(겹쳐서)
    /// </summary>
    public static event Action<int> SceneLoadedByIndex;
    public static void SceneLoadedByIndex_Invoke(int index) => SceneLoadedByIndex?.Invoke(index);

    /// <summary>
    /// 씬 한꺼번에 로드
    /// </summary>
    public static event Action<string, string, List<string>> SceneStateChangedAndLoadScenes;
    public static void SceneStateChangedAndLoadScenes_Invoke(string path1, string path2, List<string> pathList) => SceneStateChangedAndLoadScenes?.Invoke(path1, path2, pathList);

    public static event Action SceneExited;
    public static void SceneExited_Invoke() => SceneExited?.Invoke();


    public static event Func<Tween> SceneFadeOut;
    public static Tween SceneFadeOut_Invoke()
    {
        return SceneFadeOut?.Invoke();
    }

    public static event Action SceneStarted;
    public static void SceneStarted_Invoke() => SceneStarted?.Invoke();

    public static event Func<Tween> SceneFadeIn;
    public static Tween SceneFadeIn_Invoke()
    {
        return SceneFadeIn?.Invoke();
    }

}