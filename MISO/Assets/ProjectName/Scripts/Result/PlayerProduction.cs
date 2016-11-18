using UnityEngine;
using System.Collections;

public class PlayerProduction : MonoBehaviour {

    // プレイヤーの人数
    const int ConstPlayerMax = 4;

    // プレイヤーのオブジェクト
    private GameObject[] m_player;

    // 計算用
    private float m_startTime;

	// Use this for initialization
	void Start () {

        // オブジェクトのロード
        m_player = new GameObject[ConstPlayerMax];
        m_player[0] = GameObject.Find("Player01");
        m_player[1] = GameObject.Find("Player02");
        m_player[2] = GameObject.Find("Player03");
        m_player[3] = GameObject.Find("Player04");


    }

    // Update is called once per frame
    void Update () {
	
	}
}
