using UnityEngine;
using System.Collections;

public class GirlCreater : MonoBehaviour {

    GirlCreateSystem m_parntGirlCreateSystem;

    public GirlCreateSystem m_ParntGirlCreateSystem
    {
        get { return m_parntGirlCreateSystem; }
        set { m_parntGirlCreateSystem = value; }
    }

    [SerializeField, Header("出現場所(GirlAppearancePoint)の管理配列")]
    public GirlAppearancePosition[] appearancePoints;   // 出現位置管理

    

    bool isMyGirlGetting;



	// Use this for initialization
	void Start () {
	    

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public bool CreateGirl()
    {
        int num = Random.Range(0, appearancePoints.Length - 1);
        if (appearancePoints[num].IsCreate == false)
        {
            appearancePoints[num].IsCreate = true;
            return true;
        }
        else
        {
            return false;
        }
        
    }

}
