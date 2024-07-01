using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEditor.ShortcutManagement;
using NaughtyAttributes;


public class WorldEditor : EditorWindow
{

    public static Object prefab;


    [MenuItem("Window/World Editor")]
    public static void ShowWindow()
    {
        GetWindow<WorldEditor>("World Editor");
    }




    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        prefab = EditorGUILayout.ObjectField(prefab, typeof(LevelPath), true);
        GUILayout.Label("...", EditorStyles.boldLabel);

        if (GUILayout.Button(""))
        {
            
        }

    }


}
