using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpotLighter : MonoBehaviour {

    [SerializeField]
    Transform m_targetTransform;

    List<Vector3> m_vectorBuffer = new List<Vector3>();
	// Use this for initialization
	void Start () {
        for(int i = 0; i < 10 ; ++i)
        {
            m_vectorBuffer.Add(m_targetTransform.position);
        }
	}
	
	// Update is called once per frame
	void Update () {
        /*
        Quaternion targetRotation = Quaternion.LookRotation(m_vectorBuffer[0] - transform.position);
        transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, 1.0f);
        */
        this.transform.position = m_vectorBuffer[0] + Vector3.up * 15.5f;

        m_vectorBuffer.RemoveAt(0);
        m_vectorBuffer.Add(m_targetTransform.position);
	}
}
