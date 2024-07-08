using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Selectable))]
[RequireComponent(typeof(RectTransform))]
public class MapIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    GameObject iconPrefab;
    [SerializeField] string prefabPath = "";
    [SerializeField] public List<GameObject> previousLevels = new List<GameObject>();
    [SerializeField] public List<GameObject> nextLevels = new List<GameObject>();
    [SerializeField] public Level.Type levelType;
    [SerializeField] SO_MapIcons icons;
    Selectable selectable;
    Image icon;

    
    bool nameSet = false;
    void Start()
    {
        icon = GetComponent<Image>();
        selectable = GetComponent<Selectable>();
    }


    private void Update()
    {

    }


    

    void UpdateSprite()
    {
        icon = GetComponent<Image>();
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
        icon.sprite = sprite;
    }

    private void OnValidate()
    {
        UpdateSprite();
    }

    [Button]
    void AddConnectingPath()
    {
        iconPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object)).GameObject();
        GameObject newPath = Instantiate(iconPrefab, transform.parent);
        newPath.transform.position = transform.position + (Vector3.right * 50);
        nextLevels.Add(newPath);
        newPath.GetComponent<MapIcon>().previousLevels.Add(gameObject);
        newPath.transform.position = new Vector3(Mathf.Round(newPath.transform.position.x), Mathf.Round(newPath.transform.position.y), Mathf.Round(newPath.transform.position.z));
        newPath.name = "MapIcon" + newPath.GetInstanceID().ToString();
        Selection.activeObject = newPath;
    }

    [Button]
    void AddPath()
    {
        iconPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object)).GameObject();
        GameObject newPath = Instantiate(iconPrefab, transform.parent);
        newPath.transform.position = transform.position + (Vector3.right * 50);
        newPath.transform.position = new Vector3(Mathf.Round(newPath.transform.position.x), Mathf.Round(newPath.transform.position.y), Mathf.Round(newPath.transform.position.z));
        newPath.name = "MapIcon" + newPath.GetInstanceID().ToString();
        Selection.activeObject = newPath;
    }


    [Button]
    void ConnectSelected()
    {
        if (Selection.objects.Length >= 2)
        {
            for (int i = 0; i < Selection.objects.Length - 1; i++)
            {
                GameObject obj = Selection.objects[i].GameObject();
                GameObject nextObj = Selection.objects[i + 1].GameObject();
                if (!obj.TryGetComponent<MapIcon>(out MapIcon path)) continue;
                if (!nextObj.TryGetComponent<MapIcon>(out MapIcon nextPath)) continue;
                path.nextLevels.Add(nextObj);
            }
        }
    }

    [Button]
    void Disconnect()
    {
        for (int i = 0; i < previousLevels.Count; i++)
        {
            if (previousLevels[i] == null) continue;
            previousLevels[i].GetComponent<MapIcon>().RemovePath(gameObject);
            previousLevels.RemoveAt(i);
        }
        for (int i = 0; i < nextLevels.Count; i++)
        {
            if (nextLevels[i] == null) continue;
            nextLevels.RemoveAt(i);
        }
    }

    public void RemovePath(GameObject path)
    {
        for (int i = 0; i < nextLevels.Count; i++)
        {
            if (previousLevels[i] == path) { nextLevels.RemoveAt(i); continue; }
        }
        for (int i = 0; i < previousLevels.Count; i++)
        {
            if (previousLevels[i] == path) { previousLevels.RemoveAt(i); continue; }
        }
    }

    private void OnDrawGizmos()
    {
        if (!nameSet) { name = "MapIcon" + GetInstanceID().ToString(); nameSet = true; }

        Handles.Label(transform.position + transform.up * 10, name);

        //Handles.DrawOutline(Selection.objects as GameObject[], Color.blue, 1f);
        
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            float dist = 1 * Mathf.Pow(0.9f, (float)Selection.objects.Length - 1 - i);

            if (Selection.activeObject.GameObject() == gameObject) Handles.color = Color.blue; else Handles.color = Color.Lerp(Color.white,Color.blue,dist);

            if (Selection.objects[i].GameObject() == gameObject) { 
                Handles.Label(transform.position + transform.up * 20, (i).ToString());
                Handles.DrawWireDisc(transform.position,transform.forward,10,3);
            }
            
            if (i < Selection.objects.Length-1) Util.DrawArrow(Selection.objects[i + 1].GameObject().transform.position, Selection.objects[i].GameObject().transform.position);
            Handles.color = Color.white;
        }
        
        for (int i = 0; i < nextLevels.Count; i++)
        {
            if (nextLevels[i] == null) { nextLevels.RemoveAt(i); continue; }

            Handles.color = Color.yellow;
            Util.DrawArrow(nextLevels[i].transform.position, transform.position);
        }
        for (int i = 0; i < previousLevels.Count; i++)
        {
            if (previousLevels[i] == null) { previousLevels.RemoveAt(i); continue; }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Deselect");
        transform.localScale = Vector3.one;

    }

    public void OnSelect(BaseEventData eventData)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Select");
        transform.localScale = Vector3.one * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Exit");
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        transform.localScale = Vector3.one * 1.2f;
        //throw new System.NotImplementedException();
    }
}
