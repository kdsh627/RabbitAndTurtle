using DG.Tweening;
using System;
using UnityEngine;

public static class CameraEventHandler
{
    /// <summary>
    /// 카메라 셰이크
    /// </summary>
    /// <param name="duration"> 시간 </param>
    /// <param name="strength"> 강도 </param>
    /// <param name="vibrato"> 진동 횟수 </param>
    /// <param name="randomness"> 무작위성 </param>
    /// <param name="isFadeOut"> 시간이 지날수록 흔들림을 줄일 것인지 </param>
    public static void Shake(Camera camera, float duration, float strength, int vibrato, float randomness, bool isFadeOut, TweenCallback callbackAction = null)
    {
        Tween tween = camera.DOShakePosition(duration, strength, vibrato, randomness, isFadeOut);
        if (callbackAction != null)
        {
            tween.OnComplete(callbackAction);
        }
    }

    /// <summary>
    /// 카메라 줌 기능
    /// </summary>
    /// <param name="endValue"> 최종 값 </param>
    /// <param name="duration"> 시간 </param>
    public static void Zoom(Camera camera, float endValue, float duration, TweenCallback callbackAction = null)
    {
        Tween tween = camera.DOOrthoSize(endValue, duration);
        if (callbackAction != null)
        {
            tween.OnComplete(callbackAction);
        }
    }
}
