using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEditor.ShortcutManagement;


public class LayoutLoaderWindow : EditorWindow
{
    private string layoutsPath = "Assets/Editor/Layouts";
    private bool horizontalLayout = false;
    [MenuItem("Window/Layout Loader")]
    public static void ShowWindow()
    {
        GetWindow<LayoutLoaderWindow>("Layouts");
    }

    /*
    [Shortcut("LayoutSwitcher/Layout1", KeyCode.Alpha1, ShortcutModifiers.Alt)]
    public void Layout1()
    {
        LoadLayout("Default.wlt");
    }
    */
    

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha1)) 
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.Alpha2)) LoadLayout("Animation.wlt");
    }

    private void OnGUI()
    {




        if (horizontalLayout) EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Load Layout", EditorStyles.boldLabel);

        if (Directory.Exists(layoutsPath))
        {
            var layoutFiles = Directory.GetFiles(layoutsPath, "*.wlt");

            foreach (var layoutFile in layoutFiles)
            {
                string layoutName = Path.GetFileNameWithoutExtension(layoutFile);

                if (GUILayout.Button(layoutName))
                {
                    LoadLayout(layoutFile);
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox($"Directory not found: {layoutsPath}", MessageType.Error);
        }
        if (horizontalLayout) EditorGUILayout.EndHorizontal();
    }

    private void LoadLayout(string layoutPath)
    {
        if (File.Exists(layoutPath))
        {
            EditorUtility.LoadWindowLayout(layoutPath);
            
        }
        else
        {
            Debug.LogError($"Layout file not found: {layoutPath}");
        }
    }
}
