using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpeningText : MonoBehaviour {

    Image m_window;
    Text m_text;

    [SerializeField]
    string[] m_strings;

    int m_textNumber;

    bool m_setNextText;

    float m_time;

    [SerializeField]
    float m_timeAlphaLimit;

    [SerializeField]
    Color[] m_colors;

	// Use this for initialization
	void Start () {
        m_window = this.GetComponent<Image>();
        m_text = this.transform.FindChild("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(m_setNextText)
        {
            m_time += Time.deltaTime;
            m_text.color = Color.Lerp(m_colors[0], m_colors[1], m_time / m_timeAlphaLimit);
            if(m_textNumber == 1 || m_textNumber == 3)
            {
                m_window.color = Color.Lerp(m_colors[2], m_colors[3], m_time / m_timeAlphaLimit);
            }
            if(m_time > m_timeAlphaLimit)
            {
                m_textNumber++;
                m_setNextText = false;
            }
        }
	}

    public void DrawText()
    {
        m_text.text = m_strings[m_textNumber];
        m_text.color = m_colors[0];
        m_window.color = m_colors[2];
    }

    public void NextText()
    {
        if(!m_setNextText)
        {
            m_time = 0.0f;
        }
        m_setNextText = true;
        
    }
}
