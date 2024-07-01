using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using static UnityEditor.PlayerSettings;
using System.IO;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class LevelPath : MonoBehaviour
{
    
    GameObject pathPrefab;
    [SerializeField] string prefabPath = "/Prefabs/Path/Path";
    [SerializeField] public List<GameObject> previousPaths = new List<GameObject>();
    [SerializeField] public List<GameObject> nextPaths = new List<GameObject>();
    SpriteRenderer spriteRenderer;
    
    [SerializeField] public Level.Type levelType;
    [SerializeField] SO_MapIcons icons;
    // Start is called before the first frame update
    public static bool showLines = true;
    bool nameSet = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    
    void UpdateSprite()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        switch (levelType)
        {
            case Level.Type.Fight:
                SetIcon(icons.fight);
                break;
            case Level.Type.Boss:
                SetIcon(icons.boss);
                break;
            case Level.Type.Shop:
                SetIcon(icons.shop);
                break;
            case Level.Type.Encounter:
                SetIcon(icons.random);
                break;
            case Level.Type.Chest:
                SetIcon(icons.chest);
                break;
            case Level.Type.Misc:
                SetIcon(icons.random);
                break;
            default:
                SetIcon(icons.random);
                break;
        }
    }

    public void SetIcon(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    private void OnValidate()
    {
        UpdateSprite();
    }

    [Button] void AddConnectingPath()
    {
        pathPrefab = Resources.Load<GameObject>(prefabPath);
        GameObject newPath = Instantiate(pathPrefab,transform.parent);
        newPath.transform.position = transform.position + (Vector3.right*50);
        nextPaths.Add(newPath);
        newPath.GetComponent<LevelPath>().previousPaths.Add(gameObject);
        newPath.transform.position = new Vector3(Mathf.Round(newPath.transform.position.x), Mathf.Round(newPath.transform.position.y),Mathf.Round(newPath.transform.position.z));
        newPath.name = "Path" + newPath.GetInstanceID().ToString();
        Selection.activeObject = newPath;
    }

    [Button] void AddPath()
    {
        pathPrefab = Resources.Load<GameObject>(prefabPath);
        GameObject newPath = Instantiate(pathPrefab,transform.parent);
        newPath.transform.position = transform.position + (Vector3.right*50);
        newPath.transform.position = new Vector3(Mathf.Round(newPath.transform.position.x), Mathf.Round(newPath.transform.position.y),Mathf.Round(newPath.transform.position.z));
        newPath.name = "Path" + newPath.GetInstanceID().ToString();
        Selection.activeObject = newPath;
    }


    [Button] void ConnectSelected()
    {
        if (Selection.objects.Length >= 2)
        {

            for (int i = 0; i < Selection.objects.Length-1; i++)
            {
                
                GameObject obj = Selection.objects[i].GameObject();
                GameObject nextObj = Selection.objects[i + 1].GameObject();
                if (!obj.TryGetComponent<LevelPath>(out LevelPath path)) continue;
                if (!nextObj.TryGetComponent<LevelPath>(out LevelPath nextPath)) continue;
                path.nextPaths.Add(nextObj);
            }
        }
    }

    [Button] void Disconnect()
    {
        for (int i = 0; i < previousPaths.Count; i++)
        {
            if (previousPaths[i] == null) continue;
            previousPaths[i].GetComponent<LevelPath>().RemovePath(gameObject);
            previousPaths.RemoveAt(i);
        }
        for (int i = 0; i < nextPaths.Count; i++)
        {
            if (nextPaths[i] == null) continue;
            nextPaths.RemoveAt(i);
        }
    }

    public void RemovePath(GameObject path)
    {
        for (int i = 0; i < nextPaths.Count; i++)
        {
            if (previousPaths[i] == path) { nextPaths.RemoveAt(i); continue; }
        }
        for (int i = 0; i < previousPaths.Count; i++)
        {
            if (previousPaths[i] == path) { previousPaths.RemoveAt(i); continue; }
        }
    }

    private void OnDrawGizmos()
    {
        if (!nameSet) { name = "Path" + GetInstanceID().ToString(); nameSet = true; }

        Handles.Label(transform.position + transform.up * 10, name);
        if (Selection.objects.Length >= 2)
        {
            if (!Selection.activeObject.GameObject() == gameObject) return;
            for (int i = 0; i < Selection.objects.Length - 1; i++)
            {
                DrawArrow(Selection.objects[i+1].GameObject().transform.position, Selection.objects[i].GameObject().transform.position);
            }
        }


        for (int i = 0; i < nextPaths.Count; i++)
        {
            if (nextPaths[i] == null) { nextPaths.RemoveAt(i); continue; }

            Handles.color = Color.yellow;
            DrawArrow(nextPaths[i].transform.position, transform.position);


        }
        for (int i = 0; i < previousPaths.Count; i++)
        {
            if (previousPaths[i] == null) { previousPaths.RemoveAt(i); continue; }
        }
            //Gizmos.DrawLine(transform.position);
    }


    void DrawArrow(Vector3 from, Vector3 to, float lineCuttof = .1f)
    {
        Vector3 direction = from - to;
        float distance = direction.magnitude;

        Vector3 fromPos = Vector3.Lerp(to, from, lineCuttof);
        Vector3 toPos = Vector3.Lerp(to, from, 1 - lineCuttof);

        Handles.DrawLine(fromPos, toPos, 3f);
        Vector3 arrowPos = Vector3.Lerp(to, from, .5f);

        Handles.ConeHandleCap(0, arrowPos, Quaternion.LookRotation(direction), 8, EventType.Repaint);
    }
}
