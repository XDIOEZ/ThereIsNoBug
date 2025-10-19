using System.Collections.Generic;

public static class ItemUtils 
{
    private static Dictionary<int,string> items;

    public static void Init()
    {
        
    }
    
    public static string GetItem(int _id)
    {
        return items[_id];
    }
}
