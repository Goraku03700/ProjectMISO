using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour {

    const int ConstPlayerMax = 4;

    Text[] readyText;
    Text infoText;


    private bool[] m_readyFlag;

  

	// Use this for initialization
	void Start () {

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

       
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown(KeyCode.Z))
        {
            m_readyFlag[0] = true;
            readyText[0].text = "OK!";
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            m_readyFlag[1] = true;
            readyText[1].text = "OK!";

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            m_readyFlag[2] = true;
            readyText[2].text = "OK!";
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            m_readyFlag[3] = true;
            readyText[3].text = "OK!";
        }


        for (int i = 0; i < ConstPlayerMax; i++)
        {
            if(m_readyFlag[i])
            {
                readyText[i].transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                if(readyText[i].transform.localScale.x > 1.0f)
                {
                    readyText[i].transform.localScale = new Vector3(1.0f,1.0f, 1.0f);
                }                
            }
        }
        


        



    }
}
