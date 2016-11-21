using UnityEngine;
using System.Collections;

public class SelectPlayer: MonoBehaviour
{


    // プレイヤーの状態
    enum PlayerState
    {
        Default,    // デフォルト
        Walk,       // 歩き
        Run,        // ダッシュ
        Catch,      // 掴み
        Aim,        // リボン投げ
    };

    // プレイヤーの状態
    private PlayerState m_playerState;

    // パッドの番号
    private MultiInput.JoypadNumber m_joypadNumber;

    // アニメーター
    private Animator m_animator;

    private bool m_readyFlag;

    // Use this for initialization
    void Start()
    {

        // パッドの設定
        switch (gameObject.tag)
        {
            case "Player1":
                {
                    m_joypadNumber = MultiInput.JoypadNumber.Pad1;
                }
                break;

            case "Player2":
                {
                    m_joypadNumber = MultiInput.JoypadNumber.Pad2;
                }
                break;

            case "Player3":
                {
                    m_joypadNumber = MultiInput.JoypadNumber.Pad3;
                }
                break;

            case "Player4":
                {
                    m_joypadNumber = MultiInput.JoypadNumber.Pad4;
                }
                break;

            default:
                {
                    Debug.LogAssertion("タグが設定されていません");
                    break;
                }
        }       // end of switch(gameObject.tag)

        m_animator = this.GetComponent<Animator>();

        m_playerState = PlayerState.Default;

        m_readyFlag = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        // 現在のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        switch (m_playerState)
        {
            case PlayerState.Default:
                _Walk();
                _Run();
                _Catch();
                _Aim();
                break;

            case PlayerState.Walk:
                _Walk();
                _Run();
                _Catch();
                _Aim();
                break;

            case PlayerState.Run:
                _Walk();
                _Run();
                break;


            case PlayerState.Catch:
                // アニメーションが終了していたら状態を戻す
                if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Default"))
                {
                    m_playerState = PlayerState.Default;
                    m_animator.SetBool("isCatch", false);
                }

                break;

            case PlayerState.Aim:
                // アニメーションが終了していたら状態を戻す
                if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Default"))
                {
                    m_playerState = PlayerState.Default;
                    m_animator.SetBool("isAim", false);
                }

                _Aim();

                break;
            }

        // 準備のフラグ
        //if (MultiInput.GetButton("Pause", m_joypadNumber))
        //{
        //    m_readyFlag = true;
        //}

    }



    /// <summary>I
    /// 移動アニメーション
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    private void _Walk()
    {
        // 何も押されていなかったら止めるため
        m_playerState = PlayerState.Default;
        m_animator.SetBool("isWalk", false);
       
        // キーボード用-------------
        /*
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            m_animator.SetBool("isWalk", true);
            playerState = PlayerState.Walk;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
            m_animator.SetBool("isWalk", true);
            playerState = PlayerState.Walk;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
            m_animator.SetBool("isWalk", true);
            playerState = PlayerState.Walk;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
            m_animator.SetBool("isWalk", true);
            playerState = PlayerState.Walk;
        }
        */

        float horizontal = MultiInput.GetAxis(MultiInput.Key.Horizontal, m_joypadNumber);
        float vertical = MultiInput.GetAxis(MultiInput.Key.Vertical, m_joypadNumber);
        if (horizontal != .0f || vertical != .0f)
        {
            Vector3 direction = new Vector3(horizontal, .0f, vertical);
            this.transform.forward = direction;
            m_animator.SetBool("isWalk", true);
            m_playerState = PlayerState.Walk;

        }
    }

    /// <summary>
    /// 走りアニメーション
    /// </summary>
    private void _Run()
    {
        // 何も押されていなかったらやめるため
        m_playerState = PlayerState.Default;
        m_animator.SetBool("isRun", false);

        //if (Input.GetKey(KeyCode.Space))
        if(MultiInput.GetButton("Dash", m_joypadNumber))
        {
            float horizontal = MultiInput.GetAxis(MultiInput.Key.Horizontal, m_joypadNumber);
            float vertical = MultiInput.GetAxis(MultiInput.Key.Vertical, m_joypadNumber);
            if (horizontal != .0f || vertical != .0f)
            {
                Vector3 direction = new Vector3(horizontal, .0f, vertical);
                this.transform.forward = direction;
                m_playerState = PlayerState.Run;
                m_animator.SetBool("isRun", true);
            }
        }
    }


    /// <summary>
    /// 掴みアニメーション
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    private void _Catch()
    {
        /*
        if (Input.GetKey(KeyCode.A))
        {
            m_animator.SetBool("isCatch", true);
            playerState = PlayerState.Catch;
        }
        */

        if (MultiInput.GetButton("Throw", m_joypadNumber))
        {
            m_animator.SetBool("isCatch", true);
            m_playerState = PlayerState.Catch;
        }

    }

    /// <summary>
    /// 投げアニメーション
    /// </summary>
    /// <param name="playerNo">プレイヤー番号</param>
    private void _Aim()
    {
        // 離したか確認できるようにするため
        m_animator.SetBool("isAim", false);

        //if (Input.GetKey(KeyCode.S))
        if(MultiInput.GetButton("Attack", m_joypadNumber))
        {
            m_animator.SetBool("isAim", true);
            m_playerState = PlayerState.Aim;
        }



    }


    public bool getReadyFlag()
    {
        return m_readyFlag;
    }
}

