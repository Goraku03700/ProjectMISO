using UnityEngine;
using System.Collections;

public class DashGauge : MonoBehaviour {

    [SerializeField]
    private Color m_maxColor;       // 最大時の色

    [SerializeField]
    private Color m_normalColor;    // 通常時の色

    [SerializeField]
    private Color m_littleColor;    // 少ない時の色

    [SerializeField]
    private float m_littleColorRaito;   // 色を変える割合


    private SpriteRenderer m_gaugeRenderer;

    private SpriteRenderer m_frameRenderer;

    private SpriteRenderer m_backRenderer;

    private float m_maxScaleX;  // スケールの最大

    private float m_raito;  // ゲージの割合

    // ゲージの割合のプロパティ
    public float raito
    {
        get
        {
            return m_raito;
        }

        set
        {
            m_raito = value;
        }
    }

    public SpriteRenderer gaugeRenderer
    {
        get
        {
            return m_gaugeRenderer;
        }

        set
        {
            m_gaugeRenderer = value;
            //m_frameRenderer = value;
            //m_backRenderer = value;
        }
    }

    public SpriteRenderer frameRenderer
    {
        get
        {
            return m_frameRenderer;
        }

        set
        {
            m_frameRenderer = value;
        }
    }

    public SpriteRenderer backRenderer
    {
        get
        {
            return m_backRenderer;
        }

        set
        {
            m_backRenderer = value;
        }
    }



    // Use this for initialization
    void Start () {

        // スケールの最大を保存しておく
        m_maxScaleX = this.transform.localScale.x;

        m_gaugeRenderer = this.GetComponent<SpriteRenderer>();
        m_frameRenderer = this.transform.parent.FindChild("DashGaugeFlame").gameObject.GetComponent<SpriteRenderer>();
        m_backRenderer  = this.transform.parent.FindChild("DashGaugeBack").gameObject.GetComponent<SpriteRenderer>();

        m_raito = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {

        /* テスト
        if (Input.GetKey(KeyCode.A))
        {
            m_raito += 0.01f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_raito -= 0.01f;
        }
        */

        // 最大と最小
        if(m_raito > 1.0f)
        {
            m_raito = 1.0f;
        }
        if(m_raito < 0.0f)
        {
            m_raito = 0.0f;
        }



        // ゲージの大きさを決める
        this.transform.localScale = new Vector3(m_maxScaleX * m_raito,
                                                this.transform.localScale.y,
                                                this.transform.localScale.z);

        // ゲージの割合によって色を変える
        if (m_raito <= m_littleColorRaito)
        {
            m_gaugeRenderer.color = m_littleColor;
        }
        else if (m_raito >= 1.0f)
        {
            m_gaugeRenderer.color = m_maxColor;
        }
        else
        {
            m_gaugeRenderer.color = m_normalColor;
        }
    }

}
