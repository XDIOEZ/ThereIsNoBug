using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> itemInInventory;
    public Item nowItem;
    
    void Start()
    {
        itemInInventory = new List<Item>();
    }

    public void AddItem(Item item)
    {
        itemInInventory.Add(item);
    }

    public void RemoveItem(Item item)
    {
        if (itemInInventory.Contains(item))
        {
            itemInInventory.Remove(item);
        }
    }

    public void ExChangeItem(Item item ,int index)
    {
        if (itemInInventory[index] != null && itemInInventory[index] != item)
        {
            int _index = itemInInventory.IndexOf(item);
            Item _item = itemInInventory[index];
            itemInInventory[index] = item;
            itemInInventory[_index] = _item;
        }
    }
}
