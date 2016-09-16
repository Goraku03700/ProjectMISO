using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public  Rigidbody rigidbody;
    public float acceleration;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        

        // xとzに10をかけて押す力をアップ
        rigidbody.AddForce(x * acceleration, 0, z * acceleration);
	}
}
