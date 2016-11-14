using UnityEngine;
using System.Collections;

public class girl_sample : MonoBehaviour {
    private Vector3 pos;
    private Quaternion rotation;
	// Use this for initialization
	void Start () {
        pos = this.transform.position;
        rotation = this.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKey(KeyCode.A))
        {
            pos.x -= 0.1f;
            this.transform.position = pos;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += 0.1f;
            this.transform.position = pos;
            
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos.z += 0.1f;
            this.transform.position = pos;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.z -= 0.1f;
            this.transform.position = pos;
        }
	}

    
}
