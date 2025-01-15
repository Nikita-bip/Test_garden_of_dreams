using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }
}