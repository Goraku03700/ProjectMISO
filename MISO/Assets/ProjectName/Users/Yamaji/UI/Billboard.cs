using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    [SerializeField]
    private Camera m_targetCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.rotation = m_targetCamera.transform.rotation;
	
	}
}
