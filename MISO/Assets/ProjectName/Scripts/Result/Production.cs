using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
//using System;

public class Production : MonoBehaviour {

    // やること
    //  women->girl
    //  列がずれてる

    // シーンの状態
    private enum ResultState
    {
        PutCompanyProduction,
        MovePlayerProduction,
        MoveGirlProduction,
        UpPodiumProduction,
        DecideRankProducution,
    };
    ResultState resultState; 

    // 表彰台の状態
    private enum PodiumState
    {
        Up,
        Down,
        Stop,
    }
    PodiumState[] podiumState;

    /// <summary>
    /// 定数定義
    /// </summary>
    const int ConstPlayerMax = 4;    // プレイヤーの人数
    const float ConstPodiumAccel = 0.01f;   // 表彰台の加速度

    /// <summary>
    /// 変数定義
    /// </summary>
    private int[] m_score;           // 4人のプレイヤーの得点を格納(受け取る)

    // 読み込むゲームオブジェクト
    private GameObject[]    m_player;   // プレイヤー
    private GameObject[,]   m_girl;    // 女性
    private GameObject[]    m_podium;   // 表彰台
    private GameObject[]    m_company;  // 会社

    // 読み込むテキスト
    private Text[] m_scoreText;      // スコアのテキスト
    private Text[] m_rankingText;    // ランキングのテキスト

    // 読み込むパーティクル
    private GameObject     m_confettiParticleParent;
    private ParticleSystem m_confettiParticle1;
    private ParticleSystem m_confettiParticle2;
    private ParticleSystem m_confettiParticle3;


    // 計算用
    private int[]       m_playerRanking;     // スコアのランキング
    private int         m_saveTopPlayer;     // 1位のプレイヤーは誰か

    private float       m_playerLerpRate;    // 線形補間の比率
    private Vector3[]   m_playerStartPos;    // 開始位置保存用

    private Vector3[,]  m_girlGoalPosition; // 女性の目標地点
    private bool[,]     m_girlMoveFlag;  // 女性が前に進むフラグ
    private float[,]    m_girlLerpRate;  // 線形補間用
    private int         m_girlColumnCnt;    // 女性が会社から出てくる人数


    private int[]       m_scoreCount;       // スコアカウント用
    
    private int i, j, k;       // 添え字
    private int time;       // 経過時間カウント用
    
    private float[]     m_podiumSpeed;       // 表彰台の上下の移動速度
    private Vector3[]   m_savePodiumStart;   // 位置を保存 
    private float[]     m_podiumLerp;        // 線形補間用

    private bool se023_startFlag;
    private bool se024_startFlag;
    private bool bgm003_startFlag;


    /// <summary>
    /// インスペクタに表示する変数
    /// </summary>
    
    [SerializeField]
    private Transform girlColumnStartMaker;    // 女性の行列のスタート地点

    [SerializeField]
    private float girlLineInterval;            // 女性の間隔(行)

    [SerializeField]
    private float girlColumnInerver;            // 女性の間隔(列)

    [SerializeField]
    private GameObject girlPrefab; // 女性のオブジェクトのプレハブ

    [SerializeField]
    private Transform playerEndMaker;     // プレイヤーの移動する終着点

    [SerializeField]
    private float playerMoveSpeed;  // プレイヤーの移動速度

    [SerializeField]
    private float girlMoveSpeed;   // 女性の移動速度

    [SerializeField]
    private int girlLineIntervalTime;

    [SerializeField]
    private int girlColumnMax;     // 女性の列の最大数

    [SerializeField]
    private GameObject podiumPrefab;   

    [SerializeField]
    private float[] podiumSeedMax;

    [SerializeField]
    private int podiumProdcutionTime;

    [SerializeField]
    private int prodiumProductionWaitTime;

    [SerializeField]
    private Transform rank1PodiumPos;

    [SerializeField]
    private Transform rank4PodiumPos;

    private int test;


