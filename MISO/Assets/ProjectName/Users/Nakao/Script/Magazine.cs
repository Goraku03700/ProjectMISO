using UnityEngine;
using System.Collections;

public class Magazine : MonoBehaviour {
    [SerializeField]
    float m_turnOverTime;
    float m_time;

    [SerializeField]
    bool m_turnOver;

    [SerializeField]
    bool m_turnBack;
     
    [SerializeField]
    int m_pageNumber;

    [SerializeField]
    Transform[] m_pageCenters;

    [SerializeField]
    Vector3[] m_rotationAngle;

    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (m_turnOver)
        {
            m_time += Time.deltaTime;
            m_pageCenters[m_pageNumber].eulerAngles = Vector3.Lerp(m_rotationAngle[0], m_rotationAngle[1], m_time / m_turnOverTime);
            if (m_time >= m_turnOverTime)
            {
                m_time = 0.0f;
                m_turnOver = false;
                m_pageNumber++;
            }
        }
        if(m_turnBack)
        {
            m_time += Time.deltaTime;
            m_pageCenters[0].eulerAngles = Vector3.Lerp(m_rotationAngle[1], m_rotationAngle[0], m_time / m_turnOverTime);
            m_pageCenters[1].eulerAngles = Vector3.Lerp(m_rotationAngle[1], m_rotationAngle[0], m_time / m_turnOverTime);
            if (m_time >= m_turnOverTime)
            {
                m_time = 0.0f;
                m_turnBack = false;
            }
        }
	}

    public void TurnOver()
    {
        m_turnOver = true;
    }

    public void TurnBack()
    {
        m_turnBack = true;
    }
}
