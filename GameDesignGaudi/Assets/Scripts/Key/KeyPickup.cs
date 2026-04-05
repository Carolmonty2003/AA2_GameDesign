using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Tag del jugador")]
    [SerializeField] private string playerTag = "Player";

    private KeyItem keyItem;

    private void Awake()
    {
        keyItem = GetComponent<KeyItem>();

        if (keyItem == null)
        {
            Debug.LogError($"Falta KeyItem en {gameObject.name}", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (keyItem == null) return;

        KeyInventory inventory = other.GetComponent<KeyInventory>();
        if (inventory == null)
        {
            Debug.LogWarning($"El jugador no tiene KeyInventory en {other.name}", other);
            return;
        }

        inventory.AddKey(keyItem.keyID);
        keyItem.isCollected = true;

        gameObject.SetActive(false);
    }
}