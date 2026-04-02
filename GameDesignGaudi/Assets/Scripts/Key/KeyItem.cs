using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("ID de la llave")]
    public string keyID = "LlaveA";

    [HideInInspector] public bool isCollected = false;
}