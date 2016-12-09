using UnityEngine;
using System.Collections;

public class Ready : MonoBehaviour {

    [SerializeField]
    float m_speed;

    private bool m_isDisp;

	// Use this for initialization
	void Start () {

        m_isDisp = false;

	}

    // Update is called once per frame
    void Update() {

        if (m_isDisp)
        {
            Vector3 objScale = this.transform.localScale;

            objScale.x += m_speed * Time.deltaTime;
        }
	
	}
}
