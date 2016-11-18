using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerScore : MonoBehaviour {

    private Text m_scoreText;

    private int m_score;

	// Use this for initialization
	void Start () {

        m_scoreText = this.GetComponent<Text>();

        m_score = 0;
    }

    // Update is called once per frame
    void Update () {

        m_scoreText.text = m_score.ToString() + "人";

       
        
	}
}
