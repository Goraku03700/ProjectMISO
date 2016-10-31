using UnityEngine;
using System.Collections;

public class GirlAppearancePosition : MonoBehaviour
{
    public GirlNoPlayerCharacter girlObject;   // 女性オブジェクト

    //[SerializeField]


    GirlCreater myCreater;

    bool m_isMyGirlGetting;  // 取得されたかどうか

    public GirlCreater MyCreater
    {
        get { return myCreater; }
        set { myCreater = value; }
    }

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // フラグが立ってるなら汚れ作る
        if (IsCreate)
        {
            if (girlObject == null)
            {
                Vector3 pos = new Vector3();

                pos = this.transform.position;
                pos.x *= Random.Range(0.9f, 1.1f);
                pos.z *= Random.Range(0.9f, 1.1f);
                Transform girlTransform = this.transform;
                girlTransform.position = pos;
                girlObject = (GirlNoPlayerCharacter)Instantiate(girlObject,pos,this.transform.rotation);

                isCreate = false;
            }
        }
	}
}
