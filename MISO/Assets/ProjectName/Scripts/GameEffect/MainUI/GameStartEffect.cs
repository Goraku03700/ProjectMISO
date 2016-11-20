using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartEffect : MonoBehaviour {
    
    enum State
    {
        Slide,
        FadeReady,
        Go,
        FadeGo,
        None
    }

    State m_state;

    [SerializeField]
    Text[] m_texts;

    [SerializeField]
    Text m_goTexts;

    float m_time;
    [SerializeField]
    float m_slideEndTime;

    [SerializeField]
    float m_fadeReadyEndTime;

    [SerializeField]
    float m_fadeGoStartTime;

    [SerializeField]
    float m_fadeGoEndTime;

    [SerializeField]
    float m_textBetweenPerSecond;
    [SerializeField]
    float m_textBetween;

    [SerializeField]
    Color32[] lerpColors;


    [SerializeField]
    RectTransform[] m_startRectTransforms;
    [SerializeField]
    RectTransform[] m_endRectTransforms;


    [SerializeField]
    RectTransform m_thisRectTransform;

    [SerializeField]
    TimeCount m_timeCount;

    private Fade m_fadeObject;

	// Use this for initialization
	void Start () {
        m_goTexts.enabled = false;
        m_fadeObject = GameObject.FindObjectOfType<Fade>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_fadeObject.FadeEnd() || m_fadeObject == null)
        {
            m_time += Time.deltaTime;
        }
        switch (m_state)
        {
            case State.Slide:
                {

                    if (m_time > m_slideEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.FadeReady;
                        for (int i = 0; i < m_texts.Length; ++i)
                        {
                            m_texts[i].rectTransform.localPosition = Vector3.Lerp(m_startRectTransforms[i].localPosition, m_endRectTransforms[i].localPosition, 1.0f);
                        }
                    }
                    else
                    {
                        for(int i = 0 ; i < m_texts.Length ; ++i)
                        {
                             m_texts[i].rectTransform.localPosition = Vector3.Lerp(m_startRectTransforms[i].localPosition , m_endRectTransforms[i].localPosition,m_time / m_slideEndTime);
                        }
                    }
                    break;
                }
            case State.FadeReady:
                {

                    if (m_time > m_fadeReadyEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.Go;
                        m_goTexts.text = "Go!!";
                        for (int i = 0; i < m_texts.Length; ++i)
                        {
                            m_texts[i].color = lerpColors[1];
                        }
                    }
                    else
                    {
                        Vector3 thisPosition = m_thisRectTransform.localPosition;
                        m_texts[0].rectTransform.localPosition = new Vector3(thisPosition.x - m_time / m_fadeReadyEndTime * m_textBetweenPerSecond * 2 - m_textBetween * 2, 0);
                        m_texts[1].rectTransform.localPosition = new Vector3(thisPosition.x - m_time / m_fadeReadyEndTime * m_textBetweenPerSecond - m_textBetween, 0);
                        m_texts[2].rectTransform.localPosition = new Vector3(thisPosition.x, 0);
                        m_texts[3].rectTransform.localPosition = new Vector3(thisPosition.x + m_time / m_fadeReadyEndTime * m_textBetweenPerSecond + m_textBetween, 0);
                        m_texts[4].rectTransform.localPosition = new Vector3(thisPosition.x + m_time / m_fadeReadyEndTime * m_textBetweenPerSecond * 2 + m_textBetween * 2, 0);

                        for (int i = 0; i < m_texts.Length; ++i)
                        {
                            m_texts[i].color= Color32.Lerp(lerpColors[0], lerpColors[1], m_time/m_fadeReadyEndTime);
                        }
                    }
                    break;
                }
            case State.Go:
                {
                    m_goTexts.enabled = true;
                    m_state = State.FadeGo;
                    m_timeCount.StartTime();
                    break;
                }
            case State.FadeGo:
                {

                    if (m_time > m_fadeGoEndTime + m_fadeGoStartTime)
                    {
                        m_state = State.None;
                        m_goTexts.color = lerpColors[1];
                    }
                    else if(m_time < m_fadeGoStartTime)
                    {
                        m_goTexts.color = lerpColors[0];
                    }
                    else
                    {
                        m_goTexts.color = Color32.Lerp(lerpColors[0], lerpColors[1], m_time / m_fadeGoEndTime);
                     }
                    break;
                }
            case State.None:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
	}
}
