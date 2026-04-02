using UnityEngine;

public class TeleportInteractable : MonoBehaviour
{
    public Transform teleportDestination;
    public KeyCode interactKey = KeyCode.E;

    public bool playerInRange = false;
    private Transform player;
    private PlayerTeleportState teleportState;

    // Cooldown para evitar que el mismo frame del teleport dispare interacciones en el destino
    private bool justTeleported = false;

    void Update()
    {
        // Si acabamos de teleportar, esperamos un frame antes de volver a escuchar input
        if (justTeleported)
        {
            justTeleported = false;
            return;
        }

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (teleportState != null && teleportState.CanUseTeleport())
            {
                TeleportPlayer();
            }
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

        // Al teletransportar con CharacterController, OnTriggerExit NO se dispara automaticamente.
        // Reseteamos el estado manualmente para que no se pueda volver a teleportar en el mismo frame.
        playerInRange = false;
        player = null;
        teleportState = null;
        justTeleported = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
            teleportState = other.GetComponent<PlayerTeleportState>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            teleportState = null;
        }
    }

    private void OnGUI()
    {
        if (playerInRange)
        {
            if (teleportState != null && teleportState.CanUseTeleport())
            {
                GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height - 80, 200, 30), "Pulsa F para entrar");
            }
        }
    }
}