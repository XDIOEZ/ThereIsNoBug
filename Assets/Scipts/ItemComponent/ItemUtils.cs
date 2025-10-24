using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ItemUtils 
{
    private static Dictionary<int,ItemInfo> items;
    
    public static void Init(TextAsset asset)
    {
        items = new Dictionary<int, ItemInfo>();
        ItemInfo[] itemInfos = ReadSCV(asset);
        foreach (ItemInfo itemInfo in itemInfos)
        {
            items.Add(itemInfo.Id, itemInfo);
        }

        Debug.Log("初始化物品字典完毕，共有" + items.Count + "个物品");
    }
    
    private static ItemInfo[] ReadSCV(TextAsset asset)
    {
        string[] rows = asset.text.Split("\n");
        ItemInfo[] itemInfos = new ItemInfo[rows.Length-2];
        for (int i = 1; i < rows.Length-1; i++)
        {
            string[] cells = rows[i].Split(",");
            int id = int.Parse(cells[0]);
            string name = cells[1];
            string description = cells[2];
            string imagePath = cells[3];
            string prePath = cells[4];
            if (i == rows.Length-2)
            {
                prePath = prePath.Substring(0, prePath.Length - 1);
            }
            ItemInfo itemInfo = new ItemInfo(id, name, description, imagePath, prePath);
            itemInfos[i-1] = itemInfo;
        }
        return itemInfos;
    }
    
    public static ItemInfo GetItemInfo(int _id)
    {
        return items[_id];
    }
}

public class ItemInfo
{
    public ItemInfo(int _id, string _name, string _desc, string _imgpath, string _prePath)
    {
        id = _id;
        name = _name;
        description =  _desc;
        imagePath = _imgpath;
        prePath = _prePath;
    }
    private int id;
    public int Id
    {
        get { return id; }
    }
    
    private string name;

    public string Name
    {
        get { return name; }
    }
    
    private string description;

    public string Description
    {
        get { return description; }
    }
    
    private string imagePath;

    public string ImagePath
    {
        get { return imagePath; }
    }
    
    private string prePath;

    public string PrePath
    {
        get { return prePath; }
    }
}