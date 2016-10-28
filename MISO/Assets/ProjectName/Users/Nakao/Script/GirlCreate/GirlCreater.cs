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
    public GameObject[] appearancePoints;   // 出現位置管理

    bool isMyGirlGetting;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
