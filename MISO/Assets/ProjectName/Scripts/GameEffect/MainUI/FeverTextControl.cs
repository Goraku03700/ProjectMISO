using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeverTextControl : MonoBehaviour {

    [SerializeField]
    TextGradation m_textGradation;

    enum State
    {
        Slide,
        Roll,
        Color,
        Fade,
        None
    }

    State m_state;

    [SerializeField]
    Vector3 m_startSlidePosition;
    [SerializeField]
    Vector3 m_endSlidePosition;

    [SerializeField]
    RectTransform m_startRollRotation;
    [SerializeField]
    RectTransform m_endRollRotation;

    [SerializeField]
    Vector3 m_startFadePosition;
    [SerializeField]
    Vector3 m_endFadePosition;

    [SerializeField]
    Text[] m_texts;

    float m_time;
    [SerializeField]
    float m_slideEndTime;

    [SerializeField]
    float m_rollEndTime;

    [SerializeField]
    float m_colorEndTime;

    [SerializeField]
    float m_fadeEndTime;

    [SerializeField]
    float m_textBetweenPerSecond;

    [SerializeField]
    RectTransform m_thisRectTransform;

	// Use this for initialization
	void Start () {
        m_state = State.None;
	}
	
	// Update is called once per frame
	void Update () {
        m_time += Time.deltaTime;
        switch(m_state)
        {
            case State.Slide:
                {
                    
                    if (m_time > m_slideEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.Roll;
                        m_thisRectTransform.localPosition = Vector3.Lerp(m_startSlidePosition, m_endSlidePosition, 1.0f);
                        BGMManager.instance.PlaySE("se031_FeverMode_2");
                    }
                    else
                    {
                        m_thisRectTransform.localPosition = Vector3.Lerp(m_startSlidePosition, m_endSlidePosition, m_time / m_slideEndTime);
                    }
                    break;
                }
            case State.Roll:
                {
                    
                    if (m_time > m_rollEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.Color;
                        m_thisRectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                        m_textGradation.enabled = true;
                    }
                    else
                    {
                        m_thisRectTransform.localRotation = Quaternion.Euler(0,0, - m_time / m_rollEndTime * 360.0f);
                    }
                    break;
                }
            case State.Color:
                {
                    
                    if (m_time > m_colorEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.Fade;
                    }
                    break;
                }
            case State.Fade:
                {
                    
                    if (m_time > m_fadeEndTime)
                    {
                        m_time = m_fadeEndTime;
                        m_thisRectTransform.localPosition = Vector3.Lerp(m_startFadePosition, m_endFadePosition, 1.0f);
                    }
                    else
                    {
                        m_thisRectTransform.localPosition = Vector3.Lerp(m_startFadePosition, m_endFadePosition, m_time / m_fadeEndTime);
                        Vector3 thisPosition = m_thisRectTransform.localPosition;
                        m_texts[0].rectTransform.localPosition = new Vector3(thisPosition.x - m_time * m_textBetweenPerSecond  * 2 - m_textBetweenPerSecond * 2, 0);
                        m_texts[1].rectTransform.localPosition = new Vector3(thisPosition.x - m_time * m_textBetweenPerSecond - m_textBetweenPerSecond, 0);
                        m_texts[2].rectTransform.localPosition = new Vector3(thisPosition.x, 0);
                        m_texts[3].rectTransform.localPosition = new Vector3(thisPosition.x + m_time * m_textBetweenPerSecond + m_textBetweenPerSecond, 0);
                        m_texts[4].rectTransform.localPosition = new Vector3(thisPosition.x + m_time * m_textBetweenPerSecond * 2 + m_textBetweenPerSecond * 2, 0);
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

    public void StartFever()
    {
        m_state = State.Slide;
        m_time = 0.0f;
    }
}
