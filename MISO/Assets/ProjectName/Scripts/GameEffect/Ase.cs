using UnityEngine;
using System.Collections;

public class Ase : MonoBehaviour {


	// Use this for initialization
	void Start () {

       
	
	}
	
	// Update is called once per frame
	void Update () {

        // 親(プレイヤー)の位置を取得
        Vector3 playerPos = this.transform.parent.transform.position;

        // 右上に表示
        this.transform.position = new Vector3(playerPos.x + 1.0f,
            playerPos.y + 5.0f,
                                              playerPos.z);
        // 角度固定
        this.transform.rotation = Quaternion.Euler(-48.0f, 90.0f, 0.0f);
	
	}
}
