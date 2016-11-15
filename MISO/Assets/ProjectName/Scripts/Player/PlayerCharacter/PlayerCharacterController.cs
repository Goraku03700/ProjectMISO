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
        //_UpdateInputJoypad();

#if UNITY_EDITOR

        if (m_isKeybordPlay)
        {
            _UpdateInputKeyboard();
        }

#endif  // #if UNITY_EDITOR

    }

    void _UpdateInputJoypad()
    {
        float horizontal    = MultiInput.GetAxis(MultiInput.Key.Horizontal, m_joypadNumber);
        float vertical      = MultiInput.GetAxis(MultiInput.Key.Vertical,   m_joypadNumber);

        m_controlledPlayerCharacter.InputStick(horizontal, vertical);

        if (MultiInput.GetButtonDown("Throw", m_joypadNumber))
        {
            m_controlledPlayerCharacter.InputCharge();
            m_controlledPlayerCharacter.InputPull();
        }
        else if (MultiInput.GetButtonUp("Throw", m_joypadNumber))
        {
            m_controlledPlayerCharacter.InputThrow();
        }

        switch (m_releaseInputState)
        {
            case ReleaseInputState.CheckHorizontal:

                if (horizontal != .0f)
                {
                    m_releaseInputState = ReleaseInputState.CheckVertical;
                    m_releaseInputCheck++;
                }

                break;

            case ReleaseInputState.CheckVertical:

                if (vertical != .0f)
                {
                    m_releaseInputState = ReleaseInputState.CheckHorizontal;
                    m_releaseInputCheck++;
                }

                break;

            default:
                break;
        }

        if (m_releaseInputCheck >= 4)
        {
            m_releaseInput++;

            m_releaseInputCheck = 0;
        }

        if (m_releaseInput >= 2)
        {
            m_releaseInput = 0;

            m_controlledPlayerCharacter.InputRelease();
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

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    m_controlledPlayerCharacter.InputCancel(true);
        //}
        //else if(Input.GetKeyUp(KeyCode.X))
        //{
        //    m_controlledPlayerCharacter.InputCancel(false);
        //}

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

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    m_controlledPlayerCharacter.InputCancel(true);
        //}
        //else
        //{
        //    m_controlledPlayerCharacter.InputCancel(false);
        //}
        //else if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    m_controlledPlayerCharacter.InputCharge();
        //}
        //else if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    m_controlledPlayerCharacter.InputThrow();
        //    m_controlledPlayerCharacter.InputPull();
        //}

        //if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    m_controlledPlayerCharacter.InputThrow();
        //    m_controlledPlayerCharacter.InputPull();
        //}

        switch (m_releaseInputState)
        {
            case ReleaseInputState.CheckHorizontal:

                if(horizontal != .0f)
                {
                    m_releaseInputState = ReleaseInputState.CheckVertical;
                    m_releaseInputCheck ++;
                }

                break;

            case ReleaseInputState.CheckVertical:

                if(vertical != .0f)
                {
                    m_releaseInputState = ReleaseInputState.CheckHorizontal;
                    m_releaseInputCheck ++;
                }

                break;

            default:
                break;
        }

        if(m_releaseInputCheck >= 4)
        {
            m_releaseInput++;

            m_releaseInputCheck = 0;
        }

        if(m_releaseInput >= 2)
        {
            m_releaseInput = 0;

            m_controlledPlayerCharacter.InputRelease();
        }
    }

    enum ReleaseInputState
    {
        CheckHorizontal,
        CheckVertical,
    }

    private GameObject m_controlledPlayerCharacterObject;
    private PlayerCharacter m_controlledPlayerCharacter;

    [SerializeField]
    private ReleaseInputState m_releaseInputState;

    [SerializeField]
    private int m_releaseInputCheck;

    [SerializeField]
    private int m_releaseInput;

#if UNITY_EDITOR

    [SerializeField]
    private bool m_isKeybordPlay;

#endif      // #if UNITY_EDITOR


    [SerializeField]
    private MultiInput.JoypadNumber m_joypadNumber;
}
