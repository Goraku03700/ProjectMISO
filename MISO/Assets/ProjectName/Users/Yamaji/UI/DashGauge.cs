using UnityEngine;
using System.Collections;

public class DashGauge : MonoBehaviour {

    [SerializeField]
    private Camera m_targetCamera;

    [SerializeField]
    private Color m_maxColor;

    [SerializeField]
    private Color m_normalColor;

    [SerializeField]
    private Color m_littleColor;

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
        //カメラの方向を向くようにする。
        //this.transform.LookAt(this.m_targetCamera.transform.position);


    

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + 1.0f * Time.deltaTime,
                                                    this.transform.localScale.y,
                                                    this.transform.localScale.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + (-1.0f) * Time.deltaTime,
                                                    this.transform.localScale.y,
                                                    this.transform.localScale.z);
        }


        // 現在のスケールの割合を計算
        float nowScaleXRaito = m_maxScaleX / this.transform.localScale.x;
       
        if(nowScaleXRaito >= 1.0f)
        {
            m_gaugeRenderer.color = m_maxColor;
        }



    }
}
