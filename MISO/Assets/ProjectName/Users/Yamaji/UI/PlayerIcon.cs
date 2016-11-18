using UnityEngine;
using System.Collections;

public class PlayerIcon : MonoBehaviour {

    [SerializeField]
    Sprite spriteNormal;

    [SerializeField]
    Sprite spriteAngry;

    [SerializeField]
    Sprite spriteSick;

    // アイコンの状態
    enum IconState
    {
        Normal,
        Angry,
        Sick,
    };
    private IconState m_iconState;

    // 拡大・縮小の状態
    enum SizeState
    {
        Normal,
        Up,
        Down,
    };
    private SizeState m_sizeState;

	// Use this for initialization
	void Start () {

        m_iconState = IconState.Normal;
        m_sizeState = SizeState.Normal;

	}
	
	// Update is called once per frame
	void Update () {
	    
        
	}

    
}
