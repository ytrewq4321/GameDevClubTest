using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private TextMeshProUGUI weight;
    private List<InventorySlotUI> slotsUI = new List<InventorySlotUI>();

    private void Start()
    {
        inventory = User.Current.Inventory;
        InitializeInventoryUI();

        inventory.InventoryChanged += OnUpdateInventorySlotUI;
        inventory.NewSlotOpened += OnNewSlotOpen;        
    }

    private void InitializeInventoryUI()
    {
        for (int i = 0; i < inventory.maxSize; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, parent);
            var slotUI = newSlot.GetComponent<InventorySlotUI>();
            slotUI.slotId = i;
            slotsUI.Add(slotUI);
            OnUpdateInventorySlotUI(i);

            if (i > inventory.availableSize-1)
            {
                newSlot.SetActive(false);
            }
            OnUpdateInventorySlotUI(i);
        }
        SetWeight();
    }

    public void OnNewSlotOpen()
    {
        slotsUI[inventory.availableSize - 1].gameObject.SetActive(true); 
    }
    
    public void OnUpdateInventorySlotUI(int slotIndex)
    {
        InventorySlotUI slotUI = slotsUI[slotIndex];
        var slot = inventory.inventorySlots[slotIndex];
        if (slot.stackCount != 0)
        {
            if (ItemDatabase.ItemsById.ContainsKey(slot.ItemId))
            {
                slotUI.SetItem(ItemDatabase.ItemsById[slot.ItemId], slot.stackCount);
            }
        }
        else
        {
            slotUI.ClearSlot();
        }
        SetWeight();
    }

    private void SetWeight()
    {
        weight.SetText($"Weight: { inventory.currentWeight:0.00}/{ inventory.maxWeight}");
    }

    private void OnDestroy()
    {
        inventory.InventoryChanged -= OnUpdateInventorySlotUI;
        inventory.NewSlotOpened -= OnNewSlotOpen;
    }
}
