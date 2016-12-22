using UnityEngine;
using System.Collections;

public class PullStick : MonoBehaviour {

    [SerializeField]
    private float m_slopeSpeed; // 傾けるスピード

    [SerializeField]
    private float m_slopeMax;   // 傾ける最大値

    [SerializeField]
    private float m_slopeTime;   // 傾ける時間

    [SerializeField]
    private float m_slopeWaitTime; // 0度の位置で止める時間


    private SpriteRenderer m_spriteRenderer;

    /*
    [SerializeField]
    private float m_scaleSpeed; // 拡縮のスピード

    [SerializeField]
    private float m_scaleMax;   // 拡縮の最大(倍率)
    */

    enum StickState
    {
        Left,
        Right
  
    }
    private StickState m_stickState;

    private float m_rot;

    private float m_time;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            return m_spriteRenderer;
        }

        set
        {
            m_spriteRenderer = value;
        }
    }

    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {

        m_rot = 0.0f;

        m_time = 0.0f;

        // nullチェック？
        if (m_slopeSpeed == 0.0f)
            m_slopeSpeed = 150.0f;

        if (m_slopeMax == 0.0f)
            m_slopeMax = 50.0f;

        if (m_slopeTime == 0.0f)
            m_slopeTime = 1.0f;

        if (m_slopeWaitTime == 0.0f)
            m_slopeWaitTime = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {

        m_time += Time.deltaTime;

        switch (m_stickState)
        {
            case StickState.Left:
                {
                    
                    m_rot -= m_slopeSpeed * Time.deltaTime;
                    if (m_rot < -m_slopeMax)
                        m_rot = -m_slopeMax;
                   
                }
                break;

            case StickState.Right:
                {
                    
                    m_rot += m_slopeSpeed * Time.deltaTime;
                    if (m_rot > m_slopeMax)
                        m_rot = m_slopeMax;

                }
                break;
        }

        // 0度の位置で止める
        if (m_time < m_slopeWaitTime)
        {
            m_rot = 0.0f;
        }

        if (m_time > m_slopeTime)
        {
            m_time = 0.0f;
            m_rot = 0.0f;
        }

        
        Vector3 angle;
        angle.x = this.transform.eulerAngles.x;
        angle.y = this.transform.eulerAngles.y;
        angle.z = m_rot;
        
        this.transform.eulerAngles = angle;


        // 大きさの変更
        /*
        Vector3 objScale = this.transform.localScale;

        float scale = Mathf.PingPong(Time.time * m_scaleSpeed, m_scaleMax - 1.0f) + 1.0f;
        objScale.x = objScale.y = scale;

        this.transform.localScale = objScale;
        */


    }

    public void ChangeStickLeft()
    {
        m_stickState = StickState.Left;

        m_time = 0.0f;
        m_rot = 0.0f;
    }

    public void ChangeStickRight()
    {
        m_stickState = StickState.Right;

        m_time = 0.0f;
        m_rot = 0.0f;
    }
}
