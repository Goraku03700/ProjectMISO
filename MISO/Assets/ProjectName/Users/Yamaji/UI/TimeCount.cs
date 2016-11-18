using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeCount : MonoBehaviour {

    [SerializeField]
    private float timeInit;

    private float nowTime;

    private Text timeText;

    private bool isStart;

	// Use this for initialization
	void Start () {
        nowTime = 0.0f;

        timeText = this.GetComponent<Text>();

        isStart = false;
	}
	
	// Update is called once per frame
	void Update () {

        // 時間を測る
        if (isStart)
        {
            nowTime += Time.deltaTime;
        }

        // 残り時間を計算
        float dispTime = timeInit - nowTime;

        int second = (int)dispTime % 60; // 秒を計算
        int minute = (int)dispTime / 60; // 分を計算

        timeText.text = minute.ToString() + ":" + second.ToString().PadLeft(2, '0');
                 
	}

    /// <summary>
    /// カウントダウンを始める
    /// </summary>
    public void StartTime()
    {
        isStart = true;
    }
}

