using UnityEngine;
using System.Collections;

public class Magazine : MonoBehaviour {
    [SerializeField]
    float m_rightTime;
    [SerializeField]
    float m_leftTime;
    [SerializeField]
    float m_turnOverTime;

    [SerializeField]
    bool m_turnOverHalf;

    [SerializeField]
    float m_pinchInTime;  

    [SerializeField]
    float m_scrollTime;     

    [SerializeField]
    int m_pageNumber;

    [SerializeField]
    Texture[] m_pageTextures;

    [SerializeField]
    Material[] m_pageMaterials;
	// Use this for initialization
	void Start () {
        m_pageMaterials[0].SetFloat("_Flip", 1);
        m_pageMaterials[1].SetFloat("_Flip", 1);
        m_rightTime -= 0.5f;
        m_pageMaterials[0].SetTexture("_MainTex", m_pageTextures[0]);
        m_pageMaterials[1].SetTexture("_MainTex", m_pageTextures[1]);
        m_pageMaterials[2].SetTexture("_MainTex", m_pageTextures[2]);
        m_pageMaterials[3].SetTexture("_MainTex", m_pageTextures[3]);
	}
	
	// Update is called once per frame
	void Update () {
        m_rightTime += Time.deltaTime;
      
        m_pageMaterials[m_pageNumber+1].SetFloat("_Flip", 1 - m_rightTime / m_turnOverTime);
        if(m_rightTime > m_turnOverTime)
        {
            m_turnOverHalf = true;
        }
        if(m_turnOverHalf)
        {
            m_leftTime += Time.deltaTime;
            m_pageMaterials[m_pageNumber].SetFloat("_Flip", 1 - m_leftTime / m_turnOverTime);
        }
        
        //m_pageMaterials[m_pageNumber+1].SetFloat("_Flip", m_time / m_turnOverTime/2);
        /*
	    if(m_time > m_pinchInTime)
        {
            if(m_pageNumber+2 < m_pageMaterials.Length)
            {
                m_pageNumber+=2;
                m_pageMaterials[0].SetTexture("_MainTex", m_pageTextures[m_pageNumber]);
                m_pageMaterials[1].SetTexture("_MainTex", m_pageTextures[m_pageNumber+1]);
                m_pageMaterials[2].SetTexture("_MainTex", m_pageTextures[m_pageNumber+2]);
                m_pageMaterials[3].SetTexture("_MainTex", m_pageTextures[m_pageNumber+3]);
            }
        }
         * */
	}
}
