using UnityEngine;
using System.Collections;

public class Builboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        this.transform.LookAt(Camera.main.transform.position);
    }
}
