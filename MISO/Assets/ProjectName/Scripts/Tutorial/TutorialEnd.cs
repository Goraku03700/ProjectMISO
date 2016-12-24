using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class TutorialEnd : MonoBehaviour {


    private bool[] m_readyFlag; // レディ押したか

    private Animator[] m_animePlayer;

    private TimeCount m_timeCountScript;

    private bool flg;

    private bool[] se026_startFlag;


    // Use this for initialization
    void Start () {


        m_readyFlag = new bool[4];
        m_animePlayer = new Animator[4];
        for (int i = 0; i < 4; i++)
        {
            m_readyFlag[i] = false;
            //m_animePlayer[i] = GameObject.Find("PlayerCharacter" + (i+1).ToString()).GetComponent<Animator>();
            GameObject.Find("PlayerCharacter" + (i+1).ToString()).GetComponent<Animator>().Play("Base Layer.Movable.Move");
        }

        flg = false;

        // 時間をスタート
        m_timeCountScript = GameObject.Find("Time").GetComponent<TimeCount>();
        //m_timeCountScript.StartTime();

        se026_startFlag = new bool[4];
        for (int i = 0; i < 4; i++)
            se026_startFlag[i] = false;
    }

    // Update is called once per frame
    void Update() {

        XInputDotNetPure.GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
        XInputDotNetPure.GamePadState pad2 = GamePad.GetState(PlayerIndex.Two);
        XInputDotNetPure.GamePadState pad3 = GamePad.GetState(PlayerIndex.Three);
        XInputDotNetPure.GamePadState pad4 = GamePad.GetState(PlayerIndex.Four);

        // 時間経過開始
        if (!flg)
        {
            m_timeCountScript.StartTime();
            flg = true;
        }

        // スタートが押されたか
        //if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad1) || Input.GetKeyDown(KeyCode.Z))
        if( (pad1.Buttons.Start == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Find("Ready1").GetComponent<Ready>().DispReady();
            m_readyFlag[0] = true;

            if (!se026_startFlag[0])
            {
                BGMManager.instance.PlaySE("se026_PlayerPrepareFinish");
                se026_startFlag[0] = true;
            }
        }

        if ((pad2.Buttons.Start == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("Ready2").GetComponent<Ready>().DispReady();
            m_readyFlag[1] = true;

            if (!se026_startFlag[1])
            {
                BGMManager.instance.PlaySE("se026_PlayerPrepareFinish");
                se026_startFlag[1] = true;
            }

        }

        if ((pad3.Buttons.Start == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.C))
        {
            GameObject.Find("Ready3").GetComponent<Ready>().DispReady();
            m_readyFlag[2] = true;

            if (!se026_startFlag[2])
            {
                BGMManager.instance.PlaySE("se026_PlayerPrepareFinish");
                se026_startFlag[2] = true;
            }

        }

        if ((pad4.Buttons.Start == ButtonState.Pressed) || Input.GetKeyDown(KeyCode.V))
        {
            GameObject.Find("Ready4").GetComponent<Ready>().DispReady();
            m_readyFlag[3] = true;

            if (!se026_startFlag[3])
            {
                BGMManager.instance.PlaySE("se026_PlayerPrepareFinish");
                se026_startFlag[3] = true;
            }

        }

        Debug.Log(m_timeCountScript.GetTime());


        // 全員レディしたら　もしくは　残り時間が0になったら
        if ((m_readyFlag[0] && m_readyFlag[1] && m_readyFlag[2] && m_readyFlag[3]) || (m_timeCountScript.GetTime() <= 0.0f))
        {
            Fade.ChangeScene("Play");
        }

    }
}
