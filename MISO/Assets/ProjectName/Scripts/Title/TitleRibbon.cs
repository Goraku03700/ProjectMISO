using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルのリボンのクラス
/// 
/// </summary>
public class TitleRibbon : MonoBehaviour {

    const float m_limit_x = 0.0f;   //タイトルロゴが引きずる限界
    private Vector3 m_limit = new Vector3(0.0f,1.0f,0.0f);
    private GameObject m_ribbon; //タイトルに登場するリボン
    private GameObject m_logo;   //タイトルロゴの位置保管用
    private Bezier m_bezier;
    

    private Vector3 m_startpos; //初期位置の確保
    private Vector3 m_nowpos;      //リボンの位置を確保する
    private Vector3 m_startscale; //初期拡縮の確保
    private Vector3 m_nowscale;      //リボンの拡縮を確保する
    private Quaternion m_startrotation; //初期回転の確保
    private Quaternion m_nowrotation;      //リボンの回転を確保する 
    private float m_time;

    private bool m_throwfinishflg;       //リボンの投げる挙動終了の合図
    private bool m_trailfinishflg;       //リボンの引きずる挙動終了の合図

	// Use this for initialization
	void Start () {
        m_ribbon = GameObject.Find("ribbon2"); //オブジェクトのロード
        m_logo = GameObject.Find("TitleLogo");     //オブジェクトのロード
        

        //挙動計算のため、初期値を格納する。
        m_startpos = this.transform.position;
        m_nowpos = m_startpos;
        m_startscale = this.transform.localScale;
        m_nowscale = m_startscale;
        m_startrotation = this.transform.rotation;
        m_nowrotation = m_startrotation;
        m_throwfinishflg = false;
        m_trailfinishflg = false;
        m_time = 0.0f;
        
        //m_bezier = new Bezier(new Vector3(5.5f, 0.0f, 0.0f), Random.insideUnitSphere * 2.0f,
        //                    Random.insideUnitSphere * 2.0f, new Vector3(-25.5f, 1.0f, 0.0f));
        m_bezier = new Bezier(new Vector3(5.5f, 0.0f, 0.0f), new Vector3(-15.5f, 30.5f, 0.0f),
                            new Vector3(-22.0f, -19.5f, 0.0f), new Vector3(-35.5f, 1.0f, 0.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// リボンの挙動。女性がリボンを投げる動作
    /// </summary>
    public void ThrowRibbon()
    {
        //ここではリボンの挙動で演出するが
        //α版なので、タイトルロゴ(3DText)とリング(輪)のベジェ曲線で処理。
        
        if(this.transform.position.x <= m_logo.transform.position.x)
        {
            m_throwfinishflg = true;
            this.transform.localScale = m_startscale;
            this.transform.position = m_logo.transform.position;
            m_nowpos = this.transform.position;
            
        }
        else
        {
            this.transform.position = m_bezier.GetPointAtTime(m_time);
            transform.Rotate(-60.0f * Time.deltaTime, 0, 0);
            this.transform.localScale += m_startscale * (0.5f * Time.deltaTime);
            m_time += 0.35f * Time.deltaTime;
        }
    }

    public void ResetBezierRibbon()
    {
        m_bezier.ResetBezier(new Vector3(-35.5f, 1.0f, 0.0f), new Vector3(-25.0f, -30.5f, 0.0f),
                            new Vector3(-18.5f, 19.5f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
    }

    /// <summary>
    /// タイトルロゴに引っかかったリボンを引っ張る（引きずる）。
    /// </summary>
    public void TrailRibbon()
    {
        if(this.transform.position.x < m_limit.x)
        {
            this.transform.position = m_bezier.GetPointAtTime(m_time);
            transform.Rotate(30.0f * Time.deltaTime, 0, 0);
            m_time += 0.35f * Time.deltaTime;
        }
        else
        {
            m_trailfinishflg = true;
            m_nowpos = m_limit;
            this.transform.position = m_nowpos;
        }

    }

    /// <summary>
    /// リボンを投げる終了のフラグを取得する
    /// </summary>
    public bool ThrowFinishFlag
    {
        get { return m_throwfinishflg; }
    }

    /// <summary>
    /// リボンがタイトルロゴを引っ張る終了のフラグを取得する。
    /// </summary>
    public bool TrailFinishFlag
    {
        get { return m_trailfinishflg; }
    }

}
