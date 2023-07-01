using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUsable", menuName = "Inventory System/Items/Usable")]
public class Usable : ItemObject
{
    [SerializeField] private bool hasDuration;
    [SerializeField] private float duration;
    public void Awake()
    {
        itemType = ItemType.Usable;
    }
}