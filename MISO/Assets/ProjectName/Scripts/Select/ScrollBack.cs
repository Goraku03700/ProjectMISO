using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollBack : MonoBehaviour {

    [SerializeField]
    private float scrollSpeed;

    private Image image;

	// Use this for initialization
	void Start () {

        image = this.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        float scroll = Mathf.Repeat(Time.time * scrollSpeed, 1);      // 時間によってYの値が0から1に変化していく.1になったら0に戻り繰り返す.
        Vector2 offset = new Vector2(scroll, scroll);                // Xの値がずれていくオフセットを作成.
        image.material.SetTextureOffset("_MainTex", offset);
    }
}
