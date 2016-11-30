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

            Vector3 direction = new Vector3(horizontal, .0f, vertical);

            m_movable.direction = direction;

            if (m_movable.enabled ||
                m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.SizeAdjust"))
            {
                transform.forward = direction;
            }

            if (m_controlledRibbon)
            {
                m_controlledRibbon.Shake(horizontal);
            }

            if (m_caughtRibbon)
            {
                m_caughtRibbon.ViolentMove(direction);
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

    public void InputDash(bool isDash)
    {
        if(isDash)
        {
            m_movable.speed = m_playerCharacterData.dashSpeed;

            if(m_rigidbody.velocity.magnitude > .0f)
                m_dashDurationTime += Time.deltaTime;
            else
                m_dashDurationTime = .0f;

            if(m_dashDurationTime > m_playerCharacterData.dashTime)
            {
                m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.Tired]);

                m_dashDurationTime = .0f;
            }
        }
        else
        {
            m_movable.speed = m_playerCharacterData.walkSpeed;

            m_dashDurationTime = 0.0f;
        }
    }

    public void InputRelease()
    {
        if(m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.CaughtRibbon.Caught") &&
            m_caughtRibbon != null)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputRelease]);

            gameObject.layer = LayerMask.NameToLayer("PlayerCharacter");

            m_caughtRibbon.Breake();

            m_caughtRibbon = null;
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

    public void InputThrow(bool isPush)
    {
        m_animatorParameters.isPushThrowKey = isPush;
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
        if (m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.Pull"))
        {
            if (m_controlledRibbon != null)
            {
                m_controlledRibbon.Pull(transform.position, m_playerCharacterData.ribbonPullPower);
            }
        }
    }

    public void InputCancel()
    {
        if (m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.Throw.SizeAdjust"))
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputCancel]);

            m_animatorParameters.isPushThrowKey = false;
        }
    }

    public void InputCancel(bool isPush)
    {
        m_animatorParameters.isPushCancelKey = isPush;
    }

    public void SizeAdjustEnter()
    {
        GameObject ribbonObject = Instantiate(m_ribbonObject, transform.position, transform.rotation) as GameObject;

        ribbonObject.tag                        = tag;
        m_controlledRibbon                      = ribbonObject.GetComponent<Ribbons.Ribbon>();
        //m_controlledRibbon.transform.position   = transform.position;
        m_controlledRibbon.playerCharacter      = this;
        m_lengthAdjustTime                      = .0f;

        m_ribbonRandingProjection.SetActive(true);

        // 念のためリセット
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]);
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]);
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsBreak]);
        m_animator.ResetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputCancel]);

        Assert.IsNotNull(ribbonObject);
        Assert.IsNotNull(m_controlledRibbon);
    }

    public void SizeAdjustUpdate()
    {
        m_lengthAdjustTime += Time.deltaTime;

        float t = m_lengthAdjustTime / m_playerCharacterData.ribbonSizeScailingTime;

        float ribbonSize = Mathf.PingPong(t, m_playerCharacterData.ribbonMaxScale - m_playerCharacterData.ribbonMinScale) + m_playerCharacterData.ribbonMinScale;

        m_controlledRibbon.transform.localScale = new Vector3(ribbonSize, m_controlledRibbon.transform.localScale.y, ribbonSize);

        Vector3 force = Vector3.up * m_playerCharacterData.throwPower + transform.forward * m_playerCharacterData.throwSpeed;

        Vector3 point = TakashiCompany.Unity.Util.TrajectoryCalculate.Force(transform.position + new Vector3(.0f, 1.0f, 1.0f), force, m_controlledRibbon.rigidbody.mass, Physics.gravity, .0f, m_playerCharacterData.ribbonProjectionTime);

        point.y = m_ribbonRandingProjection.transform.position.y;

        m_ribbonRandingProjection.transform.position    = point;
        m_ribbonRandingProjection.transform.localScale  = new Vector3(ribbonSize, ribbonSize, 1) / 4.0f;

        Assert.IsNotNull(controlledRibbon);
    }

    public void SizeAdjustExit()
    {
        if(m_animatorParameters.isPushCancelKey)
        {
            m_animatorParameters.isPushThrowKey = false;

            if(m_controlledRibbon)
            {
                Destroy(m_controlledRibbon.gameObject);
            }
        }
        else
        {
            m_ribbonRandingProjection.SetActive(false);
        }
    }

    public void LengthAdjustEnter()
    {
        m_controlledRibbon.Throw(
                transform.position + new Vector3(.0f, 1.0f, 1.0f),
                transform.rotation,
                m_playerCharacterData.throwPower,
                m_playerCharacterData.throwSpeed);
    }

    public void LengthAdjustUpdate()
    {
        //m_controlledRibbon.Move();
    }

    public void OnRibbonLanding()
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]);
    }

    public void PullUpdate()
    {
        if(m_controlledRibbon != null)
        {
            Vector3 vector = transform.position - m_controlledRibbon.transform.position;

            if (vector.magnitude < m_playerCharacterData.ribbonCollectLength)
            {
                m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]);

                m_controlledRibbon.Pulled();

                m_controlledRibbon = null;
            }
        }
    }

    public void BreakeRibbon()
    {
        if(m_controlledRibbon)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsBreak]);

            //Destroy(m_controlledRibbon.gameObject);
            m_controlledRibbon = null;
        }
    }

    public void CaughtRibbon(Ribbon caughtRibbon)
    {
        //@todo Change SetTrigger
        m_animator.Play("Base Layer.CaughtRibbon.Caught");

        gameObject.layer    = LayerMask.NameToLayer("CaughtPlayerCharacter");
        m_caughtRibbon      = caughtRibbon;

        if(m_controlledRibbon)
        {
            Destroy(m_controlledRibbon.gameObject);

            m_controlledRibbon = null;
        }
    }

    public void CatchRelease()
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InputRelease]);

        gameObject.layer = LayerMask.NameToLayer("PlayerCharacter");
    }

    public void Collect()
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.IsCollect]);

        gameObject.layer = LayerMask.NameToLayer("PlayerCharacter");

        m_collectTime = .0f;
    }

    public void CollectUpdate()
    {
        m_collectTime += Time.deltaTime;

        if(m_collectTime > m_playerCharacterData.collectTime)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.InBuilding]);

            m_inBuildingTime = .0f;
        }

        // test
        m_meshObject.SetActive(false);
        m_buildingObject.SetActive(false);
    }

    public void InBuildingEnter()
    {
        // test
        m_meshObject.SetActive(false);
        m_buildingObject.SetActive(false);
            
        m_inBuildingTime = .0f;
    }

    public void InBuildingUpdate()
    {
        m_inBuildingTime += Time.deltaTime;

        if(m_inBuildingTime > m_playerCharacterData.inBuildingTime)
        {
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.OutBuilding]);
        }
    }

    public void InBuildingExit()
    {
        // test
        m_meshObject.SetActive(true);
        m_buildingObject.SetActive(true);

        m_caughtRibbon.playerCharacter.playerFire.Fire(transform, m_rigidbody);
    }

    public void OnHoldEnter()
    {
        Ray         ray         = new Ray(transform.position, transform.forward);
        //int         layerMask   = LayerMask.GetMask(new string[] { "PlayerCharacterBuilding" });
        int layerMask = LayerMask.GetMask(new string[] { "PlayerCharacterBuilding", "Girl" });
        var rayCastHits = Physics.SphereCastAll(ray, m_collider.radius, 1.5f, layerMask);   //
        float       minDistance = 1.5f; //
        GameObject  gameObject  = null;

        GameObject playerCharacterObject = null;

        foreach(var rayCastHit in rayCastHits)
        {
            if (rayCastHit.distance < minDistance)
            {
                minDistance = rayCastHit.distance;

                gameObject = rayCastHit.collider.gameObject;

                if (gameObject.layer == LayerMask.NameToLayer("PlayerCharacterBuiling"))
                {
                    playerCharacterObject = gameObject;
                }
            }
        }

        if(playerCharacterObject != null)
        {
            m_holdingPlayerCharacter = playerCharacterObject.GetComponent<PlayerCharacter>();

            m_holdingPlayerCharacter.Hold(this);

            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.HoldPlayer]);
        }
        else
        {
            // Girl
            m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.HoldGirl]);

            m_player.score += 1;

            m_holdingGirl = gameObject.GetComponent<GirlNoPlayerCharacter>();
            //m_holdingGirl.Hold();
        }
    }

    public void OnHoldingPlayerCharacter()
    {
        
    }

    public void ShakingUpdate()
    {
        m_shakingTime += Time.deltaTime;

        float t = m_shakingTime / m_playerCharacterData.shakingTime;

        
    }

    public void OnHoldGirl()
    {
        
    }

    public void Shake()
    {
        
    }

    IEnumerator ShakeCorutine()
    {
        float releaseAngleOffset;

        releaseAngleOffset = 180.0f / m_playerCharacterData.shakingReleaseGirl;

        Quaternion releaseAngle = Quaternion.Euler(.0f, releaseAngleOffset, .0f);

        float median = m_playerCharacterData.shakingReleaseGirl / 2.0f;

        for (int i = 0; i < m_playerCharacterData.shakingRepeat; ++i)
        {
            for (int j = 0; j < m_playerCharacterData.shakingReleaseGirl; ++j)
            {
                releaseAngle = Quaternion.Euler(.0f, releaseAngleOffset * (median + j), .0f);

                releaseAngle = Quaternion.Euler(.0f, releaseAngleOffset * (median - j), .0f);
            }

            yield return new WaitForSeconds(m_playerCharacterData.shakingInterval);
        }

        yield return null;
    }

    public void Hold(PlayerCharacter holdedPlayerCharacter)
    {
        m_holdedPlayerCharacter = holdedPlayerCharacter;

        m_animator.Play("Base Layer.Holded.Holding");
    }

    public void KnockBack(Vector3 forceDirection)
    {
        m_animator.SetTrigger(m_animatorParametersHashs[(int)AnimatorParametersID.HoldGirl]);
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
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator  = GetComponent<Animator>();
        m_movable   = GetComponent<Movable>();
        m_collider  = GetComponent<SphereCollider>();   
        m_player    = transform.parent.GetComponent<Player>();

        m_meshObject                = transform.FindChild("PlayerCharacterMesh").gameObject;
        m_buildingObject            = transform.FindChild("PlayerCharacterBuilding").gameObject;
        m_ribbonRandingProjection   = transform.FindChild("RibbonLandingProjection").gameObject;

        m_playerFire                = transform.transform.FindChild("PlayerCharacterBuilding").gameObject.GetComponent<PlayerFire>();

        _InitializeAnimatorParametersID();
        _InitializeAnimationState();

        Assert.IsNotNull(m_animator);
        Assert.IsNotNull(m_movable);

    }

    void Update()
    {
        m_animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        _UpdateAnimatorParameters();
    }

    private enum AnimatorParametersID
    {
        IsDownStick = 0,
        IsPushThrowKey,
        IsPushHoldKey,
        IsPushCancelKey,
        IsRibbonLanding,
        IsPulled,
        InputRelease,
        IsCollect,
        IsBreak,
        Velocity,
        InBuilding,
        OutBuilding,
        InputCancel,
        HoldPlayer,
        HoldGirl,
        Shake,
        Knockback,
        Tired,
    }

    private struct AnimatorParameters
    {
        public bool isDownStick;
        public bool isPushThrowKey;
        public bool isPushHoldKey;
        public bool isPushCancelKey;
        public float velocity;
    }

    private void _InitializeAnimatorParametersID()
    {
        int arraySize = Enum.GetValues(typeof(AnimatorParametersID)).Length;

        m_animatorParametersHashs = new int[arraySize];

        m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick]        = Animator.StringToHash("isDownStick");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey]     = Animator.StringToHash("isPushThrowKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey]      = Animator.StringToHash("isPushHoldKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey]    = Animator.StringToHash("isPushCancelKey");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsRibbonLanding]    = Animator.StringToHash("isRibbonLanding");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]           = Animator.StringToHash("isPulled");
        m_animatorParametersHashs[(int)AnimatorParametersID.InputRelease]       = Animator.StringToHash("inputRelease");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsCollect]          = Animator.StringToHash("isCollect");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsBreak]            = Animator.StringToHash("isBreak");
        m_animatorParametersHashs[(int)AnimatorParametersID.Velocity]           = Animator.StringToHash("velocity");
        m_animatorParametersHashs[(int)AnimatorParametersID.InBuilding]         = Animator.StringToHash("inBuilding");
        m_animatorParametersHashs[(int)AnimatorParametersID.OutBuilding]        = Animator.StringToHash("outBuilding");
        m_animatorParametersHashs[(int)AnimatorParametersID.InputCancel]        = Animator.StringToHash("inputCancel");
        m_animatorParametersHashs[(int)AnimatorParametersID.HoldPlayer]         = Animator.StringToHash("holdPlayer");
        m_animatorParametersHashs[(int)AnimatorParametersID.HoldGirl]           = Animator.StringToHash("holdGirl");
        m_animatorParametersHashs[(int)AnimatorParametersID.Knockback]          = Animator.StringToHash("knockback");
        m_animatorParametersHashs[(int)AnimatorParametersID.Tired]              = Animator.StringToHash("tired");
    }

    private void _InitializeAnimationState()
    {
        int arraySize = Enum.GetValues(typeof(MainState)).Length;

        m_mainStateHashs = new int[arraySize];

        m_mainStateHashs[(int)MainState.Start]          = Animator.StringToHash("Start");
        m_mainStateHashs[(int)MainState.Movable]        = Animator.StringToHash("Movable");
        m_mainStateHashs[(int)MainState.Throw]          = Animator.StringToHash("Throw");
        m_mainStateHashs[(int)MainState.Hold]           = Animator.StringToHash("Hold");
        m_mainStateHashs[(int)MainState.CaughtRibbon]   = Animator.StringToHash("CaughtRibbon");
        m_mainStateHashs[(int)MainState.CaughtHold]     = Animator.StringToHash("CaughtHold");

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
        m_animatorParameters.velocity = m_rigidbody.velocity.magnitude;

        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsDownStick],        m_animatorParameters.isDownStick);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushThrowKey],     m_animatorParameters.isPushThrowKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushHoldKey],      m_animatorParameters.isPushHoldKey);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPushCancelKey],    m_animatorParameters.isPushCancelKey);
        m_animator.SetFloat(m_animatorParametersHashs[(int)AnimatorParametersID.Velocity],          m_animatorParameters.velocity);
    }

    [SerializeField, Tooltip("")]
    private string m_playerCharacterDataPath;

    private Rigidbody m_rigidbody;

    public new Rigidbody rigidbody
    {
        get
        {
            return m_rigidbody;
        }

        set
        {
            m_rigidbody = value;
        }
    }

    private Movable m_movable;

    private GameObject m_ribbonObject;

    private Ribbon m_controlledRibbon;

    public Ribbon controlledRibbon
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

    public Ribbon caughtRibbon
    {
        get
        {
            return m_caughtRibbon;
        }

        set
        {
            m_caughtRibbon = value;
        }
    }

    private Ribbon m_caughtRibbon;

    private float m_lengthAdjustTime;

    private float m_collectTime;

    private float m_inBuildingTime;

    private float m_dashDurationTime;

    private Player m_player;

    public Player player
    {
        get
        {
            return m_player;
        }

        set
        {
            m_player = value;
        }
    }

    private PlayerCharacter m_holdingPlayerCharacter;

    private GirlNoPlayerCharacter m_holdingGirl;

    private PlayerCharacter m_holdedPlayerCharacter;

    private float m_shakingTime;

    private PlayerCharacterData m_playerCharacterData;
    
    public PlayerCharacterData playerCharacterData
    {
        get { return m_playerCharacterData; }
    }

    private SphereCollider m_collider;

    private GameObject m_meshObject;

    private GameObject m_buildingObject;

    private GameObject m_ribbonRandingProjection;

    private Animator m_animator;

    private AnimatorParameters m_animatorParameters;

    private AnimatorStateInfo m_animatorStateInfo;

    private int[] m_animatorParametersHashs;

    private int[] m_mainStateHashs;

    private int[] m_movableStateHashs;

    private int[] m_throwStateHashs;

    public bool isCaught
    {
        get
        {
            return 
                m_animatorStateInfo.shortNameHash == Animator.StringToHash("CaughtRibbon.Caught") ||
                m_animatorStateInfo.shortNameHash == Animator.StringToHash("CaughtRibbon.Collect");
        }
    }

    private PlayerFire m_playerFire;

    public PlayerFire playerFire
    {
        get
        {
            return m_playerFire;
        }

        set
        {
            m_playerFire = value;
        }
    }
}
