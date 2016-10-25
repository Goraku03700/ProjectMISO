using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

/// <summary>
/// PlayerCharacterオブジェクトに対して操作するオブジェクトです。
/// </summary>
/// <remarks>
/// オブジェクトにつけたタグで
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

        Assert.IsNull(m_controlledPlayerCharacterObject);
        Assert.IsNull(m_controlledPlayerCharacter);
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
        
    }

    void _UpdateInputKeyboard()
    {
        
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
