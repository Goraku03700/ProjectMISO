using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObject.Player Character Controller Data")]
public class PlayerCharacterControllerData : ScriptableObject
{
    public float halfRotationUpperLimit;

    public float halfRotationLowerLimit;

    public int pullHalfRotation;

    public int releaseHalfRotation;
}
