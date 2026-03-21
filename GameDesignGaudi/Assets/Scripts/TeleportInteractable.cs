using UnityEngine;

public class TeleportInteractable : MonoBehaviour
{
    public Transform teleportDestination;
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;
    private Transform player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        if (player == null || teleportDestination == null) return;

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;
            controller.enabled = true;
        }
        else
        {
            player.position = teleportDestination.position;
            player.rotation = teleportDestination.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    private void OnGUI()
    {
        if (playerInRange)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height - 80, 200, 30), "Pulsa E para entrar");
        }
    }
}