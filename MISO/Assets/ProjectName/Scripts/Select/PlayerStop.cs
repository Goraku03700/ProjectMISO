using UnityEngine;
using System.Collections;

public class PlayerStop : MonoBehaviour {

    private Vector3 m_savePosition;

	// Use this for initialization
	void Start () {

        // スタート時の位置を保存
        m_savePosition = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        // 位置を固定する
        this.transform.localPosition = m_savePosition;

	}
}
