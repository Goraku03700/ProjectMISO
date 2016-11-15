using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class FPSPresenter : MonoBehaviour
{
    float cpuTime, gpuTime;

    float cpuBeginTime = 0;
    float cpuEndTime = 0;
    float gpuBeginTime = 0;
    float gpuEndTime = 0;
    float fps = 0;

    [SerializeField]
    bool isVisible = false;

    /// <summary>
    /// Update()が初めて呼ばれる1フレーム前に実行される
    /// </summary>
    public void Start()
    {
    }

    void OnPreRender()
    {
        gpuBeginTime = Time.realtimeSinceStartup;
    }

    void OnPostRender()
    {
        gpuEndTime = Time.realtimeSinceStartup;
    }

    void OnGUI()
    {
        if (!isVisible)
            return;

        using (var scope = new GUILayout.VerticalScope())
        {
            GUILayout.Label(string.Format("CPU:{0}", cpuTime));
            GUILayout.Label(string.Format("GPU:{0}", gpuTime));
            GUILayout.Label(string.Format("FPS:{0}", fps));
        }
    }

    public void Update()
    {
        fps = 1.0f / Time.unscaledDeltaTime;

        cpuEndTime = Time.realtimeSinceStartup;

        cpuTime = cpuEndTime - cpuBeginTime;
        gpuTime = gpuEndTime - gpuBeginTime;

        cpuBeginTime = Time.realtimeSinceStartup;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isVisible)
                isVisible = false;
            else
                isVisible = true;
        }
    }
}