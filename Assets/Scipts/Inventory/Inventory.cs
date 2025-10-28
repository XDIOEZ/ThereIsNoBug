using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    public List<Item> itemInInventory;
    public List<Item> items;
    public Item nowItem;
    public TextAsset textAsset;
    public bool isFirst = true;

    private void OnEnable()
    {
        items = new List<Item>(24);
    }

    void Start()
    {
        InitItems();
        itemInInventory = new List<Item>(6);

    }

    public void SelectItem(int itemIndex)
    {
        //test
        if (!GamePlayManager.Instance.isOnInventory)
        {
            nowItem = itemInInventory.FirstOrDefault(item => item.GetComponent<InventoryComponent>().index == itemIndex);

            if (nowItem == null)
            {
                Debug.Log("No item selected");
                ResetInventory();
                return;
            }
            else
            {
                Debug.Log(nowItem.GetName());
            }
            nowItem.GetComponent<InventoryComponent>().Used();
        }
    }

    public void ResetInventory()
    {
        //nowItem = null;
        GamePlayManager.Instance.isOnInventory = false;
        Debug.Log("Reset Inventory");
    }
    
    // public void UseItem()
    // {
    //     nowItem.gameObject.GetComponent<InventoryComponent>().Used();
    //     ResetInventory();
    // }
    
    public Item AddItem(int id)
    {
        Debug.Log("Add item");
        string path = ItemUtils.GetItemInfo(id).PrePath;
        GameObject item = Instantiate(Resources.Load<GameObject>(path));
        item.GetComponent<Item>().id = id;
        int index = 0;
        if (itemInInventory.Count > 0)
        {
            item.GetComponent<InventoryComponent>().index = FoundIndex(0);
        }
        itemInInventory.Add(item.GetComponent<Item>());
        item.transform.SetParent(transform);
        itemInInventory.OrderBy(x=>x.GetComponent<InventoryComponent>().index);
        Item _item = item.GetComponent<Item>();
        //显示在物品栏中
        UIMgr.Instance().GetPanel<GamePanel>("GamePanel").GetItem(_item);
        return item.GetComponent<Item>();
    }

    private int FoundIndex(int itemIndex)
    {
        int index = -1;
        if (itemInInventory.Count <= itemIndex)
        {
            return itemIndex;
        }
        if (itemInInventory[itemIndex].GetComponent<InventoryComponent>().index==itemIndex)
        {
            index = FoundIndex(itemIndex + 1);
            return index;
        }
        return itemIndex + 1;
    }

    public void RemoveItem(int id)
    {
        Item item = itemInInventory.FirstOrDefault(X => X.id == id);
        if (item != null)
        {
            itemInInventory.Remove(item);
            //物品栏中移除
            UIMgr.Instance().GetPanel<GamePanel>("GamePanel").RemoveItem(item);
        }
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

    // public void ExchangeItem(Item item ,int index)
    // {
    //     Debug.Log("Exchanging item");
    //     if (itemInInventory.FirstOrDefault(item => item.GetComponent<InventoryComponent>().index == index) != null)
    //     {
    //         int _index = item.GetComponent<InventoryComponent>().index;
    //         Item _item = itemInInventory.FirstOrDefault(item => item.GetComponent<InventoryComponent>().index == index);
    //         _item.GetComponent<InventoryComponent>().index = _index;
    //         item.GetComponent<InventoryComponent>().index = index;
    //         itemInInventory.OrderBy(x=>x.GetComponent<InventoryComponent>().index);
    //         //物品栏中交换
    //         GamePanel gamePanel = UIMgr.Instance().GetPanel<GamePanel>("GamePanel");
    //         gamePanel.ChangeSprite(index, item);
    //         gamePanel.ChangeSprite(_index,_item);
    //     }
    //     else
    //     {
    //         UIMgr.Instance().GetPanel<GamePanel>("GamePanel").RemoveItem(item);
    //         item.GetComponent<InventoryComponent>().index =  index;
    //         UIMgr.Instance().GetPanel<GamePanel>("GamePanel").GetItem(item.GetComponent<Item>());
    //     }
    //     ResetInventory();
    // }
    //
    private void InitItems()
    {
        ItemUtils.Init(textAsset);
    }

    IEnumerator CountTime()
    {
        yield return new WaitForSeconds(0.05f);
    }
}
