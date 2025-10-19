using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;

    public string GetPrePath()
    {
        return ItemUtils.GetItemInfo(id).PrePath;
    }
    public string GetImagePath()
    {
        return ItemUtils.GetItemInfo(id).ImagePath;
    }
    public string GetName()
    {
        return ItemUtils.GetItemInfo(id).Name;
    }
    public string GetDescription()
    {
        return ItemUtils.GetItemInfo(id).Description;
    }
}
