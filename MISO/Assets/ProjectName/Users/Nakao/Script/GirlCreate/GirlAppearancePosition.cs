using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GirlAppearancePosition : MonoBehaviour
{
    public GirlNoPlayerCharacter girlObject;   // 女性オブジェクト

    [SerializeField]
    List<GirlNoPlayerCharacter> m_girlObjects;
    //[SerializeField]
   

    bool isCreate;
    public bool IsCreate
    {
        get { return isCreate; }
        set { isCreate = value; }
    }

    bool m_isDestroy;
    public bool IsDestroy
    {
        get { return m_isDestroy; }
        set { m_isDestroy = value; }
    }

    GirlCreater m_parntGirlCreater;

    public GirlCreater m_ParntGirlCreater
    {
        get { return m_parntGirlCreater; }
        set { m_parntGirlCreater = value; }
    }
    

	// Use this for initialization
	void Start () {
        //m_girlObject = null;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        // フラグが立ってるならNPC作る
        if (IsCreate)
        {
            
            if (m_girlObject == null)
            {
                Vector3 pos = new Vector3();

                pos = this.transform.position;
                pos.x *= Random.Range(0.9f, 1.1f);
                pos.z *= Random.Range(0.9f, 1.1f);
                Transform girlTransform = this.transform;
                girlTransform.position = pos;
                m_girlObject = (GirlNoPlayerCharacter)Instantiate(girlObject,pos,this.transform.rotation);
                m_girlObject.m_ParntGirlAppearancePosition = this;
                //isCreate = false;
            }
             
            Vector3 pos = new Vector3();

            pos = this.transform.position;
            pos.x *= Random.Range(0.9f, 1.1f);
            pos.z *= Random.Range(0.9f, 1.1f);
            Transform girlTransform = this.transform;
            girlTransform.position = pos;
            m_girlObject.Add((GirlNoPlayerCharacter)Instantiate(girlObject, pos, this.transform.rotation));
            m_girlObject[m_girlObject.Count - 1].m_ParntGirlAppearancePosition = this;
            isCreate = false;
        }
        
        for (int i = 0; i < m_girlObject.Count; ++i)
        {
            if (m_girlObject[i].IsDestroy)
            {
                IsCreate = false;
                IsDestroy = false;
                m_ParntGirlCreater.m_createGirlNumber--;
            }
        }
         */
	}

    public void CreateNoPlayerCharacter()
    {
        Vector3 pos = new Vector3();

        pos = this.transform.position;
        pos.x *= Random.Range(0.9f, 1.1f);
        pos.z *= Random.Range(0.9f, 1.1f);
        Transform girlTransform = this.transform;
        girlTransform.position = pos;
        isCreate = false;
        m_girlObjects.Add((GirlNoPlayerCharacter)Instantiate(girlObject, pos, this.transform.rotation));
        //m_girlObject.Add(girlObject);
        m_girlObjects[m_girlObjects.Count - 1].m_ParntGirlAppearancePosition = this;
        isCreate = false;

    }
}
