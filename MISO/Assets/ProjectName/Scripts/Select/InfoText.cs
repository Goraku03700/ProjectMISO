using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InfoText : MonoBehaviour {

    private Text infoText;

    private int addAlpha;

	// Use this for initialization
	void Start () {

        infoText = GameObject.Find("infoText").GetComponent<Text>();

        addAlpha = 1;
    }
	
	// Update is called once per frame
	void Update () {

        //infoText.color = Graphic.color

        //if(addAlpha)

        
	
	}
}
