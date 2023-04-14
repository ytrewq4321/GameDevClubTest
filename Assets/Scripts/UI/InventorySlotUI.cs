using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class InventorySlotUI : MonoBehaviour,  IBeginDragHandler, IEndDragHandler, IDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IDroppable<Tuple<InventorySlotUI, BaseItem>>
{
    public Image itemIcon;
    public TextMeshProUGUI stackCountText;
    public InventoryUI inventoryUI;
    private BaseItem item;
    public int slotId; 
    private int stackCount;

    private void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void SetItem(BaseItem item, int stackCount)
    {
        this.item = item;
        itemIcon.sprite = item.Icon;
        itemIcon.enabled = true;
        this.stackCount = stackCount;
        stackCountText.SetText(stackCount.ToString());
        stackCountText.enabled = stackCount > 1;
    }

    public void ClearSlot()
    {
        item = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        stackCountText.text = "";
        stackCountText.enabled = false;
    }

    public void OnDrop(Tuple<InventorySlotUI, BaseItem> data)
    {
        if (data.Item1.slotId == slotId )
        {
            return;
        }
        else
        {
            var amountAdded = User.Current.Inventory.TryAdd(data.Item2, data.Item1.stackCount, slotId);

            if (amountAdded > 0)
            {
                User.Current.Inventory.TryRemove(data.Item1.slotId, amountAdded);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        DragContainer.Context.StartDrag(new Tuple<InventorySlotUI, BaseItem>(this, item));
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        DragContainer.Context.ProcessDrag();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (item == null) return;
        DragContainer.Context.EndDrag();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragContainer.Context.EnterContainer(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DragContainer.Context.ExitContainer(this);
    }
}
