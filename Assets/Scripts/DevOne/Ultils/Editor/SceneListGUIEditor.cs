using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneListGUIEditor : EditorWindow {

    private bool runWhenClick;

    [MenuItem("DevOne/Utils/ListSceneGUI")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SceneListGUIEditor));
    }

    private void OnGUI()
    {
        runWhenClick = GUILayout.Toggle(runWhenClick,"Run When Click");

        var scenes = EditorBuildSettings.scenes;

        for (int i = 0; i < scenes.Length; i++)
        {
            if (GUILayout.Button(scenes[i].path))
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scenes[i].path);

                if (runWhenClick)
                {
                    EditorApplication.isPlaying = true;
                }
            }
        }
    }
}
