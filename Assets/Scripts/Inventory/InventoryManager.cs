using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public RemoveItemButton[] InventoryItems;

    private string savePath;

    private void Awake()
    {
        Instance = this;
        savePath = Application.persistentDataPath + "/inventory.json";
    }

    private void Start()
    {
        LoadInventory();
        ListItems();
    }

    public void Add(Item item)
    {
        Item existingItem = Items.Find(i => i.id == item.id); 

        if (existingItem != null)
        {
            existingItem.count += item.count; // Вручает только один элемент вместо всего количества
        } 
        else 
        { 
            Item newItem = ScriptableObject.CreateInstance<Item>(); 
            newItem.id = item.id; 
            newItem.count = item.count; 
            newItem.icon = item.icon; 
            Items.Add(newItem); 
        } 
        ListItems(); 
        SaveInventory(); }

    public void RemoveItem(Item item, int amount)
    {
        Item existingItem = Items.Find(i => i.id == item.id);

        if (existingItem != null)
        {
            existingItem.count -= amount;

            if (existingItem.count <= 0)
            {
                Items.Remove(existingItem);
            }
        }

        ListItems();
        SaveInventory();
    }

    public void ListItems()
    {
        CleanInventory();

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemCountText = obj.transform.Find("ItemCountText").GetComponent<TextMeshProUGUI>();
            var removeButton = obj.GetComponent<RemoveItemButton>();

            itemIcon.sprite = item.icon;

            if (item.count > 1)
            {
                itemCountText.text = item.count.ToString();
                itemCountText.gameObject.SetActive(true);
            }
            else
            {
                itemCountText.gameObject.SetActive(false);
            }

            removeButton.AddItem(item);
        }
    }

    public void CleanInventory()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
    }

    public void SaveInventory()
    {
        InventoryData data = new InventoryData();

        foreach (Item item in Items)
        {
            data.items.Add(new ItemData(item.id, item.count, item.icon));
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Инвентарь сохранен.");
    }

    public void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);

            Items.Clear();

            foreach (ItemData itemData in data.items)
            {
                // Восстанавливаем предмет по идентификатору
                Item item = ScriptableObject.CreateInstance<Item>();
                item.id = itemData.id;
                item.count = itemData.count;
                item.icon = itemData.icon;
                Items.Add(item);
            }

            Debug.Log("Инвентарь загружен.");
        }
        else
        {
            Debug.Log("Файл сохранения не найден.");
        }
    }
}

