﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObject.Player Character Data")]
public class PlayerCharacterData : ScriptableObject
{
    [Tooltip("引き寄せ入力半回転回数")]
    public int pullInputHalfRotation;

    public float ribbonMaxScale;

    public float ribbonMinScale;

    [Tooltip("リボンサイズ変更時間間隔")]
    public float ribbonSizeScailingTime;

    public float ribbonPullPower;

    public float ribbonCollectLength;

    public float ribbonProjectionTime;

    public float ribbonPenaltyTime;

    public float ribbonShakeSpeed;

    public float ribbonViolentMoveSpeed;

    public float ribbonPlayerReboundPower;

    public float ribbonPlayerReboundCountRatio;

    public float ribbonReboundPower;

    public float ribbonReboundCountRatio;

    public float chargeTimeMax;

    public float throwPower;

    public float throwSpeed;

    public float collectTime;

    public float knockbackPower;

    public float inBuildingTime;

    public float shakingTime;

    public float shakingRepeat;

    public float shakingReleaseGirl;

    public float shakingInterval;

    public float walkSpeed;

    //public float walkMinSpeed;

    public float dashSpeed;

    public float dashTime;

    //public float speedDownScoreMax;

    //public float speedMin;

    //public float speedDown;

    public int speedDownStartScore;

    public int speedDownEndScore;

    public float scoreWeight;

    public float speedDownMax;

    public float invisibleTime;

    public int collectScoreMinus;

    [Tooltip("リボンプレハブへのパス")]
    public string ribbonPrefabPath;
}
