using System.Collections.Generic;

public static class ItemUtils 
{
    private static Dictionary<int,ItemInfo> items;

    public static void Init()
    {
        
    }
    
    public static ItemInfo GetItemInfo(int _id)
    {
        return items[_id];
    }
}

public class ItemInfo
{
    private int id;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    
    private string description;

    public string Description
    {
        get { return description; }
        set { description = value; }
    }
    
    private string imagePath;

    public string ImagePath
    {
        get { return imagePath; }
        set { imagePath = value; }
    }
    
    private string prePath;

    public string PrePath
    {
        get { return prePath; }
        set { prePath = value; }
    }
}