    // Use this for initialization
    void Start() {


        // 配列の確保
        m_score = new int[ConstPlayerMax];

        /**** 仮の値 ****/
        m_score[0] = 20;
        m_score[1] = 25;
        m_score[2] = 15;
        m_score[3] = 18;

        /**** マジックナンバー使用中 ****/
        // オブジェクトのロード
        // プレイヤー
        m_player = new GameObject[ConstPlayerMax];
        m_player[0] = GameObject.Find("Player01");
        m_player[1] = GameObject.Find("Player02");
        m_player[2] = GameObject.Find("Player03");
        m_player[3] = GameObject.Find("Player04");

        // 会社
        m_company = new GameObject[ConstPlayerMax];
        m_company[0] = GameObject.Find("Company01");
        m_company[1] = GameObject.Find("Company02");
        m_company[2] = GameObject.Find("Company03");
        m_company[3] = GameObject.Find("Company04");

        // 表彰台
        m_podium = new GameObject[ConstPlayerMax];
        m_podium[0] = GameObject.Find("Podium01");
        m_podium[1] = GameObject.Find("Podium02");
        m_podium[2] = GameObject.Find("Podium03");
        m_podium[3] = GameObject.Find("Podium04");

        // スコアのテキスト
        m_scoreText = new Text[ConstPlayerMax];
        m_scoreText[0] = GameObject.Find("ScoreText01").GetComponent<Text>();
        m_scoreText[1] = GameObject.Find("ScoreText02").GetComponent<Text>();
        m_scoreText[2] = GameObject.Find("ScoreText03").GetComponent<Text>();
        m_scoreText[3] = GameObject.Find("ScoreText04").GetComponent<Text>();

        // ランキングのテキスト
        m_rankingText = new Text[ConstPlayerMax];
        m_rankingText[0] = GameObject.Find("RankingText01").GetComponent<Text>();
        m_rankingText[1] = GameObject.Find("RankingText02").GetComponent<Text>();
        m_rankingText[2] = GameObject.Find("RankingText03").GetComponent<Text>();
        m_rankingText[3] = GameObject.Find("RankingText04").GetComponent<Text>();

        // パーティクルシステム
        m_confettiParticleParent = GameObject.Find("ConfettiParticle");
        m_confettiParticle1 = GameObject.Find("ConfettiParticle1").GetComponent<ParticleSystem>();
        m_confettiParticle2 = GameObject.Find("ConfettiParticle2").GetComponent<ParticleSystem>();
        m_confettiParticle3 = GameObject.Find("ConfettiParticle3").GetComponent<ParticleSystem>();
        m_confettiParticle1.Stop();
        m_confettiParticle2.Stop();
        m_confettiParticle3.Stop();

        // テキストは最初表示しない
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_scoreText[i].enabled   = false;
            m_rankingText[i].enabled = false;
        }

        // プレイヤーの順位を判定
        m_playerRanking = new int[ConstPlayerMax];

        int[] tmp_score;   // 一時保存用
        tmp_score = new int[ConstPlayerMax];

