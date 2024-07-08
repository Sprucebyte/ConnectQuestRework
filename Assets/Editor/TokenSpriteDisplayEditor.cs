using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

[CustomEditor(typeof(Token))]
public class TokenSpriteDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector first
        DrawDefaultInspector();

        // Get the target object
        Token spriteDisplay = (Token)target;

        // Check if a sprite is assigned
        if (spriteDisplay.GetSprite() != null)
        {
            GUILayout.Space(10);

            // Draw a horizontal line
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Add some space after the line
            GUILayout.Space(5);

            GUILayout.Label("Sprite Preview", EditorStyles.boldLabel);

            // Draw the sprite in the inspector
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(AssetPreview.GetAssetPreview(spriteDisplay.GetSprite()), GUILayout.Width(80), GUILayout.Height(80));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}