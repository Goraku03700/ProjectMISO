using UnityEngine;
using System.Collections;

public class PullArrow : MonoBehaviour {

    [SerializeField]
    private float m_scaleAdd;   // どれだけ大きくするか(倍率)

    [SerializeField]
    private float m_scaleSpeed; // 大きくするスピード

    [SerializeField]
    private Sprite m_ArrowLeft;

    [SerializeField]
    private Sprite m_ArrowRight;
   
    // 矢印の種類
    enum PullArrowKind
    {
        Left,
        Right,
    };
    PullArrowKind m_pullArrowKind;

    enum PullArrowState
    {
        Big,
        Small,
    }
    PullArrowState m_pullArrowState;

    private float m_maxScale;
    private float m_minScale;

    private Vector3 m_saveScale;

	// Use this for initialization
	void Start () {

        m_pullArrowKind = PullArrowKind.Left;

        m_saveScale = this.transform.localScale;

        // 最大と最小の大きさを決めておく
        m_maxScale = m_saveScale.x * m_scaleAdd;
        m_minScale = m_saveScale.x * (1 - (m_scaleAdd - 1));

        
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 objScale = this.transform.localScale;


        // 最大超えたら
        if (m_maxScale <= objScale.y && m_pullArrowState == PullArrowState.Big)
        {
            m_pullArrowState = PullArrowState.Small;

        }
        else if (m_minScale >= objScale.y && m_pullArrowState == PullArrowState.Small)
        {
            m_pullArrowState = PullArrowState.Big;
        }


        switch (m_pullArrowState)
        {
            // 拡大中
            case PullArrowState.Big:
                {
                    objScale.x += m_scaleSpeed * Time.deltaTime;
                    objScale.y += m_scaleSpeed * Time.deltaTime;
                }
                break;
            
            // 縮小中
            case PullArrowState.Small:
                {
                    objScale.x -= m_scaleSpeed * Time.deltaTime;
                    objScale.y -= m_scaleSpeed * Time.deltaTime;
                }
                break;
        }




        this.transform.localScale = objScale;



    }

    /// <summary>
    /// 左向きの矢印にする
    /// </summary>
    public void ChangeArrowLeft()
    { 

        // 向きの反転
        if(m_pullArrowKind != PullArrowKind.Left)
        {
            // 大きさ戻す
            this.transform.localScale = m_saveScale;

            m_pullArrowKind = PullArrowKind.Left;

            this.GetComponent<SpriteRenderer>().sprite = m_ArrowLeft;

        }
    }


    /// <summary>
    /// 右向きの矢印にする
    /// </summary>
    public void ChangeArrowRight()
    {
        // 向きの反転
        if (m_pullArrowKind != PullArrowKind.Right)
        {
            // 大きさ戻す
            this.transform.localScale = m_saveScale;

            m_pullArrowKind = PullArrowKind.Right;

            this.GetComponent<SpriteRenderer>().sprite = m_ArrowRight;

        }

    }
}
