using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DushTest : MonoBehaviour {


    private ParticleSystem[] m_particles;

    private int time;

    private bool startFlag;


    [SerializeField]
    private ParticleSystem m_particleRun;

    [SerializeField]
    private int m_particleMax;

    [SerializeField]
    private float m_interval;

    [SerializeField]
    private float m_offset_y;

    // Use this for initialization
    void Start() {

        // パーティクル設定
        m_particles = new ParticleSystem[m_particleMax];
        for (int i = 0; i < m_particleMax; i++)
        {
            m_particles[i] = Instantiate(m_particleRun);
        }

        startFlag = false;

        time = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // プレイ中のパーティクルを探す
        int cnt = 0;
        for (int i = 0; i < m_particleMax; i++)
        {
            if (m_particles[i].isPlaying)
            {
                cnt++;
            }
        }
        GameObject.Find("Text").GetComponent<Text>().text = cnt.ToString();

        // フラグが立っていなかったら抜ける
        if (!startFlag)
        {
            return;
        }

        time++;

        if (time > m_interval)
        {
            time = 0;

            // 開始していないパーティクルを探す
            for (int i = 0; i < m_particleMax; i++)
            {
                if (!m_particles[i].isPlaying)
                {
                    m_particles[i].transform.localPosition = new Vector3(this.transform.localPosition.x,
                                                                         this.transform.localPosition.y + m_offset_y,
                                                                         this.transform.localPosition.z);
                    m_particles[i].Play();
                    break;
                }
            }
        }



         

    }


    public void startParticle()
    {
        startFlag = true;
    }

    public void stopParticle()
    {
        startFlag = false;
    }


   


}
