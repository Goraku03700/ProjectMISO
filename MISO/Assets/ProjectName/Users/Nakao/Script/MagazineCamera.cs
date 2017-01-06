using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XInputDotNetPure;

public class MagazineCamera : MonoBehaviour {
    Transform m_cameraTransform;

    enum State
    {
        standby,
        start,
        titlepage,
        leftzoomin,
        leftmoveright,
        rightzoomout,
        close,
        fade
    }
    State m_state;

    [SerializeField]
    int m_pageCount;    //めくる回数

    [SerializeField]
    Vector3 m_startPosition;
    [SerializeField]
    Vector3 m_endPosition;

    [SerializeField]
    float m_startQuaternion_X;
    [SerializeField]
    float m_endQuaternion_X;

    [SerializeField]
    Vector3 m_leftZoomPosition;
    [SerializeField]
    Vector3 m_rightZoomPosition;

    [SerializeField]
    float m_startZoomTime;
    [SerializeField]
    float m_startZoomTimeLimit;

    [SerializeField]
    float m_titlePageTime;
    [SerializeField]
    float m_titlePageTimeLimit;

    [SerializeField]
    float m_leftZoomInTime;
    [SerializeField]
    float m_leftZoomInTimeLimit;

    [SerializeField]
    float m_leftMoveRightTime;
    [SerializeField]
    float m_leftMoveRightLimit;

    [SerializeField]
    float m_rightZoomOutTime;
    [SerializeField]
    float m_rightZoomOutTimeLimit;

    [SerializeField]
    float m_closeTime;
    [SerializeField]
    float m_closeTimeLimit;

    [SerializeField]
    float[] m_pageWaitTimeLimits;

    [SerializeField]
    float[] m_fieldViews;


    float m_waitTime;

    [SerializeField]
    Magazine m_magazine;

    [SerializeField]
    OpeningText m_openingText;

    [SerializeField]
    Text m_skipText;

    [SerializeField]
    Color[] m_skipColors;

    bool m_next;

    GamePadState[] m_padState;

	// Use this for initialization
	void Start () {
        m_cameraTransform = this.transform;
        m_state = State.standby;
        m_cameraTransform.eulerAngles = new Vector3(m_startQuaternion_X,0,0);
        m_cameraTransform.position = m_startPosition;
        m_padState = new GamePadState[4];
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 4; ++i )
        { 
            m_padState[i] = GamePad.GetState(PlayerIndex.One+i);
            if(m_padState[i].Buttons.Start == ButtonState.Pressed)
            {
                Fade.ChangeScene("Select");
            }

        }
        m_skipText.color = Color.Lerp(m_skipColors[0], m_skipColors[1], Mathf.PingPong(Time.time, 1));
        
