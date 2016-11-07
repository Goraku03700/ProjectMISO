using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeUIScript : MonoBehaviour {

    private int time;

    private Text TimeUIText;

	// Use this for initialization
	void Start () {

        TimeUIText = GameObject.Find("TimeUI").GetComponent<Text>();

        time = 10800;   // ３分
	

	}
	
	// Update is called once per frame
	void Update () {

        time--;

        TimeUIText.text = (time / 60).ToString();
        
	}
}
