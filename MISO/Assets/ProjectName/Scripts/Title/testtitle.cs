using UnityEngine;
using System.Collections;

public class testtitle : MonoBehaviour {

    //タイトルの演出シーンの状態
    enum TitleState
    {
        Start,              //最初の状態
        Ribbon,             //リボンの挙動
        TrailTitleLogo,     //リボンでロゴを引きずる
        DisplayTitleLogo,   //所定の位置にロゴが来てUIも表示する
        Finish,             //ロゴとUIが表示されている状態
    }
    TitleState m_titlestate;    //シーンの状態の変数
    TitleGirl m_girl;
    TitleRibbon m_ribbon;
    TitleLogo m_titlelogo;
    TitleLogo m_titleui;

	// Use this for initialization
	void Start () {
        m_titlestate = TitleState.Start;
        m_girl = GameObject.Find("Girl").GetComponent<TitleGirl>();
        m_ribbon = GameObject.Find("ribbon2").GetComponent<TitleRibbon>();
        m_titlelogo = GameObject.Find("TitleLogo").GetComponent<TitleLogo>();
        m_titleui = GameObject.Find("TitleUI").GetComponent<TitleLogo>();
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
                if(m_ribbon.ThrowFinishFlag)    //リボンが所定の位置に着いたら遷移を移動させる
                {
                    m_titlelogo.Activate(true);
                    m_titlestate = TitleState.TrailTitleLogo;
                }
                break;

            case TitleState.TrailTitleLogo: //リボンを引きずる・女性を移動させる・タイトルロゴを引きずる
                //所定の位置に着いたら遷移を移動させる
                if(m_ribbon.TrailFinishFlag && m_girl.TrailGirlFinishFlag 
                   && m_titlelogo.TrailtitleLogoFinishFlag)
                {
                    m_titlestate = TitleState.DisplayTitleLogo;
                }
                else
                {
                    m_ribbon.TrailRibbon();
                    m_girl.MoveGirl();
                    m_titlelogo.TrailTitleLogo();
                }
                
                break;

            case TitleState.DisplayTitleLogo:   //UIを表示。
                m_titleui.Activate(true);
                if (Input.GetKeyDown(KeyCode.A))    //入力で次の遷移に。
                {
                    m_titlestate = TitleState.Finish;
                }
                break;

            case TitleState.Finish: //終了。ここで他の遷移に移動する
                m_titleui.Activate(false);
                Debug.Log("終了");
                break;
        }
	}
    

}
