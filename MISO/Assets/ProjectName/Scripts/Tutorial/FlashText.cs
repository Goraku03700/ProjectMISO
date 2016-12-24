using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashText : MonoBehaviour {

    [SerializeField]
    private float m_speed;


    // テキストの状態
    enum TextState
    {
        Normal,
        Decide,
    }
    TextState m_textState;

    private Text m_text;

    private float m_flashApha;
    private float m_time;


    // Use this for initialization
    void Start()
    {
        m_text = this.GetComponent<Text>();
        
        if(m_speed == 0.0f) m_speed = 1.0f;

        m_time = 0.0f;

        m_textState = TextState.Normal;


    }

    // Update is called once per frame
    void Update()
    {
        Color color = m_text.color;

        switch (m_textState)
        {
            case TextState.Normal:
                {
                    float length = Time.time * m_speed;
                    float col = Mathf.PingPong(length, 1.5f);
                    color.a = col;
                }
                break;

            case TextState.Decide:
                {
                    m_time += Time.deltaTime;

                    // α値切り替え
                    if (m_time > 0.1f)
                    {
                        if (color.a >= 1.0f)
                        {
                            color.a = 0.0f;
                        }
                        else
                        {
                            color.a = 1.0f;
                        }

                        m_time = 0.0f;
                    }
                }

                break;
        }

        m_text.color = color;

    }

    /// <summary>
    /// 文字を点滅させる
    /// </summary>
    public void DecideText()
    {
        m_textState = TextState.Decide;
       // m_time = 0.0f;
    }

}
