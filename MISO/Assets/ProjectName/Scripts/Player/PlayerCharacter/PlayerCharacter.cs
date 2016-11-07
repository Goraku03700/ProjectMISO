using UnityEngine;
using UnityEngine.Assertions;
using System;
using Ribbons;
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
        CaughtRibbon,
        CaughtHold,
        InBuilding,
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

            if(m_controlledRibbon != null)
            {
                m_controlledRibbon.StickUp();
            }
        }
    }

    public void InputRelease()
    {
        if(m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.CaughtRibbon"))
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRelease]);


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

    public void InputHold(bool isPush)
    {
        m_animatorParameters.isPushHoldKey = isPush;
    }

    public void InputPull()
    {
        if(m_controlledRibbon != null)
        {
            m_controlledRibbon.Pull(transform.position, 50.0f);
        }
    }

    public void SizeAdjustEnter()
    {
        GameObject ribbonObject = Instantiate(m_ribbonObject, transform) as GameObject;

        ribbonObject.tag                        = tag;
        m_controlledRibbon                      = ribbonObject.GetComponent<Ribbons.Ribbon>();
        m_controlledRibbon.transform.position   = transform.position;
        m_lengthAdjustTime                      = m_playerCharacterData.ribbonSizeScailingTime / 2.0f;

        //m_controlledRibbon.Throw(transform.position, transform.forward, 1.0f, 1.0f);

        Assert.IsNotNull(ribbonObject);
        Assert.IsNotNull(m_controlledRibbon);
    }

    public void SizeAdjustUpdate()
    {
        m_lengthAdjustTime += Time.deltaTime;

        float t = m_lengthAdjustTime / m_playerCharacterData.ribbonSizeScailingTime;

        float ribbonSize = Mathf.PingPong(t, m_playerCharacterData.ribbonMaxScale) + m_playerCharacterData.ribbonMinScale;

        m_controlledRibbon.transform.localScale = new Vector3(ribbonSize, 1.0f, ribbonSize);

        Assert.IsNotNull(controlledRibbon);
    }

    public void SizeAdjustExit()
    {
        m_controlledRibbon.Throw(transform.position + new Vector3(.0f, 1.0f, 1.0f), transform.rotation, 1000.0f, 1000.0f);
    }

    public void LengthAdjustUpdate()
    {
        //m_controlledRibbon.Move();
    }

    public void OnRibbonLanding()
    {
        m_animatorParameters.isRibbonLanding = true;

        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]);
    }

    public void PullUpdate()
    {
        Vector3 vector = transform.position - m_controlledRibbon.transform.position;
            
        if(vector.magnitude < 3.5f)
        {
            m_animatorParameters.isPulled = true;

            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]);

            m_controlledRibbon.Pulled();

            m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]);
            //m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]);
        }
    }

    public void CaughtRibbon()
    {
        //m_animator.Play(m_mainStateHashs[(int)MainState.CaughtRibbon], 1, .0f);
        m_animator.Play("Base Layer.CaughtRibbon.Caught");
    }

    public void Collect()
    {
        m_animatorParameters.isCollect = true;

        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsCollect]);
    }

    private enum AnimatorParametersID
    {
        IsDownStick = 0,
        IsPushThrowKey,
        IsPushCancelKey,
        IsPushHoldKey,
        IsRibbonLanding,
        IsPulled,
        IsRelease,
        IsCollect,
    }

    private struct AnimatorParameters
    {
        public bool isDownStick;
        public bool isPushThrowKey;
        public bool isPushCancelKey;
        public bool isPushHoldKey;
        public bool isRibbonLanding;
        public bool isPulled;
        public bool isRelease;
        public bool isCollect;
    }

    void Awake()
    {
        m_playerCharacterData   = Resources.Load(m_playerCharacterDataPath)                 as PlayerCharacterData;
        m_ribbonObject          = Resources.Load(m_playerCharacterData.ribbonPrefabPath)    as GameObject;

        Assert.IsNotNull(m_playerCharacterData);
        Assert.IsNotNull(m_ribbonObject);
    }

    void Start()
    {
        m_animator  = GetComponent<Animator>();
        m_movable   = GetComponent<Movable>();

        Assert.IsNotNull(m_animator);
        Assert.IsNotNull(m_movable);

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

        m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick]        = Animator.StringToHash("isDownStick");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey]     = Animator.StringToHash("isPushThrowKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey]    = Animator.StringToHash("isPushCancelKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey]      = Animator.StringToHash("isPushHoldKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]    = Animator.StringToHash("isRibbonLanding");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]           = Animator.StringToHash("isPulled");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsRelease]          = Animator.StringToHash("isRelease");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsCollect]          = Animator.StringToHash("isCollect");
    }

    private void _InitializeAnimationState()
    {
        int arraySize = Enum.GetValues(typeof(MainState)).Length;

        m_mainStateHashs = new int[arraySize];

        m_mainStateHashs[(int)MainState.Start]          = Animator.StringToHash("Base Layer.Start");
        m_mainStateHashs[(int)MainState.Movable]        = Animator.StringToHash("Base Layer.Movable");
        m_mainStateHashs[(int)MainState.Throw]          = Animator.StringToHash("Base Layer.Throw");
        m_mainStateHashs[(int)MainState.Hold]           = Animator.StringToHash("Base Layer.Hold");
        m_mainStateHashs[(int)MainState.CaughtRibbon]   = Animator.StringToHash("Base Layer.CaughtRibbon");
        m_mainStateHashs[(int)MainState.CaughtHold]     = Animator.StringToHash("Base Layer.CaughtHold");

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
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick],        m_animatorParameters.isDownStick);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey],     m_animatorParameters.isPushThrowKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey],    m_animatorParameters.isPushCancelKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey],      m_animatorParameters.isPushHoldKey);
        //m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding],    m_animatorParameters.isRibbonLanding);
        //m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled],           m_animatorParameters.isPulled);
        //m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsRelease],          m_animatorParameters.isRelease);
        //m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsCollect],          m_animatorParameters.isCollect);
    }

    [SerializeField, Tooltip("")]
    private string m_playerCharacterDataPath;

    private Movable m_movable;

    private GameObject m_ribbonObject;

    private Ribbons.Ribbon m_controlledRibbon;

    public Ribbons.Ribbon controlledRibbon
    {
        get
        {
            return m_controlledRibbon;
        }

        set
        {
            m_controlledRibbon = value;
        }
    }

    //private float m_ribbonSize;

    private float m_lengthAdjustTime;

    private PlayerCharacterData m_playerCharacterData;

    private Animator m_animator;

    private AnimatorParameters m_animatorParameters;

    private AnimatorStateInfo m_animatorStateInfo;

    private int[] m_animatorParametersHashs;

    private int[] m_mainStateHashs;

    private int[] m_movableStateHashs;

    private int[] m_throwStateHashs;

    
}
