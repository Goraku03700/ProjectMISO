using UnityEngine;
using System.Collections;

public class TestBezier : MonoBehaviour {
    [SerializeField]
    Transform[] m_trans;

    [SerializeField]
    Vector3[] pos;

    Bezier m_be;

    float time;

	// Use this for initialization
	void Start () {
        m_be = new Bezier(pos[0], Vector3.Lerp(pos[0], pos[1], 0.5f) + this.transform.up * 2f, Vector3.Lerp(pos[0], pos[1], 0.5f) + this.transform.up * 2f, pos[1]);
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime / 10;
	    if(Input.GetKeyDown(KeyCode.A))
        {
            time = 0.0f;
        }
        if(time >1.0f)
        {
            time = 1.0f;
        }
        transform.position = m_be.GetPointAtTime(time);
	}

}
