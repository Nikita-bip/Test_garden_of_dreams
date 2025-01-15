using UnityEngine;
using UnityEngine.UI;

public class RemoveItemButton : MonoBehaviour
{
    [SerializeField] private Button _removeButton;

    private Item _item;

    private void Start()
    {
        _removeButton.onClick.RemoveAllListeners();
        _removeButton.onClick.AddListener(RemoveAllItems);
    }

    public void RemoveAllItems()
    {
        InventoryManager.Instance.RemoveItem(_item, _item.count);
    }

    public void AddItem(Item newItem)
    {
        _item = newItem;
    }
}