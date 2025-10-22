using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : SingletonMono<Inventory>
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
        //todo: 添加至ui
    }

    public void RemoveItem(Item item)
    {
        if (itemInInventory.Contains(item))
        {
            itemInInventory.Remove(item);
            //todo: ui中删除
        }
    }

    public void ExChangeItem(Item item ,int index)
    {
        if (itemInInventory[index] != null && itemInInventory[index] != item)
        {
            int _index = item.GetComponent<InventoryComponent>().index;
            Item _item = itemInInventory.FirstOrDefault(item => item.GetComponent<InventoryComponent>().index == index);
            itemInInventory[index] = item;
            itemInInventory[_index] = _item;
            //todo:物品栏中交换
        }
    }
}
