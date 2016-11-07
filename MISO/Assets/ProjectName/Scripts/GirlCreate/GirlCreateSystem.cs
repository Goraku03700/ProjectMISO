using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GirlCreateSystem : MonoBehaviour {
    /*
    [SerializeField, Tooltip("女性生成エリアA")]
    GirlCreater m_areaA_GirlCreateArea;
    [SerializeField, Tooltip("女性生成エリアB")]
    GirlCreater m_areaB_GirlCreateArea;
    [SerializeField, Tooltip("女性生成エリアC")]
    GirlCreater m_areaC_GirlCreateArea;
    [SerializeField, Tooltip("女性生成エリアD")]
    GirlCreater m_areaD_GirlCreateArea;
    [SerializeField, Tooltip("女性生成エリアE")]
    GirlCreater m_areaE_GirlCreateArea;
    */
    [SerializeField, Tooltip("女性生成エリアのリスト")]
    List<GirlCreater> m_girlCreateAreaList;

    /*
    [SerializeField, Tooltip("フィーバー用女性生成エリアA")]
    GirlCreater m_feverAreaA_GirlCreateArea;
    [SerializeField, Tooltip("フィーバー用女性生成エリアB")]
    GirlCreater m_feverAreaB_GirlCreateArea;
    [SerializeField, Tooltip("フィーバー用女性生成エリアC")]
    GirlCreater m_feverAreaC_GirlCreateArea;
    [SerializeField, Tooltip("フィーバー用女性生成エリアD")]
    GirlCreater m_feverAreaD_GirlCreateArea;
    [SerializeField, Tooltip("フィーバー用女性生成エリアE")]
    GirlCreater m_feverAreaE_GirlCreateArea;
     */
    [SerializeField, Tooltip("フィーバー用女性生成エリアのリスト")]
    List<GirlCreater> m_girlFeverCreateAreaList;

    [SerializeField]
    Text m_LimitTimeUI;
    [SerializeField]
    Text m_Score1;
    [SerializeField]
    Text m_Score2;

    int m_player1 = 0;
    int m_player2 = 0;

    [SerializeField]
    float m_limit;

    public List<GirlCreateRule> m_createRules;

    public List<GirlCreateRule> m_createRules_Normal;
    public List<GirlCreateRule> m_createRules_Fever;
    public List<GirlCreateRule> m_createRules_Continue;

    [SerializeField]
    int m_girl_Count;

    public int m_GirlCount
    {
        get {return m_girl_Count ;}
        set {m_girl_Count = value ;}
    }


	// Use this for initialization
	void Start () {
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<GirlCreater>() != null)
            {
                child.GetComponent<GirlCreater>().m_ParntGirlCreateSystem = this;
                if (child.GetComponent<GirlCreater>().m_Fever)
                {
                    m_girlFeverCreateAreaList.Add(child.GetComponent<GirlCreater>());
                }
                else
                {
                    m_girlCreateAreaList.Add(child.GetComponent<GirlCreater>());
                }
            }
        }
        for (int i = 0; i < m_createRules.Count ; ++i )
        {
            if (m_createRules[i].m_continuePattern)
            {
                m_createRules_Continue.Add((GirlCreateRule)Instantiate(m_createRules[i]));
            }
            else if (m_createRules[i].m_fever)
            {
                m_createRules_Fever.Add((GirlCreateRule)Instantiate(m_createRules[i]));
            }
            else
            {
                m_createRules_Normal.Add((GirlCreateRule)Instantiate(m_createRules[i]));
            }
        }

         //   time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        m_LimitTimeUI.text = "TIME\n" + (int)m_limit;
        if (Input.GetKeyDown(KeyCode.L))
        {
            BGMManager.instance.PlaySE("Soap_Jump");
        }
        m_limit -= Time.deltaTime;
        m_girl_Count = 0;
        for (int i = 0; i < m_createRules_Continue.Count; ++i)
        {
            if(m_createRules_Continue[i].m_createTime>m_limit && m_createRules_Continue[i].m_validTime < m_limit)
            {
                for (int j = 0; j < m_girlCreateAreaList.Count; ++j )
                {
                    if(m_girlCreateAreaList[j].m_CreateGirlNumber <= 0)
                    {
                        m_girlCreateAreaList[j].CreateGirl(m_createRules_Continue[i].m_generationCount);
                    }

                }
            }
        }

        for (int i = 0; i < m_createRules_Fever.Count; ++i)
        {
            if (m_createRules_Fever[i].m_createTime >= m_limit && m_createRules_Fever[i].m_fever)
            {
                for (int j = 0; j < m_girlFeverCreateAreaList.Count; ++j)
                {
                    if (m_girlFeverCreateAreaList[j].m_CreateGirlNumber <= 0)
                    {
                        m_girlFeverCreateAreaList[j].CreateGirl(m_createRules_Fever[i].m_generationCount);
                    }

                }
                m_createRules_Fever[i].m_fever = false;
            }
        }

        for (int i = 0; i < m_createRules_Normal.Count; ++i)
        {
            for (int j = 0; j < m_girlCreateAreaList.Count; ++j)
            {
                m_GirlCount += m_girlCreateAreaList[j].m_CreateGirlNumber;
            }

            if (m_girl_Count < m_createRules_Normal[i].m_decisionCount && m_createRules_Normal[i].m_normal && m_createRules_Normal[i].m_createTime >= m_limit)
            {
                for (int j = 0; j < m_girlCreateAreaList.Count; ++j)
                {
                    if (m_girlCreateAreaList[j].m_CreateGirlNumber <= 0)
                    {
                        m_girlCreateAreaList[j].CreateGirl(m_createRules_Normal[i].m_generationCount);
                    }

                }
                m_createRules_Normal[i].m_normal = false;
            }
        }

	}

    public void GetNPC_Player(int i)
    {
        switch(i)
        {
            case 0:
                {
                    m_player1++;
                    m_Score1.text = "Player1\nScore" + m_player1;
                    break;
                }
            case 1:
                {
                    m_player2++;
                    m_Score2.text = "Player2\nScore" + m_player2;
                    break;
                }
            default:
                break;
        }
    }
    

}
