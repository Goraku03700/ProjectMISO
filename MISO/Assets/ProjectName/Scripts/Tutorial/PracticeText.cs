using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PracticeText : MonoBehaviour {

    private Text m_text;

	// Use this for initialization
	void Start () {

        m_text = this.GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {

        Color color = m_text.color;
        
	
	}
}
