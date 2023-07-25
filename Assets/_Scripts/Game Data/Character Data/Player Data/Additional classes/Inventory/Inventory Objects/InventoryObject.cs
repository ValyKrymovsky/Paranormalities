using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;
using MyBox;

namespace MyCode.Player.Inventory
{
    [CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public InventoryItem[] inventory;
        public int size;

        public event Action<InventoryItem> OnAddEquipment;

        public bool AddItem(InventoryItem _item)
        {
            if (IsFull() == true) return false;

            Debug.Log("Inventory not full");

            if (HasItem(_item)) return false;

            Debug.Log("Inventory doesn't have this item");

            inventory = inventory.Append(_item).ToArray();

            if (_item.Item.itemType == ItemObject.ItemType.Equipment) OnAddEquipment?.Invoke(_item);

            return true;
        }

        public void RemoveItem(InventoryItem _item)
        {
            if (HasItem(_item))
            {
                // inventory[inventory.IndexOfItem(_item)] ;
            }
        }

        public InventoryItem GetItemAtIndex(int _index)
        {
            try
            {
                InventoryItem item = inventory.ElementAt(_index);
                return item;
            }
            catch (ArgumentOutOfRangeException)
            {
                return new InventoryItem(0, null, null, null);
            }
        }

        public bool HasItem(InventoryItem _item)
        {
            if (_item.Item == null) return false;

            if (inventory.Where(item => item.ItemId == _item.ItemId).Any()) return true;

            return false;
        }

        public void Clear()
        {
            Array.Clear(inventory, 0, inventory.Length);
        }

        public bool IsFull()
        {
            if (inventory.Count() >= size) return true;

            return false;
        }
    }

}
