using System;
using Newtonsoft.Json;

[Serializable]
public class InventorySlot 
{
    [JsonProperty] public  int Id;
    [JsonProperty] public int ItemId { get; private set; }

    [JsonProperty] public int stackCount;

    public InventorySlot()
    {
        ClearSlot();
    }

    public void SetItem(int itemId,int amount)
    {
        this.ItemId = itemId;
        stackCount = amount;

    }

    public void ClearSlot()
    {
        ItemId = -1;
        stackCount = 0;
    }

    public bool IsEmpty()
    {
        return ItemId == -1;
    }

    public void AddToStack(int amount)
    {
        stackCount += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackCount -= amount;
        if(stackCount==0)
        {
            ClearSlot();
        }
    }
}
