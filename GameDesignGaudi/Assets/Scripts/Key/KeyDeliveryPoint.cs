using UnityEngine;

public class KeyDeliveryPoint : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string requiredKeyID = "LlaveA";
    [SerializeField] private string playerTag = "Player";

    [Header("Objetos que se activan al entregar la llave")]
    [SerializeField] private GameObject[] objectsToActivate;

    [Header("Opcional")]
    [SerializeField] private GameObject[] objectsToDeactivate;
    [SerializeField] private bool activateOnlyOnce = true;

    private bool alreadyActivated = false;

    private void Start()
    {
        //objetos empiecen apagados
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (activateOnlyOnce && alreadyActivated) return;

        KeyInventory inventory = other.GetComponent<KeyInventory>();
        if (inventory == null)
        {
            Debug.LogWarning($"El jugador no tiene KeyInventory en {other.name}", other);
            return;
        }

        if (!inventory.HasKey(requiredKeyID)) return;

        inventory.RemoveKey(requiredKeyID);

        ActivateObjects();

        alreadyActivated = true;
    }

    private void ActivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}