using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 背景を回転させるクラス
/// </summary>
public class UI_BackGround_Rotation : MonoBehaviour {
    private RectTransform m_rect_transform;

	// Use this for initialization
	void Start () {
        m_rect_transform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        m_rect_transform.Rotate(new Vector3(0, 0, -Mathf.PI * 0.1f));	
	}
}
