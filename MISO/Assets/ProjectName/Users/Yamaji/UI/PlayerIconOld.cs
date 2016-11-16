using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerIconOld : MonoBehaviour {

    const int ConstPlayerMax = 4;

    [SerializeField]
    private Sprite iconNormal;

    [SerializeField]
    private Sprite iconAngry;

    [SerializeField]
    private Sprite iconSick;

    private GameObject[] m_playerIcon;
    private Sprite[] m_nowIconTexture;

    enum IconState{
        Normal,
        Angry,
        Sick
    };

    private bool changeFlg;

    private float startTime;

    private float pow;

	// Use this for initialization
	void Start () {

        startTime = 0.0f;

        m_playerIcon = new GameObject[ConstPlayerMax];
        m_nowIconTexture = new Sprite[ConstPlayerMax];
        for (int i = 0; i < ConstPlayerMax; i++)
        {
            m_nowIconTexture[i] = iconNormal;
        }

        m_playerIcon[0] = GameObject.Find("PlayerIcon1P");

        changeFlg = false;

        pow = 2.0f;
        

        
	}
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKey(KeyCode.A))
        {
            changeFlg = true;
        }


        if (changeFlg)
        {
            if (m_playerIcon[0].transform.localScale.x < 0.0f)
            {
                pow *= -1;
                //m_playerIcon[0].GetComponent<Image>().material.mainTexture= iconAngry;
                m_playerIcon[0].GetComponent<Image>().sprite = iconAngry;
            }

            m_playerIcon[0].transform.localScale -= new Vector3(pow * Time.deltaTime, pow * Time.deltaTime, pow * Time.deltaTime);

            if (m_playerIcon[0].transform.localScale.x > 0.4f)
            {
                m_playerIcon[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
        }
    }
}
