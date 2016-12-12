/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashText : MonoBehaviour {

    [SerializeField]
    private float m_FlashSpeed; // 点滅速度

    // テキストの状態
    enum TextState{
        Normal,
        Decide,
    }
    TextState m_textState;

    enum 

    private Text m_text;        // 文字オブジェクト

    private float m_addAlpha;   // 加算値

    private float m_time;       // 経過時間
    
    // Use this for initialization
    void Start()
    {
        m_text = this.GetComponent<Text>();

        m_time = 0.0f;
   }

    // Update is called once per frame
    void Update()
    {

        m_time += Time.deltaTime;

        Color color = m_text.color;

        switch(m_textState)
        {
            case TextState.Normal:
                {
                    float addAlpha = m_time / m_FlashSpeed;
                    color.a += addAlpha;

                }
                break;
        }

        m_text.color = color;


    }
}
*/