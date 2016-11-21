using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObject.Player Character Data")]
public class PlayerCharacterData : ScriptableObject
{
    [Tooltip("リボンプレハブへのパス")]
    public string ribbonPrefabPath;

    public float ribbonMaxScale;

    public float ribbonMinScale;

    [Tooltip("リボンサイズ変更時間間隔")]
    public float ribbonSizeScailingTime;

    public float ribbonPullPower;

    public float ribbonCollectLength;

    public float throwPower;

    public float throwSpeed;

    public float collectTime;

    public float inBuildingTime;

    public float shakingTime;

    public float shakingRepeat;

    public float shakingReleaseGirl;

    public float shakingInterval;
}
