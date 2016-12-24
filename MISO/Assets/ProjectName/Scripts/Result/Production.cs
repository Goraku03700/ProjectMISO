using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        WaitKey,
    };
    ResultState m_resultState;

    // 表彰台の状態
    private enum PodiumState
    {
        Up,
        Down,
        Stop,
    }
    PodiumState[] m_podiumState;

    private enum CrownKind
    {
        Gold = 0,
        Silver,
        Bronze,
        Max,
    };
    CrownKind m_crownKind;



    /// <summary>
    /// 定数定義
    /// </summary>
    const int ConstPlayerMax = 4;    // プレイヤーの人数
    const float ConstPodiumAccel = 0.5f;   // 表彰台の加速度

    /// <summary>
    /// 変数定義
    /// </summary>
    private int[] m_score;           // 4人のプレイヤーの得点を格納(受け取る)

    // 読み込むゲームオブジェクト
    private GameObject[] m_player;   // プレイヤー
    private GameObject[,] m_girl;    // 女性
    private GameObject[] m_podium;   // 表彰台
    private GameObject[] m_company;  // 会社

    // 読み込むテキスト
    private Text[] m_scoreText;      // スコアのテキスト
    //private Text[] m_rankingText;    // ランキングのテキスト

    private GameObject[] m_Crown;
    private float[] m_saveCrownPosY;

    // 読み込むパーティクル
    private GameObject[] m_confettiParticleParent;
    private ParticleSystem[] m_confettiParticle1;
    private ParticleSystem[] m_confettiParticle2;
    private ParticleSystem[] m_confettiParticle3;

    // 読み込むマテリアル
    private Renderer[] m_podiumMaterial;

    // 王冠
    private GameObject[] m_crownGold;
    private GameObject[] m_crownSilver;
    private GameObject[] m_crownBronze;


    // 計算用
    private int[] m_playerRanking;     // スコアのランキング
    private int m_saveTopPlayer;     // 1位のプレイヤーは誰か

    private float m_playerLerpRate;    // 線形補間の比率
    private Vector3[] m_playerStartPos;    // 開始位置保存用

    private Vector3[,] m_girlGoalPosition; // 女性の目標地点
    private bool[,] m_girlMoveFlag;  // 女性が前に進むフラグ
    private float[,] m_girlLerpRate;  // 線形補間用
    private int m_girlColumnCnt;    // 女性が会社から出てくる人数

    private int[] m_scoreCount;       // スコアカウント用

    private int i, j, k;       // 添え字
    private int time;       // 経過時間カウント用

    private float m_intervalTime;

    private float[] m_podiumSpeed;       // 表彰台の上下の移動速度
    private Vector3[] m_savePodiumStart;   // 位置を保存 
    private float[] m_podiumLerp;        // 線形補間用
    private Color m_savePodiumColor;    // 表彰台の色の保存
    private float m_podiumColorLerp;    // 表彰台の色の線形補間用

    private ParticleSystem m_fireWorkParticleRed;
    private ParticleSystem m_fireWorkParticleBlue;


    private bool se023_startFlag;
    private bool se024_startFlag;
    private bool bgm003_startFlag;

    [SerializeField]
    Texture tex;

    /// <summary>
    /// インスペクタに表示する変数
    /// </summary>

    [SerializeField]
    private bool isFade;    // フェードするか

    [SerializeField]
    private GameObject m_canvasObject;

    [SerializeField]
    private Transform m_girlColumnStartMaker;    // 女性の行列のスタート地点

    [SerializeField]
    private float m_girlLineInterval;            // 女性の間隔(行)

    [SerializeField]
    private float m_girlColumnInerver;            // 女性の間隔(列)

    [SerializeField]
    private GameObject m_girlPrefab; // 女性のオブジェクトのプレハブ

    [SerializeField]
    private Transform m_playerEndMaker;     // プレイヤーの移動する終着点

    [SerializeField]
    private float m_playerMoveSpeed;  // プレイヤーの移動速度

    [SerializeField]
    private float m_girlMoveSpeed;   // 女性の移動速度

    [SerializeField]
    private float m_girlLineIntervalTime;   // 行の間隔

    [SerializeField]
    private int m_girlColumnMax;     // 女性の列の最大数

    [SerializeField]
    private GameObject m_podiumPrefab;    // 表彰台のプレハブ

    [SerializeField]
    private float[] m_podiumSeedMax;      // 上下するスピード

    [SerializeField]
    private float m_podiumUpDownProdcutionTime;   // 上下する時間

    [SerializeField]
    private float m_podiumProductionWaitTime;     // 上下してから少し待つ時間

    [SerializeField]
    private float m_podiumDecideRankWaitTime;     // 順位が表示されてから待つ時間

    [SerializeField]
    private float m_podiumDecideRankSpeed;        // 順位決定後の上下する速度

    [SerializeField]
    private Transform m_rank1PodiumPos;       // 1位の位置

    [SerializeField]
    private Transform m_rank4PodiumPos;       // 最下位の位置

    [SerializeField]
    private Color[] m_rankColor;

    [SerializeField]
    private float m_changeColorSpeed;

    [SerializeField]
    private GameObject m_crownGoldObj;

    [SerializeField]
    private GameObject m_crownSilverObj;

    [SerializeField]
    private GameObject m_crownBronzeObj;


    [SerializeField]
    private Transform[] m_crownPosX;

    [SerializeField]
    private Light m_directionalLight;

    [SerializeField]
    private float m_darkSpeed;

    [SerializeField]
    private float m_endIntensity;

    [SerializeField]
    private Light[] m_lightShaft;

    [SerializeField]
    private Light[] m_pointLight;

    private Fade m_fadeObject;

    private int m_maxGirl;

    // Use this for initialization
    void Start() {

        // 配列の確保
        m_score = new int[ConstPlayerMax];

        if (SceneSharedData.instance != null)
        {

            m_score[0] = SceneSharedData.instance.Get<int>("PlayTest", "Player1Score");
            m_score[1] = SceneSharedData.instance.Get<int>("PlayTest", "Player2Score");
            m_score[2] = SceneSharedData.instance.Get<int>("PlayTest", "Player3Score");
            m_score[3] = SceneSharedData.instance.Get<int>("PlayTest", "Player4Score");

            SceneSharedData.instance.Remove("PlayTest", "Player1Score");
            SceneSharedData.instance.Remove("PlayTest", "Player2Score");
            SceneSharedData.instance.Remove("PlayTest", "Player3Score");
            SceneSharedData.instance.Remove("PlayTest", "Player4Score");
        }
        else
        {
            m_score[0] = 20;
            m_score[1] = 25;
            m_score[2] = 25;
            m_score[3] = 20;

        }





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
        /*
        m_rankingText = new Text[ConstPlayerMax];
        m_rankingText[0] = GameObject.Find("RankingText01").GetComponent<Text>();
        m_rankingText[1] = GameObject.Find("RankingText02").GetComponent<Text>();
        m_rankingText[2] = GameObject.Find("RankingText03").GetComponent<Text>();
        m_rankingText[3] = GameObject.Find("RankingText04").GetComponent<Text>();
        */

        // パーティクルシステム
        /*
        m_confettiParticleParent = GameObject.Find("ConfettiParticle");
        m_confettiParticle1 = GameObject.Find("ConfettiParticle1").GetComponent<ParticleSystem>();
        m_confettiParticle2 = GameObject.Find("ConfettiParticle2").GetComponent<ParticleSystem>();
        m_confettiParticle3 = GameObject.Find("ConfettiParticle3").GetComponent<ParticleSystem>();
        m_confettiParticle1.Stop();
        m_confettiParticle2.Stop();
        m_confettiParticle3.Stop();
        */

        // パーティクルのオブジェクトを探す
        m_confettiParticleParent = new GameObject[ConstPlayerMax];
        m_confettiParticle1 = new ParticleSystem[ConstPlayerMax];
        m_confettiParticle2 = new ParticleSystem[ConstPlayerMax];
        m_confettiParticle3 = new ParticleSystem[ConstPlayerMax];
        for (i = 0; i < ConstPlayerMax; i++)
        {
            string findObj = "ConfettiParticle" + (i+1);
            m_confettiParticleParent[i] = GameObject.Find(findObj);
            string findObj2 = findObj + "_1";
            m_confettiParticle1[i] = GameObject.Find(findObj2).GetComponent<ParticleSystem>();
            m_confettiParticle2[i] = GameObject.Find(findObj + "_2").GetComponent<ParticleSystem>();
            m_confettiParticle3[i] = GameObject.Find(findObj + "_3").GetComponent<ParticleSystem>();
            m_confettiParticle1[i].Stop();
            m_confettiParticle2[i].Stop();
            m_confettiParticle3[i].Stop();
        }

        // 王冠
        /*
        m_Crown = new GameObject[(int)CrownKind.Max];
        m_Crown[(int)CrownKind.Gold] = GameObject.Find("CrownGold");
        m_Crown[(int)CrownKind.Silver] = GameObject.Find("CrownSilver");
        m_Crown[(int)CrownKind.Bronze] = GameObject.Find("CrownBronze");
        m_Crown[(int)CrownKind.Gold].SetActive(false);
        m_Crown[(int)CrownKind.Silver].SetActive(false);
        m_Crown[(int)CrownKind.Bronze].SetActive(false);
        */

        m_crownGold = new GameObject[ConstPlayerMax];
        m_crownSilver = new GameObject[ConstPlayerMax];
        m_crownBronze = new GameObject[ConstPlayerMax];
        m_saveCrownPosY = new float[(int)CrownKind.Max];

        for(i=0; i<ConstPlayerMax; i++)
        {
            // オブジェクトの生成
            m_crownGold[i] = Instantiate(m_crownGoldObj);
            m_crownSilver[i] = Instantiate(m_crownSilverObj);
            m_crownBronze[i] = Instantiate(m_crownBronzeObj);

            // キャンバスの中に入れる
            m_crownGold[i].transform.SetParent(m_canvasObject.transform);
            m_crownSilver[i].transform.SetParent(m_canvasObject.transform);
            m_crownBronze[i].transform.SetParent(m_canvasObject.transform);

            // scale
            m_crownGold[i].transform.localScale = m_crownGoldObj.transform.localScale;
            m_crownSilver[i].transform.localScale = m_crownSilverObj.transform.localScale;
            m_crownBronze[i].transform.localScale = m_crownBronzeObj.transform.localScale;


            // 非表示
            m_crownGold[i].SetActive(false);
            m_crownSilver[i].SetActive(false);
            m_crownBronze[i].SetActive(false);

        }

        // 王冠の高さ保存
        m_saveCrownPosY = new float[(int)CrownKind.Max];
        m_saveCrownPosY[(int)CrownKind.Gold] = m_crownGoldObj.transform.localPosition.y;
        m_saveCrownPosY[(int)CrownKind.Silver] = m_crownSilverObj.transform.localPosition.y;
        m_saveCrownPosY[(int)CrownKind.Bronze] = m_crownBronzeObj.transform.localPosition.y;

        // 非表示
        m_crownGoldObj.SetActive(false);
        m_crownSilverObj.SetActive(false);
        m_crownBronzeObj.SetActive(false);

        // 花火パーティクル
        m_fireWorkParticleRed  = GameObject.Find("FireWorkRed").GetComponent<ParticleSystem>();
        m_fireWorkParticleBlue = GameObject.Find("FireWorkBlue").GetComponent<ParticleSystem>();

        m_fireWorkParticleRed.Stop();
        m_fireWorkParticleBlue.Stop();

        
        // マテリアル読み込み
        m_podiumMaterial = new Renderer[ConstPlayerMax];
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_podiumMaterial[i] = m_podium[i].GetComponent<Renderer>();
        }

        // テキストは最初表示しない
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_scoreText[i].enabled = false;
            //m_rankingText[i].enabled = false;
        }

        // プレイヤーの順位を判定
        m_playerRanking = new int[ConstPlayerMax];

        bool same_flg = false;
        int same_cnt = 0;
        int[] tmp_score;   // 一時保存用
        tmp_score = new int[ConstPlayerMax];


        m_score.CopyTo(tmp_score, 0);       // 配列のコピー
        System.Array.Sort(tmp_score);       // 昇順にソート
        System.Array.Reverse(tmp_score);    // 反転して降順にする
        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 順位の確定
            m_playerRanking[i] = System.Array.IndexOf(tmp_score, m_score[i]);

            // 同じとき
            if(i >= 1)
            {
                if(m_player[i] == m_player[i-1])
                {
                    same_flg = true;
                    same_cnt++;
                    continue;
                }
            }

            if(same_flg)
            {
                m_playerRanking[i] += same_cnt;
                same_flg = false;
            }
        }

        int max = m_score.Max();   // スコアの最大値をとる
        m_saveTopPlayer = System.Array.IndexOf(m_score, max);   // 最大値の配列が何番目か保存
        m_maxGirl = max;
    
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
        m_girl = new GameObject[ConstPlayerMax, max];
        m_girlMoveFlag = new bool[ConstPlayerMax, max];
        m_girlGoalPosition = new Vector3[ConstPlayerMax, max];
        m_girlLerpRate = new float[ConstPlayerMax, max];


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
                m_girl[i, j] = Instantiate(m_girlPrefab);

                // 非アクティブ状態
                m_girl[i, j].SetActive(false);


                // 目標地点を設定する
                // 整列ver
                //m_womenGoalPosition[i, j] = womenColumnStartMaker.position;
                //m_womenGoalPosition[i, j].x = m_player[i].transform.position.x - (m_player[0].transform.position.x - womenColumnStartMaker.position.x) + (j / 8 * 0.5f);
                //m_womenGoalPosition[i, j] += new Vector3(0.0f, 0.0f, j%8 * 0.5f);

                // ピラミッドver
                m_girlGoalPosition[i, j] = m_girlColumnStartMaker.position;
                //m_girlGoalPosition[i, j].x = m_player[i].transform.position.x - (m_player[0].transform.position.x - womenColumnStartMaker.position.x) - (offset_x / 2 * (column_max - 1)) + (k * offset_x);
                m_girlGoalPosition[i, j].y = m_player[i].transform.position.y;
                m_girlGoalPosition[i, j].x = m_player[i].transform.position.x - (m_girlColumnInerver / 2 * (nowColumn_max - 1)) + (k * m_girlLineInterval);
                m_girlGoalPosition[i, j] += new Vector3(0.0f, 0.0f, (line_cnt - 1) * m_girlLineInterval);

                k++;
                if (k + 1 > nowColumn_max)
                {
                    k = 0;
                    nowColumn_max++;
                    if (nowColumn_max > m_girlColumnMax) nowColumn_max = m_girlColumnMax;
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
        m_podiumSpeed = new float[ConstPlayerMax];
        m_podiumState = new PodiumState[ConstPlayerMax];
        m_podiumLerp = new float[ConstPlayerMax];

        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_podiumSpeed[i] = 0.0f;
            m_podiumState[i] = PodiumState.Up;
            m_podiumSpeed[i] = m_podiumSeedMax[i];
            m_podiumLerp[i] = 0.0f;
        }
        m_savePodiumColor = m_podiumMaterial[0].material.GetColor("_EmissionColor");
        m_podiumColorLerp = 0.0f;

        // lightShaftの初期化
        for (i = 0; i < ConstPlayerMax; i++)
        {
           m_lightShaft[i].enabled = false;
           m_lightShaft[i].range = 0.0f;
        }
        for(i=0; i<ConstPlayerMax; i++)
        {
            m_pointLight[i].enabled = false;
        }

        // 演出01を初期の状態に設定
        m_resultState = ResultState.PutCompanyProduction;

        time = 0;

        m_girlColumnCnt = 1;

        m_intervalTime = 0.0f;

        se023_startFlag = false;
        se024_startFlag = false;
        bgm003_startFlag = false;

        m_fadeObject = GameObject.FindObjectOfType<Fade>();

    }

    // Update is called once per frame
    void Update() {



        switch (m_resultState)
        {
            // 会社を置く演出
            case ResultState.PutCompanyProduction:
                if (isFade)
                {
                    if (m_fadeObject.FadeEnd() || m_fadeObject == null)
                    {
                        PutCompanyProduction();
                    }
                }else
                {
                    PutCompanyProduction();
                }
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

            // 入力待ち
            case ResultState.WaitKey:
                _WaitKey();
                break;
        }



    }


    /// <summary>
    /// 演出01
    /// 会社を置く演出
    /// </summary>
    private void PutCompanyProduction()
    {
        BGMManager.instance.StopBGM(0.0f);

        BGMManager.instance.PlaySE("se_ResultStart_1");
        
        // 親子関係を解除する(その場に置く)
        for (i = 0; i < ConstPlayerMax; i++)
        {
            //m_player[i].transform.DetachChildren();
        }

        // 演出02に遷移する
        m_resultState = ResultState.MovePlayerProduction;
    }


    /// <summary>
    /// 演出02
    /// プレイヤーが前に走ってくる演出
    /// </summary>
    private void _MovePlayerProduction()
    {

        // 線形補間の比率を上げる
        m_playerLerpRate += m_playerMoveSpeed * Time.deltaTime;

        // 4キャラ全員を前に移動させる
        for (i = 0; i < ConstPlayerMax; i++)
        {

            // 歩きモーションにする
            if(!m_player[i].GetComponent<Animator>().GetBool("isWalk"))
            {
                m_player[i].GetComponent<Animator>().SetBool("isWalk", true);
            }

            // 終着点設定
            Vector3 end = new Vector3(m_podium[i].transform.position.x,
                                      m_podium[i].transform.position.y,
                                      m_playerEndMaker.position.z);

            // 線形補間で前に移動する
            Vector3 start = m_playerStartPos[i];
            m_podium[i].transform.position = Vector3.Lerp(start, end, m_playerLerpRate);
        }

        // 終着点に着いたら次の演出に遷移
        if (m_playerLerpRate >= 1.0f)
        {
            m_resultState = ResultState.MoveGirlProduction;

            // 全員のモーション止める
            for (i = 0; i < ConstPlayerMax; i++)
            {
                m_player[i].GetComponent<Animator>().SetBool("isWalk", false);
            }

            // seとめる
            BGMManager.instance.StopSE();
        }




    }

    /// <summary>
    /// 演出03
    /// 置いた会社から女性が出てくる演出
    /// </summary>
    private void _MoveGirlProduction()
    {
        m_intervalTime += Time.deltaTime;

        // 一定時間ごとに女性を会社から出す
        if (m_intervalTime > m_girlLineIntervalTime)
        {
            m_intervalTime = 0;

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


                            // アニメーション開始
                            m_girl[i, j].GetComponent<Animator>().SetBool("isWalk", true);

                            break;
                        }
                    }


                    // SE再生
                    BGMManager.instance.PlaySE("se025_NPCGoPlayer");
                }

                m_scoreText[i].enabled = true;
                m_scoreText[i].text = "？人";

            }

            m_girlColumnCnt++;

            // 列の最大人数になったら人数を固定
            if (m_girlColumnCnt > m_girlColumnMax)
            {
                m_girlColumnCnt = m_girlColumnMax;
                m_intervalTime = 0.0f;
            }
        }



        // 女性を動かす
        for (i = 0; i < ConstPlayerMax; i++)
        {
            for (j = 0; j < m_score[i]; j++)
            {
                // 動かすフラグが立っていたら
                if (m_girlMoveFlag[i, j])
                {
                    // 進める
                    m_girlLerpRate[i, j] += m_girlMoveSpeed * Time.deltaTime;

                    // 目的地まで到達したらそこに固定
                    if (m_girlLerpRate[i, j] >= 1.0f)
                    {
                        m_girlLerpRate[i, j] = 1.0f;

                        // モーション止める
                        m_girl[i, j].GetComponent<Animator>().SetBool("isWalk", false);

                    }

                    // 女性の移動
                    Vector3 start = new Vector3(m_company[i].transform.position.x,
                                                   m_player[i].transform.position.y,
                                                   m_company[i].transform.position.z);
                    m_girl[i, j].transform.position = Vector3.Lerp(start,
                                                                   m_girlGoalPosition[i, j],
                                                                   m_girlLerpRate[i, j]);
                }
            }
        }

        // 最後の女性の移動が完了したら次の状態に遷移
        if (m_maxGirl == 0) // 0人の場合
        {
            m_resultState = ResultState.UpPodiumProduction;
            time = 0;

            return;

        }

        if (m_girlLerpRate[m_saveTopPlayer, m_score[m_saveTopPlayer] - 1 ] >= 1.0f)
        {
            m_resultState = ResultState.UpPodiumProduction;
            time = 0;

            // ライトの位置を保存(2位と3位の位置にライトを移動)
            m_lightShaft[0].transform.localPosition = new Vector3(m_podium[1].transform.localPosition.x,
                                                                  m_lightShaft[0].transform.localPosition.y,
                                                                  m_podium[1].transform.localPosition.z);

            m_lightShaft[1].transform.localPosition = new Vector3(m_podium[2].transform.localPosition.x,
                                                                  m_lightShaft[1].transform.localPosition.y,
                                                                  m_podium[2].transform.localPosition.z);
        }
    }

    /// <summary>
    /// 表彰台が上下する演出
    /// </summary>
    void _UpPodiumProduction()
    {
        // ドラムロールSE再生
        if (!se023_startFlag)
        {
            BGMManager.instance.PlaySE("se023_BeforeDecideRank", 1.0f);
            se023_startFlag = true;
        }

        // 時間経過
        m_intervalTime += Time.deltaTime;

        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 一定時間経過すると停止する
            if(m_intervalTime > m_podiumUpDownProdcutionTime)
            {
                m_podiumState[i] = PodiumState.Stop;
            }

            // 状態別の速度の処理
            switch (m_podiumState[i])
            {
                // 上昇中
                case PodiumState.Up:
                    // だんだん早くする
                    m_podiumSpeed[i] += ConstPodiumAccel;
                    if (m_podiumSpeed[i] > m_podiumSeedMax[i])
                    {
                        m_podiumSpeed[i] = m_podiumSeedMax[i];
                    }
                    break;

                // 上昇中
                case PodiumState.Down:
                    m_podiumSpeed[i] -= ConstPodiumAccel;
                    if (m_podiumSpeed[i] < -m_podiumSeedMax[i])
                    {
                        m_podiumSpeed[i] = -m_podiumSeedMax[i];
                    }
                    break;

                // 停止
                case PodiumState.Stop:
                    // 速度を0に近づける
                    if (m_podiumSpeed[i] > 0.0f)
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
            if ((m_podium[i].transform.position.y > 0.2f && m_podiumState[i] == PodiumState.Up) || (m_podium[i].transform.position.y < -0.5f && m_podiumState[i] == PodiumState.Down))
            {
                // 動きの反転
                if (m_podiumState[i] == PodiumState.Up)
                {
                    m_podiumState[i] = PodiumState.Down;
                }
                else
                {
                    m_podiumState[i] = PodiumState.Up;
                }
            }

            // オブジェクトの移動
            m_podium[i].transform.Translate(0.0f, m_podiumSpeed[i] * Time.deltaTime, 0.0f);

            // 数字のランダムで表示
            m_scoreText[i].enabled = true;
            m_scoreText[i].text = Random.Range(10, 99 + 1).ToString() + "人";
        }


        // 暗くする
        this.GetComponent<SkyboxControl>().Dark();
        m_directionalLight.intensity -= m_darkSpeed * Time.deltaTime;
        if(m_directionalLight.intensity < m_endIntensity)
        {
            m_directionalLight.intensity = m_endIntensity;
        }

        // ライトを動かす
        Vector3 shaft_rot = m_lightShaft[0].transform.eulerAngles;

        // ライト1つ目
        float rot_x = Mathf.PingPong(Time.time * 60.0f, 90.0f + 30.0f) + 30.0f;
        shaft_rot.x = rot_x;
        shaft_rot.y = 90.0f;
        shaft_rot.z = 90.0f;
        m_lightShaft[0].transform.eulerAngles = shaft_rot;
        m_lightShaft[0].range = 10.0f;
        m_lightShaft[0].enabled = true;

        // ライト2つ目
        shaft_rot = m_lightShaft[1].transform.eulerAngles;
        rot_x = Mathf.PingPong(Time.time * 60.0f + 90.0f + 30.0f, 90.0f + 30.0f) + 30.0f;
        shaft_rot.x = rot_x;
        shaft_rot.y = 90.0f;
        shaft_rot.z = 90.0f;
        m_lightShaft[1].transform.eulerAngles = shaft_rot;
        m_lightShaft[1].range = 10.0f;
        m_lightShaft[1].enabled = true;


        // 一定時間たったら次の時間に決める
        if (m_intervalTime > m_podiumProductionWaitTime)
        {
            m_resultState = ResultState.DecideRankProducution;
            time = 0;
            m_intervalTime = 0.0f;

            // オブジェクトの位置を保存しておく
            for (i = 0; i < ConstPlayerMax; i++)
            {
                m_savePodiumStart[i] = m_podium[i].transform.position;
            }

            // ライトの角度戻す
            m_lightShaft[0].transform.localRotation = Quaternion.Euler(90.0f, 90.0f, 90.0f);
            m_lightShaft[1].transform.localRotation = Quaternion.Euler(90.0f, 90.0f, 90.0f);

            // ライト消す
            m_lightShaft[0].range = 0.0f;
            m_lightShaft[1].range = 0.0f;


        }

    }


    /// <summary>
    /// 表彰台の順位が確定する演出
    /// </summary>
    private void _DecideRankProduction()
    {
        // 時間経過
        m_intervalTime += Time.deltaTime;

        // スコアを表示する
        for (i = 0; i < ConstPlayerMax; i++)
        {
            m_scoreText[i].text = m_score[i].ToString() + "人";

            // 順位を表示する
            //m_rankingText[i].enabled = true;
            //m_rankingText[i].text = m_playerRanking[i] + 1 + "位";
        }

        
        // 一定時間待ってから演出を開始する
        if (m_intervalTime < m_podiumDecideRankWaitTime)
        {
            return;
        }


        
        // 順位決定音鳴らす
        if (!se024_startFlag)
        {
            BGMManager.instance.PlaySE("se024_DecideRank");
            se024_startFlag = true;
        }

        // BGM鳴らす
        if (m_intervalTime > 3.0f)
        {
            if (!bgm003_startFlag)
            {
                BGMManager.instance.PlayBGM("bgm002_Result", 0.1f);
                bgm003_startFlag = true;
            }
        }

        // 花火パーティクル開始
        if (!m_fireWorkParticleRed.isPlaying)
        {
            m_fireWorkParticleRed.Play();
            m_fireWorkParticleBlue.Play();
        }
        // 順位別の高さに移動
        for (i = 0; i < ConstPlayerMax; i++)
        {
            // 1位ならパーティクル開始
            if (m_playerRanking[i] == 0)
            {
                m_confettiParticleParent[i].transform.position = m_podium[i].transform.position;
                m_confettiParticleParent[i].transform.Translate(0.0f, 5.0f, 0.0f, Space.World);
                if (!m_confettiParticle1[i].isPlaying) // 開始中じゃなかったら
                {
                    // パーティクル開始
                    m_confettiParticle1[i].Play();
                    m_confettiParticle2[i].Play();
                    m_confettiParticle3[i].Play();
                }
            }

            // 進める
            m_podiumLerp[i] += m_podiumDecideRankSpeed * Time.deltaTime;
            if (m_podiumLerp[i] > 1.0f)
            {
                m_podiumLerp[i] = 1.0f;

                // アニメーション開始
                if (m_playerRanking[i] == 0)
                {
                    m_player[i].GetComponent<Animator>().SetBool("isGlad", true);
                }

                if (m_playerRanking[i] != 0)
                {
                    m_player[i].GetComponent<Animator>().SetBool("isSad", true);
                }

            }
            // 目標地点の設定
            float end_y = m_rank1PodiumPos.position.y - ((m_rank1PodiumPos.position.y - m_rank4PodiumPos.position.y) / 3) * m_playerRanking[i];
            Vector3 end_pos = new Vector3(m_podium[i].transform.position.x, end_y, m_podium[i].transform.position.z);

            // 線形補間で移動する
            m_podium[i].transform.position = Vector3.Lerp(m_savePodiumStart[i], end_pos, m_podiumLerp[i]);

            // 表彰台の色の変更
            //m_podium[i].GetComponent<Renderer>().material.color = m_rankColor[m_playerRanking[i]];
            //m_podiumMaterial[i].material.EnableKeyword("_EMISSION");

            //m_podiumMaterial[i].material.SetColor("_EmissionColor", m_rankColor[m_playerRanking[i]]);

            m_podiumMaterial[i].material.SetColor("_EmissionColor", Color.Lerp(m_savePodiumColor, m_rankColor[m_playerRanking[i]], m_podiumColorLerp));

            // 王冠の位置を決める
            switch (m_playerRanking[i])
            {
                case 0:
                    m_crownGold[i].SetActive(true);
                   /***/ m_crownGold[i].transform.localPosition = new Vector3(/*m_scoreText[i].transform.position.x*/m_crownPosX[i].localPosition.x,
                                                                                       m_saveCrownPosY[(int)CrownKind.Gold],
                                                                                       0.0f);
                    break;

                case 1:
                    m_crownSilver[i].SetActive(true);
                    m_crownSilver[i].transform.localPosition = new Vector3(/*m_scoreText[i].transform.position.x*/m_crownPosX[i].localPosition.x,
                                                                                         m_saveCrownPosY[(int)CrownKind.Silver],
                                                                                         0.0f);
                    break;

                case 2:
                    m_crownBronze[i].SetActive(true);
                    m_crownBronze[i].transform.localPosition = new Vector3(/*m_scoreText[i].transform.position.x*/m_crownPosX[i].localPosition.x,
                                                                                       m_saveCrownPosY[(int)CrownKind.Bronze],
                                                                                       0.0f);
                    break;
            }

            // ポイントライトを置く
            m_pointLight[i].enabled = true;
            m_pointLight[i].transform.localPosition = m_player[i].transform.position;

            // ライトの演出
            if (m_playerRanking[i] == 0)
            {
                m_lightShaft[i].transform.localPosition = new Vector3(m_podium[i].transform.localPosition.x,
                                                               m_lightShaft[i].transform.localPosition.y,
                                                               m_podium[i].transform.localPosition.z);

                // 回転もどす

                m_lightShaft[i].range += 10.0f * Time.deltaTime;
                if (m_lightShaft[i].range > 10.0f)
                {
                    m_lightShaft[i].range = 10.0f;
                }
            }
        }

        m_podiumColorLerp += m_changeColorSpeed * Time.deltaTime;




        if (m_intervalTime > 5)
        {
            m_resultState = ResultState.WaitKey;
        }
    }

    /// <summary>
    /// キーの入力待ち状態
    /// </summary>
    private void _WaitKey()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Fade.ChangeScene("Title");
        }

        if(MultiInput.GetButtonDown("Throw", MultiInput.JoypadNumber.Pad1) ||
            MultiInput.GetButtonDown("Throw", MultiInput.JoypadNumber.Pad2) ||
             MultiInput.GetButtonDown("Throw", MultiInput.JoypadNumber.Pad3) ||
              MultiInput.GetButtonDown("Throw", MultiInput.JoypadNumber.Pad4))
        {
            Fade.ChangeScene("Title");
        }


    }

      




}



// deltatime使う

// Fadeコメントアウト中360

// コルーチン
// unityチュートリアル　シューティング