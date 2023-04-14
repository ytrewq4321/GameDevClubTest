using System;
using UnityEngine;
using UnityEngine.UI;

public class DragContainer : MonoBehaviour
{
    private static DragContext<Tuple<InventorySlotUI, BaseItem>> context;
    public static DragContext<Tuple<InventorySlotUI, BaseItem>> Context => context;

    [SerializeField] private RectTransform dragContainer;

    private GameObject visualDraggableObject;

    private void Awake()
    {
        context = new DragContext<Tuple<InventorySlotUI, BaseItem>>();
        context.OnStartDrag += ContextStartDrag;
        context.OnEndDrag += ContextOnEndDrag;
        context.OnDrag += ContextOnDrag;
    }

    private void ContextOnDrag(Tuple<InventorySlotUI, BaseItem> data)
    {
        if (visualDraggableObject != null)
        {
            visualDraggableObject.transform.position = Input.mousePosition;
        }
    }

    private void ContextStartDrag(Tuple<InventorySlotUI, BaseItem> data)
    {
        visualDraggableObject = Instantiate(data.Item1.gameObject, dragContainer,true);
        visualDraggableObject.GetComponent<Image>().raycastTarget = false;
    }

    private void ContextOnEndDrag(Tuple<InventorySlotUI, BaseItem> data)
    {
         Destroy(visualDraggableObject);
    }
}