using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    /// <summary>
    /// 定数定義
    /// </summary>
    const int ConstPlayerMax = 4;

    /// <summary>
    /// 変数定義
    /// </summary>
    private int[]   m_score;
    private Text[]  m_text; 

	// Use this for initialization
	void Start () {

        // 配列初期化
        m_score = new int[ConstPlayerMax];
        for (int i = 0; i < ConstPlayerMax; i++)
        {
            m_score[i] = 0;
        }

        // テキスト読み込み
        m_text = new Text[ConstPlayerMax];
        for (int i = 0; i < ConstPlayerMax; i++)
        {
            m_text[i] = GameObject.Find("かり").GetComponent<Text>();
        }

	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < ConstPlayerMax; i++)
        {
            

        }
	
	}


    /// <summary>
    /// プレイヤーの得点を加算する
    /// </summary>
    /// <param name="player">プレイヤーの番号</param>
    /// <param name="plus">加算する値</param>
    public void PlusScore(int player, int plus)
    {
        m_score[player] += plus; 
    }

    /// <summary>
    /// プレイヤーの得点を減算する
    /// </summary>
    /// <param name="player">プレイヤーの番号</param>
    /// <param name="minus">減算する値</param>
    public void MinusScore(int player, int minus)
    {
        m_score[player] -= minus;
    }
}
