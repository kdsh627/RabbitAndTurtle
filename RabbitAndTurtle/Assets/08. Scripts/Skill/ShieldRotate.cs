using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

[Serializable]
public struct PolarCoordinates
{
    public float Radius { get; set; }
    public float Angle
    {
        get => _angle;
        set => _angle = ClampAngle(value);
    }
    private float _angle;

    public PolarCoordinates(float radius, float angle)
    {
        Radius = radius;
        _angle = ClampAngle(angle);
    }

    private static float ClampAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0f)
            angle += 360f;
        return angle;
    }

    //+x축 기준 반시계 각도 <-> +y축 기준 시계 각도 서로 변환
    private static float CovertAngle(float angle) => 90f - angle;

    //Degree(0 ~ 360)로 Sin 계산
    private static float Sin(float angle) => Mathf.Sin(angle * Mathf.Deg2Rad);

    //Degree(0 ~ 360)로 Cos 계산
    private static float Cos(float angle) => Mathf.Cos(angle * Mathf.Deg2Rad);

    public static PolarCoordinates Zero => new PolarCoordinates(0f, 0f);
    public static PolarCoordinates North => new PolarCoordinates(1f, 0f);
    public static PolarCoordinates East => new PolarCoordinates(1f, 90f);
    public static PolarCoordinates South => new PolarCoordinates(1f, 180f);
    public static PolarCoordinates West => new PolarCoordinates(1f, 270f);

    public static PolarCoordinates FromVector2(in Vector2 vec)
    {
        if (vec == Vector2.zero)
            return Zero;

        float radius = vec.magnitude;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        return new PolarCoordinates(radius, CovertAngle(angle));
    }

    public static bool operator ==(PolarCoordinates a, PolarCoordinates b)
    {
        return Mathf.Approximately(a.Angle, b.Angle) &&
                Mathf.Approximately(a.Radius, b.Radius);
    }

    public static bool operator !=(PolarCoordinates a, PolarCoordinates b)
    {
        return !(Mathf.Approximately(a.Angle, b.Angle) &&
                Mathf.Approximately(a.Radius, b.Radius));
    }

    public PolarCoordinates Normalized => new PolarCoordinates(1f, Angle);

    public Vector2 ToVector2()
    {
        if (Radius == 0f && Angle == 0f)
            return Vector2.zero;

        float angle = CovertAngle(Angle);
        return new Vector2(Radius * Cos(angle), Radius * Sin(angle));
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (obj is PolarCoordinates other)
        {
            return this == other;
        }
        else
            return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class ShieldRotate : MonoBehaviour
{
    [Header("--- 쉴드 정보 ---")]
    [SerializeField] private int _count;
    [SerializeField] private float _radius;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _gameObject;

    private List<Vector2> _pieceDirections;

    private void Start()
    {
        InitPieceDirections();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, _speed) * Time.deltaTime);
    }

    private void EnforceSkill()
    {
        _count++;
        InitPieceDirections();
    }

    private void InitPieceDirections()
    {
        if(_pieceDirections != null)
        {
            _pieceDirections.Clear();
        }

        _pieceDirections = new List<Vector2>();

        float angle = 360f / _count;

        for (int i = 0; i < _count; i++)
        {
            //극좌표 변환
            _pieceDirections.Add(new PolarCoordinates(_radius, angle * i).ToVector2());
            GameObject go = Instantiate(_gameObject, transform);
            go.transform.localPosition = _pieceDirections[i];

            transform.TransformPoint(_pieceDirections[i]);
            Vector2 center = transform.position;
            Vector2 coordinate = transform.TransformPoint(_pieceDirections[i]);

            //transform.up은 월드 좌표계 기준 계산이기 때문에 방향벡터도 월드 좌표로 해주는게 좋다.
            Vector2 dirToCenter = (center - coordinate).normalized;
            go.transform.up = dirToCenter;
        }
    }
}