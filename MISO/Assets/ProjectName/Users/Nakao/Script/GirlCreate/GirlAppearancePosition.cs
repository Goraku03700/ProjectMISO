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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // フラグが立ってるなら汚れ作る
        if (IsCreate)
        {
            Vector3 pos = new Vector3();
               
            pos = this.transform.position;
            pos.x *= Random.Range(0.9f,1.1f);
            pos.z *= Random.Range(0.9f,1.1f);
            pos = transform.rotation * pos;

            girlObject = new GirlNoPlayerCharacter();

            isCreate = false;
        }
	}
}
