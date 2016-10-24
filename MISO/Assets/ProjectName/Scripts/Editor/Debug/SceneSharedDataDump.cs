#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SceneSharedDataの中身を表示するためのクラスです。
/// </summary>
public class SceneSharedDataDump
{

    [MenuItem("Debug/SceneSharedData/Dump")]
    public static void Dump()
    {
        SceneSharedData instance = SceneSharedData.instance;

        if (instance == null)
        {
            Debug.Log("SceneSharedDataが現在のシーンにありません");

            return;
        }

        foreach (KeyValuePair<string, object> data in instance.dataDictionary)
        {
            Debug.Log(string.Format("Key : {0}, Value : {1}", data.Key, data.Value));
        }
    }

}

#endif