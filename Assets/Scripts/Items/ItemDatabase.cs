using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Items/ItemDatabase", order = 1)]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<BaseItem> itemList;
    public static Dictionary<int, BaseItem> ItemsById { get; private set; }

    public void Initialize()
    {
        ItemsById = new Dictionary<int, BaseItem>();
        foreach (BaseItem item in itemList)
        {
            ItemsById[item.Id] = item;
        }
    }
}