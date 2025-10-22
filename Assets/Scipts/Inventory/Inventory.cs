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
        //显示在物品栏中
        UIMgr.Instance().GetPanel<GamePanel>("GamePanel").GetItem(item);
    }

    public void RemoveItem(Item item)
    {
        if (itemInInventory.Contains(item))
        {
            itemInInventory.Remove(item);
            //物品栏中移除
            UIMgr.Instance().GetPanel<GamePanel>("GamePanel").RemoveItem(item);
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
            //物品栏中交换
            GamePanel gamePanel = UIMgr.Instance().GetPanel<GamePanel>("GamePanel");
            gamePanel.ChangeSprite(index, item);
            gamePanel.ChangeSprite(_index,_item);
        }
    }
}
