using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class Inventory
{
    [JsonProperty] public List<InventorySlot> inventorySlots { get; private set; }

    //Key - itemId, Value - List<slotId>
    [JsonProperty] public Dictionary<int, List<int>> slotsDict { get; private set; } 
    [JsonProperty] public int maxSize { get; private set; }
    [JsonProperty] public int availableSize { get; private set; } = 15;
    [JsonProperty] public float maxWeight { get; private set; }
    [JsonProperty] public float currentWeight { get; private set; }
    public int unlockSlotCost { get; private set; }
    public bool isEmpty => slotsDict.Count==0;
    public event Action<int> InventoryChanged;
    public event Action NewSlotOpened;

    public Inventory(int size, float maxWeight,int unlockSlotCost)
    {
        inventorySlots = new List<InventorySlot>(size);
        slotsDict = new Dictionary<int, List<int>>();
        maxSize = size;
        this.maxWeight = maxWeight;
        this.unlockSlotCost = unlockSlotCost;

        for (int i = 0; i < maxSize; i++)
        {
            inventorySlots.Add(new InventorySlot());
            inventorySlots[i].Id = i;
        }
    }

    public void UnlockNewSlot()
    {
        availableSize++;
        NewSlotOpened.Invoke();
    }

    public void TryAdd(BaseItem item, int amountToAdd)
    {
        int remainingAmount = AddToExistingStack(item, amountToAdd);

        if (remainingAmount > 0)
        {
            CreateNewStack(item, remainingAmount);
        }
    }

    private int AddToExistingStack(BaseItem item, int amountToAdd)
    {
        List<int> itemSlotIndexes;

        if (slotsDict.TryGetValue(item.Id, out itemSlotIndexes))
        {
            foreach (int slotIndex in itemSlotIndexes)
            {
                InventorySlot slot = inventorySlots[slotIndex];
                int freeAmount = item.MaxStack - slot.stackCount;

                if (freeAmount >= amountToAdd)
                {
                    AddItemToSlot(slot, item, amountToAdd);
                    return 0;
                }
                else
                {
                    AddItemToSlot(slot, item, freeAmount);
                    amountToAdd -= freeAmount;
                }
            }
        }
        return amountToAdd;
    }

    private void CreateNewStack(BaseItem item, int amountToAdd)
    {
        while (amountToAdd > 0)
        {
            int freeSlotIndex = FindFreeSlot();
            if (freeSlotIndex == -1)
            {
                return;
            }

            int amount = Math.Min(amountToAdd, item.MaxStack);
            InventorySlot freeSlot = inventorySlots[freeSlotIndex];

            AddItemToSlot(freeSlot, item, amount);
            amountToAdd -= amount;
        }
    }

    private void AddItemToSlot(InventorySlot slot, BaseItem item, int count)
    {
        if (slot.stackCount == 0)
        {
            slot.SetItem(item.Id, count);
            if (!slotsDict.ContainsKey(item.Id))
            {
                slotsDict.Add(item.Id, new List<int> { slot.Id});
            }
            else
            {
                if(!slotsDict[item.Id].Contains(slot.Id))
                {
                    slotsDict[item.Id].Add(slot.Id);
                }
            }
        }
        else
        {
            slot.AddToStack(count);
        }
        currentWeight += item.Weight * count;
        InventoryChanged?.Invoke(slot.Id);
    }

    public int TryAdd(BaseItem item, int amountToAdd, int slotId)
    {
        int amountAdded = 0;

        var freeAmount = item.MaxStack - inventorySlots[slotId].stackCount;
        amountToAdd = amountToAdd > freeAmount ? freeAmount : amountToAdd;

        if (inventorySlots[slotId].IsEmpty())
        {
            inventorySlots[slotId].SetItem(item.Id, amountToAdd);
            currentWeight += item.Weight * amountToAdd;
            amountAdded = amountToAdd;

            if (slotsDict.ContainsKey(item.Id))
            {
                slotsDict[item.Id].Add(slotId);
            }
            else
            {
                slotsDict[item.Id] = new List<int> { slotId };
            }
        }
        else if (inventorySlots[slotId].ItemId == item.Id)
        {
            inventorySlots[slotId].AddToStack(amountToAdd);
            currentWeight += item.Weight * amountToAdd;
            amountAdded = amountToAdd;
        }
        InventoryChanged?.Invoke(slotId);
        return amountAdded;
    }

    public bool TryRemove(int slotID, int count)
    {
        if (inventorySlots[slotID].stackCount != 0)
        {
            int itemId = inventorySlots[slotID].ItemId;

            if (inventorySlots[slotID].stackCount >= count)
            {
                inventorySlots[slotID].RemoveFromStack(count);
                currentWeight -= ItemDatabase.ItemsById[itemId].Weight * count;

                if (inventorySlots[slotID].IsEmpty())
                {
                    var slotsList = slotsDict[itemId];
                    slotsList.Remove(slotID);
                    if (slotsList.Count == 0)
                        slotsDict.Remove(itemId);
                }
                InventoryChanged?.Invoke(slotID);
                return true;
            }
        }
        return false;
    }

    public bool IsHasItem(BaseItem item)
    {
        return slotsDict.ContainsKey(item.Id);
    }

    public int FindSlotWithItem(BaseItem item)
    {
        if (slotsDict.TryGetValue(item.Id, out List<int> itemSlotIndexes))
        {
            return itemSlotIndexes[0];
        }
        return -1;
    }

    public int FindFreeSlot()
    {
        for (int i = 0; i < availableSize; i++)
        {
            if (inventorySlots[i].stackCount == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public int ReturnRandomBusySlotIndex()
    {
        foreach (var item in slotsDict)
        {
            return item.Value[UnityEngine.Random.Range(0, item.Value.Count)];
        }
        return -1;
    }

    public List<int> FindItemsByType<T>() where T : BaseItem
    {
        List<int> itemSlotIndexes = new List<int>();

        foreach (var kvp in slotsDict)
        {
            if (ItemDatabase.ItemsById[kvp.Key] is T)
            {
                itemSlotIndexes.AddRange(kvp.Value);
            }
        }
        return itemSlotIndexes;
    }

    public int FindItemById(int id)
    {
        if(slotsDict.ContainsKey(id))
        {
            return slotsDict[id][UnityEngine.Random.Range(0, slotsDict[id].Count)];
        }
        return -1;
    }
}

