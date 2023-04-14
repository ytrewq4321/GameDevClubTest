using System.Collections.Generic;
using UnityEngine;

public class Actions
{
    Dictionary<int,BaseItem>.ValueCollection allItems;

    public Actions()
    {
        allItems = ItemDatabase.ItemsById.Values;
    }

    public void Shoot()
    {
        var slotList = User.Current.Inventory.FindItemsByType<PatronItem>();
        if (slotList.Count > 0)
        {
            User.Current.Inventory.TryRemove(slotList[Random.Range(0, slotList.Count)], 1);
            Debug.Log("shoot");
        }
    }

    public void AddPatrons()
    {
        foreach (var item in allItems)
        {
            if (item is PatronItem)
            {
                User.Current.Inventory.TryAdd(item, item.MaxStack);
            }
        }
    }

    public void AddItems()
    {       
        foreach (var item in allItems)
        {
            if (!(item is PatronItem))
            {
                User.Current.Inventory.TryAdd(item, 1);
            }
        }
    }

    public void RemoveItem()
    {
        if (User.Current.Inventory.isEmpty)
        {
            Debug.Log("Inventory is empty!");
            return;
        }

        var slotIndex = User.Current.Inventory.ReturnRandomBusySlotIndex();

        if(slotIndex!=-1)
        {
            User.Current.Inventory.TryRemove(slotIndex, User.Current.Inventory.inventorySlots[slotIndex].stackCount);
        }
    }

    public void UnlockNewSlot()
    {
        var inventory = User.Current.Inventory;

        if (User.Current.Currency>= inventory.unlockSlotCost &&
            inventory.availableSize< inventory.maxSize)
        {
            User.Current.SetCurrency(inventory.unlockSlotCost);
            inventory.UnlockNewSlot();
        }
    }
}
