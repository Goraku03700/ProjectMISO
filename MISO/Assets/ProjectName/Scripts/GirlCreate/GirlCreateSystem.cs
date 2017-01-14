using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    float m_limit;

    public float m_Limit
    {
        get { return m_limit; }
        set { m_limit = value; }
    }

    public List<GirlCreateRule> m_createRules;

    public List<GirlCreateRule> m_createRules_Normal;
    public List<GirlCreateRule> m_createRules_Fever;
    public List<GirlCreateRule> m_createRules_Continue;
    public List<GirlCreateRule> m_createRules_Rare;

    [SerializeField]
    int m_girl_Count;

    [SerializeField]
    float m_countDownTime;

    public int m_GirlCount
    {
        get {return m_girl_Count ;}
        set {m_girl_Count = value ;}
    }

    [SerializeField]
    int m_rareGirl_Count;


    public int m_RareGirlCount
    {
        get { return m_rareGirl_Count; }
        set { m_rareGirl_Count = value; }
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
            else if(m_createRules[i].m_rarePattern)
            {
                m_createRules_Rare.Add((GirlCreateRule)Instantiate(m_createRules[i]));
            }
            else
            {
                m_createRules_Normal.Add((GirlCreateRule)Instantiate(m_createRules[i]));
            }
        }

        BGMManager.instance.PlayBGM("bgm001_GameMain", 0.0f);
         //   time = 0.0f;
	}


	
	// Update is called once per frame
	void Update () {
        m_limit -= Time.deltaTime;
        m_girl_Count = 0;
        for (int i = 0; i < m_createRules_Continue.Count; ++i)
        {
            if(m_createRules_Continue[i].m_createTime>m_limit && m_createRules_Continue[i].m_validTime < m_limit)
            {
                for (int j = 0; j < m_girlCreateAreaList.Count; ++j )
                {
                    if(m_girlCreateAreaList[j].m_CreateGirlNumber <= m_createRules_Continue[i].m_decisionCount)
                    {
                        m_girlCreateAreaList[j].CreateGirl(m_createRules_Continue[i].m_generationCount);
                    }

                }
            }
        }

        for (int i = 0; i < m_createRules_Rare.Count; ++i)
        {
            if(m_createRules_Rare[i].m_createTime>m_limit && m_createRules_Rare[i].m_rarePattern)
            {
                if (m_createRules_Rare[i].m_decisionCount > m_rareGirl_Count)
                {
                    int j = Random.Range(0, 1);
                    switch(j)
                    {
                        case 0:
                            {
                                int k = Random.Range(0, m_girlCreateAreaList.Count);
                                m_girlCreateAreaList[k].CreateRareGirl(1);
                                break;
                            }
                        case 1:
                            {
                                int k = Random.Range(0, m_girlFeverCreateAreaList.Count);
                                m_girlFeverCreateAreaList[k].CreateRareGirl(1);
                                break;
                            }
                    }
                }
                m_createRules_Rare[i].m_rarePattern = false;
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

        for (int j = 0; j < m_girlCreateAreaList.Count; ++j)
        {
            m_GirlCount += m_girlCreateAreaList[j].m_CreateGirlNumber;
        }

        for (int i = 0; i < m_createRules_Normal.Count; ++i)
        {
            if (m_createRules_Normal[i].m_createTime >= m_limit)
            {
                if (m_girl_Count < m_createRules_Normal[i].m_decisionCount && m_createRules_Normal[i].m_normal)
                {
                    for (int j = 0; j < m_girlCreateAreaList.Count; ++j)
                    {
                        m_girlCreateAreaList[j].CreateGirl(m_createRules_Normal[i].m_generationCount);
                    }
                    m_createRules_Normal[i].m_normal = false;
                }
            }
            else
            {

                m_createRules_Normal[i].m_normal = false;
            }
        }
	}


}