        switch(m_state)
        {
            case State.standby:
                {
                    if(Fade.instance != null)
                    {
                        if(Fade.instance.FadeEnd() )
                        {
                            m_state = State.start;
                        }
                    }
                    else
                    {
                        m_state = State.start;
                    }
                    break;
                }
            case State.start:
                {
                    m_startZoomTime += Time.deltaTime;
                    m_cameraTransform.eulerAngles =  new Vector3(Mathf.Lerp(m_startQuaternion_X, m_endQuaternion_X, m_startZoomTime / m_startZoomTimeLimit),0,0);
                    m_cameraTransform.position = Vector3.Lerp(m_startPosition, m_endPosition, m_startZoomTime/m_startZoomTimeLimit);
                    if (m_startZoomTime >= m_startZoomTimeLimit)
                    {
                        m_state = State.titlepage;
                        m_magazine.TurnOver();
                    }
                    break;
                }
            case State.titlepage:
                {
                    m_titlePageTime += Time.deltaTime;
                    if(m_titlePageTime >= m_titlePageTimeLimit)
                    {
                        
                        m_state = State.leftzoomin;
                    }
                    break;
                }
            case State.leftzoomin:
                {
                    m_leftZoomInTime += Time.deltaTime;
                    m_cameraTransform.position = Vector3.Lerp(m_endPosition, m_leftZoomPosition, m_leftZoomInTime / m_leftZoomInTimeLimit);
                    this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(m_fieldViews[0], m_fieldViews[1], m_leftZoomInTime / m_leftZoomInTimeLimit);
                    if (m_leftZoomInTime >= m_leftZoomInTimeLimit)
                    {
                        m_leftMoveRightTime = 0.0f;
                        m_openingText.DrawText();
                        if(m_pageCount > 0)
                        {
                            m_waitTime += Time.deltaTime;
                            if(m_waitTime >= m_pageWaitTimeLimits[0])
                            {
                                m_waitTime = 0.0f;
                                m_state = State.leftmoveright;
                                m_openingText.NextText();
                            }
                        }
                        else
                        {
                            m_waitTime += Time.deltaTime;
                            if (m_waitTime >= m_pageWaitTimeLimits[2])
                            {
                                m_waitTime = 0.0f;
                                m_state = State.leftmoveright;
                                m_openingText.NextText();
                            }
                        }
                        //m_state = State.leftmoveright;
                    }
                    break;
                }
            case State.leftmoveright:
                {
                    m_leftMoveRightTime += Time.deltaTime;
                    m_cameraTransform.position = Vector3.Lerp(m_leftZoomPosition, m_rightZoomPosition, m_leftMoveRightTime / m_leftMoveRightLimit);
                    if (m_leftMoveRightTime >= m_leftMoveRightLimit)
                    {
                        m_state = State.rightzoomout;
                    }
                    break;
                }
            case State.rightzoomout:
                {
                    if (m_pageCount > 0)
                    {
                        
                        m_waitTime += Time.deltaTime;
                        if (m_waitTime >= m_pageWaitTimeLimits[1])
                        {
                            if(!m_next)
                            {
                                m_openingText.NextText();
                                m_next = true;
                            }
                            m_rightZoomOutTime += Time.deltaTime;
                            m_cameraTransform.position = Vector3.Lerp(m_rightZoomPosition, m_endPosition, m_rightZoomOutTime / m_rightZoomOutTimeLimit);
                            this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(m_fieldViews[1], m_fieldViews[0], m_rightZoomOutTime / m_rightZoomOutTimeLimit);
                            if (m_rightZoomOutTime >= m_rightZoomOutTimeLimit)
                            {
                                //m_openingText.NextText();
                                m_state = State.titlepage;
                                m_leftZoomInTime = 0.0f;
                                m_titlePageTime = 0.0f;
                                m_waitTime = 0.0f;
                                m_magazine.TurnOver();
                                m_pageCount--;
                            }
                            /*
                            m_pageCount--;
                            m_state = State.titlepage;
                            m_leftZoomInTime = 0.0f;
                            m_titlePageTime = 0.0f;
                            m_waitTime = 0.0f;
                            m_magazine.TurnOver();
                             * */
                        }
                        else
                        {
                            m_openingText.DrawText();
                        }
                    }
                    else
                    {
                        m_openingText.DrawText();
                        m_waitTime += Time.deltaTime;
                        if (m_waitTime >= m_pageWaitTimeLimits[3])
                        {
                            m_openingText.NextText();
                            m_state = State.close;
                            m_magazine.TurnBack();
                        }

                    }
                    /*
                    m_rightZoomOutTime += Time.deltaTime;
                    m_cameraTransform.position = Vector3.Lerp(m_rightZoomPosition, m_endPosition, m_rightZoomOutTime/m_rightZoomOutTimeLimit);
                    this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(m_fieldViews[1], m_fieldViews[0], m_rightZoomOutTime / m_rightZoomOutTimeLimit);
                    if (m_rightZoomOutTime >= m_rightZoomOutTimeLimit)
                    {
                        if (m_pageCount > 0)
                        {
                            m_waitTime += Time.deltaTime;
                            if (m_waitTime >= m_pageWaitTimeLimits[1])
                            {
                                m_pageCount--;
                                m_state = State.titlepage;
                                m_leftZoomInTime = 0.0f;
                                m_titlePageTime = 0.0f;
                                m_waitTime = 0.0f;
                                m_magazine.TurnOver();
                            }
                        }
                        else
                        {
                            m_waitTime += Time.deltaTime;
                            if (m_waitTime >= m_pageWaitTimeLimits[3])
                            {
                                m_state = State.close;
                                m_magazine.TurnBack();
                            }
                            
                        }*/
        
                    break;
                }
            case State.close:
                {
                    m_closeTime += Time.deltaTime;
                    m_cameraTransform.position = Vector3.Lerp(m_rightZoomPosition, m_endPosition, m_closeTime);
                    this.GetComponent<Camera>().fieldOfView = Mathf.Lerp(m_fieldViews[1], m_fieldViews[0], m_closeTime);
                    if (m_closeTime >= m_closeTimeLimit)
                    {
                        m_state = State.fade;
                    }
                    break;
                }
            case State.fade:
                {
                    Fade.ChangeScene("Select");
                    break;
                }
        }


	}

}
