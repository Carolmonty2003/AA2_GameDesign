using UnityEngine;

public class TPToSpawn : MonoBehaviour
{
    [SerializeField] GameObject spawnTP;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = spawnTP.transform.position; 
            other.transform.rotation = spawnTP.transform.rotation; 
        }
    }
}
