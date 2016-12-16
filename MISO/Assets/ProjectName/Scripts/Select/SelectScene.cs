using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour {

    const int ConstPlayerMax = 4;

    private Text[] readyText;

    private float m_time;

    private bool[] m_readyFlag;

    private bool bgm004_startFlag;
    private bool[] se026_startFlag;

    private MultiInput.JoypadNumber[] m_joypadNumber;


    // Use this for initialization
    void Start () {

        bgm004_startFlag = false;
        se026_startFlag = new bool[ConstPlayerMax];

        for (int i = 0; i < ConstPlayerMax; i++)
        {
            se026_startFlag[i] = false;
        }

        m_readyFlag = new bool[ConstPlayerMax];
        for(int i=0; i<ConstPlayerMax; i++)
        {
            m_readyFlag[i] = false;
        }

        readyText = new Text[ConstPlayerMax];
        readyText[0] = GameObject.Find("ReadyText01").GetComponent<Text>();
        readyText[1] = GameObject.Find("ReadyText02").GetComponent<Text>();
        readyText[2] = GameObject.Find("ReadyText03").GetComponent<Text>();
        readyText[3] = GameObject.Find("ReadyText04").GetComponent<Text>();

        // はじめは空白
        for(int i=0; i<ConstPlayerMax; i++)
        {
            readyText[i].text = "";
            readyText[i].transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }

        m_time = 0.0f;

        // パッド設定
        m_joypadNumber = new MultiInput.JoypadNumber[ConstPlayerMax];
        m_joypadNumber[0] = MultiInput.JoypadNumber.Pad1;
        m_joypadNumber[1] = MultiInput.JoypadNumber.Pad2;
        m_joypadNumber[2] = MultiInput.JoypadNumber.Pad3;
        m_joypadNumber[3] = MultiInput.JoypadNumber.Pad4;

        BGMManager.instance.PlayBGM("bgm004_GameSelect", 1.0f);

    }

    // Update is called once per frame
    void Update () {

        // BGM鳴らす
        if(!bgm004_startFlag)
        {
            //BGMManager.instance.PlayBGM("bgm004_GameSelect", 1.0f);
            bgm004_startFlag = true;
        }

        /*
        for (int i = 0; i < ConstPlayerMax; i++)
        {
           
            if(MultiInput.GetButtonDown("Pause", m_joypadNumber[i]))
            {
                m_readyFlag[i] = true;
                readyText[i].text = "OK!";
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.Z) || MultiInput.GetButtonDown("Pause",MultiInput.JoypadNumber.Pad1))
        {
            m_readyFlag[0] = true;
            readyText[0].text = "OK!";

            GameObject.Find("PlayerCharacter1").GetComponent<Animator>().SetBool("isGlad", true);
        }


        if (Input.GetKeyDown(KeyCode.X) || MultiInput.GetButtonDown("Pause",MultiInput.JoypadNumber.Pad2))
        {
            m_readyFlag[1] = true;
            readyText[1].text = "OK!";

            GameObject.Find("PlayerCharacter2").GetComponent<Animator>().SetBool("isGlad", true);
        }

        if (Input.GetKeyDown(KeyCode.C) || MultiInput.GetButtonDown("Pause",MultiInput.JoypadNumber.Pad3))
        {
            m_readyFlag[2] = true;
            readyText[2].text = "OK!";

            GameObject.Find("PlayerCharacter3").GetComponent<Animator>().SetBool("isGlad", true);
        }

        if (Input.GetKeyDown(KeyCode.V) || MultiInput.GetButtonDown("Pause",MultiInput.JoypadNumber.Pad4))
        {
            m_readyFlag[3] = true;
            readyText[3].text = "OK!";

            GameObject.Find("PlayerCharacter4").GetComponent<Animator>().SetBool("isGlad", true);
        }




        // 全員分
        for (int i = 0; i < ConstPlayerMax; i++)
        {
            // レディーが押されていたら            
            if(m_readyFlag[i])
            {
                // SE鳴らす
                if(!se026_startFlag[i])
                {
                    BGMManager.instance.PlaySE("se026_PlayerPrepareFinish");
                    se026_startFlag[i] = true;
                }

                // だんだん大きくする
                readyText[i].transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                if(readyText[i].transform.localScale.x > 1.0f)
                {
                    readyText[i].transform.localScale = new Vector3(1.0f,1.0f, 1.0f);
                }                
            }
        }


        // シーン遷移
        if (m_readyFlag[0] && m_readyFlag[1] && m_readyFlag[2] && m_readyFlag[3])
        {
            // 文字を点滅させる
            //GameObject.Find("InfoText").GetComponent<InfoText>().FlashText();
            GameObject.Find("InfoText").GetComponent<FlashText>().DecideText();

            // 時間経過
            m_time += Time.deltaTime;

            // 1秒たったら遷移
            if (m_time > 1.0f)
            {
                BGMManager.instance.StopBGM(0.0f);
                Fade.ChangeScene("Tutorial");
            }
        }
        


        



    }
}
