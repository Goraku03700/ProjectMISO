using UnityEngine;
using System.Collections;

public class TitleGirl : MonoBehaviour {

    const float m_limit_x = 25.0f;  //女性が引きずる限界
    private GameObject m_girl; //タイトルに登場する女性
    private Vector3 m_startpos; //初期位置の確保
    private Vector3 m_nowpos;      //女性の位置を確保する

    private bool m_finishflg;
    
	// Use this for initialization
	void Start () {
        m_girl = GameObject.Find("Girl"); //オブジェクトのロード
        m_startpos = this.transform.position;
        m_nowpos = m_startpos; 
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    
    /// <summary>
    /// 女性がリボンを持ってロゴを引きずる
    /// </summary>
    public void TrailGirl()
    {
        //今現在は右に動くだけ。
        if (m_nowpos.x < m_limit_x)
        {
            m_nowpos.x += 20.0f * Time.deltaTime;
            this.transform.position = m_nowpos;
        }
        else
        {
            m_finishflg = true;
            m_nowpos.x = m_limit_x;
            this.transform.position = m_nowpos;
        }

    }

    /// <summary>
    /// 女性が移動終了した合図（フラグ）を送る
    /// </summary>
    public bool TrailGirlFinishFlag
    {
        get { return m_finishflg; }
    }

}
