using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
/// <summary>
/// 主なタイトルのクラス
/// </summary>
public class testtitle : MonoBehaviour {

    //タイトルの演出シーンの状態
    enum TitleState
    {
        Start,              //最初の状態
        Ribbon,             //リボンの挙動
        TrailTitleLogo,     //リボンでロゴを引きずる
        BoundTitleLogo,     //所定の位置にロゴが来てバウンドの処理
        DisplaytitleLogo,   //バウンド処理終了後UIを表示する
        Finish,             //ロゴとUIが表示されている状態
    }
    TitleState m_titlestate;    //シーンの状態の変数
    TitleGirl m_girl;
    TitleRibbon m_ribbon;
    InvisibleRibbon m_invisibleribbon;
    TitleLogo m_titlelogo;
    TitleUI m_titleui;

    // BGM・SE用
    private bool bgm000_startFlag;
    private bool se001_startFlag;
    private bool se017_startFlag;

	// Use this for initialization
	void Start () {
        m_titlestate = TitleState.Start;
        m_girl = GameObject.Find("girl").GetComponent<TitleGirl>();
        m_ribbon = GameObject.Find("ribbon2").GetComponent<TitleRibbon>();
        m_titlelogo = GameObject.Find("TitleLogo").GetComponent<TitleLogo>();
        m_titleui = GameObject.Find("TitleUI").GetComponent<TitleUI>();
        m_invisibleribbon = GameObject.Find("ribbon2/obj1").GetComponent<InvisibleRibbon>();

        bgm000_startFlag = false;
        se001_startFlag = false;
        se017_startFlag = false;
	}
	
	// Update is called once per frame
	void Update () {
        switch(m_titlestate)
        {
            case TitleState.Start:  //タイトル開始時
                m_titlestate = TitleState.Ribbon;
                break;

            case TitleState.Ribbon: //リボンを投げる
                m_ribbon.ThrowRibbon();

                // 投げるSE鳴らすse001_ThrowJustRibbon
                if(!se001_startFlag)
                {
                    BGMManager.instance.PlaySE("se001_ThrowJustRibbon");
                    se001_startFlag = true;
                }
                
                if(m_ribbon.ThrowFinishFlag)    //リボンが所定の位置に着いたら遷移を移動させる
                {
                    m_ribbon.ResetBezierRibbon();
                    m_girl.RotationGirl();
                    m_titlestate = TitleState.TrailTitleLogo;
                }
                break;

            case TitleState.TrailTitleLogo: //リボンを引きずる・女性を移動させる・タイトルロゴを引きずる
                //所定の位置に着いたら遷移を移動させる
                if(m_ribbon.TrailFinishFlag && m_titlelogo.TrailtitleLogoFinishFlag)
                {
                    m_invisibleribbon.ActiveRibbon(false);
                    m_titlestate = TitleState.BoundTitleLogo;
                }
                else
                {
                    m_ribbon.TrailRibbon();
                    m_girl.TrailGirl();
                    m_titlelogo.TrailTitleLogo();
                }
                
                break;

            case TitleState.BoundTitleLogo:     //ロゴをバウンドさせる
                m_titlelogo.BoundTitleLogo();
                if (m_titlelogo.TitleLogoBoundFinishFlag &&
                    m_titlelogo.TitleLogoTime() >= 5.0f)    //入力で次の遷移に
                {
                    m_titleui.Activate(true);
                    m_titlestate = TitleState.DisplaytitleLogo;
                }
                break;

            case TitleState.DisplaytitleLogo:   //UIを表示。
                m_titleui.ScaleTitleUI();

                // BGM鳴らす
                if(!bgm000_startFlag)
                {
                    BGMManager.instance.PlayBGM("bgm000_Title", 0.1f);
                    bgm000_startFlag = true;
                }

                if (Input.GetKeyDown(KeyCode.A))    //入力で次の遷移に。
                {
                    m_titlestate = TitleState.Finish;
                
                    // 遷移SE鳴らす
                    if(!se017_startFlag)
                    {
                        BGMManager.instance.PlaySE("se017_DecideButton");
                        se017_startFlag = true;
                    }
                }
                break;

            case TitleState.Finish: //終了。ここで他の遷移に移動する
                m_titleui.Activate(false);
                Debug.Log("終了");
              //  BGMManager.instance.StopBGM(0.0f);
                SceneManager.LoadScene("Select");
                break;
        }
	}
    

}
