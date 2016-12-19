using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    [SerializeField]
    private Camera m_targetCamera;

    [SerializeField]
    private bool m_enableRotateZ;

	// Use this for initialization
	void Start ()
    {
        if(m_targetCamera == null)
            m_targetCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {


        if (m_enableRotateZ)
        {


            Vector3 eularAngle = transform.rotation.eulerAngles;

            transform.forward = m_targetCamera.transform.forward * -1;

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, eularAngle.z);
        }
        else
        {
            transform.forward = m_targetCamera.transform.forward * -1;
        }


    }
}
