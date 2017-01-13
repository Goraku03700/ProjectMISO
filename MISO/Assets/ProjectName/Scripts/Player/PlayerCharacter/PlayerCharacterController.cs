using UnityEngine;
using UnityEngine.Assertions;
using XInputDotNetPure;
using System.Collections;

/// <summary>
/// PlayerCharacterオブジェクトに対して操作するオブジェクトです。
/// </summary>
/// <remarks>
/// オブジェクトにつけたタグでどのPlayerCharacterを操作するか検索しています
/// </remarks>
public class PlayerCharacterController : MonoBehaviour {

	void Start ()
    {
        m_controllerData = Resources.Load("ScriptableObjects/PlayerCharacterControllerData") as PlayerCharacterControllerData;

        string findGameObjectName = "PlayerCharacter";
        //XInputDotNetPure.PlayerIndex playerIndex = XInputDotNetPure.PlayerIndex.One;

        switch (gameObject.tag)
        {
            case "Player1":
                {
                    findGameObjectName  += "1";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad1;
                    m_playerIndex = XInputDotNetPure.PlayerIndex.One;
                }
                break;

            case "Player2":
                {
                    findGameObjectName += "2";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad2;
                    m_playerIndex = XInputDotNetPure.PlayerIndex.Two;
                }
                break;

            case "Player3":
                {
                    findGameObjectName += "3";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad3;
                    m_playerIndex = XInputDotNetPure.PlayerIndex.Three;
                }
                break;

            case "Player4":
                {
                    findGameObjectName += "4";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad4;
                    m_playerIndex = XInputDotNetPure.PlayerIndex.Four;
                }
                break;

            default:
                {
                    Debug.LogAssertion("タグが設定されていません");
                    break;
                }
        }       // end of switch(gameObject.tag)

        m_controlledPlayerCharacterObject   = GameObject.Find(findGameObjectName);
        m_controlledPlayerCharacter         = m_controlledPlayerCharacterObject.GetComponent<PlayerCharacter>();

        m_controlledPlayerCharacter.playerIndex = m_playerIndex;

        Assert.IsNotNull(m_controlledPlayerCharacterObject);
        Assert.IsNotNull(m_controlledPlayerCharacter);
    }
	
	void Update ()
    {
        //_UpdateInputJoypad();
        _UpdateInputXInput();

#if UNITY_EDITOR

        if (m_isKeybordPlay)
        {
            _UpdateInputKeyboard();
        }

#endif  // #if UNITY_EDITOR

    }

    void _UpdateInputJoypad()
    {
        float horizontal    = MultiInput.GetAxis("Horizontal", m_joypadNumber);
        float vertical      = MultiInput.GetAxis("Vertical",   m_joypadNumber);

        float horizontal2   = MultiInput.GetAxis("Horizontal2", m_joypadNumber);
        float vertical2     = MultiInput.GetAxis("Vertical2",    m_joypadNumber);

        m_controlledPlayerCharacter.InputStick(horizontal, vertical);

        bool isPushCancelKey = MultiInput.GetButtonDown("Cancel", m_joypadNumber);
        bool isPushDashKey = MultiInput.GetButton("Dash", m_joypadNumber);

        //m_controlledPlayerCharacter.InputCancel(isPushCancelKey);
        m_controlledPlayerCharacter.InputDash(isPushDashKey);

        bool isVertical2Down = m_prevFrameVertical2 - vertical2 > 0.0f;

        //if(vertical2 <= 1.0f)
        //{

        //}

        //GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);

        if(!isPushCancelKey)
        {
            if (vertical2 <= -0.0f && isVertical2Down)
            {
                m_controlledPlayerCharacter.InputCharge();

                //GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
            }
            else if (vertical2 >= 0.0f)
            {
                m_controlledPlayerCharacter.InputThrow();
            }
        }        

        m_prevFrameVertical2 = vertical2;

        //if(vertical2 < 0.125f && vertical2 > -0.125f)
        //{
        //    m_isCanceled = true;
        //}

        //if(vertical2 < -0.125f)
        //{
        //    if (m_isCanceled)
        //    {
        //        m_controlledPlayerCharacter.InputCharge();

        //        m_isCanceled = false;
        //    }
        //}
        //else if(vertical2 > -0.125f)
        //{
        //    m_controlledPlayerCharacter.InputThrow();
        //}

        //if(vertical2 < -0.125f)
        //{
        //    m_controlledPlayerCharacter.InputCharge(vertical2);
        //}
        //else if(vertical2 > -0.125f)
        //{
        //    m_controlledPlayerCharacter.InputThrow();
        //}

        //Debug.Log(horizontal2.ToString());

        //if (isPushCancelKey)
        //    m_controlledPlayerCharacter.InputCancel();

        //if (MultiInput.GetButtonDown("Throw", m_joypadNumber))
        //{
        //    m_controlledPlayerCharacter.InputCharge();
        //}
        //else if (MultiInput.GetButtonUp("Throw", m_joypadNumber))
        //{
        //    m_controlledPlayerCharacter.InputThrow();
        //}

        //if (_CheckStickRotation(vertical, horizontal))
        //{
        //    m_controlledPlayerCharacter.InputRelease();
        //}

        //if (_CheckStickRotation(vertical2, horizontal2))
        //{
        //    m_controlledPlayerCharacter.InputPull();
        //}

        // pull
        if (_CheckStickHalfRotation(horizontal2, vertical2, ref m_pullHalfRotationInputCheck))
        {
            m_pullHalfRotationCount++;

            if(m_pullHalfRotationCount >= m_controllerData.pullHalfRotation)
            {
                m_pullHalfRotationCount = 0;

                m_controlledPlayerCharacter.InputPull();
            }
        }

        // breake
        if (_CheckStickHalfRotation(horizontal2, vertical2, ref m_releaseHalfRotationInputCheck))
        {
            m_releaseHalfRotationInputCount++;

            if (m_releaseHalfRotationInputCount >= m_controllerData.releaseHalfRotation)
            {
                m_releaseHalfRotationInputCount = 0;

                m_controlledPlayerCharacter.InputRelease();
            }
        }

        
    }

