using UnityEngine;

public class PlayerTeleportState : MonoBehaviour
{
    private int teleportBlockCount = 0;

    public bool CanUseTeleport()
    {
        return teleportBlockCount <= 0;
    }

    public void AddBlock()
    {
        teleportBlockCount++;
    }

    public void RemoveBlock()
    {
        teleportBlockCount--;
        if (teleportBlockCount < 0)
            teleportBlockCount = 0;
    }
}