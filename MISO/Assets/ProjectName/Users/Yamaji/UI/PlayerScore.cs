using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerScore : MonoBehaviour {

    private Text m_scoreText;

    private Player m_player;


	// Use this for initialization
	void Start () {
        
        m_scoreText = this.GetComponent<Text>();

        string findGameObjectName = "Player";

        switch (gameObject.tag)
        {
            case "Player1":
                {
                    findGameObjectName += "1";
                }
                break;

            case "Player2":
                {
                    findGameObjectName += "2";
                }
                break;

            case "Player3":
                {
                    findGameObjectName += "3";
                }
                break;

            case "Player4":
                {
                    findGameObjectName += "4";
                }
                break;

            default:
                {
                    Debug.LogAssertion("タグが設定されていません");
                    break;
                }
        }       // end of switch(gameObject.tag)

        GameObject g  = GameObject.Find(findGameObjectName);
        m_player = g.GetComponent<Player>();
        

    }
    
    // Update is called once per frame
    void Update () {

        m_scoreText.text = m_player.score.ToString();

    }
}
