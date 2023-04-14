using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Items/Weapon")]
public class WeaponItem : BaseItem
{
    public PatronType PatronType;
    public int Damage;
}
