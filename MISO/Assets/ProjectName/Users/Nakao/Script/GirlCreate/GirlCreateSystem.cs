﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GirlCreateSystem : MonoBehaviour {

    [SerializeField]
    GameObject temporaryCreatePosition;

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


    [SerializeField]
    GameObject m_girlPrefabs;
    [SerializeField]
    List<GameObject> m_Girls;
    float time;

    bool createflag;

	// Use this for initialization
	void Start () {
        m_areaA_GirlCreateArea.m_ParntGirlCreateSystem = this;
        m_areaA_GirlCreateArea.CreateGirl();
        

    //    m_areaA_GirlCreateArea.
    //    CreateGirl();
        time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time>10.5f)
        {
            if(m_areaA_GirlCreateArea.CreateGirl())
            {
                time = 0.0f;
            }
            else
            {
                time = 5.0f;
            }
        }
	}

    void CreateGirl()
    {
        GameObject newGirl = Instantiate(m_girlPrefabs);
        newGirl.gameObject.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0.37f, Random.Range(-4.0f, 4.0f));
        //newGirl.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
        m_Girls.Add(newGirl);
        time = 0.0f;
    }

    void CreateCheckPosition()
    {
        Instantiate(temporaryCreatePosition);
        temporaryCreatePosition.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0.37f, Random.Range(-4.0f, 4.0f));
        createflag = true;
    }

}