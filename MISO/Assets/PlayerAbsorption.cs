using UnityEngine;
using System.Collections;

public class PlayerAbsorption : MonoBehaviour {

    bool m_startAbsorption;

    Bezier m_bezier;//

    [SerializeField]
    ParticleSystem m_absorptionParticle;

    Vector3 m_startPosition, m_endPosition;

    float m_time = 0.0f;
    float m_duration;

	// Use this for initialization
	void Start () {
        m_absorptionParticle.Stop();
        m_absorptionParticle = transform.FindChild("Particle System").GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if(m_startAbsorption)
        {
            m_time += Time.deltaTime;
            if(m_time >1.0f + m_duration)
            {
                //BGMManager.instance.PlaySE("se015_InCampany");
                m_time = 1.0f;
                m_startAbsorption = false;
                m_absorptionParticle.Stop();
            }
            transform.position = Vector3.Lerp(m_startPosition, m_endPosition, m_time / (1.0f + m_duration));
            Vector3 up = m_bezier.GetPointAtTime(m_time);
            up.z = up.x = 0.0f;
            this.transform.position += up;
            //this.transform.position = m_bezier.GetPointAtTime(m_time);
        }
	}

    public void startAbsorption(Vector3 startPosition, Vector3 endPosition)
    {
        if (m_startAbsorption == false)
        {
            m_bezier = new Bezier(startPosition, Vector3.Lerp(startPosition, endPosition, 0.4f) + Vector3.up * 6f, Vector3.Lerp(startPosition, endPosition, 0.6f) + Vector3.up * 6f, endPosition);
            m_startAbsorption = true;
            m_absorptionParticle.Play();
            m_time = 0.0f;
            m_duration = Random.Range(-0.2f, 0.2f);
        }
        m_startPosition = startPosition;
        m_endPosition = endPosition;
    }

    public void SetAbsorption(Vector3 startPosition, Vector3 endPosition)
    {
        m_startPosition = startPosition;
        m_endPosition = endPosition;
        m_bezier.ResetBezier(m_startPosition, Vector3.Lerp(m_startPosition, m_endPosition, 0.4f) + Vector3.up * 6f, Vector3.Lerp(m_startPosition, m_endPosition, 0.6f) + Vector3.up * 6f, m_endPosition);
    }

    public void SetEndPosition(Vector3 endPosition)
    {
        m_endPosition = endPosition;
        m_bezier.ResetBezier(m_startPosition, Vector3.Lerp(m_startPosition, m_endPosition, 0.4f) + Vector3.up * 6f, Vector3.Lerp(m_startPosition, m_endPosition, 0.6f) + Vector3.up * 6f, m_endPosition);
    }




    public Vector3 GetPointAtTime(float time)
    {
        return m_bezier.GetPointAtTime(m_time / (1.0f + m_duration));
    }

    public Vector3 GetLerpPointAtTime()
    {
        Vector3 position;
        position = Vector3.Lerp(m_startPosition, m_endPosition, m_time / (1.0f + m_duration));
        Vector3 up = m_bezier.GetPointAtTime(m_time / (1.0f + m_duration));
        up.z = up.x = 0.0f;
        position += up;
        return position;
    }

}
