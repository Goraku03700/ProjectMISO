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
        Fade
    }

    State m_state;

    [SerializeField]
    Vector3 m_startSlidePosition;
    [SerializeField]
    Vector3 m_endSlidePosition;

    [SerializeField]
    Quaternion m_startRollRotation;
    [SerializeField]
    Quaternion m_endRollRotation;

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


	// Use this for initialization
	void Start () {
        m_state = State.Slide;
	}
	
	// Update is called once per frame
	void Update () {
        switch(m_state)
        {
            case State.Slide:
                {
                    m_time += Time.deltaTime;
                    if (m_time > m_slideEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.Roll;
                        this.gameObject.transform.position = Vector3.Lerp(m_startSlidePosition, m_endSlidePosition, 1.0f);

                    }
                    else
                    {
                        this.gameObject.transform.position = Vector3.Lerp(m_startSlidePosition, m_endSlidePosition, m_time / m_slideEndTime);
                    }
                    break;
                }
            case State.Roll:
                {
                    m_time += Time.deltaTime;
                    if (m_time > m_rollEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.Color;
                        this.gameObject.transform.rotation = Quaternion.Lerp(m_startRollRotation, m_endRollRotation, 1.0f);
                        m_textGradation.enabled = true;
                    }
                    else
                    {
                        this.gameObject.transform.rotation = Quaternion.Lerp(m_startRollRotation, m_endRollRotation, m_time / m_rollEndTime);
                    }
                    break;
                }
            case State.Color:
                {
                    m_time += Time.deltaTime;
                    if (m_time > m_colorEndTime)
                    {
                        m_time = 0.0f;
                        m_state = State.Fade;
                    }
                    break;
                }
            case State.Fade:
                {
                    m_time += Time.deltaTime;
                    if (m_time > m_fadeEndTime)
                    {
                        m_time = m_fadeEndTime;
                        this.gameObject.transform.position = Vector3.Lerp(m_startFadePosition, m_endFadePosition, 1.0f);
                    }
                    else
                    {
                        this.gameObject.transform.position = Vector3.Lerp(m_startFadePosition, m_endFadePosition, m_time / m_fadeEndTime);
                        Vector3 thisPosition = this.gameObject.transform.position;
                        m_texts[0].transform.position = new Vector3(thisPosition.x - m_time * m_textBetweenPerSecond  * 2, thisPosition.y);
                        m_texts[1].transform.position = new Vector3(thisPosition.x - m_time * m_textBetweenPerSecond, thisPosition.y);
                        m_texts[2].transform.position = new Vector3(thisPosition.x , thisPosition.y);
                        m_texts[3].transform.position = new Vector3(thisPosition.x + m_time * m_textBetweenPerSecond * 1, thisPosition.y);
                        m_texts[4].transform.position = new Vector3(thisPosition.x + m_time * m_textBetweenPerSecond * 2, thisPosition.y);
                    }
                    break;
                }
        }
	    

	}
}
