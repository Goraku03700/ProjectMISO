using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーキャラクターオブジェクトの動作を実装するクラス。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour {

    public enum State
    {
        Move,
        ThrowSizeAdjust,
        ThrowLengthAdjust,
        ThrowFaild,
        Pull,
        PullFaild,
        PullEnd,
        Tukamaerareta,
        Hold,
        Holding,
        HoldFaild,
        Holded,
    }

    public void InputMoveKey(float horizontal, float vertical)
    {
        if(m_state == State.Move)
        {
            
        }
    }

    public void InputThrowKey()
    {
        
    }

    IEnumerator 

	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    Rigidbody m_rigidbody;

    State m_state;
}
