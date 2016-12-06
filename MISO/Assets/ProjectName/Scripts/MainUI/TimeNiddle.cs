using UnityEngine;
using System.Collections;

public class TimeNiddle : MonoBehaviour {

    [SerializeField]
    private float m_time;


    private GameObject obj;

    private float rotateFrame;
	// Use this for initialization
	void Start () {

        rotateFrame = m_time * 30 / 360;

        obj = GameObject.Find("TimeNiddle");
	
	}

    // Update is called once per frame
    void Update(){

        //this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, thi);
        this.transform.Rotate(0.0f, 0.0f, -0.3f);
        //iTween.RotateTo(this.gameObject, iTween.Hash("z", 360, "time", m_time));

        //iTween.RotateTo(this.gameObject, iTween.Hash("z", 360.0f, "time", 1.0f));
    }
}
