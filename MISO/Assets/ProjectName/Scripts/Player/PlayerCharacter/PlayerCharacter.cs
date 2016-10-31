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
public class PlayerCharacter : MonoBehaviour
{
    public enum MainState
    {
        Start = 0,
        Movable,
        Throw,
        Hold,
        Caught,
    }

    public enum MovableState
    {
        Wait = 0,
        Move,
        Exit
    }

    public enum ThrowState
    {
        SizeAdjust = 0,
        LengthAdjust,
        Pull,
        Collect,
    }

    public void InputStick(float horizontal, float vertical)
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

    public void InputCharge()
    {
        m_animatorParameters.isPushThrowKey = true;
    }

    public void InputThrow()
    {
        m_animatorParameters.isPushThrowKey = false;
    }

    public void InputHold()
    {
        m_animatorParameters.isPushHoldKey = true;
    }

    private void CreateRibbon()
    {
        //instantiate
    }

    private void Charge()
    {
        m_ribbonSize = Mathf.PingPong(Time.time / 1.0f, 10.0f);
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

    private struct AnimatorParameters
    {
        public bool isDownStick;
        public bool isPushThrowKey;
        public bool isPushCancelKey;
        public bool isPushHoldKey;
        public bool isRibbonLanding;
        public bool isPulled;
    }

    void Awake()
    {
        m_playerCharacterData = Resources.Load(m_playerCharacterDataPath) as PlayerCharacterData;

        Assert.IsNotNull(m_playerCharacterData);
    }

    void Start()
    {
        m_animator  = GetComponent<Animator>();
        m_movable   = GetComponent<Movable>();

        Assert.IsNotNull(m_animator);

        _InitializeAnimatorParametersID();
        _InitializeAnimationState();
    }

    void Update()
    {
        m_animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        _UpdateAnimatorParameters();
    }

    private void _InitializeAnimatorParametersID()
    {
        int arraySize = Enum.GetValues(typeof(AnimatorParametersID)).Length;

        m_animatorParametersHashs = new int[arraySize];

        m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick]       = Animator.StringToHash("isDownStick");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey]    = Animator.StringToHash("isPushThrowKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey]   = Animator.StringToHash("isPushCancelKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey]     = Animator.StringToHash("isPushHoldKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]   = Animator.StringToHash("isRibbonLanding");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]          = Animator.StringToHash("isPulled");
    }

    private void _InitializeAnimationState()
    {
        int arraySize = Enum.GetValues(typeof(MainState)).Length;

        m_mainStateHashs = new int[arraySize];

        m_mainStateHashs[(int)MainState.Start]      = Animator.StringToHash("Base Layer.Start");
        m_mainStateHashs[(int)MainState.Movable]    = Animator.StringToHash("Base Layer.Movable");
        m_mainStateHashs[(int)MainState.Throw]      = Animator.StringToHash("Base Layer.Throw");
        m_mainStateHashs[(int)MainState.Hold]       = Animator.StringToHash("Base Layer.Hold");
        m_mainStateHashs[(int)MainState.Caught]     = Animator.StringToHash("Base Layer.Caught");

        arraySize = Enum.GetValues(typeof(MovableState)).Length;

        m_movableStateHashs = new int[arraySize];

        m_movableStateHashs[(int)MovableState.Wait] = Animator.StringToHash("Base Layer.Movable.Wait");
        m_movableStateHashs[(int)MovableState.Move] = Animator.StringToHash("Base Layer.Movable.Move");
        m_movableStateHashs[(int)MovableState.Exit] = Animator.StringToHash("Base Layer.Movable.Exit");

        arraySize = Enum.GetValues(typeof(ThrowState)).Length;

        m_throwStateHashs = new int[arraySize];

        m_throwStateHashs[(int)ThrowState.SizeAdjust]     = Animator.StringToHash("Base Layer.Movable.SizeAdjust");
        m_throwStateHashs[(int)ThrowState.LengthAdjust]   = Animator.StringToHash("Base Layer.Movable.LengthAdjust");
        m_throwStateHashs[(int)ThrowState.Pull]           = Animator.StringToHash("Base Layer.Movable.Pull");
        m_throwStateHashs[(int)ThrowState.Collect]        = Animator.StringToHash("Base Layer.Movable.Collect");
    }

    private void _UpdateAnimatorParameters()
    {
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick],       m_animatorParameters.isDownStick);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey],    m_animatorParameters.isPushThrowKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey],   m_animatorParameters.isPushCancelKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey],     m_animatorParameters.isPushHoldKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding],   m_animatorParameters.isRibbonLanding);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled],          m_animatorParameters.isPulled);
    }

    [SerializeField, Tooltip("")]
    private string m_playerCharacterDataPath;

    private Movable m_movable;

    private Ribbon m_controlledRibbon;

    private float m_ribbonSize;

    private PlayerCharacterData m_playerCharacterData;

    private Animator m_animator;

    private AnimatorParameters m_animatorParameters;

    private AnimatorStateInfo m_animatorStateInfo;

    private int[] m_animatorParametersHashs;

    private int[] m_mainStateHashs;

    private int[] m_movableStateHashs;

    private int[] m_throwStateHashs;
}
