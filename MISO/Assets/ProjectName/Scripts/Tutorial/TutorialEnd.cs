using UnityEngine;
using System.Collections;

public class TutorialEnd : MonoBehaviour {


    private bool[] m_readyFlag; // レディ押したか

    private Animator[] m_animePlayer;

    private TimeCount m_timeCountScript;

    private bool flg;

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
    }

    // Update is called once per frame
    void Update() {

        // 時間経過開始
        if (!flg)
        {
            m_timeCountScript.StartTime();
            flg = true;
        }

        // スタートが押されたか
        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad1) || Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Find("Ready1").GetComponent<Ready>().DispReady();
            m_readyFlag[0] = true;
        }

        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad2) || Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("Ready2").GetComponent<Ready>().DispReady();
            m_readyFlag[1] = true;
        }

        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad3) || Input.GetKeyDown(KeyCode.C))
        {
            GameObject.Find("Ready3").GetComponent<Ready>().DispReady();
            m_readyFlag[2] = true;
        }

        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad4) || Input.GetKeyDown(KeyCode.V))
        {
            GameObject.Find("Ready4").GetComponent<Ready>().DispReady();
            m_readyFlag[3] = true;
        }

        Debug.Log(m_timeCountScript.GetTime());


        // 全員レディしたら　もしくは　残り時間が0になったら
        if ((m_readyFlag[0] && m_readyFlag[1] && m_readyFlag[2] && m_readyFlag[3]) || (m_timeCountScript.GetTime() <= 0.0f))
        {
            Fade.ChangeScene("Play");
        }

    }
}
