using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject shadowObject;
    SpriteRenderer shadowSpriteRenderer;
    SpriteRenderer spriteRenderer;
    [SerializeField] Color shadowColor = new Color(0,0,0,0.5f);
    [SerializeField] Vector2 offset = new Vector2(2,-2);
    void Start()
    {
       

        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer s))
        {
            spriteRenderer = s;
        }
        else {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        shadowObject = Instantiate(new GameObject(), transform);
        shadowObject.AddComponent<SpriteRenderer>();
        shadowSpriteRenderer = shadowObject.GetComponent<SpriteRenderer>();
        shadowSpriteRenderer.sprite = spriteRenderer.sprite;
        shadowSpriteRenderer.color = shadowColor;
        shadowSpriteRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
        shadowSpriteRenderer.transform.localPosition = offset;
        
    }
}
