using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class PlayerCharacterController : MonoBehaviour {

	void Start ()
    {
        string findGameObjectName = "";

        switch(gameObject.tag)
        {
            case "Player1":
                {
                    findGameObjectName  = "PlayerCharacter1";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad1;
                }
                break;

            case "Player2":
                {
                    findGameObjectName = "PlayerCharacter2";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad2;
                }
                break;

            case "Player3":
                {
                    findGameObjectName = "PlayerCharacter3";
                    m_joypadNumber = MultiInput.JoypadNumber.Pad3;
                }
                break;

            case "Player4":
                {
                    findGameObjectName = "PlayerCharacter4";
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
        _UpdateInput();
	}

    void FixedUpdate()
    {

    }

    void _UpdateInput()
    {

    }

    void _UpdateInputJoypad()
    {

    }

    void _UpdateInputKeyboard()
    {

    }

    GameObject      m_controlledPlayerCharacterObject;
    PlayerCharacter m_controlledPlayerCharacter;

    [SerializeField]
    MultiInput.JoypadNumber m_joypadNumber;
}
