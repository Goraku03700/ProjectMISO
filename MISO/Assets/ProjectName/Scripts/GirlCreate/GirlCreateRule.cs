using UnityEngine;
using System.Collections;

public class GirlCreateRule : MonoBehaviour
{

    [Tooltip("生成時間(秒数指定)")]
    public float m_createTime;
    [Tooltip("生成時間の有効期限(何もなければ０で)")]
    public float m_validTime;
    [Tooltip("生成判断数")]
    public int m_decisionCount;
    [Tooltip("生成数")]
    public int m_generationCount;
  //  [Tooltip("フィールドの女性数")]
  //  public int fieldGirlCount;
    [Tooltip("生成エリア指定(ノーマル)")]
    //public bool areaA, areaB, areaC, areaD, areaE;
    public bool m_normal;
    [Tooltip("FEVER指定")]
    public bool m_fever;
    [Tooltip("継続的な生成パターンか")]
    public bool m_continuePattern;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
