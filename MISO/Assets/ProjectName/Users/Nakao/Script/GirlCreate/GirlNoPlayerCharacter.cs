using UnityEngine;
using System.Collections;

public class GirlNoPlayerCharacter : MonoBehaviour 
{

    enum State
    {
        Generation,
        Alive,
        Caught,
        Absorption,
        None
    }
    
    bool m_isDestroy;
    public bool IsDestroy
    {
        get { return m_isDestroy; }
        set { m_isDestroy = value; }
    }

    bool m_isCaught;
    public bool IsCaught
    {
        get { return m_isCaught; }
        set { m_isCaught = value; }
    }

    bool m_isAbsorption;
    public bool IsAbsorption
    {
        get { return m_isAbsorption; }
        set { m_isAbsorption = value; }
    }

    State m_status;


    [SerializeField]
    GameObject m_girlMesh;

	// Use this for initialization
	void Start () {
        m_status = State.Generation;
      //  m_girlMesh.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	    switch(m_status)
        {
            case State.Generation:
            {
                //登場演出
                if(Input.GetKeyDown(KeyCode.L))
                {
                    m_status = State.None;
                }
                break;
            }
            case State.Alive:
            {
                //ここに移動を実装予定

                if (m_isCaught)
                {
                    m_status = State.Caught;
                }
                break;
            }
            case State.Caught:
            {
                //なんか処理
                if(m_isAbsorption)
                {
                    m_status = State.Absorption;
                }

                break;
            }
            case State.Absorption:
            {
                //ベジェ曲線での取得演出処理
                //if() 
                {
                    m_status = State.None;
                }
                break;
            }
            case State.None:
            {
                //取得をUIに通知
                Destroy(m_girlMesh);
                break;
            }
            default:
            {
                break;
            }

        }


	}
}
