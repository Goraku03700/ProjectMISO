using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using XInputDotNetPure;
/// <summary>
/// 主なタイトルのクラス
/// </summary>
public class testtitle : MonoBehaviour {

    [SerializeField]
    Canvas m_fadeCanvas;

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
    [SerializeField]
    Animator m_titlePlayerAnimator; 

    // BGM・SE用
    private bool bgm000_startFlag;
    private bool se001_startFlag;
    private bool se017_startFlag;

	// Use this for initialization
	void Start () {
        m_titlestate = TitleState.Start;
        m_girl = GameObject.Find("girl").GetComponent<TitleGirl>();
        m_ribbon = GameObject.Find("ribbon_circle").GetComponent<TitleRibbon>();
        m_titlelogo = GameObject.Find("TitleLogo").GetComponent<TitleLogo>();
        m_titleui = GameObject.Find("TitleUI").GetComponent<TitleUI>();
      //  m_invisibleribbon = GameObject.Find("ribbon2/obj1").GetComponent<InvisibleRibbon>();
        m_fadeCanvas.gameObject.SetActive(true);
        bgm000_startFlag = false;
        se001_startFlag = false;
        se017_startFlag = false;

        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {

        XInputDotNetPure.GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
        XInputDotNetPure.GamePadState pad2 = GamePad.GetState(PlayerIndex.Two);
        XInputDotNetPure.GamePadState pad3 = GamePad.GetState(PlayerIndex.Three);
        XInputDotNetPure.GamePadState pad4 = GamePad.GetState(PlayerIndex.Four);

        switch (m_titlestate)
        {
            case TitleState.Start:  //タイトル開始時
                m_titlestate = TitleState.Ribbon;
                BGMManager.instance.StopBGM(0.0f);
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
                    m_titlePlayerAnimator.SetBool("Running", true);
                    m_girl.RotationGirl();
                    m_titlestate = TitleState.TrailTitleLogo;
                }
                break;

            case TitleState.TrailTitleLogo: //リボンを引きずる・女性を移動させる・タイトルロゴを引きずる
                //所定の位置に着いたら遷移を移動させる
                if(m_ribbon.TrailFinishFlag && m_titlelogo.TrailtitleLogoFinishFlag)
                {
                    //m_invisibleribbon.ActiveRibbon(false);
                    m_ribbon.gameObject.SetActive(false);
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

                if (Input.GetKeyDown(KeyCode.A) || 
                    (pad1.Buttons.Start == ButtonState.Pressed) ||
                    (pad2.Buttons.Start == ButtonState.Pressed) ||
                    (pad3.Buttons.Start == ButtonState.Pressed) ||
                    (pad1.Buttons.Start == ButtonState.Pressed))   //入力で次の遷移に。
                {
                    m_titlestate = TitleState.Finish;
                    m_titleui.Activate(false);
                    Fade.ChangeScene("Opening");
                    BGMManager.instance.StopBGM(0.8f);
                    // 遷移SE鳴らす
                    if(!se017_startFlag)
                    {
                        BGMManager.instance.PlaySE("se017_DecideButton");
                        se017_startFlag = true;
                    }
                }
                break;

            case TitleState.Finish: //終了。ここで他の遷移に移動する
                Debug.Log("終了");
              //  BGMManager.instance.StopBGM(0.0f);
                
//                SceneManager.LoadScene("Select");
                break;
        }
	}
    

}
