using UnityEngine;
using System.Collections;

public class DashGauge : MonoBehaviour {

    [SerializeField]
    private Camera m_targetCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //カメラの方向を向くようにする。
        this.transform.LookAt(this.m_targetCamera.transform.position);
    }
}
