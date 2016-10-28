using UnityEngine;
using System.Collections;

public class TitleLogo : MonoBehaviour {

    const float m_limit_x = 0.0f;

    private GameObject m_titlelogo; //タイトルに登場するロゴ
    private Vector3 m_startpos;     //タイトルロゴの最初に位置保管用
    private Vector3 m_nowpos;       //タイトルロゴの現在の位置
    private MeshRenderer m_meshrender;  //ロゴ表示用
    private Bezier mybezier;

    private bool m_finishflg;

	// Use this for initialization
	void Start () {
        m_titlelogo = GameObject.Find("TitleLogo");     //オブジェクトのロード
        m_startpos = this.transform.position;
        m_nowpos = m_startpos;
        m_meshrender = GetComponent<MeshRenderer>();
        m_meshrender.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// タイトルロゴを引きずる
    /// </summary>
    public void TrailTitleLogo()
    {
        if (m_nowpos.x < m_limit_x)
        {
            m_nowpos.x += 4.0f * Time.deltaTime;
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
    /// タイトルロゴ
    /// </summary>
    public bool TrailtitleLogoFinishFlag
    {
        get { return m_finishflg; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active">true:表示する false:表示しない</param>
    public void Activate(bool active)
    {
        m_meshrender.enabled = active;
    }

}
