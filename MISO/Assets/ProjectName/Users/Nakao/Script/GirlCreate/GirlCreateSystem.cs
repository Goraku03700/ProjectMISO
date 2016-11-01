using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GirlCreateSystem : MonoBehaviour {

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


//    [SerializeField]
  //  GameObject m_girlPrefabs;
    //[SerializeField]
    //List<GameObject> m_Girls;

    [SerializeField]
    float time;

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
        m_areaA_GirlCreateArea.m_ParntGirlCreateSystem = this;
        for (int i = 0; i < m_createRules.Count ; ++i )
        {
            if (m_createRules[i].continuePattern)
            {
                m_createRules_Continue.Add((GirlCreateRule)Instantiate(m_createRules[i]));
            }
            else if (m_createRules[i].fever)
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
        time -= Time.deltaTime;
        m_girl_Count = 0;
        for (int i = 0; i < m_createRules_Continue.Count; ++i)
        {
            if(m_createRules_Continue[i].createTime>time && m_createRules_Continue[i].validTime < time)
            {
                if(m_areaA_GirlCreateArea.m_CreateGirlNumber <= 0)
                {
                    m_areaA_GirlCreateArea.CreateGirl(m_createRules_Continue[i].generationCount);
                }
                if (m_areaB_GirlCreateArea.m_CreateGirlNumber <= 0)
                {
                    m_areaB_GirlCreateArea.CreateGirl(m_createRules_Continue[i].generationCount);
                }
                if (m_areaC_GirlCreateArea.m_CreateGirlNumber <= 0)
                {
                    m_areaC_GirlCreateArea.CreateGirl(m_createRules_Continue[i].generationCount);
                }
                if (m_areaD_GirlCreateArea.m_CreateGirlNumber <= 0)
                {
                    m_areaD_GirlCreateArea.CreateGirl(m_createRules_Continue[i].generationCount);
                }
                if (m_areaE_GirlCreateArea.m_CreateGirlNumber <= 0)
                {
                    m_areaE_GirlCreateArea.CreateGirl(m_createRules_Continue[i].generationCount);
                }
            }
        }

        for (int i = 0; i < m_createRules_Fever.Count; ++i)
        {
            if (m_createRules_Fever[i].createTime >= time && m_createRules_Fever[i].fever)
            {

                m_feverAreaA_GirlCreateArea.CreateGirl(m_createRules_Fever[i].generationCount);

                m_feverAreaB_GirlCreateArea.CreateGirl(m_createRules_Fever[i].generationCount);

                m_feverAreaC_GirlCreateArea.CreateGirl(m_createRules_Fever[i].generationCount);

                m_feverAreaD_GirlCreateArea.CreateGirl(m_createRules_Fever[i].generationCount);

                m_feverAreaE_GirlCreateArea.CreateGirl(m_createRules_Fever[i].generationCount);

                m_createRules_Fever[i].fever = false;
            }
        }

        for (int i = 0; i < m_createRules_Normal.Count; ++i)
        {
            m_girl_Count = m_areaA_GirlCreateArea.m_CreateGirlNumber + m_areaB_GirlCreateArea.m_CreateGirlNumber + m_areaC_GirlCreateArea.m_CreateGirlNumber + m_areaD_GirlCreateArea.m_CreateGirlNumber + m_areaE_GirlCreateArea.m_CreateGirlNumber + m_feverAreaA_GirlCreateArea.m_CreateGirlNumber + m_feverAreaB_GirlCreateArea.m_CreateGirlNumber + m_feverAreaC_GirlCreateArea.m_CreateGirlNumber + m_feverAreaD_GirlCreateArea.m_CreateGirlNumber + m_feverAreaE_GirlCreateArea.m_CreateGirlNumber;
            if (m_girl_Count < m_createRules_Normal[i].decisionCount && m_createRules_Normal[i].normal && m_createRules_Normal[i].createTime >= time)
            {
                m_areaA_GirlCreateArea.CreateGirl(m_createRules_Normal[i].generationCount);
           
                m_areaB_GirlCreateArea.CreateGirl(m_createRules_Normal[i].generationCount);
           
                m_areaC_GirlCreateArea.CreateGirl(m_createRules_Normal[i].generationCount);
           
                m_areaD_GirlCreateArea.CreateGirl(m_createRules_Normal[i].generationCount);
           
                m_areaE_GirlCreateArea.CreateGirl(m_createRules_Normal[i].generationCount);
                m_createRules_Normal[i].normal = false;
            }
        }

	}
    /*
    void CreateGirl()
    {
        GameObject newGirl = Instantiate(m_girlPrefabs);
        newGirl.gameObject.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0.37f, Random.Range(-4.0f, 4.0f));
        //newGirl.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
        m_Girls.Add(newGirl);
        time = 0.0f;
    }*/

    

}
