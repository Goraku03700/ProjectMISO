using UnityEngine;
using System.Collections;

public class TestBezier : MonoBehaviour {
    [SerializeField]
    Transform m_trans;

    [SerializeField]
    Rigidbody m_rigidbody;


    [SerializeField]
    float m_pushPower;


    [SerializeField]
    float m_limit;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.A))
        {
            Fire(m_trans, m_rigidbody);
        }
	}

    public void Fire(Transform playerTransform , Rigidbody playerRigidbody)
    {
        playerTransform.position = this.transform.position + Vector3.up;
        m_trans.transform.rotation = Quaternion.Inverse(this.transform.rotation);
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
        m_rigidbody.AddForce(-this.transform.forward * m_pushPower + Vector3.up * m_pushPower);
    }
}
