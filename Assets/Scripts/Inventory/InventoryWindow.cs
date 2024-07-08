using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Inventory;

public class InventoryWindow : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] Transform tokenButtonContainer;
    [SerializeField] GameObject inventoryButton;
    List<InventoryButton> inventoryButtons = new List<InventoryButton>();

    private void HandleButtonClicked(InventoryButton button)
    {
        inventory.selectedToken = button.item;
    }


    void Start()
    {
        int i = 0;
        foreach (Slot<Token> slot in inventory.tokens)
        {
            i++;
            InventoryButton button = Instantiate(inventoryButton, tokenButtonContainer).GetComponent<InventoryButton>();
            button.item = slot.item;
            button.SetImage(slot.item.GetComponent<SpriteRenderer>().sprite);
            button.OnButtonClicked.AddListener(HandleButtonClicked);
            button.transform.localPosition = Vector3.zero + new Vector3(i * 18, 0, 0);
            button.inventoryWindow = this;
            inventoryButtons.Add(button);
        }
    }

    private void Update()
    {
        for (int i = 0; i < 10; ++i)
        {
            if (!Input.GetKeyDown("" + i)) continue;
            if (i == 0) i = 9;
            if (i-1 >= inventoryButtons.Count) return;
            Debug.Log(i - 1);
            SelectToken(i - 1);
            inventory.selectedToken = inventoryButtons[i-1].item;
        }
    }

    private void SelectToken(int i)
    {
        DeselectTokens();
        inventoryButtons[i].Select();
        inventoryButtons[i].SetHovered(true);
    }

    public void DeselectTokens()
    {
        foreach (InventoryButton inventoryButton in inventoryButtons)
        {
            inventoryButton.Deselect();
            inventoryButton.SetHovered(false);
        }
    }
}
