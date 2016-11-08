using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TextGradation : MonoBehaviour {

    [SerializeField]
    Color32[] m_topColors;

    [SerializeField]
    Color32[] m_bottomColors;

    [SerializeField]
    List<TextGR> m_Texts;


    [SerializeField]
    float m_time;

    [SerializeField]
    int m_iterator;

	// Use this for initialization
	void Start () {
        int i = 0 ;
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<TextGR>() != null)
            {
                m_Texts.Add(child.GetComponent<TextGR>());
                if (i < m_topColors.Length - 1)
                {
                    //m_setTopColors.Add(m_colors[i]);
                    //m_setBottomColors.Add(m_colors[i+1]);
                    //m_setTopColors.Add(child.GetComponent<TextGR>().TopColor);
                    //m_setBottomColors.Add(child.GetComponent<TextGR>().BottomColor);
                    //m_setTopColors[i] = m_colors[i];
                    //m_setBottomColors[i] = m_colors[i + 1];
                }
                else
                {
                    //m_setTopColors.Add(m_colors[i]);
                    //m_setBottomColors.Add(m_colors[0]);
                    //m_setTopColors.Add(child.GetComponent<TextGR>().TopColor);
                    //m_setBottomColors.Add(child.GetComponent<TextGR>().BottomColor);
                    //m_setTopColors[i] = m_colors[i];
                    //m_setBottomColors[i] = m_colors[0];
                }
                ++i;
            }
        }
        m_time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_time += Time.deltaTime;
        if(m_time > 1.0f)
        {
            m_time -=1.0f;
            m_iterator++;
            if(m_iterator >= m_topColors.Length)
            {
                m_iterator = 0;
            }
            
        }
        for (int i = 0; i < m_topColors.Length; ++i)
        {
            if(i + m_iterator < m_topColors.Length)
            {
                m_Texts[i].TopColor = m_topColors[i];
                m_Texts[i].BottomColor = m_bottomColors[i];
            }
            else
            {
                m_Texts[i].TopColor = m_topColors[m_iterator];
                m_Texts[i].BottomColor = m_bottomColors[m_iterator];
            }
            m_Texts[i].enabled = false;
            m_Texts[i].enabled = true;
         }

        
        
	}
}
