using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルのUIに関するクラス
/// ここでは表示の切り替え・拡縮の動きを管理しています
/// </summary>
public class TitleUI : MonoBehaviour {

    [SerializeField]
    private float ScaleLimit_x = 2.0f;

    [SerializeField]
    private float ScaleLimit_z = 0.7f;

    [SerializeField]
    private float ScaleMin_x = 2.0f;

    [SerializeField]
    private float ScaleMin_z = 0.7f;

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
        //m_meshrender.enabled = false;
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
        //m_meshrender.enabled = active;
    }

    /// <summary>
    /// タイトルのUIの拡縮させる
    /// </summary>
    public void ScaleTitleUI()
    {
        //if(m_scalebigflg && m_nowscale.x > ScaleLimit_x)
        //{
        //    m_scalebigflg = false;
        //    m_scalesmallflg = true;
        //    m_nowscale.x = ScaleLimit_x;
        //    m_nowscale.z = ScaleLimit_z;
        //    this.transform.localScale = m_nowscale;
        //}
        //else if(m_scalesmallflg && m_nowscale.x < m_startscale.x)
        //{
        //    m_scalebigflg = true;
        //    m_scalesmallflg = false;
        //    m_nowscale.x = m_startscale.x;
        //    m_nowscale.z = m_startscale.z;
        //    this.transform.localScale = m_nowscale;
        //}
        ////UIを拡大
        //if(m_scalebigflg)
        //{
        //    m_nowscale.x += 1.0f * Time.deltaTime;
        //    m_nowscale.z += 0.5f * Time.deltaTime;
        //    this.transform.localScale = m_nowscale;
        //}
        ////UIを縮小
        //if(m_scalesmallflg)
        //{
        //    m_nowscale.x -= 1.0f * Time.deltaTime;
        //    m_nowscale.z -= 0.5f * Time.deltaTime;
        //    this.transform.localScale = m_nowscale;
        //}

        float scaleX = 1.0f;
        float scaleZ = 1.0f;

        scaleX = Mathf.PingPong(Time.time, ScaleLimit_x - ScaleMin_x) + ScaleMin_x;
        scaleZ = Mathf.PingPong(Time.time, ScaleLimit_z - ScaleMin_z) + ScaleMin_z;

        //scaleX = Mathf.Sin(Time.time * 12.0f) * 0.35f;
        //scaleZ = Mathf.Sin(Time.time * 12.0f) * 0.35f;

        //scaleZ = Mathf.s

        //this.transform.localScale = new Vector3(scaleX, 1, scaleZ);
        this.transform.localScale = m_startscale * scaleX;
    }

    //public float scaleTime;

    /// <summary>
    /// タイトルのUIの拡縮をリセットする
    /// </summary>
    public void ResetScaleTitleUI()
    {
        this.transform.localScale = m_startscale;
    }

}
