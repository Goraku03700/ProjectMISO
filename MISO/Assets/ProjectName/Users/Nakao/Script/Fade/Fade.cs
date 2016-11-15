using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Fade : MonoBehaviour{
    private enum State { Out, In, End };
    private State fade = State.Out;

    [SerializeField]
    float time;
    [SerializeField]
    float fadeintime;             // フェードイン・アウトそれぞれにかける時間

    [SerializeField]
    float fadeouttime;             // フェードイン・アウトそれぞれにかける時間

    private static string SceneName;           // 遷移先のシーン名

    [SerializeField]
    Image bubblekun;
    
    [SerializeField]
    List<Image> m_ribbons;
    [SerializeField]
    float awainter;
    private static bool fadeflg;
    private static bool destroy;
    [SerializeField]
    Canvas screen;
    //========================================
    // Use this for initialization
    //========================================
    void Start(){
        // 一時的に破棄しない

        time = 0.0f;
        DontDestroyOnLoad(this);
	}

    void Update(){
       
        if (time>fadeouttime && Application.loadedLevelName != SceneName)
        {
            Application.LoadLevel(SceneName);
        }


        
    }

    //========================================
    // フェードイン・アウト処理
    //========================================
    
    public static void ChangeScene(string nextScene)
    {
        
    }

    public static bool FadeEnd()
    {
        return !fadeflg;
    }

}
