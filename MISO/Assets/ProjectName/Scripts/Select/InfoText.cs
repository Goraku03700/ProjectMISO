using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InfoText : MonoBehaviour {

    // テキストの状態
    enum TextState
    {
        Normal,
        Flash,
    }
    TextState textState;

    private Text infoText;

    private float m_addAlpha;
    private float m_flashApha;
    private int m_time;


	// Use this for initialization
	void Start () {

        infoText = GameObject.Find("InfoText").GetComponent<Text>();

        m_addAlpha = 0.05f;
        m_flashApha = 1.0f;
        m_time = 0;

        textState = TextState.Normal;


    }

    // Update is called once per frame
    void Update()
    {
        switch (textState)
        {
            case TextState.Normal:
                if (infoText.color.a > 1.0f || infoText.color.a < 0.0f)
                {
                    m_addAlpha *= -1;
                }

                infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, infoText.color.a + m_addAlpha);
                break;

            case TextState.Flash:
                m_time++;

                // α値切り替え
                if(m_time > 3)
                {
                    if(m_flashApha >= 1.0f)
                    {
                        m_flashApha = 0.0f;
                    }else
                    {
                        m_flashApha = 1.0f;
                    }

                    m_time = 0;
                }

                infoText.color = new Color(infoText.color.r, infoText.color.g, infoText.color.b, m_flashApha);
                break;
        }
   
    }

    /// <summary>
    /// 文字を点滅させる
    /// </summary>
    public void FlashText()
    {
        textState = TextState.Flash;
    }

}
