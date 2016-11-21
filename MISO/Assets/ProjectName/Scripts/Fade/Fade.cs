using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Fade : SingletonMonoBehaviour<Fade>{
    private enum State { Out, In, End };
    static State m_state;

    [SerializeField]
    float m_time;
    [SerializeField]
    float m_colorTime;
    [SerializeField]
    float fadeintime;             // フェードイン・アウトそれぞれにかける時間

    [SerializeField]
    float fadeouttime;             // フェードイン・アウトそれぞれにかける時間

    [SerializeField]
    float m_inRotationPerSecond;

    [SerializeField]
    float m_outRotationPerSecond;

    private static string SceneName;           // 遷移先のシーン名

    [SerializeField]
    Image m_ribbon;
    private static bool fadeflg;
    private static bool destroy;
    public Image[] sprites;

    [SerializeField]
    Color32[] m_lerpColors;

    public Material m_rend;
    void Start()
    {
        this._InitializeSingleton();
        
        m_state = State.End;
        DontDestroyOnLoad(this.gameObject);
        //m_rend = sprites[1].GetComponent<CanvasRenderer>().GetMaterial(0);
      //  m_rends[0].GetMaterial(0).shader = Shader.Find("UI/Mask");
       // m_rends[1].material.shader = Shader.Find("UI/Mask");
    }
    void Update()
    {
        m_time += Time.deltaTime;
        switch(m_state)
        {
            case State.Out:
                {
                    sprites[0].color = Color32.Lerp(m_lerpColors[0], m_lerpColors[1], m_time / m_colorTime);
                    sprites[1].color = Color32.Lerp(m_lerpColors[0], m_lerpColors[1], m_time / m_colorTime);

                    float shininess = Mathf.Lerp(0.42f, 0.0f, m_time / fadeouttime)-0.02f;
                    sprites[0].rectTransform.localRotation = Quaternion.Euler(0, 0, m_time * m_outRotationPerSecond);
                    sprites[1].rectTransform.localRotation = Quaternion.Euler(0, 0, m_time * m_outRotationPerSecond);
                    m_ribbon.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, m_time / fadeouttime);
                    m_rend.SetFloat("_Range", shininess);
                    if(m_time>fadeouttime)
                    {
                        m_state = State.In;
                        m_time = 0.0f;
                        SceneManager.LoadScene(SceneName);
                    }

                    break;
                }

            case State.In:
                {
                    sprites[0].color = Color32.Lerp(m_lerpColors[1], m_lerpColors[0], m_time / fadeintime);
                    if (m_colorTime > m_time)
                    {
                        sprites[1].color = Color32.Lerp(m_lerpColors[1], m_lerpColors[0], (m_time - m_colorTime) / m_colorTime);
                    }
                    float shininess = Mathf.Lerp(0.0f, 0.42f, m_time / fadeintime);
                    if(m_time > fadeintime)
                    {
                        shininess = 1.0f;
                    }
                    sprites[0].rectTransform.localRotation = Quaternion.Euler(0, 0, m_time * m_inRotationPerSecond);
                    sprites[1].rectTransform.localRotation = Quaternion.Euler(0, 0, m_time * m_inRotationPerSecond);
                    m_ribbon.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, m_time / fadeintime);
                    m_rend.SetFloat("_Range", shininess);
                    if (m_time > fadeintime)
                    {
                        m_state = State.End;
                        m_time = 0.0f;
                    }
                    break;
                }
            case State.End:
                {
                    sprites[0].color = m_lerpColors[0];
                    sprites[1].color = m_lerpColors[0];
                    m_time = 0.0f;
                    break;
                }
        }
        
        
        
    }
    //========================================
    // Use this for initialization
    //========================================
   /*
    void Start(){
        // 一時的に破棄しない

        time = 0.0f;
        DontDestroyOnLoad(this);
	}

    void Update(){
       
        if (time>fadeouttime && Application.loadedLevelName != SceneName)
        {
            Application.LoadLevel(SceneName);
        }


        
    }
    */
    //========================================
    // フェードイン・アウト処理
    //========================================
    
    public static void ChangeScene(string nextScene)
    {
        if (m_state == State.End)
        {
            m_state = State.Out;
        }
        SceneName = nextScene;
    }

    public bool FadeEnd()
    {
        if (m_state == State.End)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
