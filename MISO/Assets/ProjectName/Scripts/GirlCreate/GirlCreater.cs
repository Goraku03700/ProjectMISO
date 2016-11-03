using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class GirlCreater : MonoBehaviour {

    GirlCreateSystem m_parntGirlCreateSystem;

    public GirlCreateSystem m_ParntGirlCreateSystem
    {
        get { return m_parntGirlCreateSystem; }
        set { m_parntGirlCreateSystem = value; }
    }

    [SerializeField, Header("出現場所(GirlAppearancePoint)の管理配列")]
    List <GirlAppearancePosition> m_appearancePoints;   // 出現位置管理

    [SerializeField]
    bool m_fever;
    public bool m_Fever
    {
        get { return m_fever; }
        set { m_fever = value; }
    }

    int m_createGirlNumber;

    public int m_CreateGirlNumber
    {
        get { return m_createGirlNumber; }
        set { m_createGirlNumber = value; }
    }


	// Use this for initialization
	void Start () {
        foreach(Transform child in this.transform)
        {
            if(child.GetComponent<GirlAppearancePosition>() !=null)
            {
                m_appearancePoints.Add(child.GetComponent<GirlAppearancePosition>());
            }
        }
 //       m_appearancePoints = Array.FindAll(transform.GetComponentsInChildren<GirlAppearancePosition>(),);

	    for(int i = 0 ; i < m_appearancePoints.Count ; ++i)
        {
            m_appearancePoints[i].m_ParntGirlCreater = this;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public bool CreateGirl(int createCount)
    {
        for (int i = 0; i < createCount; ++i)
        {
            int num = UnityEngine.Random.Range(0, m_appearancePoints.Count);
            /*
            for (int i = 0; i < appearancePoints.Length - 1; ++i)
            {
                if (appearancePoints[num].IsCreate == false)
                {
                    appearancePoints[num].IsCreate = true;
                    m_createGirlNumber++;
                    return true;
                }
                else
                {
                    num++;
                    if(num > appearancePoints.Length - 1)
                    {
                        num = 0;
                    }
                }
            }
            */

            m_appearancePoints[num].CreateNoPlayerCharacter();
            m_createGirlNumber++;
        }
        return true;

        return false;
    }

}
