using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeCount : MonoBehaviour {

    [SerializeField]
    private float timeInit;

    [SerializeField]
    private float m_countDownStartTime;

    [SerializeField]
    private float m_feverStartTime;

    [SerializeField]
    GameEndEffect m_endEffect;

    [SerializeField]
    FeverTextControl m_fever;

    [SerializeField]
    Image m_clock;

    [SerializeField]
    TimeNiddle m_timeNiddle;

    private float nowTime;

    private Text timeText;

    private bool isStart;

    private bool m_isCountDown;

    private bool m_isFever;

    public GirlCreateSystem m_girlCreateSystem;

	// Use this for initialization
	void Start () {
        nowTime = 0.0f;

        timeText = this.GetComponent<Text>();

        isStart = false;

        m_isCountDown = false;

        m_isFever = false;

        m_endEffect.m_CountDownTime = m_countDownStartTime;


	}

    void Awake()
    {
        m_girlCreateSystem.m_Limit = timeInit;
    }
	
	// Update is called once per frame
	void Update () {

        // 時間を測る
        if (isStart)
        {
            nowTime += Time.deltaTime;
        }
        // 残り時間を計算
        float dispTime = timeInit - nowTime;

        int second = (int)dispTime % 60; // 秒を計算
        int minute = (int)dispTime / 60; // 分を計算

        timeText.text = minute.ToString() + ":" + second.ToString().PadLeft(2, '0');

        if(dispTime <= m_feverStartTime + 1.0f && !m_isFever)
        { 
            m_isFever = true;
            m_fever.StartFever();
        }
                 
        if(dispTime <= m_countDownStartTime + 1  && !m_isCountDown)
        {
            m_isCountDown = true;
            m_endEffect.StartEndEffect();
            m_clock.enabled = false;
            m_timeNiddle.enabled = false;
        }
        else if(m_isCountDown)
        {
            timeText.text = "";
        }

        if(dispTime <= 0 && isStart)
        {
            isStart = false;
            SetData();
        }
	}

    /// <summary>
    /// カウントダウンを始める
    /// </summary>
    public void StartTime()
    {
        m_girlCreateSystem.m_Limit = timeInit;
        isStart = true;

        m_timeNiddle.StartRotate();
    }

    void SetData()
    {
        Player playerDataObject = GameObject.Find("Player1").GetComponent<Player>();
        SceneSharedData.instance.Set("PlayTest", "Player1Score", playerDataObject.score);
        playerDataObject = GameObject.Find("Player2").GetComponent<Player>();
        SceneSharedData.instance.Set("PlayTest", "Player2Score", playerDataObject.score);
        playerDataObject = GameObject.Find("Player3").GetComponent<Player>();
        SceneSharedData.instance.Set("PlayTest", "Player3Score", playerDataObject.score);
        playerDataObject = GameObject.Find("Player4").GetComponent<Player>();
        SceneSharedData.instance.Set("PlayTest", "Player4Score", playerDataObject.score);
    }
}

