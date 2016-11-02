using UnityEngine;
using System.Collections;

public class GirlCreateRule : MonoBehaviour
{

    [Tooltip("生成時間(秒数指定)")]
    public float createTime;
    [Tooltip("生成時間の有効期限(何もなければ０で)")]
    public float validTime;
    [Tooltip("生成判断数")]
    public int decisionCount;
    [Tooltip("生成数")]
    public int generationCount;
  //  [Tooltip("フィールドの女性数")]
  //  public int fieldGirlCount;
    [Tooltip("生成エリア指定(ノーマル)")]
    //public bool areaA, areaB, areaC, areaD, areaE;
    public bool normal;
    [Tooltip("FEVER指定")]
    public bool fever;
    [Tooltip("継続的な生成パターンか")]
    public bool continuePattern;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
