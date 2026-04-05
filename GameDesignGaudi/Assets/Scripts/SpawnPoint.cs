using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] int numCheckPoint;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            TeleportarJugador(other.gameObject);
        }
    }

    public void TeleportarJugador(GameObject jugador)
    {
        if (spawnPoint == null) 
        {
            Debug.LogError("Error: No has asignado el 'spawnPoint' en el inspector de la pared " + gameObject.name);
            return;
        }

        
        GameManager.Instance.CheckPointToTick(numCheckPoint);

        CharacterController cc = jugador.GetComponent<CharacterController>();
        
        if (cc != null)
        {
            cc.enabled = false;
            jugador.transform.position = spawnPoint.transform.position;
            jugador.transform.rotation = spawnPoint.transform.rotation;
            cc.enabled = true;
        }
        else
        {
            jugador.transform.position = spawnPoint.transform.position;
            jugador.transform.rotation = spawnPoint.transform.rotation;
        }
    }
}
