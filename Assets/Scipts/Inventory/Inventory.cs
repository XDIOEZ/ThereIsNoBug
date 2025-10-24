using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Inventory : SingletonMono<Inventory>
{
    public List<Item> itemInInventory;
    public Item nowItem;
    public TextAsset textAsset;
    public bool isFirst = true;
    
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
            if (itemIndex == 1 && isFirst)
            {
                AddItem(1);
                isFirst = false;
                ResetInventory();
                return;
            }
            nowItem = itemInInventory.FirstOrDefault(item => item.GetComponent<InventoryComponent>().index == itemIndex);
            GamePlayManager.Instance.isOnInventory = true;
            if (nowItem == null)
            {
                Debug.Log("No item selected");
                ResetInventory();
            }
            else
            {
                Debug.Log(nowItem.GetName());
            }
        }
        else
        {
            ExchangeItem(nowItem , itemIndex);
        }
    }

    public void ResetInventory()
    {
        nowItem = null;
        GamePlayManager.Instance.isOnInventory = false;
    }
    
    public void UseItem()
    {
        nowItem.gameObject.GetComponent<InventoryComponent>().Used();
        ResetInventory();
    }
    
    public void AddItem(int id)
    {
        Debug.Log("Add item");
        string path = ItemUtils.GetItemInfo(id).PrePath;
        GameObject item = Instantiate(Resources.Load<GameObject>(path));
        item.GetComponent<Item>().id = id;
        itemInInventory.Add(item.GetComponent<Item>());
        item.transform.SetParent(transform);
        int index = 0;
        for (int i = 0; itemInInventory[i].GetComponent<InventoryComponent>().index != index; i++)
        {
            index = i;
        }
        item.GetComponent<InventoryComponent>().index = index;
        //显示在物品栏中
        UIMgr.Instance().GetPanel<GamePanel>("GamePanel").GetItem(item.GetComponent<Item>());
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

    public void ExchangeItem(Item item ,int index)
    {
        Debug.Log("Exchanging item");
        if (itemInInventory.FirstOrDefault(item => item.GetComponent<InventoryComponent>().index == index) != null)
        {
            int _index = item.GetComponent<InventoryComponent>().index;
            Item _item = itemInInventory.FirstOrDefault(item => item.GetComponent<InventoryComponent>().index == index);
            _item.GetComponent<InventoryComponent>().index = _index;
            item.GetComponent<InventoryComponent>().index = index;
            //物品栏中交换
            GamePanel gamePanel = UIMgr.Instance().GetPanel<GamePanel>("GamePanel");
            gamePanel.ChangeSprite(index, item);
            gamePanel.ChangeSprite(_index,_item);
        }
        else
        {
            UIMgr.Instance().GetPanel<GamePanel>("GamePanel").RemoveItem(item);
            item.GetComponent<InventoryComponent>().index =  index;
            UIMgr.Instance().GetPanel<GamePanel>("GamePanel").GetItem(item.GetComponent<Item>());
        }
        ResetInventory();
    }
    
    private void InitItems()
    {
        ItemUtils.Init(textAsset);
    }
}
