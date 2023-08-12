using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MyCode.GameData.Inventory
{
    public class InventoryItemStorage
    {
        public static List<InventoryItem> activeInventoryItems = new List<InventoryItem>();

        public static bool ContainsItem(int _itemId)
        {
            if (activeInventoryItems.Exists(item => item.ItemId == _itemId)) return true;

            return false;
        }

        public static InventoryItem GetItem(int _itemId)
        {
            if (ContainsItem(_itemId)) return activeInventoryItems.Where(item => item.ItemId == _itemId).First();

            return null;
        }

        public static void AddItem(InventoryItem _item)
        {
            if (activeInventoryItems.Exists(item => item.ItemId == _item.ItemId)) return;

            activeInventoryItems.Add(_item);
        }

        public static IEnumerable GetAllObjects()
        {
            foreach (InventoryItem _item in activeInventoryItems) yield return _item;
        }
    }
}
