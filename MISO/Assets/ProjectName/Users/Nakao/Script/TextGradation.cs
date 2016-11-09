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
    List<TextGradationEffect> m_Texts;

    [SerializeField]
    float m_limit;
    float m_time;

    [SerializeField]
    int m_iterator;

	// Use this for initialization
	void Start () {
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<TextGradationEffect>() != null)
            {
                m_Texts.Add(child.GetComponent<TextGradationEffect>());
            }
        }
        m_time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_time += Time.deltaTime;
        if(m_time > m_limit)
        {
            m_time -= m_limit;
            m_iterator++;
            if(m_iterator >= m_topColors.Length)
            {
                m_iterator = 0;
            }
            
        }
        for (int i = 0; i < m_topColors.Length; ++i)
        {
            if (i + m_iterator + 1 < m_topColors.Length)
            {
                m_Texts[i].TopColor = Color32.Lerp(m_topColors[i + m_iterator], m_topColors[i + m_iterator + 1], m_time);
                m_Texts[i].BottomColor = Color32.Lerp(m_bottomColors[i + m_iterator], m_bottomColors[i + m_iterator + 1], m_time);
            }
            else if (i + m_iterator < m_topColors.Length)
            {
                m_Texts[i].TopColor = Color32.Lerp(m_topColors[m_iterator + i], m_topColors[0], m_time);
                m_Texts[i].BottomColor = Color32.Lerp(m_bottomColors[m_iterator + i], m_bottomColors[0], m_time);
            }
            else
            {
                m_Texts[i].TopColor = Color32.Lerp(m_topColors[i + m_iterator - m_topColors.Length], m_topColors[i + m_iterator - m_topColors.Length + 1], m_time);
                m_Texts[i].BottomColor = Color32.Lerp(m_bottomColors[i + m_iterator - m_topColors.Length], m_bottomColors[i + m_iterator - m_topColors.Length + 1], m_time);
            }
            m_Texts[i].enabled = false;
            m_Texts[i].enabled = true;
        }
    }
}
