using System.Collections.Generic;
using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    private HashSet<string> collectedKeys = new HashSet<string>();

    public void AddKey(string keyID)
    {
        if (!string.IsNullOrEmpty(keyID))
        {
            collectedKeys.Add(keyID);
        }
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }

    public void RemoveKey(string keyID)
    {
        if (collectedKeys.Contains(keyID))
        {
            collectedKeys.Remove(keyID);
        }
    }
}