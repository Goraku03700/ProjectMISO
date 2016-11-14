using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    const int ConstPlayerMax = 4;

    // プレイヤーの状態
    enum PlayerState
    {
        Default,
        Walk,
        Catch,
        Aim,
    };
    
    // プレイヤーの状態
    private PlayerState[] playerState;

    // パッドの番号
    private MultiInput.JoypadNumber[] m_joypadNumber;

    // プレイヤーオブジェクト
    private GameObject[] m_player;

    // アニメータ
    private Animator[] m_animator;


  
        

	// Use this for initialization
	void Start () {

        m_joypadNumber = new MultiInput.JoypadNumber[ConstPlayerMax];
        m_joypadNumber[0] = MultiInput.JoypadNumber.Pad1;
        m_joypadNumber[1] = MultiInput.JoypadNumber.Pad2;
        m_joypadNumber[2] = MultiInput.JoypadNumber.Pad3;
        m_joypadNumber[3] = MultiInput.JoypadNumber.Pad4;



        playerState = new PlayerState[ConstPlayerMax];
        for (int i = 0; i < ConstPlayerMax; i++)
            playerState[i] = PlayerState.Default;

        m_animator = new Animator[ConstPlayerMax];
        m_animator[0] = GameObject.Find("Player01").GetComponent<Animator>();
        m_animator[1] = GameObject.Find("Player02").GetComponent<Animator>();
        m_animator[2] = GameObject.Find("Player03").GetComponent<Animator>();
        m_animator[3] = GameObject.Find("Player04").GetComponent<Animator>();

        m_player = new GameObject[ConstPlayerMax];
        m_player[0] = GameObject.Find("Player01");
        m_player[1] = GameObject.Find("Player02");
        m_player[2] = GameObject.Find("Player03");
        m_player[3] = GameObject.Find("Player04");
	}

    // Update is called once per frame
    void Update()
    {

        // 4プレイヤー分
        for (int i = 0; i < ConstPlayerMax; i++)
        {
            AnimatorStateInfo stateInfo = m_animator[0].GetCurrentAnimatorStateInfo(0);

            switch (playerState[i])
            {
                case PlayerState.Default:
                    _Walk(i);
                    _Catch(i);
                    _Aim(i);
                    break;

                case PlayerState.Walk:
                    _Walk(i);
                    _Catch(i);
                    _Aim(i);
                    break;


                case PlayerState.Catch:
                    if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Default"))
                    {
                        playerState[i] = PlayerState.Default;
                        m_animator[i].SetBool("isCatch", false);
                    }

                    break;

                case PlayerState.Aim:
                    if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Default"))
                    {
                        playerState[i] = PlayerState.Default;
                        m_animator[i].SetBool("isAim", false);
                    }

                    _Aim(i);

                    break;
            }
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

        float horizontal = MultiInput.GetAxis(MultiInput.Key.Horizontal, m_joypadNumber[playerNo]);
        float vertical = MultiInput.GetAxis(MultiInput.Key.Vertical, m_joypadNumber[playerNo]);
        if (horizontal != .0f || vertical != .0f)
        {
            Vector3 direction = new Vector3(horizontal, .0f, vertical);
            m_player[playerNo].transform.forward = direction;
            Debug.Log(vertical);
            Debug.Log(horizontal);
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

        if (MultiInput.GetButton("Attack", m_joypadNumber[playerNo]))
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

