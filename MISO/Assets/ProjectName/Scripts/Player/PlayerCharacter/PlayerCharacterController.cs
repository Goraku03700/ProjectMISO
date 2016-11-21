using UnityEngine;
using UnityEngine.Assertions;
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
        string findGameObjectName = "PlayerCharacter";

        switch(gameObject.tag)
        {
            case "Player1":
                {
                    findGameObjectName  += "1";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad1;
                }
                break;

            case "Player2":
                {
                    findGameObjectName += "2";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad2;
                }
                break;

            case "Player3":
                {
                    findGameObjectName += "3";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad3;
                }
                break;

            case "Player4":
                {
                    findGameObjectName += "4";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad4;
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

        Assert.IsNotNull(m_controlledPlayerCharacterObject);
        Assert.IsNotNull(m_controlledPlayerCharacter);
    }
	
	void Update ()
    {
        _UpdateInputJoypad();

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

        m_controlledPlayerCharacter.InputStick(horizontal, vertical);

        bool isPushCancelKey = MultiInput.GetButtonDown("Cancel", m_joypadNumber);

        m_controlledPlayerCharacter.InputCancel(isPushCancelKey);

        if (MultiInput.GetButtonDown("Throw", m_joypadNumber))
        {
            m_controlledPlayerCharacter.InputCharge();
        }
        else if (MultiInput.GetButtonUp("Throw", m_joypadNumber))
        {
            m_controlledPlayerCharacter.InputThrow();
        }

        if (_CheckStickRotation(vertical, horizontal))
        {
            m_controlledPlayerCharacter.InputRelease();
            m_controlledPlayerCharacter.InputPull();
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

#if UNITY_EDITOR

    [SerializeField]
    private bool m_isKeybordPlay;

#endif      // #if UNITY_EDITOR


    [SerializeField]
    private MultiInput.JoypadNumber m_joypadNumber;
}
