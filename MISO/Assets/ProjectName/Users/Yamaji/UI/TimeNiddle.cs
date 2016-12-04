using UnityEngine;
using System.Collections;

public class TimeNiddle : MonoBehaviour {

    [SerializeField]
    private int m_time;

    private float rotateFrame;
	// Use this for initialization
	void Start () {

        rotateFrame = m_time * 30 / 360;
	
	}

    // Update is called once per frame
    void Update(){

        //this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, thi);
        //this.transform.Rotate(0.0f, 0.0f, -rotateFrame);
        iTween.RotateTo(this.gameObject, iTween.Hash("z", 360, "time", m_time));
    }
}
