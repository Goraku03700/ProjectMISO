using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Production : MonoBehaviour {


    // シーンの状態
    public enum ResultState
    {
        Production01,
        Production02,
        Production03,
        
    };
    ResultState resultState;        // シーンの状態

    /// <summary>
    /// 定数定義
    /// </summary>
    const int ConstPlayerMax = 4;    // プレイヤーの人数

    /// <summary>
    /// 変数定義
    /// </summary>
    private int[] m_score;           // 4人のプレイヤーの得点を格納

    private float  m_lerpRate;       // 線形補間の比率
    private Vector3[] m_startPos;    // プレイヤー開始位置

    private GameObject[,] m_women;  // 女性のオブジェクト
    private Vector3[,] m_womenGoalPosition; // 女性の目標地点
    private bool[,] m_womenMoveFlag;  // 女性が前に進むフラグ
    private float[,] m_womenMoveLerp;  // 線形補間用

    private int[] m_scoreCount;       // スコアカウント用
    
    private int i, j, k;       // 添え字
    private int time;       // 経過時間カウント用
    private int out_cnt;    // 女性が会社から出てくる人数


    /// <summary>
    /// インスペクタに表示する変数
    /// </summary>
    [SerializeField]
    private GameObject[] m_player;   // プレイヤーのオブジェクト
    
    [SerializeField]
    private GameObject[] m_Company;   // 会社のオブジェクト

    [SerializeField]
    private Text[] scoreText;      // スコアのテキスト
    
    [SerializeField]
    private Transform womenColumnStartMaker;    // 女性の行列のスタート地点

    [SerializeField]
    private GameObject womenPrefab; // 女性のオブジェクトのプレハブ

    [SerializeField]
    private Transform endMaker;     // プレイヤーの移動する終着点

    [SerializeField]
    private float playerMoveSpeed;  // プレイヤーの移動速度

    [SerializeField]
    private float womenMoveSpeed;   // 女性の移動速度

    [SerializeField]
    private int womenColumnIntervalTime;

    [SerializeField]
    private int womenColumnMax;     // 女性の列の最大数


    /*************************/
    // テスト 

    GameObject cube1;
    GameObject cube2;

    // Use this for initialization
    void Start () {

        cube1 = GameObject.Find("Cube");
        cube2 = GameObject.Find("Cube2");

        // 配列の確保
        m_score = new int[ConstPlayerMax];

        /**** 仮の値 ****/
        m_score[0] = 20;
        m_score[1] = 25;
        m_score[2] = 15;
        m_score[3] = 18;

        /**** マジックナンバー使用中 ****/
        // オブジェクトのロード
        //m_player = new GameObject[ConstPlayerMax];
        //m_player[0] = GameObject.Find("Player01");
        //m_player[1] = GameObject.Find("Player02");
        //m_player[2] = GameObject.Find("Player03");
        //m_player[3] = GameObject.Find("Player04");

        //m_Company = new GameObject[ConstPlayerMax];
        //m_Company[0] = GameObject.Find("Company01");
        //m_Company[1] = GameObject.Find("Company02");
        //m_Company[2] = GameObject.Find("Company03");
        //m_Company[3] = GameObject.Find("Company04");

        m_scoreCount = new int[ConstPlayerMax];

        // プレイヤーの初期位置を保存
        m_startPos = new Vector3[ConstPlayerMax];
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_startPos[i] = m_player[i].transform.position;

            m_scoreCount[i] = 0;
        }

        m_lerpRate = 0.0f;



        // スコアの最大値をとる
        int max = m_score.Max();
        m_women = new GameObject[ConstPlayerMax,max];

        // 配列初期化
        m_womenMoveFlag = new bool[ConstPlayerMax, max];
        m_womenGoalPosition = new Vector3[ConstPlayerMax, max];
        m_womenMoveLerp = new float[ConstPlayerMax, max];

        float offset_x = 0.3f;
                     
        // 女性の目標地点を決める
        for (i = 0; i < ConstPlayerMax; i++)
        {
            int line_cnt = 1;   // 行の数
            int column_max = 1; // 現在の行の人数の最大
            int k = 0;          // 現在何番目の列か

            // 獲得した人数分だけ
            for (j = 0; j < m_score[i]; j++)
            {
                // 女性のオブジェクトを生成
                m_women[i, j] = Instantiate(womenPrefab);

                // 非アクティブ状態
                m_women[i, j].SetActive(false);


                // 目標地点を設定する
                // 整列ver
                //m_womenGoalPosition[i, j] = womenColumnStartMaker.position;
                //m_womenGoalPosition[i, j].x = m_player[i].transform.position.x - (m_player[0].transform.position.x - womenColumnStartMaker.position.x) + (j / 8 * 0.5f);
                //m_womenGoalPosition[i, j] += new Vector3(0.0f, 0.0f, j%8 * 0.5f);

                // ピラミッドver
                m_womenGoalPosition[i, j] = womenColumnStartMaker.position;
                m_womenGoalPosition[i, j].x = m_player[i].transform.position.x - (m_player[0].transform.position.x - womenColumnStartMaker.position.x) - (offset_x/2 * (column_max-1)) + (k * offset_x);
                m_womenGoalPosition[i, j] += new Vector3(0.0f, 0.0f, (line_cnt-1) * 0.5f);

                k++;
                if(k+1 > column_max)
                {
                    k = 0;
                    column_max++;
                    if (column_max > womenColumnMax) column_max = womenColumnMax;
                    line_cnt++;
                   
                }

                // 動くフラグの初期化
                m_womenMoveFlag[i, j] = false;

                // 線形補間用
                m_womenMoveLerp[i, j] = 0.0f;

            }
        }

        
        

        // 演出01を初期の状態に設定
        resultState = ResultState.Production01;

        time = 0;

        out_cnt = 1;
    }

    // Update is called once per frame
    void Update () {

        /*
        switch(resultState)
        {
            // 会社を置く演出
            case ResultState.Production01:
                _Production01();
                break;

            // プレイヤーが前に走ってくる演出
            case ResultState.Production02:
                _Production02();
                break;

            // 置いた会社から女性が出て来る演出
            case ResultState.Production03:
                _Production03();
                break;
        }
        */
   

	}


    /// <summary>
    /// 演出01
    /// 会社を置く演出
    /// </summary>
    private void _Production01()
    {
        
        // 親子関係を解除する(その場に置く)
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_player[i].transform.DetachChildren();
        }
        
        // 演出02に遷移する
        resultState = ResultState.Production02;
    }
            

    /// <summary>
    /// 演出02
    /// プレイヤーが前に走ってくる演出
    /// </summary>
    private void _Production02()
    {

        // 線形補間の比率を上げる
        m_lerpRate += playerMoveSpeed;

        // 4キャラ全員を前に移動させる
        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 終着点設定
            Vector3 end = new Vector3(m_player[i].transform.position.x,
                                      m_player[i].transform.position.y,
                                      endMaker.position.z);

            // 線形補間で前に移動する
            m_player[i].transform.position = Vector3.Lerp(m_startPos[i], end, m_lerpRate);
        }

        // 終着点に着いたら次の演出に遷移
        if (m_lerpRate >= 1.0f)
        {
            resultState = ResultState.Production03;
        }
        

        

    }

    /// <summary>
    /// 演出03
    /// 置いた会社から女性が出てくる演出
    /// </summary>
    private void _Production03()
    {
        time++;
        
        // 一定時間ごとに女性を会社から出す
        if (time > womenColumnIntervalTime)
        {
            time = 0;

            // プレイヤーの数
            for (i = 0; i < ConstPlayerMax; i++)
            {
                // 行の人数分出す
                for (k = 0; k < out_cnt; k++)
                {
                    // 動かしていない女性を探す
                    for (j = 0; j < m_score[i]; j++)
                    {
                        // 現在動いていない女性のみ
                        if (!m_womenMoveFlag[i, j])
                        {
                            // 動かす
                            m_womenMoveFlag[i, j] = true;
                            m_women[i, j].SetActive(true);  // アクティブ化

                            // スコア加算
                            m_scoreCount[i]++;
                            scoreText[i].text = m_scoreCount[i].ToString() + "人";

                            break;
                        }
                    }
                }
            }

            out_cnt++;

            // 列の最大人数になったら人数を固定
            if (out_cnt > womenColumnMax)
            {
               out_cnt = womenColumnMax;
            }
        }
        

        
        // 女性を動かす
        for (i = 0; i < ConstPlayerMax; i++)
        {
            for (j = 0; j <m_score[i]; j++)
            {
                // 動かすフラグが立っていたら
                if (m_womenMoveFlag[i, j])
                {
                    // 進める
                    m_womenMoveLerp[i, j] += womenMoveSpeed;

                    // 目的地まで到達したらそこに固定
                    if (m_womenMoveLerp[i, j] >= 1.0f)
                    { 
                        m_womenMoveLerp[i, j] = 1.0f;
                    }

                    // 女性の移動
                    m_women[i, j].transform.position = Vector3.Lerp(m_Company[i].transform.position,
                                                                   m_womenGoalPosition[i, j],
                                                                   m_womenMoveLerp[i, j]);
                }
            }
        }

    }

}
// コルーチン
// unityチュートリアル　シューティング