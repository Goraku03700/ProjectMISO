using UnityEngine;
using System.Collections;

public class TitleLogo : MonoBehaviour {

    private const float Limit_x = 0.0f;   //ベジェの動きの限界値
    private const float ScaleLimit = 1.5f;
    private const int   TimeLimit = 10;  //UIを表示するまでの余韻
    private GameObject m_titlelogo; //タイトルに登場するロゴ
    private GameObject m_ribbon;
    private Vector3 m_startpos;     //タイトルロゴの最初に位置保管用
    private Vector3 m_nowpos;       //タイトルロゴの現在の位置
    private Vector3 m_startscale;   //タイトルロゴの最初の拡縮
    private Vector3 m_nowscale;     //タイトルロゴの現在の拡縮
    //タイトルロゴのバウンドに使用する
    private bool m_boundupflg;     //バウンド（上がる）フラグ
    private bool m_bounddownflg;   //バウンド（下がる）フラグ
    private bool m_boundfinishflg;       //バウンドの終了フラグ
    private bool m_scaleflg;
    private float  m_time; 

    private bool m_finishflg;   //ベジェの動きの終了の合図
    private float m_boundlimit_y = 6.0f;


	// Use this for initialization
	void Start () {
        m_titlelogo = GameObject.Find("TitleLogo");     //オブジェクトのロード
        m_ribbon = GameObject.Find("ribbon2");
        m_startpos = this.transform.position;
        m_nowpos = m_startpos;
        m_startscale = this.transform.localScale;
        m_nowscale = m_startscale;
        m_boundupflg = true;
        m_bounddownflg = false;
        m_boundfinishflg = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    /// <summary>
    /// タイトルロゴを引きずる
    /// </summary>
    public void TrailTitleLogo()
    {
        if (this.transform.position.x < Limit_x)
        {
            this.transform.position = m_ribbon.transform.position;
            //this.transform.position = m_bezier.GetPointAtTime(m_time);
            //transform.Rotate(30.0f * Time.deltaTime, 0, 0);
            //m_time += 0.35f * Time.deltaTime;
        }
        else
        {
            m_finishflg = true;
            m_nowpos.x = Limit_x;
            this.transform.position = m_nowpos;
        }
    }

    /// <summary>
    /// タイトルロゴのはねる動き
    /// </summary>
    public void BoundTitleLogo()
    {
        //バウンド（上がる）の処理
        if (m_boundupflg)
        {
            if (m_nowscale.z >= ScaleLimit)
            {
                m_nowscale.x = 2.0f;
                m_nowscale.z = 1.0f;
                this.transform.localScale = m_nowscale;
            }
            else
            {
                m_nowscale.x -= 0.01f;
                m_nowscale.z += 0.01f;
                this.transform.localScale = m_nowscale;
            }

            m_nowpos.y += 10.0f * Time.deltaTime;
            this.transform.position = m_nowpos;
            //バウンド（上がる）の上限が来たら、落ちる方のフラグを立てる。
            if(m_nowpos.y > m_boundlimit_y)
            {
                m_nowpos.y = m_boundlimit_y;
                m_boundupflg = false;
                m_bounddownflg = true;
            }
        }

        //バウンド（落ちる）の処理
        if (m_bounddownflg)
        {
            if (m_nowscale.z <= m_startscale.z)
            {
                m_nowscale.x = 2.0f;
                m_nowscale.z = 1.0f;
                this.transform.localScale = m_nowscale;
            }
            else
            {
                m_nowscale.x += 0.01f;
                m_nowscale.z -= 0.01f;
                this.transform.localScale = m_nowscale;
            }
            
            m_nowpos.y -= 10.0f * Time.deltaTime;
            this.transform.position = m_nowpos;
            //バウンド（落ちる）の下限が来たら、バウンド終了
            if(m_nowpos.y < m_startpos.y)
            {
                m_nowpos.y = m_startpos.y;
                this.transform.position = m_nowpos;
                m_nowscale.x = 2.0f;
                m_nowscale.z = 1.0f;
                this.transform.localScale = m_nowscale;
                m_boundfinishflg = true;
                m_bounddownflg = false;

            }
        }
    }

    /// <summary>
    /// タイトルロゴがベジェ曲線の動きを終了する合図
    /// </summary>
    public bool TrailtitleLogoFinishFlag
    {
        get { return m_finishflg; }
    }

    /// <summary>
    /// タイトルのUIを表示する余韻を与える
    /// </summary>
    public float TitleLogoTime()
    {
        return m_time += 10.0f * Time.deltaTime;
    }

    /// <summary>
    /// バウンド終了の合図
    /// </summary>
    public bool TitleLogoBoundFinishFlag
    {
        get { return m_boundfinishflg; }
    }

}
