using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


    // プレイヤーの状態
    enum PlayerState
    {
        Default,
        Walk,
        Catch,
        Aim,
    };
    PlayerState[] playerState;

    private GameObject[] m_player;

    private Animator[] m_animator;
        

	// Use this for initialization
	void Start () {

        playerState = new PlayerState[4];
        for (int i = 0; i < 4; i++)
            playerState[i] = PlayerState.Default;

        m_animator = new Animator[4];
        m_animator[0] = GameObject.Find("Player01").GetComponent<Animator>();
        m_animator[1] = GameObject.Find("Player02").GetComponent<Animator>();
        m_animator[2] = GameObject.Find("Player03").GetComponent<Animator>();
        m_animator[3] = GameObject.Find("Player04").GetComponent<Animator>();

        m_player = new GameObject[4];
        m_player[0] = GameObject.Find("Player01");
        m_player[1] = GameObject.Find("Player02");
        m_player[2] = GameObject.Find("Player03");
        m_player[3] = GameObject.Find("Player04");
	}

    // Update is called once per frame
    void Update()
    {

        // フラグ初期化
        //m_animator[0].SetBool("isWalk", false);
        //m_animator[0].SetBool("isCatch", false);
        //m_animator[0].GetComponent<Animator>().SetBool("isAim", false);
        //playerState[0] = PlayerState.Default;

        AnimatorStateInfo stateInfo = m_animator[0].GetCurrentAnimatorStateInfo(0);


            switch (playerState[0])
        {
            case PlayerState.Default:
                _Walk(0);
                _Catch(0);
                _Aim(0);
                break;

            case PlayerState.Walk:
                _Walk(0);
                _Catch(0);
                _Aim(0);
                break;


            case PlayerState.Catch:
                if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Default"))
                {
                    playerState[0] = PlayerState.Default;
                    m_animator[0].SetBool("isCatch", false);
                }

                break;

            case PlayerState.Aim:
                if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Default"))
                {
                    playerState[0] = PlayerState.Default;
                    m_animator[0].SetBool("isAim", false);
                }

                _Aim(0);

                break;
        }


    }


    /// <summary>
    /// 移動アニメーション
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    private void _Walk(int playerNo)
    {
        // 何も押されていなかったら止めるため
        playerState[playerNo] = PlayerState.Default;
        m_animator[playerNo].SetBool("isWalk", false);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_player[playerNo].transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            m_animator[playerNo].SetBool("isWalk", true);
            playerState[playerNo] = PlayerState.Walk;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            m_player[playerNo].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            m_animator[playerNo].SetBool("isWalk", true);
            playerState[playerNo] = PlayerState.Walk;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_player[playerNo].transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
            m_animator[playerNo].SetBool("isWalk", true);
            playerState[playerNo] = PlayerState.Walk;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_player[playerNo].transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            m_animator[playerNo].SetBool("isWalk", true);
            playerState[playerNo] = PlayerState.Walk;
        }

    }

    /// <summary>
    /// 掴みアニメーション
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    private void _Catch(int playerNo)
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_animator[playerNo].SetBool("isCatch", true);
            playerState[playerNo] = PlayerState.Catch;
        }
    }

    /// <summary>
    /// 投げアニメーション
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    private void _Aim(int playerNo)
    {
        // 離したか確認できるようにするため
        m_animator[playerNo].SetBool("isAim", false);
            
        if (Input.GetKey(KeyCode.S))
        {
            m_animator[playerNo].SetBool("isAim", true);
            playerState[playerNo] = PlayerState.Aim;
        }
    }


}

