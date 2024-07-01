using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(ShapedRequirement))]
public class ShapedRequirementEditor : Editor
{
    private ShapedRequirement shapedRequirement;
    private float previewInterval = 1f;
    private float nextSwitchTime = 0f;
    private Dictionary<ShapedRequirement.RequiredTokens, int> tokenIndexDict = new Dictionary<ShapedRequirement.RequiredTokens, int>();
    private float scale = 2f;
    [SerializeField] private Sprite centerSprite;

    private void OnEnable()
    {
        shapedRequirement = (ShapedRequirement)target;
        InitializeTokenIndices();
        nextSwitchTime = (float)EditorApplication.timeSinceStartup + previewInterval;
        EditorApplication.update += UpdatePreview;
    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdatePreview;
    }

    private void OnDestroy()
    {
        EditorApplication.update -= UpdatePreview;
    }

    private void InitializeTokenIndices()
    {
        tokenIndexDict.Clear();
        if (shapedRequirement.requiredTokens != null)
        {
            foreach (var requiredToken in shapedRequirement.requiredTokens)
            {
                tokenIndexDict[requiredToken] = 0;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Ensure token indices are synchronized with the list
        SyncTokenIndices();

        // Add a button to refresh the preview
        if (GUILayout.Button("Refresh Preview"))
        {
            InitializeTokenIndices();
            SceneView.RepaintAll();
        }

        // Add a slider to adjust the preview interval
        previewInterval = EditorGUILayout.Slider("Preview Interval", previewInterval, 0.1f, 5f);
        scale = EditorGUILayout.Slider("Preview Scale", scale, 1f, 5f);

        // Draw the preview window
        DrawPreviewWindow();
    }

    private void SyncTokenIndices()
    {
        // Remove any keys that are no longer in the list
        var keysToRemove = tokenIndexDict.Keys.Where(key => !shapedRequirement.requiredTokens.Contains(key)).ToList();
        foreach (var key in keysToRemove)
        {
            tokenIndexDict.Remove(key);
        }

        // Add any new keys that are not yet in the dictionary
        foreach (var requiredToken in shapedRequirement.requiredTokens)
        {
            if (!tokenIndexDict.ContainsKey(requiredToken))
            {
                tokenIndexDict[requiredToken] = 0;
            }
        }
    }

    private void UpdatePreview()
    {
        if (EditorApplication.timeSinceStartup >= nextSwitchTime)
        {
            foreach (var key in tokenIndexDict.Keys.ToList())
            {
                if (key.validTokens.Count > 0)
                {
                    tokenIndexDict[key] = (tokenIndexDict[key] + 1) % key.validTokens.Count;
                }
            }
            SceneView.RepaintAll();
            nextSwitchTime = (float)EditorApplication.timeSinceStartup + previewInterval;
        }
    }

    private void DrawPreviewWindow()
    {
        GUILayout.Label("Preview", EditorStyles.boldLabel);

        // Calculate the size of the preview area
        Rect rect = GUILayoutUtility.GetRect(200, 500);
        EditorGUI.DrawRect(rect, Color.gray);
        if (shapedRequirement.centerToken)
            centerSprite = shapedRequirement.centerToken.GetComponent<SpriteRenderer>().sprite;
        
        // Draw the center token
        DrawSprite(centerSprite, rect.center);

        // Draw the tokens
        foreach (var requiredToken in shapedRequirement.requiredTokens)
        {
            if (requiredToken.validTokens.Count > 0)
            {
                Sprite sprite = requiredToken.validTokens[tokenIndexDict[requiredToken]].GetComponent<SpriteRenderer>().sprite;
                Vector2 position = rect.center + (Vector2)requiredToken.relativePosition * 21 * scale; // Scale position for visibility
                DrawSprite(sprite, position);
            }
        }
    }

    private void DrawSprite(Sprite sprite, Vector2 position)
    {
        if (sprite == null) return;

        Rect spriteRect = new Rect(
            position.x - (sprite.rect.width / 2) * scale,
            position.y - (sprite.rect.height / 2) * scale,
            sprite.rect.width * scale,
            sprite.rect.height * scale
        );

        GUI.DrawTexture(spriteRect, sprite.texture);
    }
}
