using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public List<ItemData> items = new List<ItemData>();
}

[Serializable]
public class ItemData
{
    public int id;
    public int count;
    public Sprite icon;

    public ItemData(int id, int count, Sprite icon)
    {
        this.id = id;
        this.count = count;
        this.icon = icon;
    }
}
