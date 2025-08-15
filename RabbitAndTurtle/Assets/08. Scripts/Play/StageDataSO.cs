using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct StageData
{
    public int MaxWaveCount;
    public float[] MaxWaveTime;
}

[CreateAssetMenu(fileName = "StageDataSO", menuName = "Scriptable Objects/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    public List<StageData> DefaultStageDataList;
    public List<StageData> BossStageDataList;
}
