using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerIcon : MonoBehaviour {

    const float scalingSpeed = 4.0f;    // 大きさを変えるスピード

    [SerializeField]
    Sprite spriteNormal;

    [SerializeField]
    Sprite spriteAngry;

    [SerializeField]
    Sprite spriteSad;

    // アイコンの状態
    public enum IconState
    {
        Normal,
        Angry,
        Sad,
    };
    private IconState m_nowIconState;  // 現在のアイコン
    private IconState m_nextIconState;   // 次に表示するアイコン

    // 拡大・縮小の状態
    enum SizeState
    {
        Normal,
        Up,
        Down,
    };
    private SizeState m_sizeState;

    // オブジェクトの元の大きさを保存しておく
    private Vector3 m_saveScale;    


	// Use this for initialization
	void Start () {

        m_nowIconState = IconState.Normal;
        m_sizeState = SizeState.Normal;

        m_saveScale = this.transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {

        switch (m_sizeState)
        {
            // 通常状態
            case SizeState.Normal:
                break;

            // 縮小中
            case SizeState.Down:

                // 縮小
                this.transform.localScale -= new Vector3(scalingSpeed * Time.deltaTime,
                                                         scalingSpeed * Time.deltaTime,
                                                         scalingSpeed * Time.deltaTime);
                // スケールが0未満になったら状態とテクスチャ変更
                if(this.transform.localScale.x < 0)
                {
                    m_sizeState = SizeState.Up;
                    m_nowIconState = m_nextIconState;
                    switch (m_nowIconState)
                    {
                        case IconState.Normal:
                            this.GetComponent<Image>().sprite = spriteNormal;
                            break;

                        case IconState.Angry:
                            this.GetComponent<Image>().sprite = spriteAngry;
                            break;

                        case IconState.Sad:
                            this.GetComponent<Image>().sprite = spriteSad;
                            break;

                    }

                    this.transform.localScale = new Vector3(0, 0, 0);

                }
                break;

            // 拡大中
            case SizeState.Up:

                // 拡大
                this.transform.localScale += new Vector3(scalingSpeed * Time.deltaTime,
                                                         scalingSpeed * Time.deltaTime,
                                                         scalingSpeed * Time.deltaTime);
                // 元の大きさになったら終わり
                if(this.transform.localScale.x > m_saveScale.x)
                {
                    this.transform.localScale = m_saveScale;
                }

                break;
        }

    }

    
    /// <summary>
    /// アイコンを通常状態に変える
    /// </summary>
    public void ChangeIconNormal()
    {
        // アイコンが変わっている時のみ
        if (m_nowIconState != IconState.Normal)
        {
            // アイコンを縮小状態にする
            m_sizeState = SizeState.Down;
            m_nextIconState = IconState.Normal;
        }   
    }
    
    /// <summary>
    /// アイコンを怒り状態に変える
    /// </summary>
    public void ChangeIconAngry()
    {
        // アイコンが変わっている時のみ
        if (m_nowIconState != IconState.Angry)
        {
            // アイコンを縮小状態にする
            m_sizeState = SizeState.Down;
            m_nextIconState = IconState.Angry;
        }
    }

    /// <summary>
    /// アイコンを悲しみ状態に変える
    /// </summary>
    public void ChangeIconSad()
    {
        // アイコンが変わっている時のみ
        if (m_nowIconState != IconState.Sad)
        {
            // アイコンを縮小状態にする
            m_sizeState = SizeState.Down;
            m_nextIconState = IconState.Sad;
        }
    }
    



}
