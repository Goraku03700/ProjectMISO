using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルで使用するリボンの表示クラス
/// </summary>
public class InvisibleRibbon : MonoBehaviour {

    private MeshRenderer m_meshrender;

	// Use this for initialization
	void Start () {
        m_meshrender = GetComponent<MeshRenderer>();
        m_meshrender.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// リボンの表示の切り替え
    /// </summary>
    /// <param name="active">true:表示する false:表示しない</param>
    public void ActiveRibbon(bool active)
    {
        m_meshrender.enabled = active;
    }

}
