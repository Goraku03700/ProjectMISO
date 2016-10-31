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
        float horizontal    = MultiInput.GetAxis(MultiInput.Key.Horizontal, m_joypadNumber);
        float vertical      = MultiInput.GetAxis(MultiInput.Key.Vertical,   m_joypadNumber);

        m_controlledPlayerCharacter.InputStick(horizontal, vertical);
    }

    void _UpdateInputKeyboard()
    {
        float horizontal    = Input.GetAxis("Horizontal");
        float vertical      = Input.GetAxis("Vertical");

        m_controlledPlayerCharacter.InputStick(horizontal, vertical);

        if(Input.GetButton("OK"))
        {
            m_controlledPlayerCharacter.InputCharge();
        }
        else if(Input.GetButton("OK"))
        {
            m_controlledPlayerCharacter.InputThrow();
        }

        if(Input.GetButton("Cancel"))
        {
            m_controlledPlayerCharacter.InputHold();
        }
    }

    GameObject      m_controlledPlayerCharacterObject;
    PlayerCharacter m_controlledPlayerCharacter;

#if UNITY_EDITOR

    [SerializeField]
    bool m_isKeybordPlay;

#endif      // #if UNITY_EDITOR


    [SerializeField]
    MultiInput.JoypadNumber m_joypadNumber;
}