        m_score.CopyTo(tmp_score, 0);       // 配列のコピー
        System.Array.Sort(tmp_score);       // 昇順にソート
        System.Array.Reverse(tmp_score);    // 反転して降順にする
        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 順位の確定a
            m_playerRanking[i] = System.Array.IndexOf(tmp_score, m_score[i]);
        }

        int max = m_score.Max();   // スコアの最大値をとる
        m_saveTopPlayer = System.Array.IndexOf(m_score, max);   // 最大値の配列が何番目か保存



        // スコア関係の初期化
        m_scoreCount = new int[ConstPlayerMax];
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_scoreCount[i] = 0;
        }



        // プレイヤー関係の初期化
        m_playerStartPos = new Vector3[ConstPlayerMax];
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_playerStartPos[i] = m_podium[i].transform.position;
        }
        m_playerLerpRate = 0.0f;



        // 女性関係の初期化
        m_girl             = new GameObject[ConstPlayerMax, max];
        m_girlMoveFlag     = new bool[ConstPlayerMax, max];
        m_girlGoalPosition = new Vector3[ConstPlayerMax, max];
        m_girlLerpRate     = new float[ConstPlayerMax, max];


        // 女性の目標地点を決める
        for (i = 0; i < ConstPlayerMax; i++)
        {
            int line_cnt = 1;   // 行の数
            int nowColumn_max = 1; // 現在の行の人数の最大
            int k = 0;          // 現在何番目の列か

            // 獲得した人数分だけ
            for (j = 0; j < m_score[i]; j++)
            {
                // 女性のオブジェクトを生成
                m_girl[i, j] = Instantiate(girlPrefab);

                // 非アクティブ状態
                m_girl[i, j].SetActive(false);


                // 目標地点を設定する
                // 整列ver
                //m_womenGoalPosition[i, j] = womenColumnStartMaker.position;
                //m_womenGoalPosition[i, j].x = m_player[i].transform.position.x - (m_player[0].transform.position.x - womenColumnStartMaker.position.x) + (j / 8 * 0.5f);
                //m_womenGoalPosition[i, j] += new Vector3(0.0f, 0.0f, j%8 * 0.5f);

                // ピラミッドver
                m_girlGoalPosition[i, j] = girlColumnStartMaker.position;
                //m_girlGoalPosition[i, j].x = m_player[i].transform.position.x - (m_player[0].transform.position.x - womenColumnStartMaker.position.x) - (offset_x / 2 * (column_max - 1)) + (k * offset_x);
                m_girlGoalPosition[i, j].x = m_player[i].transform.position.x - (girlColumnInerver/2 * (nowColumn_max-1)) + (k * girlLineInterval);
                m_girlGoalPosition[i, j] += new Vector3(0.0f, 0.0f, (line_cnt - 1) * girlLineInterval);

                k++;
                if (k + 1 > nowColumn_max)
                {
                    k = 0;
                    nowColumn_max++;
                    if (nowColumn_max > girlColumnMax) nowColumn_max = girlColumnMax;
                    line_cnt++;

                }

                // 動くフラグの初期化
                m_girlMoveFlag[i, j] = false;

                // 線形補間用
                m_girlLerpRate[i, j] = 0.0f;

            }
        }


        // 表彰台関係の初期化
        m_savePodiumStart = new Vector3[ConstPlayerMax];
        m_podiumSpeed     = new float[ConstPlayerMax];
        podiumState       = new PodiumState[ConstPlayerMax];
        m_podiumLerp      = new float[ConstPlayerMax];

        for (i = 0; i< ConstPlayerMax; i++)
        {
            m_podiumSpeed[i] = 0.0f;
            podiumState[i] = PodiumState.Up;
            m_podiumSpeed[i] = podiumSeedMax[i];
            m_podiumLerp[i] = 0.0f;

        }


        // 演出01を初期の状態に設定
        resultState = ResultState.PutCompanyProduction;

        time = 0;

        m_girlColumnCnt = 1;

        se023_startFlag = false;
        se024_startFlag = false;
        bgm003_startFlag = false;
    
    }

    // Update is called once per frame
    void Update () {



        switch (resultState)
        {
            // 会社を置く演出
            case ResultState.PutCompanyProduction:
                PutCompanyProduction();
                break;

            // プレイヤーが前に走ってくる演出
            case ResultState.MovePlayerProduction:
                _MovePlayerProduction();
                break;

            // 置いた会社から女性が出て来る演出
            case ResultState.MoveGirlProduction:
                _MoveGirlProduction();
                break;
            
            // 表彰台が上下する演出
            case ResultState.UpPodiumProduction:
                _UpPodiumProduction();
                break;
            
            // 順位が確定する演出
            case ResultState.DecideRankProducution:
                _DecideRankProduction();
                break;
        }
        
   

	}


    /// <summary>
    /// 演出01
    /// 会社を置く演出
    /// </summary>
    private void PutCompanyProduction()
    {
        
        // 親子関係を解除する(その場に置く)
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_player[i].transform.DetachChildren();
        }
        
        // 演出02に遷移する
        resultState = ResultState.MovePlayerProduction;
    }
            

    /// <summary>
    /// 演出02
    /// プレイヤーが前に走ってくる演出
    /// </summary>
    private void _MovePlayerProduction()
    {
        /*
        if (test == 0)
        {
            BGMManager.instance.PlaySE("se_000", 1.0f);
            test = 1;
        }

        */
        // 線形補間の比率を上げる
        m_playerLerpRate += playerMoveSpeed;

        // 4キャラ全員を前に移動させる
        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 終着点設定
            Vector3 end = new Vector3(m_podium[i].transform.position.x,
                                      m_podium[i].transform.position.y,
                                      playerEndMaker.position.z);

            // 線形補間で前に移動する
            
            m_podium[i].transform.position = Vector3.Lerp(m_playerStartPos[i], end, m_playerLerpRate);
        }

        // 終着点に着いたら次の演出に遷移
        if (m_playerLerpRate >= 1.0f)
        {
            resultState = ResultState.MoveGirlProduction;
        }
        

        

    }

    /// <summary>
    /// 演出03
    /// 置いた会社から女性が出てくる演出
    /// </summary>
    private void _MoveGirlProduction()
    {
        time++;



        // 一定時間ごとに女性を会社から出す
        if (time > girlLineIntervalTime)
        {
            time = 0;

            // プレイヤーの数
            for (i = 0; i < ConstPlayerMax; i++)
            {
                // 列の人数分出す
                for (k = 0; k < m_girlColumnCnt; k++)
                {
                    // 動かしていない女性を探す
                    for (j = 0; j < m_score[i]; j++)
                    {
                        // 現在動いていない女性のみ
                        if (!m_girlMoveFlag[i, j])
                        {
                            // 動かす
                            m_girlMoveFlag[i, j] = true;
                            m_girl[i, j].SetActive(true);  // アクティブ化

                            // スコア加算
                            m_scoreCount[i]++;
                            m_scoreText[i].enabled = true;
                            m_scoreText[i].text = "？人";

                            break;
                        }
                    }
                }
            }

            m_girlColumnCnt++;

            // 列の最大人数になったら人数を固定
            if (m_girlColumnCnt > girlColumnMax)
            {
               m_girlColumnCnt = girlColumnMax;
            }
        }
        

        
        // 女性を動かす
        for (i = 0; i < ConstPlayerMax; i++)
        {
            for (j = 0; j <m_score[i]; j++)
            {
                // 動かすフラグが立っていたら
                if (m_girlMoveFlag[i, j])
                {
                    // 進める
                    m_girlLerpRate[i, j] += girlMoveSpeed;

                    // 目的地まで到達したらそこに固定
                    if (m_girlLerpRate[i, j] >= 1.0f)
                    { 
                        m_girlLerpRate[i, j] = 1.0f;
                    }

                    // 女性の移動
                    m_girl[i, j].transform.position = Vector3.Lerp(m_company[i].transform.position,
                                                                   m_girlGoalPosition[i, j],
                                                                   m_girlLerpRate[i, j]);
                }
            }
        }

        // 最後の女性の移動が完了したら次の状態に遷移
        if(m_girlLerpRate[m_saveTopPlayer, m_score[m_saveTopPlayer]-1] >= 1.0f)
        {
            resultState = ResultState.UpPodiumProduction;
            time = 0;
        }
    }

    /// <summary>
    /// 表彰台が上下する演出
    /// </summary>
    void _UpPodiumProduction()
    {
        // ドラムロールSE再生
        if(!se023_startFlag)
        {
            BGMManager.instance.PlaySE("se023_BeforeDecideRank", 1.0f);
            se023_startFlag = true;
        }

        // 時間経過
        time++;

        
        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 一定時間経過すると停止する
            if(time > podiumProdcutionTime)
            {
                podiumState[i] = PodiumState.Stop;
            }

            // 状態別の速度の処理
            switch (podiumState[i])
            {
                // 上昇中
                case PodiumState.Up:
                    // だんだん早くする
                    m_podiumSpeed[i] += ConstPodiumAccel;
                    if(m_podiumSpeed[i] > podiumSeedMax[i])
                    {
                        m_podiumSpeed[i] = podiumSeedMax[i];
                    }
                    break;

                // 上昇中
                case PodiumState.Down:
                    m_podiumSpeed[i] -= ConstPodiumAccel;
                    if (m_podiumSpeed[i] < -podiumSeedMax[i])
                    {
                        m_podiumSpeed[i] = -podiumSeedMax[i];
                    }
                    break;

                // 停止
                case PodiumState.Stop:
                    // 速度を0に近づける
                    if(m_podiumSpeed[i] > 0.0f)
                    {
                        m_podiumSpeed[i] -= ConstPodiumAccel;
                        if (m_podiumSpeed[i] < 0.0f)
                        {
                            m_podiumSpeed[i] = 0.0f;
                        }
                    }
                    else
                    {
                        m_podiumSpeed[i] += ConstPodiumAccel;
                        if (m_podiumSpeed[i] > 0.0f)
                        {
                            m_podiumSpeed[i] = 0.0f;
                        }
                    }
                    break;
            }

            // 一定の地点まで到達したら移動方向を変える
            if ( (m_podium[i].transform.position.y > 0.2f && podiumState[i] == PodiumState.Up) || (m_podium[i].transform.position.y < -1.0f && podiumState[i] == PodiumState.Down))
            {
                // 動きの反転
                if(podiumState[i] == PodiumState.Up)
                {
                    podiumState[i] = PodiumState.Down;
                }
                else
                {
                    podiumState[i] = PodiumState.Up;
                }
            }

            // オブジェクトの移動
            m_podium[i].transform.Translate(0.0f, m_podiumSpeed[i], 0.0f);

            // 数字のランダムで表示
            m_scoreText[i].text = Random.Range(10, 99 + 1).ToString() + "人";
        }

        

        if (time > podiumProdcutionTime + 10)
        {
            resultState = ResultState.DecideRankProducution;
            time = 0;
        }


    }


    /// <summary>
    /// 表彰台の順位が確定する演出
    /// </summary>
    private void _DecideRankProduction()
    {
        // 時間経過
        time++;

        // スコアを表示する
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_scoreText[i].text = m_score[i].ToString() + "人";

            // 順位を表示する
            m_rankingText[i].enabled = true;
            m_rankingText[i].text = m_playerRanking[i] + 1 + "位";
        }


        // 一定時間待ってから演出を開始する
        if (time < prodiumProductionWaitTime)
        {
            // オブジェクトの位置を保存しておく
            for (i = 0; i < ConstPlayerMax; i++)
            {
                m_savePodiumStart[i] = m_podium[i].transform.position;
            }

            return;
        }

        // 順位決定音鳴らす
        if(!se024_startFlag)
        {
            BGMManager.instance.PlaySE("se024_DecideRank");
            se024_startFlag = true;
        }

        // BGM鳴らす
        if(time > 200)
        {
            if(!bgm003_startFlag)
            {
                BGMManager.instance.PlayBGM("bgm003_Confetti", 0.1f);
                bgm003_startFlag = true;
            }
        }
 
        // パーティクル開始
        m_confettiParticleParent.transform.position = m_podium[m_saveTopPlayer].transform.position;
        m_confettiParticleParent.transform.Translate(0.0f, 5.0f, 0.0f, Space.World);
        if (!m_confettiParticle1.isPlaying) // 開始中じゃなかったら
        {
            // パーティクル開始
            m_confettiParticle1.Play();
            m_confettiParticle2.Play();
            m_confettiParticle3.Play();
        }
       

        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 進める
            m_podiumLerp[i] += 0.02f;
            if(m_podiumLerp[i] > 1.0f)
            {
                m_podiumLerp[i] = 1.0f;
            }


            // 目標地点の設定
            float end_y = rank1PodiumPos.position.y - ((rank1PodiumPos.position.y - rank4PodiumPos.position.y) / 3) * m_playerRanking[i];
            Vector3 end_pos = new Vector3(m_podium[i].transform.position.x, end_y, m_podium[i].transform.position.z);

            // 線形補間で移動する
            m_podium[i].transform.position = Vector3.Lerp(m_savePodiumStart[i], end_pos, m_podiumLerp[i]);

        }




    }
     
   



}





// コルーチン
// unityチュートリアル　シューティング