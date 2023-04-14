using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Items/Armor")]
public class ArmorItem : BaseItem
{
    public ArmorType ArmorType;
    public int ArmorValue;
}
