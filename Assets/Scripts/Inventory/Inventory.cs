using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;







public class Inventory : MonoBehaviour
{
    [Serializable]
    public class Slot<T>
    {
        public T item;
        public int totalAmount;
        public int currentAmount;
        public Slot(T item, int amount)
        {
            totalAmount = amount;
            currentAmount = amount;
            this.item = item;
        }
        public void AddTotal(int amount)
        {
            totalAmount += amount;
            currentAmount += amount;
        }
        public void AddCurrent(int amount)
        {
            currentAmount += amount;
            currentAmount = Mathf.Clamp(currentAmount, 0, totalAmount);
        }
        public void DecreaseTotal(int amount)
        {
            totalAmount -= amount;
            currentAmount = Mathf.Min(currentAmount, totalAmount);
        }
        public void DecreaseCurrent(int amount)
        {
            currentAmount -= amount;
            currentAmount = Mathf.Clamp(currentAmount, 0, totalAmount);
        }
    }

    public List<Slot<Token>> tokens;
    public List<Slot<Item>> items;
    public Token selectedToken;
    public Item selectedItem;

    [SerializeField] public Token itemToAdd;

    [Button]
    public void AddTest()
    {
        AddTotal(tokens, itemToAdd);
    }
    [Button]
    public void RemoveTest()
    {
        DecreaseTotalAndRemove(tokens, itemToAdd);
    }
    [Button]
    public void Check()
    {
        int amount = GetCurrentAmount(tokens, itemToAdd);
        Debug.Log(amount);
    }

    public void AddTotal<T>(List<Slot<T>> inventory, T item, int amount = 1)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null)
        {
            inventory.Add(new Slot<T>(item, amount));
            return;
        }
        slot.AddTotal(amount);
    }
    public void AddCurrent<T>(List<Slot<T>> inventory, T item, int amount = 1)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null)
        {
            inventory.Add(new Slot<T>(item, amount));
            return;
        }
        slot.AddCurrent(amount);
    }
    public int GetCurrentAmount<T>(List<Slot<T>> inventory, T item)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null) return 0;
        return slot.currentAmount;
    }
    public int GetTotalAmount<T>(List<Slot<T>> inventory, T item)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null) return 0;
        return slot.totalAmount;
    }
    public void DecreaseTotal<T>(List<Slot<T>> inventory, T item, int amount = 1)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null) return;
        slot.DecreaseTotal(amount);
    }
    public void DecreaseCurrent<T>(List<Slot<T>> inventory, T item, int amount = 1)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null) return;
        slot.DecreaseCurrent(amount);
    }
    public void Remove<T>(List<Slot<T>> inventory, T item)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null) return;
        inventory.Remove(slot);
    }
    public void DecreaseTotalAndRemove<T>(List<Slot<T>> inventory, T item, int amount = 1)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null) return;
        slot.DecreaseTotal(amount);
        if (slot.totalAmount == 0) inventory.Remove(slot);
    }
    public void DecreaseCurrentAndRemove<T>(List<Slot<T>> inventory, T item, int amount = 1)
    {
        Slot<T> slot = inventory.FirstOrDefault(s => s.item.Equals(item));
        if (slot == null) return;
        slot.DecreaseCurrent(amount);
        if (slot.currentAmount == 0) inventory.Remove(slot);
    }
    public void ResetAmount(int slot)
    {
        if (tokens[slot] != null)
        {
            tokens[slot].currentAmount = tokens[slot].totalAmount;
        }
    }
}