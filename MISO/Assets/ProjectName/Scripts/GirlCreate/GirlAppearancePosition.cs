using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GirlAppearancePosition : MonoBehaviour
{
    public GirlNoPlayerCharacter girlObject;   // 女性オブジェクト

    [SerializeField]
    List<GirlNoPlayerCharacter> m_girlObjects;
    [SerializeField, Header("生成位置の範囲設定(生成位置からrangeの値で生成)"),Tooltip("0.1なら生成位置から-0.1～0.1の範囲で生成")]
    float m_range;
    

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

    Vector2 m_movementAreaX; //xに最小値yに最大値
    public Vector2 m_MovementAreaX
    {
        get { return m_movementAreaX; }
        set { m_movementAreaX = value; }
    }
    Vector2 m_movementAreaZ;
    public Vector2 m_MovementAreaZ
    {
        get { return m_movementAreaZ; }
        set { m_movementAreaZ = value; }
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
        pos.x += Random.Range(-m_range, m_range);
        pos.z += Random.Range(-m_range, m_range);
        isCreate = false;
        m_girlObjects.Add((GirlNoPlayerCharacter)Instantiate(girlObject, pos, this.transform.rotation));
        m_girlObjects[m_girlObjects.Count - 1].m_ParntGirlAppearancePosition = this;
        m_girlObjects[m_girlObjects.Count - 1].m_MovementAreaX = m_movementAreaX;
        m_girlObjects[m_girlObjects.Count - 1].m_MovementAreaZ = m_movementAreaZ;
        m_girlObjects[m_girlObjects.Count - 1].GetRandomPositionOnLevel();
    }
}
