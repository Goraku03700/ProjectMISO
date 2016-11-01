using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class CreateRulePrefabs : Editor {


    [MenuItem("CreateRule/生成パターンのプレハブを作成")]
    public static void CreateNewRule()
    {
        string name = "GirlCreateRule";
        string outPath = "Assets/ProjectName/Prefabs/GirlCreateRule/";
        string[] path_Array = Directory.GetFiles("Assets/ProjectName/Prefabs/GirlCreateRule/", "*.prefab");
        int array_num = path_Array.Length;
        outPath += "GirlCreateRule"  + array_num + ".prefab";
        GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags(
            name,
            HideFlags.HideInHierarchy,
            typeof(GirlCreateRule)
        );


        PrefabUtility.CreatePrefab(outPath, gameObject, ReplacePrefabOptions.Default);
        Editor.DestroyImmediate(gameObject);

        
    }
}
