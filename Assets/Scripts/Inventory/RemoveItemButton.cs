using UnityEngine;
using UnityEngine.UI;

public class RemoveItemButton : MonoBehaviour
{
    private Item item;
    public Button RemoveButton;

    private void Start()
    {
        RemoveButton.onClick.RemoveAllListeners();
        RemoveButton.onClick.AddListener(RemoveAllItems);
    }

    public void RemoveAllItems()
    {
        InventoryManager.Instance.RemoveItem(item, item.count); // Удаляем все экземпляры предмета
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }
}
