using UnityEngine;
using UnityEditor;
using System.Collections;

public class MultiInputEditorWindow : EditorWindow
{
    [MenuItem("Window/MultiInputEditor Window")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MultiInputEditorWindow));
    }

    void OnGUI()
    {
        // The actual window code goes here
        //EditorGUILayout.BeginFadeGroup(10);
    }
}