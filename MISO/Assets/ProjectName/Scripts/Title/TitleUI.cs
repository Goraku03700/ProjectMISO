using UnityEngine;
using System.Collections;

public class TitleUI : MonoBehaviour {

    private const float ScaleLimit_x = 2.0f;
    private const float ScaleLimit_z = 0.7f;
    private Vector3 m_startscale;   //タイトルUIの最初の拡縮
    private Vector3 m_nowscale;     //タイトルUIの現在の拡縮
    private MeshRenderer m_meshrender;  //UI表示用
    private bool m_scalebigflg;      //拡大のフラグ
    private bool m_scalesmallflg;    //縮小のフラグ

	// Use this for initialization
	void Start () {
        m_startscale = this.transform.localScale;
        m_nowscale = m_startscale;
        m_meshrender = GetComponent<MeshRenderer>();
        m_meshrender.enabled = false;
        m_scalebigflg = true;
        m_scalesmallflg = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// タイトルロゴの表示切り替え
    /// </summary>
    /// <param name="active">true:表示する false:表示しない</param>
    public void Activate(bool active)
    {
        m_meshrender.enabled = active;
    }

    /// <summary>
    /// タイトルのUIの拡縮させる
    /// </summary>
    public void ScaleTitleUI()
    {
        if(m_scalebigflg && m_nowscale.x > ScaleLimit_x)
        {
            m_scalebigflg = false;
            m_scalesmallflg = true;
            m_nowscale.x = ScaleLimit_x;
            m_nowscale.z = ScaleLimit_z;
            this.transform.localScale = m_nowscale;
        }
        else if(m_scalesmallflg && m_nowscale.x < m_startscale.x)
        {
            m_scalebigflg = true;
            m_scalesmallflg = false;
            m_nowscale.x = m_startscale.x;
            m_nowscale.z = m_startscale.z;
            this.transform.localScale = m_nowscale;
        }
        //UIを拡大
        if(m_scalebigflg)
        {
            m_nowscale.x += 1.5f * Time.deltaTime;
            m_nowscale.z += 1.0f * Time.deltaTime;
            this.transform.localScale = m_nowscale;
        }
        //UIを縮小
        if(m_scalesmallflg)
        {
            m_nowscale.x -= 1.5f * Time.deltaTime;
            m_nowscale.z -= 1.0f * Time.deltaTime;
            this.transform.localScale = m_nowscale;
        }
    }

    /// <summary>
    /// タイトルのUIの拡縮をリセットする
    /// </summary>
    public void ResetScaleTitleUI()
    {
        this.transform.localScale = m_startscale;
    }

}
