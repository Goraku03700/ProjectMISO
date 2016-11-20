using UnityEngine;
using System.Collections;

public class animation_test : MonoBehaviour {

    [SerializeField]
    private GameObject m_obj;
    [SerializeField]
    private float m_changeframesecond;
    [SerializeField]
    private string m_flodername;
    [SerializeField]
    private string m_headText;
    [SerializeField]
    private int m_imageLength;

    private int m_firstFrameNum;
    private float m_nowtime;

	// Use this for initialization
	void Start () {
        m_firstFrameNum = 1;
        m_nowtime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        m_nowtime += Time.deltaTime;
        if(m_changeframesecond < m_nowtime)
        {
            m_nowtime = 0.0f;
            m_firstFrameNum++;
            if (m_firstFrameNum > m_imageLength) m_firstFrameNum = 1;
        }
        Texture tex = Resources.Load(m_flodername + "/" + m_headText + m_firstFrameNum) as Texture;
        
	}
}
