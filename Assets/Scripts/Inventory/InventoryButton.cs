using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public UnityEvent<InventoryButton> OnButtonClicked = new UnityEvent<InventoryButton>();
    public Token item;
    public bool selected { get; private set; } = false;
    public bool hovered { get; private set; } = false;
    public bool highlighted { get; private set; } = false;


    private Image image;
    public InventoryWindow inventoryWindow;
    public void SetImage(Sprite sprite)
    {
        image = GetComponent<Image>();
        image.sprite = sprite;
    }


    public void OnDeselect(BaseEventData eventData)
    {
        //Debug.Log("Deselect");
        transform.localScale = Vector3.one;
        selected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Debug.Log("Select");
        transform.localScale = Vector3.one * 1.2f;
        selected = true;
        OnButtonClicked.Invoke(this);
    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = true;
    }

    public void SetHovered(bool hovered)
    {
        this.hovered = hovered;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        transform.SetAsLastSibling();
    }

    public void Update()
    {
        if (highlighted)  
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.2f, Time.deltaTime*20);
        else 
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime*20);
    }

}
