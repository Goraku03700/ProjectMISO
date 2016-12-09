using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameEndEffect : MonoBehaviour {

    [SerializeField]
    Image[] m_sprites;
    [SerializeField]
    Image[] m_textSprites;

    [SerializeField]
    Image m_countdownImage;

    [SerializeField]
    Sprite[] m_countdownSprites;
    enum State
    {
        CountDown,
        Ribbon,
        End,
        None
    }

    State m_state;

    float m_countDownTime;

    public float m_CountDownTime
    {
        get { return m_countDownTime; }
        set { m_countDownTime = value; }
    }

    [SerializeField]
    float m_lerpTime;

    [SerializeField]
    float m_startTime;

    [SerializeField]
    float m_endTime;
    [SerializeField]
    float m_time;

	// Use this for initialization
    void Start()
    {
        m_state = State.None;
    }

	
	// Update is called once per frame
	void Update () {
        m_time += Time.deltaTime;

        switch(m_state)
        {
            case State.CountDown:
                {
                    if(m_countdownImage.enabled == false)
                    {
                        m_countdownImage.enabled = true;
                    }
                    m_countdownImage.sprite = m_countdownSprites[(int)(m_countDownTime-m_time)];
                    m_countdownImage.color = new Color(1f,0,0,(1f - (m_time - Mathf.FloorToInt(m_time))));
                    if(m_countDownTime <m_time)
                    {
                        m_countdownImage.enabled = false;
                        m_state = State.Ribbon;
                        m_time = 0.0f;
                        BGMManager.instance.PlaySE("se032_GameOverRibbon");
                    }
                    break;
                }
            case State.Ribbon:
                {
                    if (m_startTime < m_time)
                    {
                        for (int i = 0; i < m_sprites.Length; ++i)
                        {
                            m_sprites[i].fillAmount = (m_time - m_startTime) / (m_lerpTime);
                            m_textSprites[i].fillAmount = (m_time - m_startTime) / (m_lerpTime);
                        }
                    }
                    if (m_endTime < m_time)
                    {
                        m_state = State.End;
                    }
                    break;
                }
            case State.End:
                {
                    Fade.ChangeScene("Result");
                    m_state = State.None;
                    break;
                }
            default:
                m_countdownImage.enabled = false;
                break;
        }

	}

    public bool StartEndEffect()
    {
        if (m_state == State.None)
        {
            m_time = 0f;
            m_state = State.CountDown;
            return true;
        }
        else
        {
            return false;
        }
    }
}
