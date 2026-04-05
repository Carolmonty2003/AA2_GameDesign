using UnityEngine;

public class PuzzleRoomTeleportBlocker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTeleportState state = other.GetComponent<PlayerTeleportState>();
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerTeleportState state = other.GetComponent<PlayerTeleportState>();
         
        }
    }
}