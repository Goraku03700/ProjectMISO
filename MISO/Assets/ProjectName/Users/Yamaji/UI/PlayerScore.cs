﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerScore : MonoBehaviour {

    [SerializeField]
    private float m_downSpeed;  // だんだん下げる時の速度

    private Text m_scoreText;  

    private Player m_player;

    private bool m_downFlag;

    private int m_saveScore;

    private float m_time;


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

        m_time = 0.0f;

    }
    
    // Update is called once per frame
    void Update () {

        m_scoreText.text = m_player.score.ToString();

        // スコアを少しずつ下げる
        if(m_downFlag)
        {
            m_time += Time.deltaTime;


    }


    /// <summary>
    /// 数字を
    /// </summary>
    /// <param name="donwNum"></param>
    public void DownCount(int donwNum)
    {
        // カウントダウンフラグ
        m_downFlag = true;

        // 数値を保存する
        m_saveScore = int.Parse(m_scoreText.text);

        m_time = 0.0f;
    }
}
