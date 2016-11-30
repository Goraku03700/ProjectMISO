using UnityEngine;
using System.Collections;

public class DashGauge : MonoBehaviour {

    [SerializeField]
    private Color m_maxColor;

    [SerializeField]
    private Color m_normalColor;

    [SerializeField]
    private Color m_littleColor;

    [SerializeField]
    private float m_littleColorRaito;

    private SpriteRenderer m_gaugeRenderer;

    private float m_maxScaleX;  // スケールの最大

   


	// Use this for initialization
	void Start () {

        // スケールの最大を保存しておく
        m_maxScaleX = this.transform.localScale.x;

        m_gaugeRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKey(KeyCode.A))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + 6.0f * Time.deltaTime,
                                                    this.transform.localScale.y,
                                                    this.transform.localScale.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + (-6.0f) * Time.deltaTime,
                                                    this.transform.localScale.y,
                                                    this.transform.localScale.z);
        }

        // 最大値と最小値の設定
        float nowScaleX = this.transform.localScale.x;
        
        // 最大
        if(nowScaleX > m_maxScaleX)
        {
            this.transform.localScale = new Vector3(m_maxScaleX,
                                                    this.transform.localScale.y,
                                                    this.transform.localScale.z);

        }

        // 最小
        if (nowScaleX < 0.0f)
        {
            this.transform.localScale = new Vector3(0.0f,
                                                    this.transform.localScale.y,
                                                    this.transform.localScale.z);
        }


        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();

        // 現在のスケールの割合を計算
        float nowScaleXRaito = this.transform.localScale.x / m_maxScaleX;
     
        // 割合によって色を変える
        if(nowScaleXRaito <= m_littleColorRaito)
        {
            m_gaugeRenderer.color = m_littleColor;
        }else if(nowScaleXRaito >= 1.0f)
        {
            m_gaugeRenderer.color = m_maxColor;
        }else
        {
            m_gaugeRenderer.color = m_normalColor;
        }
    }


    //public void UpGauge()
}
