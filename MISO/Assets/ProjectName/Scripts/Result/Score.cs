using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    private int m_score;      // スコア
    private GUIText m_text;   // 文字

	// Use this for initialization
	void Start () {

        m_score = 0;    // 点数初期化

        m_text = GetComponent<GUIText>();   // コンポーネント取得
	}
	
	// Update is called once per frame
	void Update () {

        // スコアを反映
        m_text.text = m_score.ToString() + "人";

	}

    public void AddScore()
    {
        m_score++;
    }
}
