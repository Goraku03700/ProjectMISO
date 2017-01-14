using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleCaption : MonoBehaviour {

    [SerializeField]
    float m_alpha;

    [SerializeField]
    float m_speed;

    Text m_text;

	// Use this for initialization
	void Start () {
        m_text = GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {

        float alpha = 1.0f;

        //float sin = Mathf.Sin(Time.frameCount * m_speed);
        //alpha = sin * m_alpha;

        alpha = Mathf.PingPong(Time.time * m_speed, m_alpha);
            
        Color color = m_text.color;
        color.a = alpha;
        m_text.color = color;
    }
}
