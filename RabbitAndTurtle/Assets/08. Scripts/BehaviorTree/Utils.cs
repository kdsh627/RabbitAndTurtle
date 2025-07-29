using System;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// 각도를 기준으로 원 둘레 구하는 로직
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 GetPositionFromAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = (float)Math.Cos(angle) * radius;
        position.y = (float)Math.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// Degree값을 Radion 값으로 변화
    /// 1도는 PI / 180 Radian
    /// angle도는 PI/180 * angle radian
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float DegreeToRadian(float angle)
    {
        return (float)Math.PI * angle / 180;
    }
}
