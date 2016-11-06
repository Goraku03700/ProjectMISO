using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObject.Player Character Data")]
public class PlayerCharacterData : ScriptableObject
{
    [Tooltip("リボンプレハブへのパス")]
    public string ribbonPrefabPath;

    public Vector3 ribbonDefaultScale;

    public float ribbonMaxScale;

    public float ribbonMinScale;

    [Tooltip("リボンサイズ変更時間間隔")]
    public float ribbonSizeScailingTime;
}
