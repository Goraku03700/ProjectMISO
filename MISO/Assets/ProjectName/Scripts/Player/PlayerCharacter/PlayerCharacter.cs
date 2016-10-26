using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;

/// <summary>
/// プレイヤーキャラクターオブジェクトの動作を実装するクラス。
/// </summary>
[RequireComponent(
    //typeof(Rigidbody),
    typeof(Animator),
    typeof(Movable))]
public class PlayerCharacter : MonoBehaviour {

    public struct AnimatorParameters
    {
        public bool isDownStick;
        public bool isPushThrowKey;
        public bool isPushCancelKey;
        public bool isPushHoldKey;
        public bool isRibbonLanding;
        public bool isPulled;
    }

    public void Move(float horizontal, float vertical)
    {
        if (horizontal != .0f || vertical != .0f)
        {
            m_animatorParameters.isDownStick = true;

            if(m_movable.enabled)
            {
                Vector3 direction = new Vector3(horizontal, .0f, vertical);

                m_movable.direction = direction;
                transform.forward   = direction;
            }
        }
        else
        {
            m_animatorParameters.isDownStick = false;

            m_movable.direction = Vector3.zero;
        }
    }

    private enum AnimatorParametersID
    {
        IsDownStick = 0,
        IsPushThrowKey,
        IsPushCancelKey,
        IsPushHoldKey,
        IsRibbonLanding,
        IsPulled,
    }

    void Start()
    {
        m_animator  = GetComponent<Animator>();
        m_movable   = GetComponent<Movable>();

        Assert.IsNotNull(m_animator);

        _InitializeAnimatorParametersID();
    }

    void Update()
    {
        _UpdateAnimatorParameters();
    }

    private void _InitializeAnimatorParametersID()
    {
        int arraySize = Enum.GetValues(typeof(AnimatorParametersID)).Length;

        m_animatorParametersID = new int[arraySize];

        m_animatorParametersID[(int)AnimatorParametersID.IsDownStick]       = Animator.StringToHash("isDownStick");
        m_animatorParametersID[(int)AnimatorParametersID.IsPushThrowKey]    = Animator.StringToHash("isPushThrowKey");
        m_animatorParametersID[(int)AnimatorParametersID.IsPushCancelKey]   = Animator.StringToHash("isPushCancelKey");
        m_animatorParametersID[(int)AnimatorParametersID.IsPushHoldKey]     = Animator.StringToHash("isPushHoldKey");
        m_animatorParametersID[(int)AnimatorParametersID.IsRibbonLanding]   = Animator.StringToHash("isRibbonLanding");
        m_animatorParametersID[(int)AnimatorParametersID.IsPulled]          = Animator.StringToHash("isPulled");
    }

    private void _UpdateAnimatorParameters()
    {
        m_animator.SetBool(m_animatorParametersID[(int)AnimatorParametersID.IsDownStick],       m_animatorParameters.isDownStick);
        m_animator.SetBool(m_animatorParametersID[(int)AnimatorParametersID.IsPushThrowKey],    m_animatorParameters.isPushThrowKey);
        m_animator.SetBool(m_animatorParametersID[(int)AnimatorParametersID.IsPushCancelKey],   m_animatorParameters.isPushCancelKey);
        m_animator.SetBool(m_animatorParametersID[(int)AnimatorParametersID.IsPushHoldKey],     m_animatorParameters.isPushHoldKey);
        m_animator.SetBool(m_animatorParametersID[(int)AnimatorParametersID.IsRibbonLanding],   m_animatorParameters.isRibbonLanding);
        m_animator.SetBool(m_animatorParametersID[(int)AnimatorParametersID.IsPulled],          m_animatorParameters.isPulled);
    }

    private const float RotationLerpSmooth = 12.5f;

    private Animator m_animator;

    private Movable m_movable;

    private AnimatorParameters m_animatorParameters;

    public AnimatorParameters animatorParameters
    {
        get { return m_animatorParameters; }
        set { m_animatorParameters = value; }
    }

    private int[] m_animatorParametersID;
}
