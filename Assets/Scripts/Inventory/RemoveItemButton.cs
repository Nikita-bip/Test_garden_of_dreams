using UnityEngine;
using UnityEngine.UI;

public class RemoveItemButton : MonoBehaviour
{
    private Item item;
    public Button RemoveButton;

    private void Start()
    {
        RemoveButton.onClick.AddListener(RemoveItem);
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.RemoveItem(item);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }
}