    void _UpdateInputXInput()
    {
        XInputDotNetPure.GamePadState gamePadState = XInputDotNetPure.GamePad.GetState(m_playerIndex);

        float horizontal = gamePadState.ThumbSticks.Left.X;
        float vertical = gamePadState.ThumbSticks.Left.Y;

        float horizontal2 = gamePadState.ThumbSticks.Right.X;
        float vertical2 = gamePadState.ThumbSticks.Right.Y;

        m_controlledPlayerCharacter.InputStick(horizontal, vertical);

        bool isPushCancelKey = gamePadState.Buttons.LeftShoulder == ButtonState.Pressed;
        bool isPushDashKey = gamePadState.Buttons.RightShoulder == ButtonState.Pressed;

        m_controlledPlayerCharacter.InputDash(isPushDashKey);

        bool isVertical2Down = m_prevFrameVertical2 - vertical2 > 0.0f;

        //if(vertical2 <= 1.0f)
        //{

        //}

        //GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);


        if (vertical2 <= -0.0f && isVertical2Down)
        {
            m_controlledPlayerCharacter.InputCharge();

            //GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else if (vertical2 >= 0.0f)
        {
            m_controlledPlayerCharacter.InputThrow();
        }

        m_controlledPlayerCharacter.InputCancel(isPushCancelKey);


        m_prevFrameVertical2 = vertical2;

        // pull
        if (_CheckStickHalfRotation(horizontal2, vertical2, ref m_pullHalfRotationInputCheck))
        {
            m_pullHalfRotationCount++;

            if (m_pullHalfRotationCount >= m_controllerData.pullHalfRotation)
            {
                m_pullHalfRotationCount = 0;

                m_controlledPlayerCharacter.InputPull();
            }
        }

        // breake
        if (_CheckStickHalfRotation(horizontal, vertical, ref m_releaseHalfRotationInputCheck))
        {
            m_releaseHalfRotationInputCount++;

            if (m_releaseHalfRotationInputCount >= m_controllerData.releaseHalfRotation)
            {
                m_releaseHalfRotationInputCount     = 0;
                m_releaseHalfRotationInputCount2    = 0;

                //m_controlledPlayerCharacter.InputRelease();
                m_controlledPlayerCharacter.InputRebound();
            }
        }

        if (_CheckStickHalfRotation(horizontal2, vertical2, ref m_releaseHalfRotationInputCheck2))
        {
            m_releaseHalfRotationInputCount2++;

            if (m_releaseHalfRotationInputCount2 >= m_controllerData.releaseHalfRotation)
            {
                m_releaseHalfRotationInputCount     = 0;
                m_releaseHalfRotationInputCount2    = 0;

                //m_controlledPlayerCharacter.InputRelease();
                m_controlledPlayerCharacter.InputRebound();
            }
        }
    }

    void _UpdateInputKeyboard()
    {
        float horizontal    = Input.GetAxis("Horizontal");
        float vertical      = Input.GetAxis("Vertical");

        m_controlledPlayerCharacter.InputStick(horizontal, vertical);

        //bool isPushThrowKey     = Input.GetKey(KeyCode.Z);
        bool isPushCancelKey    = Input.GetKeyDown(KeyCode.X); 

        //m_controlledPlayerCharacter.InputThrow(isPushThrowKey);
        m_controlledPlayerCharacter.InputCancel(isPushCancelKey);

        m_controlledPlayerCharacter.InputDash(Input.GetKey(KeyCode.C));

        if (Input.GetKeyDown(KeyCode.X))
        {
            m_controlledPlayerCharacter.InputPull();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            m_controlledPlayerCharacter.InputCharge();
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            m_controlledPlayerCharacter.InputThrow();
        }

        if(_CheckStickRotation(vertical, horizontal))
        {
            m_controlledPlayerCharacter.InputRelease();
            m_controlledPlayerCharacter.InputPull();
        }
    }

    bool _CheckStickRotation(float vertical, float horizontal)
    {
        switch (m_releaseInputState)
        {
            case ReleaseInputState.CheckHorizontal:

                if (Mathf.Abs(horizontal) > 0.75f &&
                    Mathf.Abs(vertical) < 0.1f)
                {
                    m_releaseInputState = ReleaseInputState.CheckVertical;
                    m_releaseInputCheckCount++;
                }

                break;

            case ReleaseInputState.CheckVertical:

                if (Mathf.Abs(vertical) > 0.75f &&
                    Mathf.Abs(horizontal) < 0.1f)
                {
                    m_releaseInputState = ReleaseInputState.CheckHorizontal;
                    m_releaseInputCheckCount++;
                }

                break;

            default:
                break;
        }

        if (m_releaseInputCheckCount >= m_releaseInputCheck)
        {
            m_releaseInputCount++;

            m_releaseInputCheckCount = 0;
        }

        if (m_releaseInputCount >= m_releaseInput)
        {
            m_releaseInputCount = 0;

            return true;
        }

        return false;
    }

    bool _CheckStickHalfRotation(
        float horizontal,
        float vertical,
        ref HalfRotationInputCheck halfRotationInputCheck)
    {
        bool result = false;

        switch (halfRotationInputCheck)
        {
            case HalfRotationInputCheck.Horizontal:

                if (Mathf.Abs(horizontal) > m_controllerData.halfRotationUpperLimit &&
                    Mathf.Abs(vertical) < m_controllerData.halfRotationLowerLimit)
                {
                    result = true;

                    halfRotationInputCheck = HalfRotationInputCheck.Vertical;
                }

                    break;

            case HalfRotationInputCheck.Vertical:

                if (Mathf.Abs(vertical) > m_controllerData.halfRotationUpperLimit &&
                    Mathf.Abs(horizontal) < m_controllerData.halfRotationLowerLimit)
                {
                    result = true;

                    halfRotationInputCheck = HalfRotationInputCheck.Horizontal;
                }

                    break;

            default:
                break;
        }

        return result;
    }

    enum HalfRotationInputCheck
    {
        Horizontal,
        Vertical,
    }

    enum ReleaseInputState
    {
        CheckHorizontal,
        CheckVertical,
    }

    [SerializeField]
    private int m_releaseInputCheck;

    [SerializeField]
    private int m_releaseInput;

    private GameObject m_controlledPlayerCharacterObject;
    private PlayerCharacter m_controlledPlayerCharacter;

    [SerializeField]
    private ReleaseInputState m_releaseInputState;

    [SerializeField]
    private int m_releaseInputCheckCount;

    [SerializeField]
    private int m_releaseInputCount;

    private HalfRotationInputCheck m_pullHalfRotationInputCheck;

    private int m_pullHalfRotationCount;

    private HalfRotationInputCheck m_releaseHalfRotationInputCheck;
    private HalfRotationInputCheck m_releaseHalfRotationInputCheck2;

    private int m_releaseHalfRotationInputCount;
    private int m_releaseHalfRotationInputCount2;

    private PlayerCharacterControllerData m_controllerData;

    private float m_prevFrameVertical2;

    private bool m_isCanceled;

#if UNITY_EDITOR

    [SerializeField]
    private bool m_isKeybordPlay;

#endif      // #if UNITY_EDITOR


    [SerializeField]
    private MultiInput.JoypadNumber m_joypadNumber;

    private XInputDotNetPure.PlayerIndex m_playerIndex;
}
