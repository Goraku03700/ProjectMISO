using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのスコアなどを格納するオブジェクト
/// </summary>
public class Player : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //private PlayerCharacterController m_playerCharacterController;

    //private PlayerCharacter m_playerCharacter;

    [SerializeField]
    private int m_score;

    public int score
    {
        get
        {
            return m_score;
        }

        set
        {
            m_score = value;
        }
    }

    private List<bool> m_caughtGirlList;

    public List<bool> caughtGirlList
    {
        get
        {
            return m_caughtGirlList;
        }

        set
        {
            m_caughtGirlList = value;
        }
    }

}
