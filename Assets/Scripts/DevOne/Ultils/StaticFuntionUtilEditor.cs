using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DevOne.Utils
{
    public class StaticFuntionUtilEditor
    {

        [MenuItem("Clear Cache", menuItem = "DevOne/ Clear Cache", priority = 0)]
        public static void ClearCache()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif
